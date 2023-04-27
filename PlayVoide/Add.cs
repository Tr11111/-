using Google.Protobuf.WellKnownTypes;
using MySql.Data.MySqlClient;
using PlayVideo.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace PlayVideo
{
    public partial class Add : Form
    {
        private static int duration = 60;
        private static bool isfasong = true;
        private static string sjstr;
        
        public Add()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBox1.Text) || string.IsNullOrWhiteSpace(textBox2
                .Text))
                {
                    MessageBox.Show("用户名或密码不能为空");
                }
                else if (textBox2.Text != textBox3.Text)
                {
                    MessageBox.Show("两次密码不一致");
                }
                else if (!Regex.IsMatch(textBox4.Text, "^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$"))
                {
                    MessageBox.Show("邮箱格式错误");
                }
                else if (Regex.IsMatch(textBox1.Text.ToLower(), "/response|group_concat|cmd|sysdate|xor|declare|db_name|char| and| or|truncate| asc| desc|drop |table|count|from|select|insert|update|delete|union|into|load_file|outfile/") || Regex.IsMatch(textBox2.Text.ToLower(), "/response|group_concat|cmd|sysdate|xor|declare|db_name|char| and| or|truncate| asc| desc|drop |table|count|from|select|insert|update|delete|union|into|load_file|outfile/") || Regex.IsMatch(textBox3.Text.ToLower(), "/response|group_concat|cmd|sysdate|xor|declare|db_name|char| and| or|truncate| asc| desc|drop |table|count|from|select|insert|update|delete|union|into|load_file|outfile/") || Regex.IsMatch(textBox4.Text.ToLower(), "/response|group_concat|cmd|sysdate|xor|declare|db_name|char| and| or|truncate| asc| desc|drop |table|count|from|select|insert|update|delete|union|into|load_file|outfile/"))
                {
                    MessageBox.Show("内容有非法字符");
                }
                else if (textBox5.Text == sjstr)
                {
                    Helper dBHelper = new Helper();
                    dBHelper.Add(textBox1.Text, textBox2.Text, textBox4.Text);
                }
                else
                {
                    MessageBox.Show("注册失败");
                }
            }
            catch (Exception )
            {
                MessageBox.Show("注册失败");
            }
            
        }

        private void Add_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void linkLabel1_MouseDown(object sender, MouseEventArgs e)
        {
            textBox2.Font = new Font("微软雅黑", textBox2.Font.Size, FontStyle.Regular);
            textBox2.PasswordChar = '\0';
        }

        private void linkLabel1_MouseUp(object sender, MouseEventArgs e)
        {
            textBox2.Font = new Font("微软雅黑", textBox2.Font.Size, FontStyle.Bold);
            textBox2.PasswordChar = '·';
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            this.Hide();
        }

        private void linkLabel3_MouseDown(object sender, MouseEventArgs e)
        {
            textBox3.Font = new Font("微软雅黑", textBox2.Font.Size, FontStyle.Regular);
            textBox3.PasswordChar = '\0';
        }

        private void linkLabel3_MouseUp(object sender, MouseEventArgs e)
        {
            textBox3.Font = new Font("微软雅黑", textBox2.Font.Size, FontStyle.Bold);
            textBox3.PasswordChar = '·';
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (isfasong)
                {
                    if (Regex.IsMatch(textBox4.Text, "^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$"))
                    {
                        duration = 60;
                        timer1 = new System.Windows.Forms.Timer();
                        timer1.Tick += new EventHandler(count_down);
                        timer1.Interval = 1000;
                        timer1.Start();
                        sjstr = Helper.getStr(false, 6);
                        Helper.SendMail("Login验证码", Helper.e_mail(sjstr), "", "getchatgptai0409@163.com", "SJVEBCQKWNLQEQGN", textBox4.Text);

                    }
                    else
                    {
                        MessageBox.Show("邮箱格式不正确！");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("邮箱格式不正确！");
            }

        }
        
        private void count_down(object sender, EventArgs e)
        {
            if (duration == 0)
            {
                timer1.Stop();
                button2.Text = "点击验证";
                isfasong = true;
            }
            else if (duration > 0)
            {
                duration--;
                button2.Text = $"稍后重试({duration})";
                isfasong = false;
            }
        }

        private void Add_Load(object sender, EventArgs e)
        {
            this.Icon = Resources.Video;
        }
    }
}
