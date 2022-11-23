using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace PlayVideo
{
    public partial class Login : Form
    {
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
                    DBHelper dBHelper = new DBHelper();
                    
                    if (dBHelper.Logins(comboBox1.Text, textBox2.Text))
                    {
                        name = this.comboBox1.Text;
                        Study study = new Study();
                        study.Show();
                        this.Hide();
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
            Study study = new Study();
            study.Show();
            this.Hide();
        }

        public Dictionary<string, User> users =new Dictionary<string, User>();
        private void Login_Load(object sender, EventArgs e)
        {
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
    }
}
