using A_4_Client.MyWebFileService;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace A_4_Client
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        WebFile[] fileList;
        WebFileServiceClient client;
        public MainWindow()
        {
            InitializeComponent();
            client = new WebFileServiceClient();

        }

        private async void GetInfo_Click(object sender, RoutedEventArgs e)
        {
            infoShow.Dispatcher.Invoke(() =>
            {
                infoShow.Text += string.Format("[{0}] 正在获取文件列表……\n", DateTime.Now.ToString());
            });
            fileList = await client.GetFileListAsync();
            filesShow.ItemsSource = fileList;
            infoShow.Dispatcher.Invoke(() =>
            {
                infoShow.Text += string.Format("[{0}] 文件列表获取完毕\n", DateTime.Now.ToString());
            });
        }

        private void StartDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WebFile f = filesShow.SelectedItem as WebFile;
                infoShow.Dispatcher.Invoke(() =>
                {
                    infoShow.Text += string.Format("[{0}] 文件 {1} [{2}KB] 开始下载\n", DateTime.Now.ToString(), f.fileName, f.fileSize);
                });
                try
                {
                    using (Stream stream = client.GetFile(f.filePath))
                    {
                        using (FileStream fs = File.Open(f.fileName, FileMode.Create, FileAccess.Write))
                        {
                            const int MAX_SIZE = 2048;
                            byte[] buf = new byte[MAX_SIZE];
                            for (int counter = stream.Read(buf, 0, MAX_SIZE); counter > 0; counter = stream.Read(buf, 0, MAX_SIZE))
                            {
                                fs.Write(buf, 0, counter);
                            }
                        }
                    }
                    infoShow.Dispatcher.Invoke(() =>
                    {
                        infoShow.Text += string.Format("[{0}] 文件 {1} 下载完毕\n", DateTime.Now.ToString(), f.fileName);
                    });
                }
                catch
                {
                    infoShow.Dispatcher.Invoke(() =>
                    {
                        infoShow.Text += string.Format("[{0}] 下载文件 {1} 时发生了错误\n", DateTime.Now.ToString(), f.fileName);
                    });
                }

            }
            catch
            {
                infoShow.Dispatcher.Invoke(() =>
                {
                    infoShow.Text += string.Format("没有选择文件\n", DateTime.Now.ToString());
                });
            }
        }
    }
}