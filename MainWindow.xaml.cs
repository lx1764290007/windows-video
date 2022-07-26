using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using System.Collections;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Diagnostics;
using System.Windows.Shapes;
using VencentLum的视频桌面;
using System.Threading.Tasks;

namespace VencentLum
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window

    {

        ArrayList file_paths = new ArrayList();
        UIElement active_border_element;
        Player window_player;
        Boolean player_visible = false;
        System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon();

        public MainWindow()
        {
            Loaded += OnLoad;
            InitializeComponent();
        }

        public void OnLoad(object sender, RoutedEventArgs e)
        {
            SetValuesFromStorage();
            SetIcon();
            VcUtils.MyUtils.SendSortcutToDesktop();
        }
        /// <summary>
        /// 设置后台图标
        /// </summary>
        public void SetIcon()
        {
            this.notifyIcon.Text = "VencentLum的视频桌面";
            this.notifyIcon.Icon = System.Drawing.Icon.ExtractAssociatedIcon(System.Windows.Forms.Application.ExecutablePath);
            this.notifyIcon.Visible = false;
            this.notifyIcon.Click += FShow;
        }
        /// <summary>
        /// 完全退出程序
        /// </summary>
        private void Exit_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            Environment.Exit(0);
        }
        /// <summary>
        /// 打开主窗口，关闭托盘图标
        /// </summary>
        private void FShow(object sender, EventArgs e)
        {
            this.Show();
            WindowState = WindowState.Normal;
            this.notifyIcon.Visible = false;
        }


        private void FClose(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        /// <summary>
        /// 从本地读取文件
        /// </summary>
        private void SetValuesFromStorage()
        {
            JObject obj = Storage.LocalStorage.GetObject();
            var array = obj["source"];
          
            foreach (var item in array)
            {
                string path = item.ToString();
                
                // 判断文件是否存在
                if (File.Exists(path))
                {
                    file_paths.Add(path);
                }
            }
            Add_All_Item_In_View(file_paths);
        }

        // 动态加载图片资源
        private Image Load_Image(string path)
        {

            Image i = new Image();
            BitmapImage src = new BitmapImage();
            src.BeginInit();
            src.UriSource = new Uri(path, UriKind.Absolute);
            src.EndInit();
            i.Source = src;
            i.Stretch = Stretch.Uniform;
            int q = src.PixelHeight;        // Image loads here
            return i;
        }
        /// <summary>
        /// 视频预览
        /// </summary>
        private void SetBorderAndMarkVideoPath(string file_path, Image image)
        {

            ToolTip tootip = new ToolTip();
            
            tootip.Placement = System.Windows.Controls.Primitives.PlacementMode.Bottom;
            Border myBorder = new Border();
            myBorder.BorderBrush = Brushes.SkyBlue;
            myBorder.BorderThickness = new Thickness(2);
            myBorder.Background = Brushes.AliceBlue;
            myBorder.Padding = new Thickness(5);
            myBorder.Margin = new Thickness(3);
            myBorder.Width = 240;
            
            myBorder.CornerRadius = new CornerRadius(1);
            myBorder.Cursor = Cursors.Hand;
            // 绑定右键菜单
            myBorder.ContextMenu = this.Resources["ContextMenu"] as ContextMenu;
            myBorder.MouseEnter += MyBorder_MouseEnter;
            myBorder.MouseLeave += MyBorder_MouseLeave;
            myBorder.MouseDown += MyBorder_MouseDown;
            
            //MediaElement mediaElement = new MediaElement();
            // 预加载媒体资源，但不播放
            //mediaElement.LoadedBehavior = MediaState.Manual;
            //mediaElement.IsMuted = true; 
            //// 设置控件尺寸比例与媒体资源自适应
            //mediaElement.Stretch = System.Windows.Media.Stretch.Uniform;

            //// 视频的第一帧作为视频封面 start
            //mediaElement.ScrubbingEnabled = true;
            //mediaElement.Pause();
            //mediaElement.Position = TimeSpan.FromTicks(1);
            //// 视频的第一帧作为视频封面 end

            //// 鼠标经过时开始播放媒体
            //mediaElement.MouseEnter += On_MediaPlayer_Play;
            //// 鼠标离开时结束播放媒体
            //mediaElement.MouseLeave += On_MediaPlayer_Stop;
            //// 视频播放失败时回收资源
            //mediaElement.MediaFailed += MediaPlayer_Failed;
            //// 视频播放完毕时重置播放进度为第一帧
            //mediaElement.MediaEnded += new RoutedEventHandler(MediaPlayer_Ended);
            //// 设置资源 URI
            //mediaElement.Source = new Uri(file_path, UriKind.Absolute);


            //// 在布局控件里设置媒体控件
            //MediaIndexMark.Mark.SetMarkIndex(myBorder, index);
            //myBorder.Child = mediaElement;
            MediaMarkInImage.Mark.SetMark(myBorder, file_path);
            tootip.Content = MediaMarkInImage.Mark.GetMark(myBorder);
            myBorder.ToolTip = tootip;
            myBorder.Child = image;
            _ = wrap_pannel.Children.Add(myBorder);
        }

        private void FFmpegPlay(string video_path)
        {
            Task task = new Task(() =>
            {
                VcUtils.MyUtils.FFmpegPlay(video_path);
            });
            task.Start();
        
        }

        private  void MyBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
            {
                Border myBorder = sender as Border;
                string video_path = MediaMarkInImage.Mark.GetMark(myBorder);
                 
                FFmpegPlay(video_path);
            }

        }

        private void MyBorder_MouseLeave(object sender, MouseEventArgs e)
        {
            Border? border = sender as Border;
            border.Background = Brushes.AliceBlue;
        }

        private void MyBorder_MouseEnter(object sender, MouseEventArgs e)
        {
            Border? border = sender as Border;
            if (border != null)
            {
                active_border_element = border;
                border.Background = Brushes.Salmon;
            }
        }

        private void MediaPlayer_Failed(object? sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show(e.ErrorException.ToString());
        }
        /// <summary>
        /// 视频播放完毕再重新播放
        /// </summary>
        private void MediaPlayer_Ended(object sender, RoutedEventArgs routedEventArgs)
        {
            MediaElement? media = sender as MediaElement;
            media.Position = TimeSpan.FromTicks(1);
            media.Play();
        }
        void On_MediaPlayer_Play(object sender, RoutedEventArgs routedEventArgs)
        {
            MediaElement? media = sender as MediaElement;
            media.Play();
        }
        void On_MediaPlayer_Stop(object sender, RoutedEventArgs routedEventArgs)
        {
            MediaElement? media = sender as MediaElement;
            media.Stop();
        }
        /// <summary>
        /// 在文件系统中选取文件
        /// </summary>
        void On_Select_File(object sender, RoutedEventArgs routedEventArgs)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "选择数据源文件";
            openFileDialog.Filter = "视频文件|*.MP4;*.MOV;*.WMV;*.WAV;*.3gp;*.avi";
            openFileDialog.FileName = string.Empty;
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;
            openFileDialog.RestoreDirectory = true;
            openFileDialog.DefaultExt = "mp4";
            if (openFileDialog.ShowDialog() == false)
            {
                return;
            }
            string file_path = openFileDialog.FileName;
            Add_Files_Path(file_path);
            Storage.LocalStorage.SaveJson(file_paths);
        }
        /// <summary>
        /// 异步转码并返回新的文件地址
        /// </summary>
        /// <param name="file_path"></param>
        /// <returns></returns>
       Task<string> GetNewVideoPath(string file_path)
        {
            Task<string> task = new Task<string>(() =>
            {
                string video_path = VcUtils.MyUtils.ConvertVideo(file_path);
                return video_path;
            });
            task.Start();
            return task;
        }
        /// <summary>
        /// 添加文件进来
        /// </summary>
        async void Add_Files_Path(string file_path)
        {
            bool is_includes = ((IList)file_paths).Contains(file_path);

            if (is_includes == false)
            {
                LockWindow lock_window = new LockWindow();
                lock_window.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                lock_window.Owner = this;

                lock_window.Show();
                string video_path = await GetNewVideoPath(file_path);

                string image_path = VcUtils.MyUtils.CutVideoToImage(video_path);
                lock_window.Close();
                if (File.Exists(image_path))
                {
                    Image image = Load_Image(image_path);

                    Add_Item_In_View(video_path, image);
                    file_paths.Add(video_path);

                }
                else
                {
                    MessageBox.Show("可以试试其它的视频或者检查文件名是否包含空格和其它特殊字符", "哦豁！！视频转码失败了");
                }
            }
            else
            {
                MessageBox.Show("此文件已经添加过");
            }
        }
        void Add_Item_In_View(string path, Image image)
        {
            SetBorderAndMarkVideoPath(path, image);
        }
        void Add_All_Item_In_View(ArrayList paths)
        {
        
            for (int i = 0; i < paths.Count; i++)
            {
                if (paths[i] != null)
                {
                    string video_path = paths[i].ToString();
                    string? image_path = VcUtils.MyUtils.GetVideoPreviewImage(video_path);
                    
                    if(image_path != null)
                    {
                        Image image = Load_Image(image_path);
                        Add_Item_In_View(video_path, image);
                    }
                }

            }
        }
        /// <summary>
        /// 设置后台播放
        /// </summary>
        void Set_Window_Background(object sender, RoutedEventArgs routedEventArgs)
        {
      
            Uri uri = new Uri(MediaMarkInImage.Mark.GetMark(active_border_element), UriKind.Absolute);
            if (!player_visible)
            {
                window_player = new Player();
                window_player.uri = uri;
                window_player.Show();
                player_visible = true;
            }
            else
            {
                On_Stop_window(sender, routedEventArgs);
                Set_Window_Background(sender, routedEventArgs);
            }
        }
        /// <summary>
        /// 移除视频
        /// </summary>
        void Remove_Item_Handler(object sender, RoutedEventArgs routedEventArgs)
        {
            int has_element = wrap_pannel.Children.IndexOf(active_border_element);

            if (active_border_element != null && has_element != -1)
            {
                int index = wrap_pannel.Children.IndexOf(active_border_element);
               

                try
                {
                    file_paths.RemoveAt(index);
                    wrap_pannel.Children.Remove(active_border_element);
                }
                catch (Exception msg)
                {
                    MessageBox.Show(msg.Message);
                }
                finally
                {
                    Storage.LocalStorage.SaveJson(file_paths);
                }
            }
        }
 
        void On_Stop_window(object sender, RoutedEventArgs routedEventArgs)
        {
            if (window_player != null && window_player.ShowActivated)
            {
                window_player.Close();
                player_visible = false;
            }
        }
        /// <summary>
        /// 根据类型查找子元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="typename"></param>
        /// <returns></returns>
        public List<T> GetChildObjects<T>(DependencyObject obj, Type typename) where T : FrameworkElement
        {
            DependencyObject? child = null;
            List<T> childList = new List<T>();

            for (int i = 0; i <= VisualTreeHelper.GetChildrenCount(obj) - 1; i++)
            {
                child = VisualTreeHelper.GetChild(obj, i);

                if (child is T && (((T)child).GetType() == typename))
                {
                    childList.Add((T)child);
                }
                childList.AddRange(GetChildObjects<T>(child, typename));
            }
            return childList;
        }

        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
 
            Environment.Exit(0);
        }
        //拦截最小化事件
        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
               // WindowState = WindowState.Normal;
                this.Hide();
                this.notifyIcon.Visible = true;
            }
        }

    }
}
