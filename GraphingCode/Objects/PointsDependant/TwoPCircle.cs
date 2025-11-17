using System;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;

namespace Graphing.Objects
{
    public class TwoPCircle : PointsDependant
    {

        public override PointDependantType Type { get; } = PointDependantType.TwoPCircle;
        public override string Get_Type => "Circle";
        public override int PointNumberAtLeast { get; } = 2;
        public override int PointNumberAtMost { get; } = 2;

        public TwoPCircle(GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(GraphSetting_, control)
        {
        }

        public TwoPCircle(PointsDependant pd, GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(pd, GraphSetting_, control)
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
                Graphics.DrawEllipse(Pen, GraphSetting.xChangeToPixel(vars[0] - vars[2], vars[1] + vars[2]), GraphSetting.yChangeToPixel(vars[0] - vars[2], vars[1] + vars[2]), (float)(2 * vars[2] * GraphSetting.X_Stretch), (float)(2 * vars[2] * GraphSetting.Y_Stretch));
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

            double x1, y1;
            x1 = Points[0].Get_X_Value();
            y1 = Points[0].Get_Y_Value();
            double radius = Math.Pow(Math.Pow((Points[1].Get_X_Value() - x1), 2) + Math.Pow((Points[1].Get_Y_Value() - y1), 2), 0.5);

            return new double[] { x1, y1, radius };

        }

        public override string DiscriptionString()
        {
            double[] vars = GetCenter();
            return "(" + GraphSetting.var + " - " + vars[0] + ")^2 + (y - " + vars[1] + ")^2 = " + Math.Pow(vars[2], 2);
        }

    }
}
