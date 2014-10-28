using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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

namespace A_2
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        int sta, fin;

        public MainWindow()
        {
            InitializeComponent();
        }
        private void GOGOGO_Click(object sender, RoutedEventArgs e)
        {
            sta = int.Parse(ipSta.Text.Trim());
            fin = int.Parse(ipFin.Text.Trim());
            for (int i = sta; i <= fin; i++)
            {
                IPScaner sc = new IPScaner(ipPre.Text + i.ToString(), resultShower);
                // 把工作扔给线程池
                // 注意：线程池里面的线程默认是后台线程
                ThreadPool.QueueUserWorkItem(new WaitCallback(sc.scan));
            }
        }
        public static bool IsIPv4(string input)
        {
            string[] IPs = input.Split('.');

            for (int i = 0; i < IPs.Length; i++)
            {
                if (!Regex.IsMatch(@"^\d+$", IPs[i]))
                {
                    return false;
                }
                if (Convert.ToUInt16(IPs[i]) > 255)
                {
                    return false;
                }
            }
            return true;
        }

        private void ip_LostFocus(object sender, RoutedEventArgs e)
        {
            // 初始化控件状态
            GOGOGO.IsEnabled = true;
            WarningWrongIP.Visibility = Visibility.Collapsed;
            WarningWrongIPSta.Visibility = Visibility.Collapsed;
            WarningWrongIPFin.Visibility = Visibility.Collapsed;
            
            bool hasError = false;
            try
            {
                // ipPre
                try
                {
                    string[] numbers = ipPre.Text.Split('.');
                    if (numbers.Length != 4)
                        throw new Exception();
                    if (numbers[3]!="")
                        throw new Exception();
                    for (int i = 0; i < 3; i++)
                    {
                        int temp = int.Parse(numbers[i]);
                        if (temp < 0 || temp > 255)
                            throw new Exception();
                    }
                }
                catch
                {
                    WarningWrongIP.Visibility = Visibility.Visible;
                    hasError = true;
                }
                // ipSta
                try
                {
                    sta = -1;
                    sta = int.Parse(ipSta.Text.Trim());
                    if (sta < 0 || sta > 255)
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    WarningWrongIPSta.Visibility = Visibility.Visible;
                    hasError = true;
                }
                // ipFin
                try
                {
                    fin = -1;
                    fin = int.Parse(ipFin.Text.Trim());
                    if (fin < 0 || fin > 255)
                    {
                        throw new Exception();
                    }
                    if ((!hasError) && (sta>fin))
                    {
                        throw new Exception();
                    }
                }
                catch
                {
                    WarningWrongIPFin.Visibility = Visibility.Visible;
                    hasError = true;
                }
                if (hasError)
                {
                    throw new Exception();
                }
            }
            catch {
                GOGOGO.IsEnabled = false;
            }
        }
    }
    public class IPScaner
    {
        private IPAddress ip;
        private ListBox resultShower;
        public IPScaner(String sip, ListBox res)
        {
            ip = IPAddress.Parse(sip);
            this.resultShower = res;
        }
        public void scan(Object obj)
        {
            string name;
            // 这东西是秒表……用来卡时间的
            Stopwatch sw = new Stopwatch();
            sw.Start();
            try
            {
                name = Dns.GetHostEntry(ip).HostName;
            }
            catch
            {
                name = "[不在线]";
            }
            sw.Stop();
            // 用自动调度器解决多线程访问同一个控件
            resultShower.Dispatcher.Invoke(
                    () =>
                    {
                        resultShower.Items.Add(String.Format("扫描地址：{0}\t　扫描用时{1}毫秒\t　主机名称：{2}", ip.ToString(), sw.ElapsedMilliseconds, name));
                    }
            );
        }
    }
}