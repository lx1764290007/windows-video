using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IW =IWshRuntimeLibrary;

namespace VcUtils
{
    internal class MyUtils
    {
        public static void SendSortcutToDesktop()
        {

            String shortcutPath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "VencentLum的视频桌面" + ".lnk");
            if (!System.IO.File.Exists(shortcutPath))
            {
                string AppName = GetAppName();
                // 获取当前应用程序目录地址
                String exePath = GetAppDirectory() + AppName + ".exe";
                IW.IWshShell shell = new IW.WshShell();
                // 确定是否已经创建的快捷键被改名了
                foreach (var item in Directory.GetFiles(Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory), "*.lnk"))
                {
                    IW.WshShortcut tempShortcut = (IW.WshShortcut)shell.CreateShortcut(item);
                    if (tempShortcut.TargetPath == exePath)
                    {
                        return;
                    }
                }
                IW.WshShortcut shortcut = (IW.WshShortcut)shell.CreateShortcut(shortcutPath);
                shortcut.TargetPath = exePath;
                shortcut.Arguments = "";// 参数
                shortcut.Description = AppName + exePath;
                shortcut.WorkingDirectory = Environment.CurrentDirectory;//程序所在文件夹，在快捷方式图标点击右键可以看到此属性
                shortcut.IconLocation = exePath;//图标，该图标是应用程序的资源文件
                                                //shortcut.Hotkey = "CTRL+SHIFT+W";//热键，发现没作用，大概需要注册一下
                shortcut.WindowStyle = 1;
                shortcut.Save();
               // MessageBox.Show("桌面快捷方式已创建！");
            }
    
        }
        /// <summary>
        /// 查询程序所在路径 
        /// </summary>
        /// <returns>string</returns>
        public static string GetAppDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
        /// <summary>
        /// 查询app名字
        /// </summary>
        /// <returns>string</returns>
        public static string GetAppName()
        {
            return Path.GetFileName(Assembly.GetEntryAssembly().GetName().Name);
        }
        /// <summary>
        /// 创建子进程实现文件转码
        /// </summary>
        public static string ConvertVideo(string path)
        {

            string app_dir = GetAppDirectory().Replace(@"\", "/"); 
            string target_path = app_dir + "video/" + Path.GetFileName(path);
            string ffmpeg_path = app_dir + "lib/FFmpeg/bin/ffmpeg.exe";
            // 如果本身就是属于输出目录的文件，那就不再转码，直接返回

            if (path.Replace(@"\", "/").IndexOf(app_dir + "video") != -1)
            {
                return path;
            }
            ProcessStartInfo start_info = new ProcessStartInfo(ffmpeg_path);
            
            start_info.Arguments = string.Format("-i  {0}  -r 32 -vcodec libx264  -vf scale=1366:768 -preset:v fast -crf 8 {1}", "\""+ path + "\"", "\"" + target_path + "\"");//参数(这里就是FFMPEG的参数了)
            start_info.UseShellExecute = false;//不使用操作系统外壳程序启动线程(一定为FALSE,详细的请看MSDN)
            start_info.RedirectStandardError = true;//把外部程序错误输出写到StandardError流中(这个一定要注意,FFMPEG的所有输出信息,都为错误输出流,用StandardOutput是捕获不到任何消息的...这是我耗费了2个多月得出来的经验...mencoder就是用standardOutput来捕获的)
            start_info.CreateNoWindow = true;//不创建进程窗口

            start_info.RedirectStandardOutput = true;
            start_info.RedirectStandardInput = true;
            Process p = Process.Start(start_info);//启动线程
                                                  //p.WaitForExit();//等待后台程序退出 [他喵的这个方法会阻塞FFMPEG]

            p.StandardInput.Close();
            p.StandardError.ReadToEnd();//开始同步读取
            p.Close();//关闭进程
            p.Dispose();//释放资源
            return target_path;
           // CutVideo();
        }
        /// <summary>
        ///从视频里截取一帧当做预览图
        /// </summary>
        /// <param name="video_path"></param>
        /// <returns>预览图的绝对路径</returns>
        public static string CutVideoToImage(string video_path)
        {

            string app_dir = GetAppDirectory().Replace(@"\", "/"); ;
            string target_path = app_dir + "video_preview/" + Path.GetFileName(video_path) + ".jpg";
            string ffmpeg_path = app_dir + "lib/FFmpeg/bin/ffmpeg.exe";
            // Process p = new Process();//建立外部调用线程
            ProcessStartInfo start_info = new ProcessStartInfo(ffmpeg_path);

            start_info.Arguments = string.Format("-i {0} -r 1 -q:v 2 -f image2 {1}", video_path, target_path);//参数(这里就是FFMPEG的参数了)
            start_info.UseShellExecute = false;//不使用操作系统外壳程序启动线程(一定为FALSE,详细的请看MSDN)
            start_info.RedirectStandardError = true;//把外部程序错误输出写到StandardError流中(这个一定要注意,FFMPEG的所有输出信息,都为错误输出流,用StandardOutput是捕获不到任何消息的...这是我耗费了2个多月得出来的经验...mencoder就是用standardOutput来捕获的)
            start_info.CreateNoWindow = true;//不创建进程窗口

            start_info.RedirectStandardOutput = true;
            start_info.RedirectStandardInput = true;
            Process p = Process.Start(start_info);//启动线程
                                                  //p.WaitForExit();//等待后台程序退出 [他喵的这个方法会阻塞FFMPEG]

            p.StandardInput.Close();
            p.StandardError.ReadToEnd();//开始同步读取
            p.Close();//关闭进程
            p.Dispose();//释放资源
            return target_path;
        }
        public static void FFmpegPlay(string video_path)
        {

            string app_dir = GetAppDirectory().Replace(@"\", "/"); ;
            string ffmpeg_path = app_dir + "lib/FFmpeg/bin/ffplay.exe";
            // Process p = new Process();//建立外部调用线程
            ProcessStartInfo start_info = new ProcessStartInfo(ffmpeg_path);

            start_info.Arguments = string.Format("-i -x 1200 -an -loop 10 {0}", video_path);//参数(这里就是FFMPEG的参数了)
            start_info.UseShellExecute = false;//不使用操作系统外壳程序启动线程(一定为FALSE,详细的请看MSDN)
            start_info.RedirectStandardError = true;//把外部程序错误输出写到StandardError流中(这个一定要注意,FFMPEG的所有输出信息,都为错误输出流,用StandardOutput是捕获不到任何消息的...这是我耗费了2个多月得出来的经验...mencoder就是用standardOutput来捕获的)
            start_info.CreateNoWindow = true;//不创建进程窗口

            start_info.RedirectStandardOutput = true;
            start_info.RedirectStandardInput = true;
            Process p = Process.Start(start_info);//启动线程
                                                  //p.WaitForExit();//等待后台程序退出 [他喵的这个方法会阻塞FFMPEG]

            p.StandardInput.Close();
            p.StandardError.ReadToEnd();//开始同步读取
            p.Close();//关闭进程
            p.Dispose();//释放资源
           
        }
        public static string GetVideoPreviewImage(string video_path)
        {
            string app_dir = GetAppDirectory().Replace(@"\", "/"); ;
            string target_path = app_dir + "video_preview/" + Path.GetFileName(video_path) + ".jpg";
            if (File.Exists(target_path))
            {
                return target_path;
            }
            else return null;
        }
    }
}
