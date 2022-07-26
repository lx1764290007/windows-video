using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace VencentLum的视频桌面
{
    /// <summary>
    /// LockWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LockWindow : Window
    {
        private System.Windows.Threading.DispatcherTimer dt = new DispatcherTimer();
        public LockWindow()
        {
            InitializeComponent();
            Loaded += Lock_Loaded;
        }

        private void Lock_Loaded(object sender, RoutedEventArgs e)
        {
            dt.Interval = TimeSpan.FromMilliseconds(500);
            dt.Tick += new EventHandler(SetProgress);
            dt.Start();
        }
        private void SetProgress(object sender, EventArgs eventHandler)
        {
            double value = progress_bar.Value;
            if (value <= 96)
            {
                progress_bar.Value = value + 4;
            }
            else
            {
                dt.Stop();
            }
        }
    }
}
