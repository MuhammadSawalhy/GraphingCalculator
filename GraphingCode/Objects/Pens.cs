using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace Graphing.Objects
{
    public abstract class Pen_ : GraphObject
    {

        public Pen_(GraphSetting GraphSetting_) : base(GraphSetting_, null)
        {
        }

        public override Pen Pen
        {
            get
            {
                return Pen;
            }
            set
            {
                Pen.Color = value.Color;
                Pen.Width = value.Width;
                Pen.DashStyle = value.DashStyle;
            }
        }
        /// <summary>
        ///         ''' as System.Drawing.Point with respect to the Origin(The center of the Drawing)
        ///         ''' </summary>
        public List<PointF> Points = new List<PointF>();

        public override string DiscriptionString()
        {
            return "";
        }
        public override string ToString()
        {
            return "";
        }

        public override void UpdateScriptText()
        {
            throw new NotImplementedException();
        }


    }

    public class StaticPen : Pen_
    {

        public override Pen Pen
        {
            get { return Pen; }
            set
            {
                Pen.Color = value.Color;
                Pen.Width = value.Width;
                Pen.DashStyle = value.DashStyle;
            }
        }


        public StaticPen(GraphSetting GraphSetting_) : base(GraphSetting_)
        {
        }
        public override void Draw(bool throwError = false)
        {
            Bitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            Graphics = Graphics.FromImage(Bitmap);
            Graphics.SmoothingMode = MainFunctions.Smoothing;
            Graphics.CompositingQuality = MainFunctions.Composition;
            List<PointF> points_ = new List<PointF>();
            if (Points.Count == 1)
            {
                Graphics.FillEllipse(new SolidBrush(Pen.Color), Points[0].X, Points[0].Y, 2 * Pen.Width, 2 * Pen.Width);
                return;
            }
            else
                points_.AddRange(Points.ToArray());
            Graphics.DrawCurve(Pen, points_.ToArray());
        }
        public override string Get_Type => "StaticPen";
        public override void Delete()
        {
            GraphSetting.Sketch.Pens.DeleteStaticPen(this);
        }

        public override System.Xml.Linq.XElement GetAsXml()
        {
            System.Xml.Linq.XElement element = new System.Xml.Linq.XElement("StaticPen");
            string points = "";
            foreach (PointF p in Points)
            {
                if (points == "")
                    points += $"({p.X}, {p.Y})";
                else
                    points += ", " + $"({p.X}, {p.Y})";
            }
            element.SetElementValue("Points", "{" + points + "}");
            element.SetElementValue("Pen", MainFunctions.GetPenAsString(Pen));
            return element;
        }



    }

    public class DinamicPen : Pen_
    {
        public DinamicPen(GraphSetting GraphSetting_) : base(GraphSetting_)
        {
        }

        /// <summary>
        ///         ''' as System.Drawing.Point with respect to the Origin(The center of the Drawing)
        ///         ''' </summary>

        /// <summary>
        ///         ''' theGraphSetting.Zoom while drawing.
        ///         ''' </summary>

        public override string Get_Type => "DinamicPen";

        public override void Draw(bool throwError = false)
        {
            Bitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            Graphics = Graphics.FromImage(Bitmap);
            Graphics.SmoothingMode = MainFunctions.Smoothing;
            Graphics.CompositingQuality = MainFunctions.Composition;
            List<PointF> points_ = new List<PointF>();
            if (Points.Count == 1)
            {
                Graphics.FillEllipse(new SolidBrush(Pen.Color), Convert.ToInt32(GraphSetting.xChangeToPixel(Points[0].X, Points[0].Y) - Pen.Width), Convert.ToInt32(GraphSetting.yChangeToPixel(Points[0].X, Points[0].Y) - Pen.Width), 2 * Pen.Width, 2 * Pen.Width);
                return;
            }
            else
                for (int i = 0; i <= Points.Count - 1; i++)
                    points_.Add(new PointF(GraphSetting.xChangeToPixel(Points[i].X, Points[i].Y), GraphSetting.yChangeToPixel(Points[i].X, Points[i].Y)));

            Graphics.DrawLines(Pen, points_.ToArray());
        }

        public override void Delete()
        {
            GraphSetting.Sketch.Pens.DinamicPenArray.Remove(this);
        }

        public override System.Xml.Linq.XElement GetAsXml()
        {
            System.Xml.Linq.XElement element = new System.Xml.Linq.XElement("DinamicPen");
            string points = "";
            foreach (PointF p in Points)
            {
                if (points == "")
                    points += $"({p.X}, {p.Y})" ;
                else
                    points += ", " + $"({p.X}, {p.Y})";
            }
            element.SetElementValue("Points", "{" + points + "}");
            element.SetElementValue("Pen", MainFunctions.GetPenAsString(Pen));
            return element;
        }


    }

}
