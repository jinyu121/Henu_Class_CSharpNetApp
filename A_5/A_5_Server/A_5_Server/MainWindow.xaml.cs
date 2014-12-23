using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
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

namespace A_5_Server
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private ServiceHost host;

        public MainWindow()
        {
            InitializeComponent();
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
            this.Closing += MainWindow_Closing;
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                host.Close();
            }
            catch { }
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            host = new ServiceHost(typeof(ChatService));
            host.Open();
            infoShow.Items.Add(string.Format("[{0}] 服务已经启动 {1}",DateTime.Now,host.Description.Endpoints[0].ListenUri.ToString()));
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            host.Close();
            infoShow.Items.Add(string.Format("[{0}] 服务已经关闭。", DateTime.Now));
            btnStart.IsEnabled = true;
            btnStop.IsEnabled = false;
        }
    }
}
