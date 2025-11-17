using System;
using System.Drawing;

namespace Graphing.Objects
{
    public class Distance : PointsDependant
    {
        public override PointDependantType Type { get; } = PointDependantType.Length;
        public override string Get_Type => "Distance";
        public override int PointNumberAtLeast { get; } = 2;
        public override int PointNumberAtMost { get; } = 2;
        public Distance(GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(GraphSetting_, control)
        {
        }

        public Distance(PointsDependant pd, GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(pd, GraphSetting_, control)
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

                PointF[] location_ = new PointF[2];
                location_[0] = Points[0].Location;
                location_[1] = Points[1].Location;

                Graphics.DrawLine(Pen, location_[0], location_[1]);
                Text Text = new Text();
                Text.Value = Math.Pow((Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2)), 0.5).ToString();
                Text.Font = GraphSetting.Font;
                Text.Color = Color.Black;
                Text.BackColor = Color.FromArgb(150, 255, 255, 255);
                Text.Location = new Point((int)GraphSetting.xChangeToPixel((x1 + x2) / 2, (y1 + y2) / 2), (int)GraphSetting.yChangeToPixel((x1 + x2) / 2, (y1 + y2) / 2));
                Text.DrawTo(Graphics);
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
            return Math.Pow((Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2)), 0.5).ToString();
        }


    }
}
