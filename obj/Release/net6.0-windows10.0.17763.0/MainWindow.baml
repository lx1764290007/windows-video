   M S B A M L   `   `   `  ���� T  PVencentLum的视频桌面, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:MediaMarkInImageMediaMarkInImage      VencentLum.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     0localclr-namespace:MediaMarkInImage     -     � WindowStartupLocation$  CenterScreen=� � Title$ VencentLum的视频桌面��$	��500��$	��800�� � 	Resources    ContextMenu��%t   &	        �� .@�m� $��set_background��-   $��设置为背景���� m� $��
set_remove��-   $��移除��.���  � RowDefinitions �� $��* ��� $
��Auto �.+��� $��0q�$	��Top=�$��0��$	��Top=� �� TabStripPlacement$
 Left=�.@��� $��主要功能��.���� $��0q�$
��Auto=�.���� $
��10 0q�.+�� -    �Name$ wrap_pannel���� -   $��	open_file��$��0 30 0 0q�$	��333��
�������$��75��
�������
����  � � Cursor$
 Hand��$��添 加���� -   $��kill_window��$	��333��$
��0 30q�$��75��
�������
�������
������$
 Hand��.��停用视频桌面]� �� $��说明��.���� $
��10 0q�$��1q�
�������.@��� 
�������$��功能说明��$��Normal$�$��14(�
����U���� $����0、此功能的原理是在UI的桌面图标层和背景层之间嵌入了一个全屏的、无边框的且自动循环播放的视频播放器���� $����1、添加视频到列表中之后，鼠标选中并单击可以打开预览，选中目标后单击鼠标右键选择将视频设置为背景���� $v��p2、点击应用主窗口右上角的窗口最小化按钮会让窗口以小图标的形式寄生在任务栏内���� $=��73、点击任务栏的小图标可以恢复应用窗口���� $[��U4、首次运行程序会在桌面生成一个图标作为程序运行的快捷方式���� $N��H5、如需完全退出此程序则在应用主窗口中点击'X''按钮 ���� $z��t6、被添加的视频文件名称中不能有空格，否则可能会导致添加失败（主要是转码失败） ���� $����7、视频文件会被转码（压缩）成新的文件储存在/video/目录中（不会更改源文件，也不会对/video/目录的文件二次转码）���� $��版本历史��.���� $
��10 0q�$��1q�
�������.@��� 
�������$��v1.0_2022_07_03��$��Normal$�$��14(�
���E���� $<��61、实现了基本的UI界面还有较完整的功能���� 
�������$��v2.0_2022_07_06��$��Normal$�$��14(�
���E���� $2��,1、添加了第三方视频解码器 FFmpeg���� $2��,2、添加了第三方视频播放器 FFplay���� $:��43、添加视频时创建一个转码过后的副本���� $O��I4、大幅度降低内存占用, CPU占用以及磁盘读写通道占用 ���� $��1��.+�� $	��0 5q�$	��500�� � ColumnDefinitions �� 