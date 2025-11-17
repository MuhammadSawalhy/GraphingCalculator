using System;
using System.Drawing;

namespace Graphing.Objects
{
    public class Line : PointsDependant
    {
        public override PointDependantType Type { get; } = PointDependantType.Line;
        public override string Get_Type => "Line";
        public override int PointNumberAtLeast { get; } = 2;
        public override int PointNumberAtMost { get; } = 2;
        public Line(GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(GraphSetting_, control)
        {
        }

        public Line(PointsDependant pd, GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(pd, GraphSetting_, control)
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
                Graphics.DrawLine(Pen, Points[0].Location, Points[1].Location);
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
