using System;
using System.Collections.Generic;
using System.Xml.Linq;

using System.Drawing;

namespace Graphing.Objects
{
    public abstract class PointsDependant : GraphObject
    {

        public PointsDependant(GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(GraphSetting_, control)
        {
            Pen = new Pen(Color.FromArgb(43, 44, 51), 2);
        }
        public PointsDependant(PointsDependant pd, GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(GraphSetting_, control)
        {
            Pen = new Pen(Color.FromArgb(43, 44, 51), 2);
            this.Points.AddRange(pd.Points.ToArray());
            this.Type = pd.Type;
        }

        public List<Points> Points = new List<Points>();
        public virtual int PointNumberAtLeast { get; }
        public virtual int PointNumberAtMost { get; }
        public virtual PointDependantType Type { get; }
        public enum PointDependantType
        {
            Angle,
            TwoPCircle,
            TwoPCircle2,
            ThreePCircle,
            TwoPSemiCircle,
            TwoPSemiCircle2,
            Length,
            Line,
            Polygone,
            Curve,
        }

        public override string Get_Type => "PointsDependant";
        public override void Delete(bool removeControl)
        {
            foreach(Points p in Points)
            {
                p.Dependants.Remove(this);
            }
            GraphSetting.Sketch.Objects.Delete(this, removeControl);
        }
        public override string DiscriptionString()
        {
            return "";
        }
        public override string ToString()
        {
            string points_ = "";
            foreach(Points p  in Points)
            {
                points_ += string.IsNullOrEmpty(points_) ? (string.IsNullOrEmpty(p.Name) ? p.ToString() : p.Name) : 
                    (string.IsNullOrEmpty(p.Name) ? ", " + p.ToString() : ", " + p.Name);                  
            }
            return Name + " = " + Get_Type + "(" + points_ + ")";
        }
        public override void UpdateScriptText()
        {
            this.Control.Script.Text = this.ToString();
        }
        public override System.Xml.Linq.XElement GetAsXml()
        {
            XElement element = new XElement(Get_Type);

            element.SetAttributeValue("Name", Name);
            if (!Visible) element.SetAttributeValue("Visible", Visible);

            string points = "";
            foreach (Points p in Points)
            {
                if (points == "")
                    points += string.IsNullOrEmpty(p.Name) ? p.ToString() : p.Name;
                else
                    points += ", " + (string.IsNullOrEmpty(p.Name) ? p.ToString() : p.Name);
            }
            XElement node = new XElement("Points");
            node.SetAttributeValue("Value", "{" + points + "}");
            element.Add(node);

            node = new XElement("Pen");
            node.SetAttributeValue("Value", MainFunctions.GetPenAsString(Pen));
            element.Add(node);


            return element;
        }

    }
}
