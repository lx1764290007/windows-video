using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace VencentLum
{
    /// <summary>
    /// Player.xaml 的交互逻辑
    /// </summary>
    public partial class Player : Window
    {
        IntPtr programHandle;

        public Uri? uri { get; set; }

        public Player()
        {
            InitializeComponent();
            //向桌面发送消息
            SendMsgToProgman();
            SetFullScreen();
            Loaded += Player_Loaded;
            IsVisibleChanged += Player_IsVisibleChanged;
        }

        private void Player_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
 
            if (IsVisible)
            {
                SetMedia();
            }else
            {
                media.Source = null;
                media.Close();
            }
        }

 

        private void Player_Activated(object? sender, EventArgs e)
        {
            
            
        }

        private void Player_ContentRendered1(object? sender, EventArgs e)
        {
            
        }
        private void SetWindowZone()
        {
            Win32Func.SetParent(new System.Windows.Interop.WindowInteropHelper(this).Handle, programHandle);
        }
        private void Player_Loaded(object sender, RoutedEventArgs e)
        {
            SetWindowZone();
            SetMedia();
        }
        void SetFullScreen()
        {
            #region 设置全屏
            this.WindowState = System.Windows.WindowState.Normal;
            this.WindowStyle = System.Windows.WindowStyle.None;
            this.ResizeMode = System.Windows.ResizeMode.NoResize;
            this.Left = 0.0;
            this.Top = 0.0;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight;
            #endregion
        }
        void SetMedia()
        {
            media.Width = ActualWidth;
            media.Height = ActualHeight;
            media.Source = new Uri("D:\\output\\video\\1.mp4", UriKind.Absolute);
            media.Position = TimeSpan.FromTicks(1);
            media.MediaFailed += Media_MediaFailed;
            media.MediaEnded += new RoutedEventHandler(MediaPlayer_Ended);
            media.Play();
        }

        private void Media_MediaFailed(object? sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show(e.ErrorException.Message,"┗|｀O′|┛ 嗷~~，视频播放不了");
        }
        private void MediaPlayer_Ended(object sender, RoutedEventArgs routedEventArgs)
        {
            MediaElement? media = sender as MediaElement;
            if (media != null)
            {
                media.Position = TimeSpan.FromTicks(1);
                media.Play();
            }
 
        }
        //Win32方法
        public static class Win32Func
        {
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern IntPtr FindWindow(string className, string? winName);

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern IntPtr SendMessageTimeout(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, uint fuFlage, uint timeout, IntPtr result);

            //查找窗口的委托 查找逻辑
            public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);
            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern bool EnumWindows(EnumWindowsProc proc, IntPtr lParam);

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string className, string winName);

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern bool ShowWindow(IntPtr hwnd, int nCmdShow);

            [System.Runtime.InteropServices.DllImport("user32.dll")]
            public static extern IntPtr SetParent(IntPtr hwnd, IntPtr parentHwnd);
        }
        /// <summary>
        /// 向桌面发送消息
        /// </summary>
        public void SendMsgToProgman()
        {
            // 桌面窗口句柄，在外部定义，用于后面将我们自己的窗口作为子窗口放入
            programHandle = Win32Func.FindWindow("Progman", null);

            IntPtr result = IntPtr.Zero;
            // 向 Program Manager 窗口发送消息 0x52c 的一个消息，超时设置为2秒
            Win32Func.SendMessageTimeout(programHandle, 0x52c, IntPtr.Zero, IntPtr.Zero, 0, 2, result);

            // 遍历顶级窗口
            Win32Func.EnumWindows((hwnd, lParam) =>
            {
                // 找到第一个 WorkerW 窗口，此窗口中有子窗口 SHELLDLL_DefView，所以先找子窗口
                if (Win32Func.FindWindowEx(hwnd, IntPtr.Zero, "SHELLDLL_DefView", null) != IntPtr.Zero)
                {
                    // 找到当前第一个 WorkerW 窗口的，后一个窗口，及第二个 WorkerW 窗口。
                    IntPtr tempHwnd = Win32Func.FindWindowEx(IntPtr.Zero, hwnd, "WorkerW", null);

                    // 隐藏第二个 WorkerW 窗口
                    Win32Func.ShowWindow(tempHwnd, 0);
                }
                return true;
            }, IntPtr.Zero);
        }
       static void CloseWindow()
        {
             
        }

    }
}
