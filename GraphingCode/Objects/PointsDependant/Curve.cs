using System;
using System.Collections.Generic;
using System.Drawing;

namespace Graphing.Objects
{
    public class Curve : PointsDependant
    {
        public override PointDependantType Type { get; } = PointDependantType.Curve;
        public override string Get_Type => "Curve";
        public override int PointNumberAtLeast { get; } = 2;
        public override int PointNumberAtMost { get; } = int.MaxValue;
        public MathPackage.Main.AngleType angletype = MathPackage.Main.AngleType.Degree;

        public Curve(GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(GraphSetting_, control)
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
                List<PointF> points = new List<PointF>();
                for (int i = 0; i < Points.Count; i++)
                {
                    points.Add(Points[i].Location);
                }
                Graphics.DrawCurve(Pen, points.ToArray());

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
            return string.Empty;
        }

    }
}
