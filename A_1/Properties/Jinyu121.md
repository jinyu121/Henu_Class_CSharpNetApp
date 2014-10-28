[小金鱼儿](mailto:jinyu121@126.com)
[小金鱼儿的博客](http://haoyu.de)
说明

下面的程序是第一版

```
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
        public MainWindow()
        {
            InitializeComponent();
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
        MediaPlayer iPlayer;
        public MyRenderer()
        {
            iPlayer = CreateMediaPlayer();
        }
        public static MediaPlayer CreateMediaPlayer()
        {
            MediaPlayer iPlayer = new MediaPlayer();
            iPlayer.Open(new Uri(@"C:\Users\Public\Videos\Sample Videos\Wildlife.wmv"));
            return iPlayer;
        }
        /*
        protected override void OnDraw(DrawingContext drawingContext, StylusPointCollection stylusPoints, Geometry geometry, Brush fillBrush)
        {
            iPlayer.Play();
            drawingContext.DrawVideo(iPlayer, new Rect((Point)stylusPoints.First(), (Point)stylusPoints.Last()));
        }
         * */
    }

    /// <summary>
    /// 笔刷
    /// </summary>
    public partial class MyStroke : Stroke
    {
        MediaPlayer iPlayer = MyRenderer.CreateMediaPlayer();
        public MyStroke(StylusPointCollection points, DrawingAttributes da)
            : base(points, da)
        {
            while (StylusPoints.Count > 2)
            {
                StylusPoints.RemoveAt(StylusPoints.Count - 2);
            }
        }
        protected override void DrawCore(DrawingContext drawingContext, DrawingAttributes drawingAttributes)
        {
            iPlayer.Play();
            drawingContext.DrawVideo(iPlayer, new Rect((Point)StylusPoints.First(), (Point)StylusPoints.Last()));
        }
    }
}
```


目前正在用的这一版，所有的视频都是同一个，我不知道如何每次生成一个新的出来。因为没有拷贝构造方法。

如果想每次生成一个新的，就把Mian里面的Public Static 的给改掉，改成string传URI就好了。


写了两天了，终于能出点东西了……太恶心了

书上把所有东西都打包做成了InkObject，做得各种复杂。理了好久才理清楚……还是靠MSDN的例子才理清楚的

[小金鱼儿](mailto:jinyu121@126.com)
[小金鱼儿的博客](http://haoyu.de)