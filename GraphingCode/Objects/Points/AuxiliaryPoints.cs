using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace Graphing.Objects
{
    public class AuxiliaryPoints
    {

        public List<string> Type = new List<string>();

        public int Size = 10;

        public AuxiliaryPoints(double x, double y, PointF point, string title)
        {
            Title = title;
            X = x; Y = y;
            Point = new PointF(point.X - Size / 2, point.Y - Size / 2);
        }
        public override string ToString()
        {
            return Title + ": " + String.Format("({0}, {1})", MathPackage.Main.Approximate(X, "9", "0", "###").ToString(), MathPackage.Main.Approximate(Y, "9", "0", "###").ToString());
        }
        public string Title;

        public PointF Point;

        public double X, Y;

        public void DrawTo(Graphics g)
        {
                RectangleF rect = new RectangleF(Point, new SizeF(Size, Size));
                g.FillEllipse(new SolidBrush(Color.Black), rect);
        }

    }
}
