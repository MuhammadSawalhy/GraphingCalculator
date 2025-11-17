using System;

using System.Linq;

using System.Drawing;

namespace Graphing.Objects
{
    public class TwoPSemiCircle : PointsDependant
    {
        public override PointDependantType Type { get; } = PointDependantType.TwoPSemiCircle;
        public override string Get_Type => "SemiCircle";
        public override int PointNumberAtLeast { get; } = 2;
        public override int PointNumberAtMost { get; } = 2;

        public TwoPSemiCircle(GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(GraphSetting_, control)
        {
        }
        public TwoPSemiCircle(PointsDependant pd, GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(pd, GraphSetting_, control)
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

                double x1, y1, x2, y2;
                x1 = Points[0].Get_X_Value();
                y1 = Points[0].Get_Y_Value();
                x2 = Points[1].Get_X_Value();
                y2 = Points[1].Get_Y_Value();
                double x_center = (x1 + x2) / 2;
                double y_center = (y1 + y2) / 2;
                double radius = Math.Pow((Math.Pow((x_center - x1), 2) + Math.Pow((y_center - y1), 2)), 0.5);
                if (radius == 0)
                {
                    return;
                }

                double sloop;
                try
                {
                    sloop = MainFunctions.GetSloop(Points[1], Points[0]);
                }
                catch
                {
                    return;
                }

                MainFunctions.Draw_arc(Graphics, Pen, sloop, Math.PI, new PointF(GraphSetting.xChangeToPixel(x_center, y_center), GraphSetting.yChangeToPixel(x_center, y_center)), Math.Ceiling(radius * GraphSetting.X_Stretch), Math.Ceiling(radius * GraphSetting.Y_Stretch), true);
            }
            catch (Exception ex)
            {
                if (throwError)
                    throw ex;
                ShowError(ex.Message);
            }
        }

        public override string DiscriptionString()
        {
            double x1, y1, x2, y2;
            x1 = Points[0].Get_X_Value();
            y1 = Points[0].Get_Y_Value();
            x2 = Points[1].Get_X_Value();
            y2 = Points[1].Get_Y_Value();
            double x_center = (x1 + x2) / 2;
            double y_center = (y1 + y2) / 2;
            double radius = Math.Pow((Math.Pow((x_center - x1), 2) + Math.Pow((y_center - y1), 2)), 0.5);
            double sloop1, sloop2;
            try
            {
                sloop1 = MainFunctions.GetSloop(Points[1], Points[0]);
                sloop2 = MainFunctions.GetSloop(Points[1], Points[2]);
            }
            catch
            {
                return "";
            }
            return "(" + GraphSetting.var + " - " + x2 + ")^2 + (y - " + y2 + ")^2 = " + Math.Pow(radius, 2);
        }


    }
}
