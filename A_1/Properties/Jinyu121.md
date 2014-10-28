[С�����](mailto:jinyu121@126.com)
[С������Ĳ���](http://haoyu.de)
˵��

����ĳ����ǵ�һ��

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
    /// ����
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    /// <summary>
    /// ����
    /// </summary>
    public partial class MyInkCanvas : InkCanvas
    {
        // ����ʹ�� ���캯�� ������ OnInitialized �ص�������
        // �����д���캯����xaml���治������ؼ�
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
    /// �����
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
    /// ��ˢ
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


Ŀǰ�����õ���һ�棬���е���Ƶ����ͬһ�����Ҳ�֪�����ÿ������һ���µĳ�������Ϊû�п������췽����

�����ÿ������һ���µģ��Ͱ�Mian�����Public Static �ĸ��ĵ����ĳ�string��URI�ͺ��ˡ�


д�������ˣ������ܳ��㶫���ˡ���̫������

���ϰ����ж��������������InkObject�����ø��ָ��ӡ����˺þò�������������ǿ�MSDN�����Ӳ��������

[С�����](mailto:jinyu121@126.com)
[С������Ĳ���](http://haoyu.de)