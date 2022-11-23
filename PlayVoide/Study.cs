using AxWMPLib;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using Timer = System.Threading.Timer;

namespace PlayVideo
{
    public partial class Study : Form
    {
        public static List<string> urls = new List<string>()
        {
            "https://tucdn.wpon.cn/api-girl/index.php?wpon=json",//一代
            "https://v.api.aa1.cn/api/api-dy-girl/index.php?aa1=json",//二代
            "https://v.api.aa1.cn/api/api-girl-11-02/index.php?type=json",//实时
            "https://v.api.aa1.cn/api/api-video-qinglvduihua/index.php?aa1=json"//搞笑
        };
        public static string thisUrl = urls[0];
        static bool isplay = true;
        public static string url_v;
        private static bool isDz;
        private static int id;
        private static DBHelper dBHelper = new DBHelper();
        private static List<string> history = new List<string>();
        private static int video = 0;

        public Study()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            playVoide();
        }

        private void axWindowsMediaPlayer1_StatusChange(object sender, EventArgs e)
        {
            if ((int)axWindowsMediaPlayer1.playState == 1)
            {
                Thread.Sleep(1000);
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.FindForm().Width = 439;            
            playVoide();
            if (string.IsNullOrEmpty(Login.name))
            {
                button3.Text = "前往登录";
            }
            else
            {
                id = dBHelper.Select(Login.name);
                dBHelper.SelectList(id);
                listBox1.Items.AddRange(DBHelper.dz.ToArray());
                button3.Text = Login.name;
            }
            trackBar2.Value = axWindowsMediaPlayer1.settings.volume / 10;

            trackBar3.Value = (int)(this.Opacity * 10);

            timer1.Enabled = true;
        }

        public void playVoide()
        {
            axWindowsMediaPlayer1.close();
            this.trackBar3.Value = Convert.ToInt32(this.Opacity * 10);
            using (var client = new HttpClient())
            {
                try
                {
                    var data = client.GetAsync(thisUrl).Result;
                    var url = data.Content.ReadAsStringAsync().Result;
                    if (url.Contains("</html>"))
                    {
                        url = "{" + url.Split('{')[1];
                    }
                    url = JsonConvert.DeserializeObject<VoideModel>(url).mp4;
                    url_v = $"https:{url}";
                    history.Add(url_v);
                    axWindowsMediaPlayer1.URL = url_v;
                    //axWindowsMediaPlayer1.settings.volume = 0;
                    axWindowsMediaPlayer1.Ctlcontrols.play();

                    if (dBHelper.Dianz(id, url_v))
                    {
                        label1.ForeColor = Color.Red;
                        isDz = true;
                    }
                    else
                    {
                        label1.ForeColor = Color.Black;
                        isDz = false;
                    }
                    if (history.Count - 1 == 0 || video == 0)
                    {
                        label4.ForeColor = Color.Gray;
                    }
                    else
                    {
                        label4.ForeColor = Color.Black;
                    }
                    if (dBHelper.Dianz(url_v) > 0)
                    {
                        label3.Text = dBHelper.Dianz(url_v).ToString();
                    }
                    else
                    {
                        label3.Text = "";
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("播放或切换失败");
                }
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            var all = axWindowsMediaPlayer1.currentMedia.duration;
            var thiss = all * (Convert.ToDouble(trackBar1.Value) / 10);
            axWindowsMediaPlayer1.Ctlcontrols.currentPosition = thiss;
        }

        public int i = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            i++;
            if (i > 3)
            {
                i = 0;
            }
            if (i > 3)
            {
                i = 0;
            }
            if (i > 3)
            {
                i = 0;
            }
            thisUrl = urls[i];
            switch (i)
            {
                case 0:
                    button2.Text = "一代短视频";
                    break;
                case 1:
                    button2.Text = "二代短视频";
                    break;
                case 2:
                    button2.Text = "实时短视频";
                    break;
                case 3:
                    button2.Text = "搞笑短视频";
                    break;
                default:
                    break;
            }
            playVoide();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            axWindowsMediaPlayer1.close();
            Application.Exit();
        }

        private void button2_MouseEnter(object sender, EventArgs e)
        {
            ToolTip toolTip2 = new ToolTip();
            toolTip2.AutoPopDelay = 5000; toolTip2.InitialDelay = 500; toolTip2.ReshowDelay = 500;
            toolTip2.ShowAlways = true;
            toolTip2.SetToolTip(this.button2, "切换源");  //设置提示信息为自定义
        }

        private void axWindowsMediaPlayer1_ClickEvent(object sender, _WMPOCXEvents_ClickEvent e)
        {
            if (isplay)
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                label6.Text = "⏯️";
                isplay = false;
            }
            else
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
                label6.Text = "⏸️";
                isplay = true;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Login.name))
            {
                MessageBox.Show("请先前往登录");
            }
            else
            {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(DBHelper.dz.ToArray());
                if (!isDz)
                {
                    if (!dBHelper.Isinsert(id, url_v))
                    {
                        if (dBHelper.Insert(id, url_v) > 0)
                        {
                            label1.ForeColor = Color.Red;
                        }
                        isDz = true;
                    }
                }
                else
                {
                    if (dBHelper.Qvxiao(id, url_v) > 0)
                    {
                        label1.ForeColor = Color.Black;
                    }

                    isDz = false;
                }
                if (dBHelper.Dianz(url_v) > 0)
                {
                    label3.Text = dBHelper.Dianz(url_v).ToString();
                }
                else
                {
                    label3.Text = "";
                }
                dBHelper.SelectList(id);
                listBox1.Items.Clear();
                listBox1.Items.AddRange(DBHelper.dz.ToArray());
            }
        }
        public static bool isZhank = false;
        private void label2_Click(object sender, EventArgs e)
        {
            if (!isZhank)
            {
                this.FindForm().Width = 739;
                isZhank = true;
            }
            else
            {
                this.FindForm().Width = 439;
                isZhank = false;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            if (history.Count - 1 == 0 || video == 0)
            {
                label4.ForeColor = Color.Gray;
            }
            else
            {
                video--;
                axWindowsMediaPlayer1.URL = history[video];
                axWindowsMediaPlayer1.Ctlcontrols.play();

                if (dBHelper.Dianz(id, history[video]))
                {
                    label1.ForeColor = Color.Red;
                    isDz = true;
                }
                else
                {
                    label1.ForeColor = Color.Black;
                    isDz = false;
                }
                if (history.Count - 1 == 0 || video == 0)
                {
                    label4.ForeColor = Color.Gray;
                }
                else
                {
                    label4.ForeColor = Color.Black;
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            if (history.Count - 1 > video)
            {
                video++;
                axWindowsMediaPlayer1.URL = history[video];
                axWindowsMediaPlayer1.Ctlcontrols.play();

                if (dBHelper.Dianz(id, history[video]))
                {
                    label1.ForeColor = Color.Red;
                    isDz = true;
                }
                else
                {
                    label1.ForeColor = Color.Black;
                    isDz = false;
                }

                if (history.Count - 1 == 0 || video == 0)
                {
                    label4.ForeColor = Color.Gray;
                }
                else
                {
                    label4.ForeColor = Color.Black;
                }
            }
            else
            {
                video++;
                playVoide();
            }
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            var a = this.trackBar3.Value * 10;
            axWindowsMediaPlayer1.settings.volume = a;
        }

        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            var a = Convert.ToDouble(this.trackBar3.Value) / 10;
            if (a >= 0.1)
            {
                this.Opacity = a;
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {
            if (isplay)
            {
                axWindowsMediaPlayer1.Ctlcontrols.pause();
                label6.Text = "⏯️";
                isplay = false;
            }
            else
            {
                axWindowsMediaPlayer1.Ctlcontrols.play();
                label6.Text = "⏸️";
                isplay = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Login.name))
            {
                Login login = new Login();
                login.Show();
                this.Hide();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var aaa = axWindowsMediaPlayer1.Ctlcontrols.currentPosition/axWindowsMediaPlayer1.currentMedia.duration;
            if (aaa > 0)
            {
                trackBar1.Value = (int)(aaa * 10);
            }
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.X, e.Y);
            listBox1.SelectedIndex = index;
            if (listBox1.SelectedIndex != -1)
            {
                url_v = listBox1.SelectedItem.ToString();
                axWindowsMediaPlayer1.URL = url_v;
                axWindowsMediaPlayer1.Ctlcontrols.play();

                if (dBHelper.Dianz(id, url_v))
                {
                    label1.ForeColor = Color.Red;
                    isDz = true;
                }
                else
                {
                    label1.ForeColor = Color.Black;
                    isDz = false;
                }

                if (history.Count - 1 == 0 || video == 0)
                {
                    label4.ForeColor = Color.Gray;
                }
                else
                {
                    label4.ForeColor = Color.Black;
                }
                if (dBHelper.Dianz(url_v) > 0)
                {
                    label3.Text = dBHelper.Dianz(url_v).ToString();
                }
                else
                {
                    label3.Text = "";
                }
            }
        }
    }
}
