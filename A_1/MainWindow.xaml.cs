using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Input.StylusPlugIns;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace A_1
{
    /// <summary>
    /// 程序
    /// </summary>
    public partial class MainWindow : Window
    {
        public static String iPlayer;
        protected String[] players;
        public MainWindow()
        {
            InitializeComponent();
            players = new String[2];
            players[0] = @"C:\Users\Public\Videos\Sample Videos\Wildlife.wmv";
            players[1] = @"C:\Users\Public\Videos\Sample Videos\Wildlife.wmv";
            iPlayer = players[0];
        }

        private void change_video(object sender, RoutedEventArgs e)
        {
            RadioButton rb = e.Source as RadioButton;
            switch (rb.Tag.ToString())
            {
                case "video1": iPlayer = players[0]; break;
                case "video2": iPlayer = players[1]; break;
            }
        }
    }

    /// <summary>
    /// 画板
    /// </summary>
    public partial class MyInkCanvas : InkCanvas
    {
        // 建议使用 构造函数 而不是 OnInitialized 回调函数。
        // 如果不写构造函数，xaml里面不认这个控件
        public MyInkCanvas()
        {
            DynamicRenderer dr = new MyRenderer();
            dr.DrawingAttributes = this.DefaultDrawingAttributes;
            this.DynamicRenderer = dr;
        }
        protected override void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e)
        {
            base.OnStrokeCollected(e);
            StylusPointCollection spc = e.Stroke.StylusPoints.Clone();

            this.Strokes.Remove(e.Stroke);
            this.Strokes.Add(new MyStroke(spc, e.Stroke.DrawingAttributes));
        }
    }
    /// <summary>
    /// 提出器
    /// </summary>
    public partial class MyRenderer : DynamicRenderer
    {
        protected Point firstPoint;
        protected override void OnDraw(DrawingContext drawingContext, StylusPointCollection stylusPoints, Geometry geometry, Brush fillBrush)
        {
            drawingContext.DrawRectangle(new SolidColorBrush(Colors.Black), null, new Rect(firstPoint, (Point)stylusPoints.Last()));
        }
        protected override void OnStylusDown(RawStylusInput rawStylusInput)
        {
            base.OnStylusDown(rawStylusInput);
            // 记录下起点，为了能拖出来一个完整的矩形而不是矩形序列
            firstPoint = (Point)rawStylusInput.GetStylusPoints().First();
        }
        protected override void OnStylusMove(RawStylusInput rawStylusInput)
        {
            StylusPointCollection stylusPoints = rawStylusInput.GetStylusPoints();
            //// 让收集到的点只剩下起点和终点
            while (stylusPoints.Count > 2)
            {
                stylusPoints.RemoveAt(stylusPoints.Count - 2);
            }

            this.Reset(Stylus.CurrentStylusDevice, stylusPoints);
            base.OnStylusMove(rawStylusInput);
        }
    }

    /// <summary>
    /// 笔刷
    /// </summary>
    public partial class MyStroke : Stroke
    {
        public MyStroke(StylusPointCollection points, DrawingAttributes da)
            : base(points, da)
        {
        }
        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            MediaPlayer iPlayer = new MediaPlayer();
            iPlayer.Open(new Uri(MainWindow.iPlayer));
            iPlayer.Play();
            drawingContext.DrawVideo(iPlayer, new Rect((Point)StylusPoints.First(), (Point)StylusPoints.Last()));
        }
    }
}