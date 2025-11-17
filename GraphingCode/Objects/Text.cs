using System;
using System.Drawing;
using System.Windows.Forms;

namespace Graphing.Objects
{
    public class Text : GraphObject
    {

        public string Value{get;set;}
        public Font Font { get; set; }
        public Color Color = Color.Black;
        public Color BackColor = Color.White;
        public StringAlignment HorizontalAlignment = StringAlignment.Center;
        public StringAlignment VarticalAlignment = StringAlignment.Center;

        public override string Get_Type => "Texts";

        public override string DiscriptionString()
        {
            return "";
        }
        public override string ToString()
        {
            return "";
        }

        public Controls.GraphControl Control_;

        public System.Drawing.Point Location;

        public Text(string Text_, Font Font_, Color Color_, Color BackColor_, Point Location_, string Name_)
        {
            SetName(Name_);
            Value = Text_;
            Font = Font_;
            Color = Color_;
            BackColor = BackColor_;
            Location = Location_;
        }
        public Text(string Text_, Font Font_, Color Color_, Color BackColor_,Point Location_)
        {
            Value = Text_;
            Font = Font_;
            Color = Color_;
            BackColor = BackColor_;
            Location = Location_;
        }

        public SizeF BoxSize()
        {
            using (Bitmap b = new Bitmap(100,100))
            {
                using (Graphics g = Graphics.FromImage(b))
                {
                    return g.MeasureString(Value, Font);
                }
            }      
        }

        public Text()
        {
            Font = new Font("Segoe UI Semilight", 9);
        }

        public override void DrawTo(Graphics g)
        {
            using (Brush brush = new SolidBrush(Color))
            {
                Brush brush_ = MainFunctions.IsColorDark(Color) ? Brushes.White : Brushes.Black;
                float x = 0, y = 0;
                SizeF size = g.MeasureString(Value, Font);
                x = Align(Location.X, size.Width, HorizontalAlignment);
                y = Align(Location.Y, size.Height, VarticalAlignment);
                using (var rectbrush = new SolidBrush(BackColor))
                    g.FillRectangle(rectbrush, x, y, size.Width, size.Height);
                g.DrawString(Value, Font, brush_, x - 1, y);
                g.DrawString(Value, Font, brush_, x + 1, y);
                g.DrawString(Value, Font, brush_, x, y - 1);
                g.DrawString(Value, Font, brush_, x, y + 1);
                g.DrawString(Value, Font, brush, x, y);
            }
        }

        public static void DrawText(Graphics g,string Text_, Font Font_, Color Color_, Color BackColor_, Point Location_)
        {
            Text t = new Text(Text_, Font_, Color_, BackColor_, Location_);
            t.DrawTo(g);
        }

        static float Align(float x, float size, StringAlignment align) =>
            align == StringAlignment.Center ? x - size / 2 :
            align == StringAlignment.Far ? x - size : x;

        public override System.Xml.Linq.XElement GetAsXml()
        {
            System.Xml.Linq.XElement element = new System.Xml.Linq.XElement("Text");
            element.SetAttributeValue("Name", Name);
            element.SetElementValue("Font", Font.ToString());
            element.SetElementValue("Color", Color.ToString());
            element.SetElementValue("Location", Location.ToString());
            return element;
        }

        public override void UpdateScriptText()
        {
            throw new NotImplementedException();
        }

    }
}
