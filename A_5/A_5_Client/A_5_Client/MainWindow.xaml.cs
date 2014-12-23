using A_5_Client.ChatServiceReference;
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
using System.Windows.Shapes;

namespace ChatClient
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window,IChatServerCallback
    {
        A_5_Client.ChatServiceReference.ChatServerClient client;
        string username="Unknow";
        Boolean logined = false;
        public MainWindow()
        {
            InitializeComponent();
            mainArea.Visibility = Visibility.Hidden;
            this.Closing += MainWindow_Closing;
        }

        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (logined)
            try
            {
                client.Logout(username);
            }
            catch { }
            client.Close();
        }

        private void sendButton_Click(object sender, RoutedEventArgs e)
        {
            client.Talk(username, inputArea.Text);
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            username = inputUsername.Text;
            InstanceContext context = new InstanceContext(this);
            client = new A_5_Client.ChatServiceReference.ChatServerClient(context);
            try
            {
                client.Login(username);
            }
            catch
            {
                MessageBox.Show("与服务器连接失败");
            }
        }

        public void ShowMessage(string message)
        {
            chatShower.Items.Add(message);
        }

        public void ShowUsers(string[] users)
        {
            onlineList.Items.Clear();
            foreach (var u in users){
                onlineList.Items.Add(u);
            }
        }

        public void LoginState(Boolean state)
        {
            logined = state;
            if (state) {
                loginBar.Visibility = Visibility.Collapsed;
                mainArea.Visibility = Visibility.Visible;
            }
        }
    }
}
