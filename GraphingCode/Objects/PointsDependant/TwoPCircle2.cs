using System;
using System.Linq;
using System.Drawing;

namespace Graphing.Objects
{
    public class TwoPCircle2 : PointsDependant
    {

        public override PointDependantType Type { get; } = PointDependantType.TwoPCircle2;
        public override string Get_Type => "Circle2";
        public override int PointNumberAtLeast { get; } = 2;
        public override int PointNumberAtMost { get; } = 2;

        public TwoPCircle2(GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(GraphSetting_, control)
        {
        }

        public TwoPCircle2(PointsDependant pd, GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(pd, GraphSetting_, control)
        {
        }

        public override void Draw(bool throwError = false)
        {
            try
            {

                Bitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
                Graphics = Graphics.FromImage(Bitmap);
                Graphics.SmoothingMode = MainFunctions.Smoothing;
                Graphics.CompositingQuality = MainFunctions.Composition;
                ///x_center, y_center, radius
                double[] vars;
                try
                {
                    vars = GetCenter();
                }
                catch
                {
                    return;
                }
                Graphics.DrawEllipse(Pen, (int)GraphSetting.xChangeToPixel(vars[0] - vars[2], vars[1] + vars[2]), (int)GraphSetting.yChangeToPixel(vars[0] - vars[2], vars[1] + vars[2]), (int)(2 * vars[2] * GraphSetting.X_Stretch), (int)(2 * vars[2] * GraphSetting.Y_Stretch));
            }
            catch (Exception ex)
            {
                if (throwError)
                    throw ex;
                ShowError(ex.Message);
            }
        }

        private double[] GetCenter()
        {
            double x1, y1, x2, y2;
            x1 = Points[0].Get_X_Value();
            y1 = Points[0].Get_Y_Value();
            x2 = Points[1].Get_X_Value();
            y2 = Points[1].Get_Y_Value();
            double x_center = (x1 + x2) / (double)2;
            double y_center = (y1 + y2) / (double)2;
            double radius = Math.Pow((Math.Pow((x_center - x1), 2) + Math.Pow((y_center - y1), 2)), 0.5);

            return new double[] { x_center, y_center, radius };

        }

        public override string DiscriptionString()
        {
            double[] vars = GetCenter();
            return "(" + GraphSetting.var + " - " + vars[0] + ")^2 + (y - " + vars[1] + ")^2 = " + Math.Pow(vars[2], 2);
        }


    }
}
