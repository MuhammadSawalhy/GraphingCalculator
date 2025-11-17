using System;
using System.Drawing;


namespace Graphing.Objects
{
    public class ThreePCircle : PointsDependant
    {
        public override PointDependantType Type { get; } = PointDependantType.ThreePCircle;
        public override string Get_Type => "Circle";
        public override int PointNumberAtLeast { get; } = 3;
        public override int PointNumberAtMost { get; } = 3;

        public ThreePCircle(GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(GraphSetting_, control)
        {
        }
        public ThreePCircle(PointsDependant pd, GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(pd, GraphSetting_, control)
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
                double x1, y1, x2, y2, x3, y3;
                x1 = Points[0].Get_X_Value();
                y1 = Points[0].Get_Y_Value();
                x2 = Points[1].Get_X_Value();
                y2 = Points[1].Get_Y_Value();
                x3 = Points[2].Get_X_Value();
                y3 = Points[2].Get_Y_Value();
                double x_center;
                double y_center;
line:
                x_center = ((y2 - y1) * (Math.Pow(x2, 2) + Math.Pow(y2, 2) - Math.Pow(x3, 2) - Math.Pow(y3, 2)) + (y3 - y2) * (Math.Pow(x2, 2) + Math.Pow(y2, 2) - Math.Pow(x1, 2) - Math.Pow(y1, 2))) / ((x2 - x3) * (y2 - y1) + (x2 - x1) * (y3 - y2)) / 2;
                if (double.IsNaN(x_center) || double.IsInfinity(x_center)) { return; }
                y_center = (-(Math.Pow(x3, 2) + Math.Pow(y3, 2)) + (Math.Pow(x2, 2) + Math.Pow(y2, 2)) - 2 * x_center * (x2 - x3)) / (2 * (y2 - y3));
                if(double.IsNaN(y_center) || double.IsInfinity(y_center))
                {
                    if (x3 == x1)
                        y_center = (y1 + y3) / 2;
                    else if (x2 == x1)
                        y_center = (y2 + y1) / 2;
                    else
                    {
                        double a, a1;
                        a = x1; a1 = y1; x1 = x2; y1 = y2; x2 = a; y2 = a1;
                        goto line;
                    }
                }
                double radius = Math.Pow((Math.Pow((x_center - x1), 2) + Math.Pow((y_center - y1), 2)), 0.5);
                if (radius == 0)
                {
                    return;
                }
                Graphics.DrawEllipse(Pen, (float)GraphSetting.xChangeToPixel(x_center - radius, y_center + radius), (float)GraphSetting.yChangeToPixel(x_center - radius, y_center + radius), (float)(2 * radius * GraphSetting.X_Stretch), (float)(2 * radius * GraphSetting.Y_Stretch));
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
            double x1, y1, x2, y2, x3, y3;
            x1 = Points[0].Get_X_Value();
            y1 = Points[0].Get_Y_Value();
            x2 = Points[1].Get_X_Value();
            y2 = Points[1].Get_Y_Value();
            x3 = Points[2].Get_X_Value();
            y3 = Points[2].Get_Y_Value();
            double x_center;
            double y_center;
            line:
            ;
            try
            {
                x_center = ((y2 - y1) * (Math.Pow(x2, 2) + Math.Pow(y2, 2) - Math.Pow(x3, 2) - Math.Pow(y3, 2)) + (y3 - y2) * (Math.Pow(x2, 2) + Math.Pow(y2, 2) - Math.Pow(x1, 2) - Math.Pow(y1, 2))) / ((x2 - x3) * (y2 - y1) + (x2 - x1) * (y3 - y2)) / 2;
            }
            catch
            {
                return "";
            }
            try
            {
                y_center = (-(Math.Pow(x3, 2) + Math.Pow(y3, 2)) + (Math.Pow(x2, 2) + Math.Pow(y2, 2)) - 2 * x_center * (x2 - x3)) / (2 * (y2 - y3));
            }
            catch
            {
                if (x3 == x1)
                    y_center = (y1 + y3) / 2;
                else if (x2 == x1)
                    y_center = (y2 + y1) / 2;
                else
                {
                    double a, a1;
                    a = x1; a1 = y1; x1 = x2; y1 = y2; x2 = a; y2 = a1;
                    goto line;
                }
            }
            double radius = Math.Pow((Math.Pow((x_center - x1), 2) + Math.Pow((y_center - y1), 2)), 0.5);

            return "(" + GraphSetting.var + " - " + x_center + ")^2 + (y - " + y_center + ")^2 = " + Math.Pow(radius, 2);
        }



    }
}
