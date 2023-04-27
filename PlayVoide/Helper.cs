using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PlayVideo
{
    public class Helper
    {
        public static string IP = "121.4.64.98";
        public static int Port = 3306;
        public static MySqlConnectionStringBuilder builder;
        public static MySqlConnection connection;
        public static bool IsInit = false;
        public static readonly object locker = new object();
        private List<string> users = new List<string>();
        private List<bool> cw = new List<bool>();
        public static List<string> dz = new List<string>();
        public Helper()
        {
            lock (locker)
            {
                if (!IsInit)
                {
                    builder = new MySqlConnectionStringBuilder();
                    //用户名
                    builder.UserID = "zmz";
                    //密码
                    builder.Password = "KJMAZLmiyW5RXYh7";
                    //端口
                    builder.Port = (uint)Port;
                    //服务器地址
                    builder.Server = IP;
                    //连接时的数据库
                    builder.Database = "test123";
                    //定义与数据连接的链接
                    connection = new MySqlConnection(builder.ConnectionString);
                    IsInit = true;
                }
            }
        }
        public void Add(string username, string password, string e_mail)
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
                if (reader2.GetString(0).Contains("https://tucdn.wpon.cn/api-girl/videos/"))
                {
                    dz.Add(reader2.GetString(0).Split(new string[] { "https://tucdn.wpon.cn/api-girl/videos/" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                }
                else
                {
                    //MessageBox.Show("路径错误");
                }
            }
            connection.Close();

        }
        public int Insert(int userid, string url)
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
        public bool Dianz(int id, string url)
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
        public bool ChaChong(string url,int id)
        {
            connection.Open();
            string sqlsel = $"SELECT COUNT(*) FROM `urls` WHERE url='{url}' && voide_Id={id}";
            var cmd = new MySqlCommand(sqlsel, connection);
            var reader = cmd.ExecuteScalar();
            var iss = int.Parse(reader.ToString());
            connection.Close();
            return iss > 0;
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
                message.IsBodyHtml = true;

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
                Console.WriteLine("发送成功");
            }
            catch (Exception ex)
            {
                Console.WriteLine("发送失败");
                throw (ex);
            }
            /// <summary>
            /// 验证所有传入字符串不能为空或null
            /// </summary>
            /// <param name="ps">参数列表</param>
            /// <returns>都不为空或null返回true，否则返回false</returns>
            bool CheckIsNotEmptyOrNull(params string[] ps)
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
        internal static string e_mail(string sjstr)
        {
            return "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional //EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">\r\n<!--[if IE]><html xmlns=\"http://www.w3.org/1999/xhtml\" class=\"ie\"><![endif]--><!--[if !IE]><!-->\r\n<html style=\"margin: 0;padding: 0;\" xmlns=\"http://www.w3.org/1999/xhtml\"><!--<![endif]--><head>\r\n    <meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">\r\n    <title></title>\r\n    <!--[if !mso]><!-->\r\n    <meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge\"><!--<![endif]-->\r\n    <meta name=\"viewport\" content=\"width=device-width\">\r\n    <style type=\"text/css\">\r\n        #context {\r\n            width: 600px;\r\n            margin: 30px auto;\r\n            box-shadow: 0 0 30px #636363;\r\n        }\r\n        #body {\r\n            /*height:947px;*/\r\n            /*background-color: #dddddd;*/\r\n        }\r\n        @media only screen and (min-width: 620px) {\r\n            .wrapper {\r\n                min-width: 600px !important\r\n            }\r\n\r\n                .wrapper h1 {\r\n                }\r\n\r\n                .wrapper h1 {\r\n                    font-size: 34px !important;\r\n                    line-height: 43px !important\r\n                }\r\n\r\n                .wrapper h2 {\r\n                }\r\n\r\n                .wrapper h2 {\r\n                    font-size: 24px !important;\r\n                    line-height: 32px !important\r\n                }\r\n\r\n                .wrapper h3 {\r\n                }\r\n\r\n                .wrapper h3 {\r\n                    font-size: 18px !important;\r\n                    line-height: 26px !important\r\n                }\r\n\r\n            .column {\r\n            }\r\n\r\n            .wrapper .size-8 {\r\n                font-size: 8px !important;\r\n                line-height: 14px !important\r\n            }\r\n\r\n            .wrapper .size-9 {\r\n                font-size: 9px !important;\r\n                line-height: 16px !important\r\n            }\r\n\r\n            .wrapper .size-10 {\r\n                font-size: 10px !important;\r\n                line-height: 18px !important\r\n            }\r\n\r\n            .wrapper .size-11 {\r\n                font-size: 11px !important;\r\n                line-height: 19px !important\r\n            }\r\n\r\n            .wrapper .size-12 {\r\n                font-size: 12px !important;\r\n                line-height: 19px !important\r\n            }\r\n\r\n            .wrapper .size-13 {\r\n                font-size: 13px !important;\r\n                line-height: 21px !important\r\n            }\r\n\r\n            .wrapper .size-14 {\r\n                font-size: 14px !important;\r\n                line-height: 21px !important\r\n            }\r\n\r\n            .wrapper .size-15 {\r\n                font-size: 15px !important;\r\n                line-height: 23px !important\r\n            }\r\n\r\n            .wrapper .size-16 {\r\n                font-size: 16px !important;\r\n                line-height: 24px !important\r\n            }\r\n\r\n            .wrapper .size-17 {\r\n                font-size: 17px !important;\r\n                line-height: 26px !important\r\n            }\r\n\r\n            .wrapper .size-18 {\r\n                font-size: 18px !important;\r\n                line-height: 26px !important\r\n            }\r\n\r\n            .wrapper .size-20 {\r\n                font-size: 20px !important;\r\n                line-height: 28px !important\r\n            }\r\n\r\n            .wrapper .size-22 {\r\n                font-size: 22px !important;\r\n                line-height: 31px !important\r\n            }\r\n\r\n            .wrapper .size-24 {\r\n                font-size: 24px !important;\r\n                line-height: 32px !important\r\n            }\r\n\r\n            .wrapper .size-26 {\r\n                font-size: 26px !important;\r\n                line-height: 34px !important\r\n            }\r\n\r\n            .wrapper .size-28 {\r\n                font-size: 28px !important;\r\n                line-height: 36px !important\r\n            }\r\n\r\n            .wrapper .size-30 {\r\n                font-size: 30px !important;\r\n                line-height: 38px !important\r\n            }\r\n\r\n            .wrapper .size-32 {\r\n                font-size: 32px !important;\r\n                line-height: 40px !important\r\n            }\r\n\r\n            .wrapper .size-34 {\r\n                font-size: 34px !important;\r\n                line-height: 43px !important\r\n            }\r\n\r\n            .wrapper .size-36 {\r\n                font-size: 36px !important;\r\n                line-height: 43px !important\r\n            }\r\n\r\n            .wrapper\r\n            .size-40 {\r\n                font-size: 40px !important;\r\n                line-height: 47px !important\r\n            }\r\n\r\n            .wrapper .size-44 {\r\n                font-size: 44px !important;\r\n                line-height: 50px !important\r\n            }\r\n\r\n            .wrapper .size-48 {\r\n                font-size: 48px !important;\r\n                line-height: 54px !important\r\n            }\r\n\r\n            .wrapper .size-56 {\r\n                font-size: 56px !important;\r\n                line-height: 60px !important\r\n            }\r\n\r\n            .wrapper .size-64 {\r\n                font-size: 64px !important;\r\n                line-height: 68px !important\r\n            }\r\n\r\n            .wrapper .size-72 {\r\n                font-size: 72px !important;\r\n                line-height: 76px !important\r\n            }\r\n\r\n            .wrapper .size-80 {\r\n                font-size: 80px !important;\r\n                line-height: 84px !important\r\n            }\r\n\r\n            .wrapper .size-96 {\r\n                font-size: 96px !important;\r\n                line-height: 100px !important\r\n            }\r\n\r\n            .wrapper .size-112 {\r\n                font-size: 112px !important;\r\n                line-height: 116px !important\r\n            }\r\n\r\n            .wrapper .size-128 {\r\n                font-size: 128px !important;\r\n                line-height: 132px !important\r\n            }\r\n\r\n            .wrapper .size-144 {\r\n                font-size: 144px !important;\r\n                line-height: 148px !important\r\n            }\r\n        }\r\n    </style>\r\n    <meta name=\"x-apple-disable-message-reformatting\">\r\n    <style type=\"text/css\">\r\n                .main, .mso {\r\n                    margin: 0;\r\n                    padding: 0;\r\n                }\r\n\r\n                table {\r\n                    border-collapse: collapse;\r\n                    table-layout: fixed;\r\n                }\r\n\r\n                * {\r\n                    line-height: inherit;\r\n                }\r\n\r\n                [x-apple-data-detectors] {\r\n                    color: inherit !important;\r\n                    text-decoration: none !important;\r\n                }\r\n\r\n                .wrapper .footer__share-button a:hover,\r\n                .wrapper .footer__share-button a:focus {\r\n                    color: #ffffff !important;\r\n                }\r\n\r\n                .wrapper .footer__share-button a.icon-white:hover,\r\n                .wrapper .footer__share-button a.icon-white:focus {\r\n                    color: #ffffff !important;\r\n                }\r\n\r\n                .wrapper .footer__share-button a.icon-black:hover,\r\n                .wrapper .footer__share-button a.icon-black:focus {\r\n                    color: #000000 !important;\r\n                }\r\n\r\n                .btn a:hover,\r\n                .btn a:focus,\r\n                .footer__share-button a:hover,\r\n                .footer__share-button a:focus,\r\n                .email-footer__links a:hover,\r\n                .email-footer__links a:focus {\r\n                    opacity: 0.8;\r\n                }\r\n\r\n                .preheader,\r\n                .header,\r\n                .layout,\r\n                .column {\r\n                    transition: width 0.25s ease-in-out, max-width 0.25s ease-in-out;\r\n                }\r\n\r\n                    .preheader td {\r\n                        padding-bottom: 8px;\r\n                    }\r\n\r\n                .layout,\r\n                div.header {\r\n                    max-width: 400px !important;\r\n                    -fallback-width: 95% !important;\r\n                    width: calc(100% - 20px) !important;\r\n                }\r\n\r\n                div.preheader {\r\n                    max-width: 360px !important;\r\n                    -fallback-width: 90% !important;\r\n                    width: calc(100% - 60px) !important;\r\n                }\r\n\r\n                .snippet,\r\n                .webversion {\r\n                    Float: none !important;\r\n                }\r\n\r\n                .stack .column {\r\n                    max-width: 400px !important;\r\n                    width: 100% !important;\r\n                }\r\n\r\n                .fixed-width.has-border {\r\n                    max-width: 402px !important;\r\n                }\r\n\r\n                    .fixed-width.has-border .layout__inner {\r\n                        box-sizing: border-box;\r\n                    }\r\n\r\n                .snippet,\r\n                .webversion {\r\n                    width: 50% !important;\r\n                }\r\n\r\n                .ie .btn {\r\n                    width: 100%;\r\n                }\r\n\r\n                .ie .stack .column,\r\n                .ie .stack .gutter {\r\n                    display: table-cell;\r\n                    float: none !important;\r\n                }\r\n\r\n                .ie div.preheader,\r\n                .ie .email-footer {\r\n                    max-width: 560px !important;\r\n                    width: 560px !important;\r\n                }\r\n\r\n                .ie .snippet,\r\n                .ie .webversion {\r\n                    width: 280px !important;\r\n                }\r\n\r\n                .ie div.header,\r\n                .ie .layout {\r\n                    max-width: 600px !important;\r\n                    width: 600px !important;\r\n                }\r\n\r\n                .ie .two-col .column {\r\n                    max-width: 300px !important;\r\n                    width: 300px !important;\r\n                }\r\n\r\n                .ie .three-col .column,\r\n                .ie .narrow {\r\n                    max-width: 200px !important;\r\n                    width: 200px !important;\r\n                }\r\n\r\n                .ie .wide {\r\n                    width: 400px !important;\r\n                }\r\n\r\n                .ie .stack.fixed-width.has-border,\r\n                .ie .stack.has-gutter.has-border {\r\n                    max-width: 602px !important;\r\n                    width: 602px !important;\r\n                }\r\n\r\n                .ie .stack.two-col.has-gutter .column {\r\n                    max-width: 290px !important;\r\n                    width: 290px !important;\r\n                }\r\n\r\n                .ie .stack.three-col.has-gutter .column,\r\n                .ie .stack.has-gutter .narrow {\r\n                    max-width: 188px !important;\r\n                    width: 188px !important;\r\n                }\r\n\r\n                .ie .stack.has-gutter .wide {\r\n                    max-width: 394px !important;\r\n                    width: 394px !important;\r\n                }\r\n\r\n                .ie .stack.two-col.has-gutter.has-border .column {\r\n                    max-width: 292px !important;\r\n                    width: 292px !important;\r\n                }\r\n\r\n                .ie .stack.three-col.has-gutter.has-border .column,\r\n                .ie .stack.has-gutter.has-border .narrow {\r\n                    max-width: 190px !important;\r\n                    width: 190px !important;\r\n                }\r\n\r\n                .ie .stack.has-gutter.has-border .wide {\r\n                    max-width: 396px !important;\r\n                    width: 396px !important;\r\n                }\r\n\r\n                .ie .fixed-width .layout__inner {\r\n                    border-left: 0 none white !important;\r\n                    border-right: 0 none white !important;\r\n                }\r\n\r\n                .ie .layout__edges {\r\n                    display: none;\r\n                }\r\n\r\n                .mso .layout__edges {\r\n                    font-size: 0;\r\n                }\r\n\r\n                .layout-fixed-width,\r\n                .mso .layout-full-width {\r\n                    background-color: #ffffff;\r\n                }\r\n\r\n                @media only screen and (min-width: 620px) {\r\n                    .column,\r\n                    .gutter {\r\n                        display: table-cell;\r\n                        Float: none !important;\r\n                        vertical-align: top;\r\n                    }\r\n\r\n                    div.preheader,\r\n                    .email-footer {\r\n                        max-width: 560px !important;\r\n                        width: 560px !important;\r\n                    }\r\n\r\n                    .snippet,\r\n                    .webversion {\r\n                        width: 280px !important;\r\n                    }\r\n\r\n                    div.header,\r\n                    .layout,\r\n                    .one-col .column {\r\n                        max-width: 600px !important;\r\n                        width: 600px !important;\r\n                    }\r\n\r\n                    .fixed-width.has-border,\r\n                    .fixed-width.x_has-border,\r\n                    .has-gutter.has-border,\r\n                    .has-gutter.x_has-border {\r\n                        max-width: 602px !important;\r\n                        width: 602px !important;\r\n                    }\r\n\r\n                    .two-col .column {\r\n                        max-width: 300px !important;\r\n                        width: 300px !important;\r\n                    }\r\n\r\n                    .three-col .column,\r\n                    .column.narrow,\r\n                    .column.x_narrow {\r\n                        max-width: 200px !important;\r\n                        width: 200px !important;\r\n                    }\r\n\r\n                    .column.wide,\r\n                    .column.x_wide {\r\n                        width: 400px !important;\r\n                    }\r\n\r\n                    .two-col.has-gutter .column,\r\n                    .two-col.x_has-gutter .column {\r\n                        max-width: 290px !important;\r\n                        width: 290px !important;\r\n                    }\r\n\r\n                    .three-col.has-gutter .column,\r\n                    .three-col.x_has-gutter .column,\r\n                    .has-gutter .narrow {\r\n                        max-width: 188px !important;\r\n                        width: 188px !important;\r\n                    }\r\n\r\n                    .has-gutter .wide {\r\n                        max-width: 394px !important;\r\n                        width: 394px !important;\r\n                    }\r\n\r\n                    .two-col.has-gutter.has-border .column,\r\n                    .two-col.x_has-gutter.x_has-border .column {\r\n                        max-width: 292px !important;\r\n                        width: 292px !important;\r\n                    }\r\n\r\n                    .three-col.has-gutter.has-border .column,\r\n                    .three-col.x_has-gutter.x_has-border .column,\r\n                    .has-gutter.has-border .narrow,\r\n                    .has-gutter.x_has-border .narrow {\r\n                        max-width: 190px !important;\r\n                        width: 190px !important;\r\n                    }\r\n\r\n                    .has-gutter.has-border .wide,\r\n                    .has-gutter.x_has-border .wide {\r\n                        max-width: 396px !important;\r\n                        width: 396px !important;\r\n                    }\r\n                }\r\n\r\n                @supports (display: flex) {\r\n                    @media only screen and (min-width: 620px) {\r\n                        .fixed-width.has-border .layout__inner {\r\n                            display: flex !important;\r\n                        }\r\n                    }\r\n                }\r\n                /***\r\n        * Mobile Styles\r\n        *\r\n        * 1. Overriding inline styles\r\n        */\r\n                @media(max-width: 619px) {\r\n                    .email-flexible-footer .left-aligned-footer .column,\r\n                    .email-flexible-footer .center-aligned-footer,\r\n                    .email-flexible-footer .right-aligned-footer .column {\r\n                        max-width: 100% !important; /* [1] */\r\n                        text-align: center !important; /* [1] */\r\n                        width: 100% !important; /* [1] */\r\n                    }\r\n\r\n                    .flexible-footer-logo {\r\n                        margin-left: 0px !important; /* [1] */\r\n                        margin-right: 0px !important; /* [1] */\r\n                    }\r\n\r\n                    .email-flexible-footer .left-aligned-footer .flexible-footer__share-button__container,\r\n                    .email-flexible-footer .center-aligned-footer .flexible-footer__share-button__container,\r\n                    .email-flexible-footer .right-aligned-footer .flexible-footer__share-button__container {\r\n                        display: inline-block;\r\n                        margin-left: 5px !important; /* [1] */\r\n                        margin-right: 5px !important; /* [1] */\r\n                    }\r\n\r\n                    .email-flexible-footer__additionalinfo--center {\r\n                        text-align: center !important; /* [1] */\r\n                    }\r\n\r\n                    .email-flexible-footer .left-aligned-footer table,\r\n                    .email-flexible-footer .center-aligned-footer table,\r\n                    .email-flexible-footer .right-aligned-footer table {\r\n                        display: table !important; /* [1] */\r\n                        width: 100% !important; /* [1] */\r\n                    }\r\n\r\n                    .email-flexible-footer .footer__share-button,\r\n                    .email-flexible-footer .email-footer__additional-info {\r\n                        margin-left: 20px;\r\n                        margin-right: 20px;\r\n                    }\r\n                }\r\n\r\n                @media only screen and (-webkit-min-device-pixel-ratio: 2), only screen and (min--moz-device-pixel-ratio: 2), only screen and (-o-min-device-pixel-ratio: 2/1), only screen and (min-device-pixel-ratio: 2), only screen and (min-resolution: 192dpi), only screen and (min-resolution: 2dppx) {\r\n                    .fblike {\r\n                        background-image: url(https://i7.createsend1.com/static/eb/master/13-the-blueprint-3/images/fblike@2x.png) !important;\r\n                    }\r\n\r\n                    .tweet {\r\n                        background-image: url(https://i8.createsend1.com/static/eb/master/13-the-blueprint-3/images/tweet@2x.png) !important;\r\n                    }\r\n\r\n                    .linkedinshare {\r\n                        background-image: url(https://i9.createsend1.com/static/eb/master/13-the-blueprint-3/images/lishare@2x.png) !important;\r\n                    }\r\n\r\n                    .forwardtoafriend {\r\n                        background-image: url(https://i10.createsend1.com/static/eb/master/13-the-blueprint-3/images/forward@2x.png) !important;\r\n                    }\r\n                }\r\n\r\n                @media (max-width: 321px) {\r\n                    .fixed-width.has-border .layout__inner {\r\n                        border-width: 1px 0 !important;\r\n                    }\r\n\r\n                    .layout,\r\n                    .stack .column {\r\n                        min-width: 320px !important;\r\n                        width: 320px !important;\r\n                    }\r\n\r\n                    .border {\r\n                        display: none;\r\n                    }\r\n\r\n                    .has-gutter .border {\r\n                        display: table-cell;\r\n                    }\r\n                }\r\n\r\n                .mso div {\r\n                    border: 0 none white !important;\r\n                }\r\n\r\n                .mso .w560 .divider {\r\n                    Margin-left: 260px !important;\r\n                    Margin-right: 260px !important;\r\n                }\r\n\r\n                .mso .w360 .divider {\r\n                    Margin-left: 160px !important;\r\n                    Margin-right: 160px !important;\r\n                }\r\n\r\n                .mso .w260 .divider {\r\n                    Margin-left: 110px !important;\r\n                    Margin-right: 110px !important;\r\n                }\r\n\r\n                .mso .w160 .divider {\r\n                    Margin-left: 60px !important;\r\n                    Margin-right: 60px !important;\r\n                }\r\n\r\n                .mso .w354 .divider {\r\n                    Margin-left: 157px !important;\r\n                    Margin-right: 157px !important;\r\n                }\r\n\r\n                .mso .w250 .divider {\r\n                    Margin-left: 105px !important;\r\n                    Margin-right: 105px !important;\r\n                }\r\n\r\n                .mso .w148 .divider {\r\n                    Margin-left: 54px !important;\r\n                    Margin-right: 54px !important;\r\n                }\r\n\r\n                .mso .size-8,\r\n                .ie .size-8 {\r\n                    font-size: 8px !important;\r\n                    line-height: 14px !important;\r\n                }\r\n\r\n                .mso .size-9,\r\n                .ie .size-9 {\r\n                    font-size: 9px !important;\r\n                    line-height: 16px !important;\r\n                }\r\n\r\n                .mso .size-10,\r\n                .ie .size-10 {\r\n                    font-size: 10px !important;\r\n                    line-height: 18px !important;\r\n                }\r\n\r\n                .mso .size-11,\r\n                .ie .size-11 {\r\n                    font-size: 11px !important;\r\n                    line-height: 19px !important;\r\n                }\r\n\r\n                .mso .size-12,\r\n                .ie .size-12 {\r\n                    font-size: 12px !important;\r\n                    line-height: 19px !important;\r\n                }\r\n\r\n                .mso .size-13,\r\n                .ie .size-13 {\r\n                    font-size: 13px !important;\r\n                    line-height: 21px !important;\r\n                }\r\n\r\n                .mso .size-14,\r\n                .ie .size-14 {\r\n                    font-size: 14px !important;\r\n                    line-height: 21px !important;\r\n                }\r\n\r\n                .mso .size-15,\r\n                .ie .size-15 {\r\n                    font-size: 15px !important;\r\n                    line-height: 23px !important;\r\n                }\r\n\r\n                .mso .size-16,\r\n                .ie .size-16 {\r\n                    font-size: 16px !important;\r\n                    line-height: 24px !important;\r\n                }\r\n\r\n                .mso .size-17,\r\n                .ie .size-17 {\r\n                    font-size: 17px !important;\r\n                    line-height: 26px !important;\r\n                }\r\n\r\n                .mso .size-18,\r\n                .ie .size-18 {\r\n                    font-size: 18px !important;\r\n                    line-height: 26px !important;\r\n                }\r\n\r\n                .mso .size-20,\r\n                .ie .size-20 {\r\n                    font-size: 20px !important;\r\n                    line-height: 28px !important;\r\n                }\r\n\r\n                .mso .size-22,\r\n                .ie .size-22 {\r\n                    font-size: 22px !important;\r\n                    line-height: 31px !important;\r\n                }\r\n\r\n                .mso .size-24,\r\n                .ie .size-24 {\r\n                    font-size: 24px !important;\r\n                    line-height: 32px !important;\r\n                }\r\n\r\n                .mso .size-26,\r\n                .ie .size-26 {\r\n                    font-size: 26px !important;\r\n                    line-height: 34px !important;\r\n                }\r\n\r\n                .mso .size-28,\r\n                .ie .size-28 {\r\n                    font-size: 28px !important;\r\n                    line-height: 36px !important;\r\n                }\r\n\r\n                .mso .size-30,\r\n                .ie .size-30 {\r\n                    font-size: 30px !important;\r\n                    line-height: 38px !important;\r\n                }\r\n\r\n                .mso .size-32,\r\n                .ie .size-32 {\r\n                    font-size: 32px !important;\r\n                    line-height: 40px !important;\r\n                }\r\n\r\n                .mso .size-34,\r\n                .ie .size-34 {\r\n                    font-size: 34px !important;\r\n                    line-height: 43px !important;\r\n                }\r\n\r\n                .mso .size-36,\r\n                .ie .size-36 {\r\n                    font-size: 36px !important;\r\n                    line-height: 43px !important;\r\n                }\r\n\r\n                .mso .size-40,\r\n                .ie .size-40 {\r\n                    font-size: 40px !important;\r\n                    line-height: 47px !important;\r\n                }\r\n\r\n                .mso .size-44,\r\n                .ie .size-44 {\r\n                    font-size: 44px !important;\r\n                    line-height: 50px !important;\r\n                }\r\n\r\n                .mso .size-48,\r\n                .ie .size-48 {\r\n                    font-size: 48px !important;\r\n                    line-height: 54px !important;\r\n                }\r\n\r\n                .mso .size-56,\r\n                .ie .size-56 {\r\n                    font-size: 56px !important;\r\n                    line-height: 60px !important;\r\n                }\r\n\r\n                .mso .size-64,\r\n                .ie .size-64 {\r\n                    font-size: 64px !important;\r\n                    line-height: 68px !important;\r\n                }\r\n\r\n                .mso .size-72,\r\n                .ie .size-72 {\r\n                    font-size: 72px !important;\r\n                    line-height: 76px !important;\r\n                }\r\n\r\n                .mso .size-80,\r\n                .ie .size-80 {\r\n                    font-size: 80px !important;\r\n                    line-height: 84px !important;\r\n                }\r\n\r\n                .mso .size-96,\r\n                .ie .size-96 {\r\n                    font-size: 96px !important;\r\n                    line-height: 100px !important;\r\n                }\r\n\r\n                .mso .size-112,\r\n                .ie .size-112 {\r\n                    font-size: 112px !important;\r\n                    line-height: 116px !important;\r\n                }\r\n\r\n                .mso .size-128,\r\n                .ie .size-128 {\r\n                    font-size: 128px !important;\r\n                    line-height: 132px !important;\r\n                }\r\n\r\n                .mso .size-144,\r\n                .ie .size-144 {\r\n                    font-size: 144px !important;\r\n                    line-height: 148px !important;\r\n                }\r\n                /***\r\n        * Online store block styles\r\n        *\r\n        * 1. maintaining right and left margins in outlook\r\n        * 2. respecting line-height for tds in outlook\r\n        */\r\n                .mso .cmctbl table td, .mso .cmctbl table th {\r\n                    Margin-left: 20px !important; /* [1] */\r\n                    Margin-right: 20px !important; /* [1] */\r\n                }\r\n\r\n                .cmctbl--inline table {\r\n                    border-collapse: collapse;\r\n                }\r\n\r\n                .mso .cmctbl--inline table, .mso .cmctbl table {\r\n                    mso-table-lspace: 0pt;\r\n                    mso-table-rspace: 0pt;\r\n                    mso-line-height-rule: exactly; /* [2] */\r\n                }\r\n    </style>\r\n\r\n    <!--[if !mso]><!-->\r\n    <style type=\"text/css\">\r\n        @import url(https://fonts.googleapis.com/css?family=Montserrat:400,700,400italic,700italic);\r\n    </style>\r\n    <link href=\"https://fonts.googleapis.com/css?family=Montserrat:400,700,400italic,700italic\" rel=\"stylesheet\" type=\"text/css\"><!--<![endif]-->\r\n    <style type=\"text/css\">\r\n        body{\r\n            /*background-color:#a7a7a7*/\r\n        }\r\n        .main, .mso {\r\n            \r\n        }\r\n\r\n        .logo a:hover, .logo a:focus {\r\n            color: #1e2e3b !important\r\n        }\r\n\r\n        .footer-logo a:hover, .footer-logo a:focus {\r\n            color: #372d1b !important\r\n        }\r\n\r\n        .mso .layout-has-border {\r\n            border-top: 1px solid #ccc;\r\n            border-bottom: 1px solid #ccc\r\n        }\r\n\r\n        .mso .layout-has-bottom-border {\r\n            border-bottom: 1px solid #ccc\r\n        }\r\n\r\n        .mso .border, .ie .border {\r\n            background-color: #ccc\r\n        }\r\n\r\n        .mso h1, .ie h1 {\r\n        }\r\n\r\n        .mso h1, .ie h1 {\r\n            font-size: 34px !important;\r\n            line-height: 43px !important\r\n        }\r\n\r\n        .mso h2, .ie h2 {\r\n        }\r\n\r\n        .mso h2, .ie h2 {\r\n            font-size: 24px !important;\r\n            line-height: 32px !important\r\n        }\r\n\r\n        .mso h3, .ie h3 {\r\n        }\r\n\r\n        .mso h3, .ie h3 {\r\n            font-size: 18px !important;\r\n            line-height: 26px !important\r\n        }\r\n\r\n        .mso .layout__inner, .ie .layout__inner {\r\n        }\r\n\r\n        .mso .footer__share-button p {\r\n        }\r\n\r\n        .mso .footer__share-button p {\r\n            font-family: Montserrat,DejaVu Sans,Verdana,sans-serif\r\n        }\r\n    </style>\r\n    <meta name=\"robots\" content=\"noindex,nofollow\">\r\n    <meta property=\"og:title\" content=\"My First Campaign\">\r\n</head>\r\n<!--[if mso]>\r\n  <body class=\"mso\">\r\n<![endif]-->\r\n<!--[if !mso]><!-->\r\n<body class=\"main full-padding\" style=\"margin: 0;padding: 0;-webkit-text-size-adjust: 100%;\">\r\n    <!--<![endif]-->\r\n    <div id=\"body\">\r\n        <div id=\"context\">\r\n            <table class=\"wrapper\" style=\"border-collapse: collapse;table-layout: fixed;min-width: 320px;width: 100%;background-color: #fff;\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\">\r\n                <tbody>\r\n                    <tr>\r\n                        <td>\r\n                            <div role=\"banner\">\r\n                                <div class=\"preheader\" style=\"Margin: 0 auto;max-width: 560px;min-width: 280px; width: 280px;width: calc(28000% - 167440px);\">\r\n                                    <div style=\"border-collapse: collapse;display: table;width: 100%;\">\r\n                                        <!--[if (mso)|(IE)]><table align=\"center\" class=\"preheader\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\"><tr><td style=\"width: 280px\" valign=\"top\"><![endif]-->\r\n                                        <!--<div class=\"snippet\" style=\"display: table-cell;Float: left;font-size: 12px;line-height: 19px;max-width: 280px;min-width: 140px; width: 140px;width: calc(14000% - 78120px);padding: 10px 0 5px 0;color: #717a8a;font-family: Montserrat,DejaVu Sans,Verdana,sans-serif;\">\r\n\r\n                                    </div>-->\r\n                                        <!--[if (mso)|(IE)]></td><td style=\"width: 280px\" valign=\"top\"><![endif]-->\r\n                                        <!--[if (mso)|(IE)]></td></tr></table><![endif]-->\r\n                                    </div>\r\n                                </div>\r\n\r\n                            </div>\r\n                            <div>\r\n                                <div class=\"layout one-col fixed-width stack\" style=\"Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 167400px);overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;\">\r\n                                    <div class=\"layout__inner\" style=\"border-collapse: collapse;display: table;width: 100%;background-color: #000000;\">\r\n                                        <!--[if (mso)|(IE)]><table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\"><tr class=\"layout-fixed-width\" style=\"background-color: #000000;\"><td style=\"width: 600px\" class=\"w560\"><![endif]-->\r\n                                        <div class=\"column\" style=\"text-align: left;color: #525252;font-size: 16px;line-height: 24px;font-family: Montserrat,DejaVu Sans,Verdana,sans-serif;\">\r\n\r\n                                            <div style=\"Margin-left: 20px;Margin-right: 20px;Margin-top: 24px;\">\r\n                                                <div style=\"mso-line-height-rule: exactly;line-height: 29px;font-size: 1px;\">&nbsp;</div>\r\n                                            </div>\r\n\r\n                                            <div style=\"Margin-left: 20px;Margin-right: 20px;\">\r\n                                                <div style=\"mso-line-height-rule: exactly;mso-text-raise: 11px;vertical-align: middle;\">\r\n                                                    <h1 style=\"Margin-top: 0;Margin-bottom: 20px;font-style: normal;font-weight: normal;color: #f5f6f8;font-size: 30px;line-height: 38px;text-align: center;\">学习资料-v2.0.1.0</h1>\r\n                                                </div>\r\n                                            </div>\r\n\r\n                                            <div style=\"font-size: 12px;font-style: normal;font-weight: normal;line-height: 19px;\" align=\"center\">\r\n                                                \r\n                                            </div>\r\n\r\n                                        </div>\r\n                                        <!--[if (mso)|(IE)]></td></tr></table><![endif]-->\r\n                                    </div>\r\n                                </div>\r\n\r\n                                <div class=\"layout one-col fixed-width stack\" style=\"Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 167400px);overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;\">\r\n                                    <div class=\"layout__inner\" style=\"border-collapse: collapse;display: table;width: 100%;background-color: #ffffff;\">\r\n                                        <!--[if (mso)|(IE)]><table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\"><tr class=\"layout-fixed-width\" style=\"background-color: #ffffff;\"><td style=\"width: 600px\" class=\"w560\"><![endif]-->\r\n                                        <div class=\"column\" style=\"text-align: left;color: #525252;font-size: 16px;line-height: 24px;font-family: Montserrat,DejaVu Sans,Verdana,sans-serif;\">\r\n\r\n                                            <div style=\"Margin-left: 20px;Margin-right: 20px;Margin-top: 24px;\">\r\n                                                <div style=\"mso-line-height-rule: exactly;mso-text-raise: 11px;vertical-align: middle;\">\r\n                                                    <p style=\"Margin-top: 0;Margin-bottom: 20px;\">欢迎体验新版本学习资料-v2.0.1.0</p>\r\n                                                </div>\r\n                                            </div>\r\n\r\n                                            <div style=\"Margin-left: 20px;Margin-right: 20px;Margin-bottom: 24px;\">\r\n                                                <div style=\"mso-line-height-rule: exactly;mso-text-raise: 11px;vertical-align: middle;\">\r\n                                                    <p style=\"Margin-top: 0;Margin-bottom: 0;\">\r\n                                                        - 你想看的这里都有<br>\r\n                                                        &nbsp;\r\n                                                    </p>\r\n                                                    <p style=\"font-size: 20px;font-weight: bold;\">\r\n                                                        以下为您的验证码<br>\r\n                                                        &nbsp;\r\n                                                    </p>\r\n                                                </div>\r\n                                            </div>\r\n\r\n                                        </div>\r\n                                        <!--[if (mso)|(IE)]></td></tr></table><![endif]-->\r\n                                    </div>\r\n                                </div>\r\n\r\n                                <div class=\"layout one-col fixed-width stack\" style=\"Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 167400px);overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;\">\r\n                                    <div class=\"layout__inner\" style=\"border-collapse: collapse;display: table;width: 100%;background-color: #ffffff;\">\r\n                                        <!--[if (mso)|(IE)]><table align=\"center\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\"><tr class=\"layout-fixed-width\" style=\"background-color: #ffffff;\"><td style=\"width: 600px\" class=\"w560\"><![endif]-->\r\n                                        <div class=\"column\" style=\"text-align: left;color: #525252;font-size: 16px;line-height: 24px;font-family: Montserrat,DejaVu Sans,Verdana,sans-serif;\">\r\n\r\n                                            <div style=\"Margin-left: 20px;Margin-right: 20px;Margin-top: 20px;Margin-bottom: 24px;\">\r\n                                                <div class=\"btn btn--flat btn--large\" style=\"text-align:left;\">\r\n                                                    <!--[if !mso]--><p style=\"border-radius: 4px; display: inline-block; display: block; font-size: 14px; font-weight: bold; line-height: 24px; padding: 12px 24px; text-align: center; text-decoration: none !important; transition: opacity 0.1s ease-in; color: #ffffff !important; background-color: #1063fe; font-family: Montserrat, DejaVu Sans, Verdana, sans-serif; \">" + sjstr + "</p><!--[endif]-->\r\n                                                    <!--[if mso]><p style=\"line-height:0;margin:0;\">&nbsp;</p><v:roundrect xmlns:v=\"urn:schemas-microsoft-com:vml\" href=\"https://github.com/Tr11111/learning-materials/releases/tag/C%23\" style=\"width:128.25pt\" arcsize=\"9%\" fillcolor=\"#1063FE\" stroke=\"f\"><v:textbox style=\"mso-fit-shape-to-text:t\" inset=\"0pt,8.25pt,0pt,8.25pt\"><center style=\"font-size:14px;line-height:24px;color:#FFFFFF;font-family:Montserrat,DejaVu Sans,Verdana,sans-serif;font-weight:bold;mso-line-height-rule:exactly;mso-text-raise:1.5px\">Register to Join</center></v:textbox></v:roundrect><![endif]-->\r\n                                                </div>\r\n                                            </div>\r\n\r\n                                        </div>\r\n                                        <!--[if (mso)|(IE)]></td></tr></table><![endif]-->\r\n                                    </div>\r\n                                </div>\r\n\r\n                                <div style=\"mso-line-height-rule: exactly;line-height: 20px;font-size: 20px;\">&nbsp;</div>\r\n                            </div>\r\n                            <div role=\"contentinfo\">\r\n                                <div style=\"line-height:4px;font-size:4px;\" id=\"footer-top-spacing\">&nbsp;</div><div class=\"layout email-flexible-footer email-footer\" style=\"Margin: 0 auto;max-width: 600px;min-width: 320px; width: 320px;width: calc(28000% - 167400px);overflow-wrap: break-word;word-wrap: break-word;word-break: break-word;\" id=\"footer-content\">\r\n                                    <div class=\"layout__inner center-aligned-footer\" style=\"border-collapse: collapse;display: table;width: 100%;\">\r\n                                        <!--[if (mso)|(IE)]><table style=\"width: 600px;\" align=\"center\" cellpadding=\"0\" cellspacing=\"0\" role=\"presentation\"><![endif]-->\r\n                                        <!--[if (mso)|(IE)]><tr class=\"layout-email-footer\"><![endif]-->\r\n                                        <div class=\"column\" style=\"text-align: center;font-size: 12px;line-height: 19px;color: #717a8a;font-family: Montserrat,DejaVu Sans,Verdana,sans-serif;display: none;\" align=\"center\">\r\n                                            <div class=\"footer-logo emb-logo-margin-box\" style=\"font-size: 26px;line-height: 32px;Margin-top: 10px;Margin-bottom: 20px;color: #7b663d;font-family: Roboto,Tahoma,sans-serif;\" align=\"center\">\r\n                                                <div emb-flexible-footer-logo=\"\" align=\"center\"></div>\r\n                                            </div>\r\n                                        </div>\r\n                                        <!--[if (mso)|(IE)]></tr><![endif]-->\r\n                                        <!--[if (mso)|(IE)]><tr class=\"layout-email-footer\"><![endif]-->\r\n                                        <div class=\"column\" style=\"text-align: center;font-size: 12px;line-height: 19px;color: #717a8a;font-family: Montserrat,DejaVu Sans,Verdana,sans-serif;display: none;\" align=\"center\">\r\n\r\n                                        </div>\r\n                                        <!--[if (mso)|(IE)]></tr><![endif]-->\r\n                                        <!--[if (mso)|(IE)]><tr class=\"layout-email-footer\"><![endif]-->\r\n                                        <table style=\"border-collapse: collapse;table-layout: fixed;width: 100%;\" cellpadding=\"0\" cellspacing=\"0\">\r\n                                            <tbody>\r\n                                                <tr>\r\n                                                    <td>\r\n                                                        <div class=\"column js-footer-additional-info\" style=\"text-align: center;font-size: 12px;line-height: 19px;color: #717a8a;font-family: Montserrat,DejaVu Sans,Verdana,sans-serif;display: inline;width: 100%;\" align=\"center\">\r\n                                                            <div style=\"margin-left: 0;margin-right: 0;Margin-top: 10px;Margin-bottom: 10px;\">\r\n                                                                <div class=\"email-footer__additional-info\" style=\"font-size: 12px;line-height: 19px;margin-bottom: 18px;margin-top: 0px;\">\r\n                                                                    <div bind-to=\"address\"><p class=\"email-flexible-footer__additionalinfo--center\" style=\"Margin-top: 0;Margin-bottom: 0;\">本邮件为自动发送测试邮件</p></div>\r\n                                                                </div>\r\n                                                                <div class=\"email-footer__additional-info\" style=\"font-size: 12px;line-height: 19px;margin-bottom: 18px;margin-top: 0px;\">\r\n                                                                    <div><p class=\"email-flexible-footer__additionalinfo--center\" style=\"Margin-top: 0;Margin-bottom: 0;\">请勿回复</p></div>\r\n                                                                </div>\r\n                                                                <div class=\"email-footer__additional-info\" style=\"font-size: 12px;line-height: 19px;margin-bottom: 15px;\">\r\n                                                                    <div bind-to=\"permission\"><p class=\"email-flexible-footer__additionalinfo--center\" style=\"Margin-top: 0;Margin-bottom: 0;\">这是一封自动生成的电子邮件，请不要回复此邮件</p></div>\r\n                                                                </div>\r\n                                                                \r\n                                                                <div class=\"email-footer__additional-info\" style=\"font-size: 12px;line-height: 19px;margin-bottom: 15px;\">\r\n                                                                    <p><a href=\"https://github.com/Tr11111/learning-materials/releases/tag/C%23\" style=\"text-decoration: auto;\">GitHub 查看详情</a></p>\r\n                                                                </div>\r\n                                                                <!--[if mso]>&nbsp;<![endif]-->\r\n                                                            </div>\r\n                                                        </div>\r\n                                                    </td>\r\n                                                </tr>\r\n                                            </tbody>\r\n                                        </table>\r\n                                        <!--[if (mso)|(IE)]></tr></table><![endif]-->\r\n                                    </div>\r\n                                </div><div style=\"line-height:40px;font-size:40px;\" id=\"footer-bottom-spacing\">&nbsp;</div>\r\n                            </div>\r\n\r\n                        </td>\r\n                    </tr>\r\n                </tbody>\r\n            </table>\r\n        </div>\r\n    </div>\r\n    <style type=\"text/css\">\r\n        @media (max-width:619px) {\r\n            .email-flexible-footer .left-aligned-footer .column, .email-flexible-footer .center-aligned-footer, .email-flexible-footer .right-aligned-footer .column {\r\n                max-width: 100% !important;\r\n                text-align: center !important;\r\n                width: 100% !important\r\n            }\r\n\r\n            .flexible-footer-logo {\r\n                margin-left: 0px !important;\r\n                margin-right: 0px !important\r\n            }\r\n\r\n            .email-flexible-footer .left-aligned-footer .flexible-footer__share-button__container, .email-flexible-footer .center-aligned-footer .flexible-footer__share-button__container, .email-flexible-footer .right-aligned-footer .flexible-footer__share-button__container {\r\n                display: inline-block;\r\n                margin-left: 5px !important;\r\n                margin-right: 5px !important\r\n            }\r\n\r\n            .email-flexible-footer__additionalinfo--center {\r\n                text-align: center !important\r\n            }\r\n\r\n            .email-flexible-footer .left-aligned-footer table, .email-flexible-footer .center-aligned-footer table, .email-flexible-footer .right-aligned-footer\r\n            table {\r\n                display: table !important;\r\n                width: 100% !important\r\n            }\r\n\r\n            .email-flexible-footer .footer__share-button, .email-flexible-footer .email-footer__additional-info {\r\n                margin-left: 20px;\r\n                margin-right: 20px\r\n            }\r\n        }\r\n    </style>\r\n\r\n\r\n</body></html>";
        }
    }
}
