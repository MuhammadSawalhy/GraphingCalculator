using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Loyc;
namespace Graphing.Objects
{
    public abstract class GraphObject : Object, IDisposable
    {
        public GraphObject(GraphSetting GraphSetting_, Controls.GraphControl Control_)
        {
            GraphSetting = GraphSetting_;
            Control = Control_;
            Bitmap = new Bitmap(GraphSetting_.Width, GraphSetting_.Height);
        }
        public GraphObject(string Text_, Font Font_, Color Color_, Color BackColor_, string Name_) { }
        public GraphObject() { }

        #region "Name"

        public string Name { get; internal set; }

        public Controls.GraphControl Control;

        public virtual void SetName(string Name_)
        {
            MainFunctions.CheckName(Name_);

            if (GraphSetting.IsNameUsed(Name_,new List<string>(new[] { Name })))
            {
                throw new Exception($"This name \"{Name_}\" has already been used.");
            }

            Name = Name_;
        }

        #endregion

        #region Variables
        public virtual Pen Pen { get; set; }
        public Bitmap Bitmap;
        public Graphics Graphics;
        public GraphSetting GraphSetting;
        public virtual bool Visible { get; set; } = true;
        public virtual string Get_Type { get; }
        #endregion

        #region Drawing
        public virtual void Draw(bool throwError = false)
        {
            throw new NotImplementedException();
        }

        public virtual void Draw(PointF center) { }
        public virtual void DrawTo(Graphics g, int x , int y)
        {
            g.DrawImage(Bitmap,x,y);
        }
        public virtual void DrawTo(Graphics g)
        {
            g.DrawImage(Bitmap, 0, 0);
        }
        #endregion

        #region Methods

        public abstract System.Xml.Linq.XElement GetAsXml();

        public abstract string DiscriptionString();
        public abstract override string ToString();
        public abstract void UpdateScriptText();
        public virtual void Delete(bool removeControl) { }
        public virtual void Delete() { }

        public virtual void Dispose() { GC.SuppressFinalize(this); }

        public virtual void ShowError(string message)
        {
            if (Control != null)
            {
                Control.ShowError(message);
                Visible = false;
            }
        }

        #endregion
    }
}
