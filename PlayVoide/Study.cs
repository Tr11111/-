using AxWMPLib;
using Newtonsoft.Json;
using PlayVideo.Properties;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Timer = System.Threading.Timer;
using ToolTip = System.Windows.Forms.ToolTip;

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
        private static Helper dBHelper = new Helper();
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
            this.Icon = Resources.Video;
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
                listBox1.Items.AddRange(Helper.dz.ToArray());
                button3.Text = Login.name;
            }
            trackBar2.Value = axWindowsMediaPlayer1.settings.volume / 10 ;

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
                    if (!string.IsNullOrEmpty(Login.name))
                    {
                        var isp = dBHelper.ChaChong(url_v, id);
                        if (isp)
                        {
                            playVoide();
                        }
                    }
                    if (!history.Contains(url_v))
                    {
                        history.Add(url_v);
                    }
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
                    button2.Text = "一代源";
                    break;
                case 1:
                    button2.Text = "二代源";
                    break;
                case 2:
                    button2.Text = "实时源";
                    break;
                case 3:
                    button2.Text = "搞笑源";
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
            pPlay();
        }
        public void pPlay()
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
            pDianZan();
        }
        public void pDianZan()
        {
            if (string.IsNullOrEmpty(Login.name))
            {
                MessageBox.Show("请先前往登录");
            }
            else
            {
                listBox1.Items.Clear();
                listBox1.Items.AddRange(Helper.dz.ToArray());
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
                listBox1.Items.AddRange(Helper.dz.ToArray());

            }
        }
        public static bool isZhank = false;
        private void label2_Click(object sender, EventArgs e)
        {
            if (!isZhank)
            {
                this.FindForm().Width = 966;
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
            pLeft();
        }
        public void pLeft()
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
            pRight();
        }
        public void pRight()
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
            var a = this.trackBar2.Value * 10;
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
                axWindowsMediaPlayer1.close();
                new Login().Show();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            var aaa = axWindowsMediaPlayer1.Ctlcontrols.currentPosition / axWindowsMediaPlayer1.currentMedia.duration;
            if (aaa > 0)
            {
                trackBar1.Value = (int)(aaa * 10);
            }
        }

        private void listBox1_MouseClick(object sender, MouseEventArgs e)
        {
            List_c(e);
        }
        public void List_c(MouseEventArgs e)
        {
            int index = listBox1.IndexFromPoint(e.X, e.Y);
            listBox1.SelectedIndex = index;
            if (listBox1.SelectedIndex != -1)
            {
                url_v = "https://tucdn.wpon.cn/api-girl/videos/" + listBox1.SelectedItem.ToString();
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
        private void button3_MouseEnter(object sender, EventArgs e)
        {
            ToolTip toolTip2 = new ToolTip();
            toolTip2.AutoPopDelay = 5000; toolTip2.InitialDelay = 500; toolTip2.ReshowDelay = 500;
            toolTip2.ShowAlways = true;
            toolTip2.SetToolTip(this.button3, "登录/重新登陆");  //设置提示信息为自定义
        }

        private void label1_MouseEnter(object sender, EventArgs e)
        {
            ToolTip toolTip2 = new ToolTip();
            toolTip2.AutoPopDelay = 5000; toolTip2.InitialDelay = 500; toolTip2.ReshowDelay = 500;
            toolTip2.ShowAlways = true;
            toolTip2.SetToolTip(this.label1, "视频点赞");  //设置提示信息为自定义
        }

        private void label2_MouseEnter(object sender, EventArgs e)
        {
            ToolTip toolTip2 = new ToolTip();
            toolTip2.AutoPopDelay = 5000; toolTip2.InitialDelay = 500; toolTip2.ReshowDelay = 500;
            toolTip2.ShowAlways = true;
            toolTip2.SetToolTip(this.label2, "扩展设置");  //设置提示信息为自定义
        }

        private void label5_MouseEnter(object sender, EventArgs e)
        {
            ToolTip toolTip2 = new ToolTip();
            toolTip2.AutoPopDelay = 5000; toolTip2.InitialDelay = 500; toolTip2.ReshowDelay = 500;
            toolTip2.ShowAlways = true;
            toolTip2.SetToolTip(this.label5, "下一条");  //设置提示信息为自定义
        }

        private void label4_MouseEnter(object sender, EventArgs e)
        {
            ToolTip toolTip2 = new ToolTip();
            toolTip2.AutoPopDelay = 5000; toolTip2.InitialDelay = 500; toolTip2.ReshowDelay = 500;
            toolTip2.ShowAlways = true;
            toolTip2.SetToolTip(this.label4, "上一条");  //设置提示信息为自定义
        }

        private void label6_MouseEnter(object sender, EventArgs e)
        {
            ToolTip toolTip2 = new ToolTip();
            toolTip2.AutoPopDelay = 5000; toolTip2.InitialDelay = 500; toolTip2.ReshowDelay = 500;
            toolTip2.ShowAlways = true;
            toolTip2.SetToolTip(this.label6, "播放/切换");  //设置提示信息为自定义
        }

        private void label8_MouseEnter(object sender, EventArgs e)
        {
            ToolTip toolTip2 = new ToolTip();
            toolTip2.AutoPopDelay = 5000; toolTip2.InitialDelay = 500; toolTip2.ReshowDelay = 500;
            toolTip2.ShowAlways = true;
            toolTip2.SetToolTip(this.label8, "透明度");  //设置提示信息为自定义
        }

        private void label7_MouseEnter(object sender, EventArgs e)
        {
            ToolTip toolTip2 = new ToolTip();
            toolTip2.AutoPopDelay = 5000; toolTip2.InitialDelay = 500; toolTip2.ReshowDelay = 500;
            toolTip2.ShowAlways = true;
            toolTip2.SetToolTip(this.label7, "音量");  //设置提示信息为自定义
        }

        private void listBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Clipboard.SetDataObject("https://tucdn.wpon.cn/api-girl/videos/" + this.listBox1.Text);
            ToolTip toolTip2 = new ToolTip();
            toolTip2.AutoPopDelay = 5000; toolTip2.InitialDelay = 500; toolTip2.ReshowDelay = 500;
            toolTip2.ShowAlways = true;
            toolTip2.SetToolTip(this.listBox1, "已复制到剪切板");  //设置提示信息为自定义
        }

        private void Study_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.KeyCode:
                    break;
                case Keys.Modifiers:
                    break;
                case Keys.None:
                    break;
                case Keys.LButton:
                    break;
                case Keys.RButton:
                    break;
                case Keys.Cancel:
                    break;
                case Keys.MButton:
                    break;
                case Keys.XButton1:
                    break;
                case Keys.XButton2:
                    break;
                case Keys.Back:
                    break;
                case Keys.Tab:
                    break;
                case Keys.LineFeed:
                    break;
                case Keys.Clear:
                    break;
                case Keys.Return:
                    break;
                //case Keys.Enter:
                //    break;
                case Keys.ShiftKey:
                    break;
                case Keys.ControlKey:
                    break;
                case Keys.Menu:
                    break;
                case Keys.Pause:
                    break;
                case Keys.Capital:
                    break;
                //case Keys.CapsLock:
                //    break;
                case Keys.KanaMode:
                    break;
                //case Keys.HanguelMode:
                //    break;
                //case Keys.HangulMode:
                //    break;
                case Keys.JunjaMode:
                    break;
                case Keys.FinalMode:
                    break;
                case Keys.HanjaMode:
                    break;
                //case Keys.KanjiMode:
                //break;
                case Keys.Escape:
                    break;
                case Keys.IMEConvert:
                    break;
                case Keys.IMENonconvert:
                    break;
                case Keys.IMEAccept:
                    break;
                //case Keys.IMEAceept:
                //    break;
                case Keys.IMEModeChange:
                    break;
                case Keys.Space:
                    pPlay();
                    break;
                case Keys.Prior:
                    break;
                //case Keys.PageUp:
                //break;
                case Keys.Next:
                    break;
                //case Keys.PageDown:
                //break;
                case Keys.End:
                    break;
                case Keys.Home:
                    break;
                case Keys.Left:
                    
                    break;
                case Keys.Up:
                    break;
                case Keys.Right:
                    break;
                case Keys.Down:
                    break;
                case Keys.Select:
                    break;
                case Keys.Print:
                    break;
                case Keys.Execute:
                    break;
                case Keys.Snapshot:
                    break;
                //case Keys.PrintScreen:
                //break;
                case Keys.Insert:
                    break;
                case Keys.Delete:
                    break;
                case Keys.Help:
                    break;
                case Keys.D0:
                    break;
                case Keys.D1:
                    break;
                case Keys.D2:
                    break;
                case Keys.D3:
                    break;
                case Keys.D4:
                    break;
                case Keys.D5:
                    break;
                case Keys.D6:
                    break;
                case Keys.D7:
                    break;
                case Keys.D8:
                    break;
                case Keys.D9:
                    break;
                case Keys.A:
                    break;
                case Keys.B:
                    break;
                case Keys.C:
                    pDianZan();
                    break;
                case Keys.D:
                    break;
                case Keys.E:
                    break;
                case Keys.F:
                    break;
                case Keys.G:
                    break;
                case Keys.H:
                    break;
                case Keys.I:
                    break;
                case Keys.J:
                    break;
                case Keys.K:
                    break;
                case Keys.L:
                    break;
                case Keys.M:
                    break;
                case Keys.N:
                    break;
                case Keys.O:
                    break;
                case Keys.P:
                    break;
                case Keys.Q:
                    break;
                case Keys.R:
                    break;
                case Keys.S:
                    break;
                case Keys.T:
                    break;
                case Keys.U:
                    break;
                case Keys.V:
                    break;
                case Keys.W:
                    break;
                case Keys.X:
                    pRight();
                    break;
                case Keys.Y:
                    break;
                case Keys.Z:
                    pLeft();
                    break;
                case Keys.LWin:
                    break;
                case Keys.RWin:
                    break;
                case Keys.Apps:
                    break;
                case Keys.Sleep:
                    break;
                case Keys.NumPad0:
                    break;
                case Keys.NumPad1:
                    break;
                case Keys.NumPad2:
                    break;
                case Keys.NumPad3:
                    break;
                case Keys.NumPad4:
                    break;
                case Keys.NumPad5:
                    break;
                case Keys.NumPad6:
                    break;
                case Keys.NumPad7:
                    break;
                case Keys.NumPad8:
                    break;
                case Keys.NumPad9:
                    break;
                case Keys.Multiply:
                    break;
                case Keys.Add:
                    break;
                case Keys.Separator:
                    break;
                case Keys.Subtract:
                    break;
                case Keys.Decimal:
                    break;
                case Keys.Divide:
                    break;
                case Keys.F1:
                    break;
                case Keys.F2:
                    break;
                case Keys.F3:
                    break;
                case Keys.F4:
                    break;
                case Keys.F5:
                    break;
                case Keys.F6:
                    break;
                case Keys.F7:
                    break;
                case Keys.F8:
                    break;
                case Keys.F9:
                    break;
                case Keys.F10:
                    break;
                case Keys.F11:
                    break;
                case Keys.F12:
                    break;
                case Keys.F13:
                    break;
                case Keys.F14:
                    break;
                case Keys.F15:
                    break;
                case Keys.F16:
                    break;
                case Keys.F17:
                    break;
                case Keys.F18:
                    break;
                case Keys.F19:
                    break;
                case Keys.F20:
                    break;
                case Keys.F21:
                    break;
                case Keys.F22:
                    break;
                case Keys.F23:
                    break;
                case Keys.F24:
                    break;
                case Keys.NumLock:
                    break;
                case Keys.Scroll:
                    break;
                case Keys.LShiftKey:
                    break;
                case Keys.RShiftKey:
                    break;
                case Keys.LControlKey:
                    break;
                case Keys.RControlKey:
                    break;
                case Keys.LMenu:
                    break;
                case Keys.RMenu:
                    break;
                case Keys.BrowserBack:
                    break;
                case Keys.BrowserForward:
                    break;
                case Keys.BrowserRefresh:
                    break;
                case Keys.BrowserStop:
                    break;
                case Keys.BrowserSearch:
                    break;
                case Keys.BrowserFavorites:
                    break;
                case Keys.BrowserHome:
                    break;
                case Keys.VolumeMute:
                    break;
                case Keys.VolumeDown:
                    break;
                case Keys.VolumeUp:
                    break;
                case Keys.MediaNextTrack:
                    break;
                case Keys.MediaPreviousTrack:
                    break;
                case Keys.MediaStop:
                    break;
                case Keys.MediaPlayPause:
                    break;
                case Keys.LaunchMail:
                    break;
                case Keys.SelectMedia:
                    break;
                case Keys.LaunchApplication1:
                    break;
                case Keys.LaunchApplication2:
                    break;
                case Keys.OemSemicolon:
                    break;
                //case Keys.Oem1:
                //    break;
                case Keys.Oemplus:
                    break;
                case Keys.Oemcomma:
                    break;
                case Keys.OemMinus:
                    break;
                case Keys.OemPeriod:
                    break;
                case Keys.OemQuestion:
                    break;
                //case Keys.Oem2:
                //    break;
                //case Keys.Oemtilde:
                //    break;
                //case Keys.Oem3:
                //    break;
                //case Keys.OemOpenBrackets:
                //    break;
                //case Keys.Oem4:
                //    break;
                //case Keys.OemPipe:
                //    break;
                //case Keys.Oem5:
                //    break;
                //case Keys.OemCloseBrackets:
                //    break;
                //case Keys.Oem6:
                //    break;
                //case Keys.OemQuotes:
                //    break;
                //case Keys.Oem7:
                //    break;
                case Keys.Oem8:
                    break;
                case Keys.OemBackslash:
                    break;
                //case Keys.Oem102:
                //    break;
                case Keys.ProcessKey:
                    break;
                case Keys.Packet:
                    break;
                case Keys.Attn:
                    break;
                case Keys.Crsel:
                    break;
                case Keys.Exsel:
                    break;
                case Keys.EraseEof:
                    break;
                case Keys.Play:
                    break;
                case Keys.Zoom:
                    break;
                case Keys.NoName:
                    break;
                case Keys.Pa1:
                    break;
                case Keys.OemClear:
                    break;
                case Keys.Shift:
                    break;
                case Keys.Control:
                    break;
                case Keys.Alt:
                    break;
                default:
                    break;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
                axWindowsMediaPlayer1.close();
                axWindowsMediaPlayer1.URL = textBox1.Text;
                axWindowsMediaPlayer1.Ctlcontrols.play();
            }
            catch (Exception)
            {
                MessageBox.Show("播放或切换失败");
            }
        }

        private void listBox1_MouseHover(object sender, EventArgs e)
        {
            ToolTip toolTip2 = new ToolTip();
            toolTip2.AutoPopDelay = 5000; toolTip2.InitialDelay = 500; toolTip2.ReshowDelay = 500;
            toolTip2.ShowAlways = true;
            toolTip2.SetToolTip(this.listBox1, "双击复制路径");
        }
    }
}
