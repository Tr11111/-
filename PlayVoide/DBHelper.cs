using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PlayVideo
{
    public class DBHelper
    {
        public static MySqlConnectionStringBuilder builder;
        public static MySqlConnection connection;
        public static bool IsInit = false;
        public static readonly object locker = new object();
        private List<string> users = new List<string>();
        private List<bool> cw = new List<bool>();
        public static List<string> dz = new List<string>();
        public DBHelper()
        {
            lock (locker)
            {
                if (!IsInit)
                {
                    builder = new MySqlConnectionStringBuilder();
                    //用户名
                    builder.UserID = "root";
                    //密码
                    builder.Password = "123456";
                    //端口
                    builder.Port = 13441;
                    //服务器地址
                    builder.Server = "9.tcp.cpolar.top";
                    //连接时的数据库
                    builder.Database = "test";
                    //定义与数据连接的链接
                    connection = new MySqlConnection(builder.ConnectionString);
                    IsInit = true;
                }
            }
        }
        public void Add(string username, string password,string e_mail)
        {
            connection.Open();
            string sqlyz = "select username from user";
            MySqlCommand cmd3 = new MySqlCommand(sqlyz, connection);
            MySqlDataReader reader1 = cmd3.ExecuteReader();
            
            while (reader1.Read())
            {
                users.Add(reader1.GetString(0));
            }
            connection.Close();
            if (!users.Any(v => v == Md5(username)))
            {
                connection.Open();
                string sqladd = $"insert into user (id,username,password,e_mail) values (null,'{Md5(username)}','{BCrypt.Net.BCrypt.HashPassword(password, BCrypt.Net.BCrypt.GenerateSalt(10, 'a'))}','{e_mail}')";
                MySqlCommand cmd1 = new MySqlCommand(sqladd, connection);
                cmd1.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("注册成功");
            }
            else
            {
                MessageBox.Show("已有该账户");
            }
        }
        public bool Logins(string username, string password)
        {
            connection.Open();
            string sqlsel = "select * from user";
            MySqlCommand cmd2 = new MySqlCommand(sqlsel, connection);
            MySqlDataReader reader2 = cmd2.ExecuteReader();
            while (reader2.Read())
            {
                var a = reader2.GetString(1);
                var b = reader2.GetString(2);
                cw.Add(BCrypt.Net.BCrypt.Verify(password, b) && Md5(username) == a);
            }
            if (cw.Any(v => v == true))
            {
                connection.Close();
                //System.Diagnostics.Process.Start("explorer.exe", "http://www.baidu.com");
                return true;
            }
            else
            {
                connection.Close();
                MessageBox.Show("登陆失败");
                return false;
            }
        }

        public int Select(string username)
        {
            connection.Open();
            string sqlsel = $"select id from user where username='{Md5(username)}'";
            MySqlCommand cmd2 = new MySqlCommand(sqlsel, connection);
            MySqlDataReader reader2 = cmd2.ExecuteReader();
            var id = "";
            while (reader2.Read())
            {
                id = reader2.GetString(0);
            }
            connection.Close();
            if (!string.IsNullOrWhiteSpace(id))
            {
                return int.Parse(id);
            }
            else
            {
                return 1;
            }
        }
        public void SelectList(int userId)
        {
            connection.Open();
            string sqlsel = $"select url from urls where voide_Id={userId}";
            MySqlCommand cmd2 = new MySqlCommand(sqlsel, connection);
            MySqlDataReader reader2 = cmd2.ExecuteReader();
            dz.Clear();
            while (reader2.Read())
            {
                dz.Add(reader2.GetString(0));
            }
            connection.Close();
            
        }
        public int Insert(int userid,string url)
        {
            connection.Open();
            string sqladd = $"insert into urls (Id,url,voide_Id) values (null,'{url}',{userid})";
            MySqlCommand cmd1 = new MySqlCommand(sqladd, connection);
            var count = cmd1.ExecuteNonQuery();
            connection.Close();
            return count;
        }
        public bool Isinsert(int userid, string url)
        {
            connection.Open();
            string sqladd = $"select url from urls where url='{url}' and voide_Id={userid}";
            MySqlCommand cmd1 = new MySqlCommand(sqladd, connection);
            cmd1.ExecuteNonQuery();
            MySqlDataReader reader2 = cmd1.ExecuteReader();
            var isIn = reader2.Read();
            connection.Close();
            return isIn;
        }
        public int Qvxiao(int userid, string url)
        {
            connection.Open();
            string sqladd = $"DELETE FROM urls WHERE url='{url}' AND voide_Id={userid}";
            MySqlCommand cmd1 = new MySqlCommand(sqladd, connection);
            var count = cmd1.ExecuteNonQuery();
            connection.Close();
            return count;
        }
        public bool Dianz(int id,string url)
        {
            connection.Open();
            string sqlsel = $"select * from urls where url = '{url}' and voide_Id = {id}";
            MySqlCommand cmd2 = new MySqlCommand(sqlsel, connection);
            MySqlDataReader reader2 = cmd2.ExecuteReader();
            var iss = reader2.Read();
            connection.Close();
            return iss;
        }

        public int Dianz(string url)
        {
            connection.Open();
            string sqlsel = $"SELECT COUNT(url) FROM urls WHERE url='{url}'";
            MySqlCommand cmd2 = new MySqlCommand(sqlsel, connection);
            MySqlDataReader reader2 = cmd2.ExecuteReader();
            var iss = "";
            while (reader2.Read())
            {
                iss = reader2.GetString(0);
            }
            connection.Close();
            return int.Parse(iss);
        }

        public static string Md5(string txt)
        {
            byte[] sor = Encoding.UTF8.GetBytes(txt);
            MD5 md5 = MD5.Create();
            byte[] result = md5.ComputeHash(sor);
            StringBuilder strbul = new StringBuilder(40);
            for (int i = 0; i < result.Length; i++)
            {
                strbul.Append(result[i].ToString("x2"));
            }
            return strbul.ToString();
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="subject">邮件主题</param>
        /// <param name="msg">邮件内容</param>
        /// <param name="filePath">附件地址，如果不添加附件传null或""</param>
        /// <param name="senderEmail">发送人邮箱地址</param>
        /// <param name="senderPwd">发送人邮箱密码</param>
        /// <param name="recipientEmail">接收人邮箱</param>
        public static void SendMail(string subject, string msg, string filePath, string senderEmail, string senderPwd, params string[] recipientEmail)
        {
            if (!CheckIsNotEmptyOrNull(subject, msg, senderEmail, senderPwd) || recipientEmail == null || recipientEmail.Length == 0)
            {
                throw new Exception("输入信息无效");
            }
            try
            {

                string[] sendFromUser = senderEmail.Split('@');

                //构造一个Email的Message对象
                MailMessage message = new MailMessage();

                //确定smtp服务器地址。实例化一个Smtp客户端
                System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient("smtp." + sendFromUser[1]);

                //构造发件人地址对象
                message.From = new MailAddress(senderEmail, sendFromUser[0], Encoding.UTF8);

                //构造收件人地址对象
                foreach (string userName in recipientEmail)
                {
                    message.To.Add(new MailAddress(userName, userName.Split('@')[0], Encoding.UTF8));
                }

                if (!string.IsNullOrEmpty(filePath))
                {
                    Attachment attach = new Attachment(filePath);
                    //得到文件的信息
                    ContentDisposition disposition = attach.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(filePath);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(filePath);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(filePath);
                    //向邮件添加附件
                    message.Attachments.Add(attach);
                }

                //添加邮件主题和内容
                message.Subject = subject;
                message.SubjectEncoding = Encoding.UTF8;
                message.Body = msg;
                message.BodyEncoding = Encoding.UTF8;

                //设置邮件的信息
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                message.IsBodyHtml = false;

                //如果服务器支持安全连接，则将安全连接设为true。
                //gmail,qq支持，163不支持
                switch (sendFromUser[1])
                {
                    case "gmail.com":
                    case "qq.com":
                        client.EnableSsl = true;
                        break;
                    default:
                        client.EnableSsl = false;
                        break;
                }

                //设置用户名和密码。
                client.UseDefaultCredentials = false;
                //用户登陆信息
                NetworkCredential myCredentials = new NetworkCredential(senderEmail, senderPwd);
                client.Credentials = myCredentials;
                //发送邮件
                client.Send(message);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// 验证所有传入字符串不能为空或null
        /// </summary>
        /// <param name="ps">参数列表</param>
        /// <returns>都不为空或null返回true，否则返回false</returns>
        public static bool CheckIsNotEmptyOrNull(params string[] ps)
        {
            if (ps != null)
            {
                foreach (string item in ps)
                {
                    if (string.IsNullOrEmpty(item)) return false;
                }
                return true;
            }
            return false;
        }

        public static string getStr(bool b, int n)//b：是否有复杂字符，n：生成的字符串长度
        {
            string str = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            if (b == true)
            {
                str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";//复杂字符
            }
            StringBuilder SB = new StringBuilder();
            Random rd = new Random();
            for (int i = 0; i < n; i++)
            {
                SB.Append(str.Substring(rd.Next(0, str.Length), 1));
            }
            return SB.ToString();

        }
    }
}
