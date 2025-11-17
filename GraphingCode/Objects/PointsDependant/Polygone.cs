using System;
using System.Drawing;
using System.Collections.Generic;

namespace Graphing.Objects
{
    public class Polygone : PointsDependant
    {
        public override PointDependantType Type { get; } = PointDependantType.Polygone;
        public override string Get_Type => "Polygone";
        public override int PointNumberAtLeast { get; } = 0;
        public override int PointNumberAtMost { get; } = 0;
        public Polygone(GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(GraphSetting_, control)
        {
        }

        public Polygone(PointsDependant pd, GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(pd, GraphSetting_, control)
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
                Graphics.FillPolygon(new SolidBrush(Color.FromArgb(100, Pen.Color.R, Pen.Color.G, Pen.Color.B)), points.ToArray());
                points.Add(Points[0].Location);
                Graphics.DrawLines(Pen, points.ToArray());
            }
            catch (Exception ex)
            {
                if (throwError)
                    throw ex;
                ShowError(ex.Message);
            }

        }


    }
}
