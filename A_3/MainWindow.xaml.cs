using System;
using System.Collections.Concurrent;
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

namespace WpfApplication1
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StringBuilder sb = new StringBuilder();
            ipt.Items.Add("2014 11 11");
            ipt.Items.Add("4134 3 1390");
            ipt.Items.Add("4134 313 90");
            ipt.Items.Add("413 431 390");
        }

        private void GOGOGO_Click(object sender, RoutedEventArgs e)
        {
            foreach (var i in ipt.Items)
            {
                string[] str = (i as String).Split();
                try
                {
                    pro p = new pro(int.Parse(str[0]), int.Parse(str[1]), int.Parse(str[2]), resultShower);
                    // 分别尝试异步和并行
                    Random r = new Random();
                    if (r.Next(2) == 0)
                    {
                        // 并行
                        // 注意：以下代码解决使用Parallel.Invoke() 导致界面死掉的问题
                        ParallelOptions op = new ParallelOptions();
                        op.TaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
                        Parallel.Invoke(op, () => { p.process(); });
                    }
                    else
                    {
                        // 异步
                        new Task(() => { p.process(); }).Start();
                    }
                }
                catch { }
            }
        }

        private void ipt_Go_Click(object sender, RoutedEventArgs e)
        {
            ipt.Items.Add(ipt_A.Text + " " + ipt_B.Text + " " + ipt_C.Text);
            ipt_A.Text = "";
            ipt_B.Text = "";
            ipt_C.Text = "";
        }
    }
    public class pro
    {
        int a, b, c;
        ListBox lv;
        // 这里使用并行选项
        ParallelOptions op;
        public pro(int a, int b, int c, ListBox lv)
        {
            this.a = a;
            this.b = b;
            this.c = c;
            this.lv = lv;
        }
        public void process()
        {
            int[,] x = new int[a, b];
            int[,] y = new int[b, c];
            int[,] z = new int[a, c];
            Stopwatch iWatch = new Stopwatch();
            // 初始化
            // 这里使用了“按范围分区”
            OrderablePartitioner<Tuple<int, int>> range;
            range = Partitioner.Create(0, a);
            Parallel.ForEach(range, (r, loopstate) =>
            {
                for (int i = r.Item1; i < r.Item2; i++)
                    for (int j = 0; j < b; j++)
                        x[i, j] = i + j;
            });
            range = Partitioner.Create(0, b);
            Parallel.ForEach(range, (r, loopstate) =>
            {
                for (int i = r.Item1; i < r.Item2; i++)
                    for (int j = 0; j < c; j++)
                        y[i, j] = i + j;
            });
            op = new ParallelOptions();
            op.MaxDegreeOfParallelism = -1;
            // 开始算
            iWatch.Start();
            Parallel.For(0, b, op, (k) =>
            {
                for (int i = 0; i < a; i++)
                    for (int j = 0; j < c; j++)
                        z[i, j] += (x[i, k] * y[k, j]);
            });
            iWatch.Stop();
            // 完成了
            lv.Dispatcher.Invoke(() =>
            {
                StringBuilder ss = new StringBuilder();
                ss.AppendLine(String.Format("计算用时：{0}毫秒：", iWatch.ElapsedMilliseconds));
                ss.AppendLine(String.Format("\t数组 1 大小：{0}*{1}", a, b));
                ss.AppendLine(String.Format("\t数组 2 大小：{0}*{1}", b, c));
                ss.AppendLine(String.Format("\t数组 3 大小：{0}*{1}", a, c));
                ss.AppendLine(String.Format("\t最大并行度：{0}", op.MaxDegreeOfParallelism));
                ss.AppendLine();
                lv.Items.Add(ss.ToString());
            });
        }
    }
}
