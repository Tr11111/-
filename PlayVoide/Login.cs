using MySql.Data.MySqlClient;
using PlayVideo.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PlayVideo
{
    public partial class Login : Form
    {
        public int adminNum = 0;
        public static string name;
        public Login()
        {
            InitializeComponent();
        }

        private void linkLabel1_MouseEnter(object sender, EventArgs e)
        {
            this.linkLabel1.ForeColor = System.Drawing.Color.Red;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(comboBox1.Text))
                {
                    MessageBox.Show("账户或密码不能为空！");
                    return;
                }
                string username = this.comboBox1.Text.Trim();
                string password = this.textBox2.Text.Trim();

                User user = new User();
                FileStream fs = new FileStream("data.bin", FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                user.Username = username;
                if (this.checkBox1.Checked)       //  如果单击了记住密码的功能
                {   //  在文件中保存密码
                    user.Password = password;
                }
                else
                {   //  不在文件中保存密码
                    user.Password = "";
                }

                //  选在集合中是否存在用户名 
                if (users.ContainsKey(user.Username))
                {
                    users.Remove(user.Username);
                }

                users.Add(user.Username, user);
                //要先将User类先设为可以序列化(即在类的前面加[Serializable])
                bf.Serialize(fs, users);
                //user.Password = this.PassWord.Text;
                fs.Close();


                if (!Regex.IsMatch(comboBox1.Text.ToLower(), "/response|group_concat|cmd|sysdate|xor|declare|db_name|char| and| or|truncate| asc| desc|drop |table|count|from|select|insert|update|delete|union|into|load_file|outfile/") && !Regex.IsMatch(textBox2.Text.ToLower(), "/response|group_concat|cmd|sysdate|xor|declare|db_name|char| and| or|truncate| asc| desc|drop |table|count|from|select|insert|update|delete|union|into|load_file|outfile/"))
                {
                    Helper dBHelper = new Helper();

                    if (dBHelper.Logins(comboBox1.Text, textBox2.Text))
                    {
                        name = this.comboBox1.Text;
                        this.Hide();
                        new Study().Show();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Add add = new Add();
            add.Show();
            this.Hide();
        }

        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel2_MouseUp(object sender, MouseEventArgs e)
        {
            textBox2.Font = new Font("微软雅黑", textBox2.Font.Size, FontStyle.Bold);
            textBox2.PasswordChar = '·';
        }

        private void linkLabel2_MouseDown(object sender, MouseEventArgs e)
        {
            textBox2.Font = new Font("微软雅黑", textBox2.Font.Size, FontStyle.Regular);
            textBox2.PasswordChar = '\0';
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            new Study().Show();
        }

        public Dictionary<string, User> users = new Dictionary<string, User>();
        private void Login_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.Video;
            //  读取配置文件寻找记住的用户名和密码
            FileStream fs = new FileStream("data.bin", FileMode.OpenOrCreate);

            if (fs.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();
                users = bf.Deserialize(fs) as Dictionary<string, User>;
                foreach (User user in users.Values)
                {
                    this.comboBox1.Items.Add(user.Username);
                }

                for (int i = 0; i < users.Count; i++)
                {
                    if (this.comboBox1.Text != "")
                    {
                        if (users.ContainsKey(this.comboBox1.Text))
                        {
                            this.comboBox1.Text = users[this.comboBox1.Text].Password;
                            this.checkBox1.Checked = true;
                        }
                    }
                }
            }
            fs.Close();
            //  用户名默认选中第一个
            if (this.comboBox1.Items.Count > 0)
            {
                this.comboBox1.SelectedIndex = this.comboBox1.Items.Count - 1;
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            //  首先读取记住密码的配置文件
            FileStream fs = new FileStream("data.bin", FileMode.OpenOrCreate);

            if (fs.Length > 0)
            {
                BinaryFormatter bf = new BinaryFormatter();

                users = bf.Deserialize(fs) as Dictionary<string, User>;

                for (int i = 0; i < users.Count; i++)
                {
                    if (this.comboBox1.Text != "")
                    {
                        if (users.ContainsKey(comboBox1.Text) && users[comboBox1.Text].Password != "")
                        {
                            this.textBox2.Text = users[comboBox1.Text].Password;
                            this.checkBox1.Checked = true;
                        }
                        else
                        {
                            this.textBox2.Text = "";
                            this.checkBox1.Checked = false;
                        }
                    }
                }
            }

            fs.Close();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            adminNum = 0;
            var isOk = MessageBox.Show("是否清空全部历史记录？", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (isOk == DialogResult.OK)
            {
                FileStream fs = new FileStream("data.bin", FileMode.Create);
                BinaryFormatter bf = new BinaryFormatter();
                users.Clear();
                bf.Serialize(fs, users);
                fs.Close();
                comboBox1.Items.Clear();
                comboBox1.Text = "";
                textBox2.Text = "";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(IPtext.Text) && !string.IsNullOrWhiteSpace(Porttext.Text))
            { 
                var condition1 = Regex.IsMatch(Porttext.Text, @"^[0-9]+$");
                var condition2 = IPtext.Text.Split('.').Length == 4;
                if (condition1 && condition2)
                {
                    Helper.IP = IPtext.Text;
                    Helper.Port = int.Parse(Porttext.Text);
                    Helper.builder = new MySqlConnectionStringBuilder();
                    //用户名
                    Helper.builder.UserID = "root";
                    //密码
                    Helper.builder.Password = "123456";
                    //端口
                    Helper.builder.Port = (uint)Helper.Port;
                    //服务器地址
                    Helper.builder.Server = Helper.IP;
                    //连接时的数据库
                    Helper.builder.Database = "test";
                    //定义与数据连接的链接
                    Helper.connection = new MySqlConnection(Helper.builder.ConnectionString);
                    MessageBox.Show("更换成功!");
                }
                else
                {
                    MessageBox.Show("格式不正确");
                }
            }
            else
            {
                MessageBox.Show("不能为空");
            }
        }
    }
}
