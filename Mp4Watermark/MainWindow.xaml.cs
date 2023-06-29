using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Mp4Watermark
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //创建一个打开文件式的对话框
            OpenFileDialog ofd = new()
            {
                //设置打开的文件的类型，注意过滤器的语法
                Filter = "Mp4视频|*.mp4"
            };
            if (ofd.ShowDialog() == true)
            {
                txt_input.Text = ofd.FileName;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //创建一个保存文件式的对话框
            SaveFileDialog sfd = new()
            {
                //设置保存的文件的类型，注意过滤器的语法
                Filter = "Mp4视频|*.mp4"
            };
            //调用ShowDialog()方法显示该对话框，该方法的返回值代表用户是否点击了确定按钮
            if (sfd.ShowDialog() == true)
            {
                txt_output.Text = sfd.FileName;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            string inputFilePath = txt_input.Text;
            string outputFilePath = txt_output.Text;
            string watermarkText = txt_water.Text;
            string ffmpegPath = @"ffmpeg.exe";

            // 创建一个进度条对象
            ProgressBar progressBar = new();

            // 启动FFmpeg进程
            Process ffmpegProcess = new();
            ffmpegProcess.StartInfo.FileName = ffmpegPath;
            ffmpegProcess.StartInfo.Arguments = $"-i {inputFilePath} -vf \"drawtext=fontfile=Coca-ColaCareFontKaiTi.TTF:text='{watermarkText}':fontsize=24:fontcolor=white:x=10:y=10\" {outputFilePath} -y";
            ffmpegProcess.StartInfo.RedirectStandardError = true;
            ffmpegProcess.StartInfo.UseShellExecute = false;
            ffmpegProcess.StartInfo.CreateNoWindow = true;

            // 捕捉进程输出信息
            ffmpegProcess.ErrorDataReceived += (sender, e) =>
            {
                if (e.Data != null && e.Data.StartsWith("frame="))
                {
                    Dispatcher.InvokeAsync(new Action(() =>
                    {
                        txt_result.Text = e.Data.Trim() + "\n";
                    }));
                }
                if (e.Data != null && e.Data.StartsWith("[out#0/mp4 @"))
                {
                    MessageBox.Show("添加水印成功！");
                }
            };
            // 启动进程并开始捕捉输出信息
            ffmpegProcess.Start();
            ffmpegProcess.BeginErrorReadLine();

            // 等待进程完成
            // ffmpegProcess.WaitForExit();
        }
    }
}
