using System;
using System.Collections.Generic;
using System.Drawing;
using System.Xml.Linq;
using System.Threading.Tasks;

using MathPackage;
using MathPackage.Operations;

namespace Graphing.Objects
{
    public class Points : GraphObject
    {
        #region Constructors
        public Points(Point _Location, GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(GraphSetting_, control)
        {
            Pen = new Pen(Color.RoyalBlue);
            Size = 10;
            X_Value = new Constant(GraphSetting.xChangeToCoor(_Location.X, _Location.Y));
            Y_Value = new Constant(GraphSetting.yChangeToCoor(_Location.X, _Location.Y));
        }
        public Points(Node XEquation, Node YEquation, GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(GraphSetting_, control)
        {
            Shape = PointShape.SolidCircle;
            Pen = new Pen(Color.RoyalBlue);
            Size = 10;
            Size = 10;
            X_Value = XEquation;
            Y_Value = YEquation;
        }
        public Points(GraphSetting GraphSetting_, Controls.GraphControl control = null) : base(GraphSetting_, control)
        {
            Shape = PointShape.SolidCircle;
            Pen = new Pen(Color.RoyalBlue);
            Size = 10;
            X_Value = new Constant(0);
            Y_Value = new Constant(0);
        }
        public Points(double XValue, double YValue, GraphSetting GraphSetting_, Controls.GraphControl control) : base(GraphSetting_, control)
        {
            Shape = PointShape.SolidCircle;
            Pen = new Pen(Color.RoyalBlue);
            Size = 10;
            X_Value = new Constant(XValue);
            Y_Value = new Constant(YValue);
        }
        #endregion

        #region Varaibles

        public override string Get_Type => "Points";

        public PointF Location
        {
            get
            {
                return new PointF(GraphSetting.xChangeToPixel(Get_X_Value(), Get_Y_Value()), GraphSetting.yChangeToPixel(Get_X_Value(), Get_Y_Value()));
            }
            set
            {
                x_static = false;
                y_static = false;
                X_Value = new Constant((value.X - GraphSetting.Center.X) / GraphSetting.X_Stretch);
                Y_Value = new Constant((GraphSetting.Center.Y - value.Y) / GraphSetting.Y_Stretch);
            }
        }

        protected bool Is_Selected_ = false;

        public bool Is_Selected
        {
            get
            {
                return Is_Selected_;
            }
            set
            {
                Is_Selected_ = value;
                if (!value)
                {
                    UpdatePDsDiscription();
                }
            }
        }

        public int Size { get; set; }

        public PointShape Shape { get; set; }

        public enum PointShape
        {
            SolidCircle,
            ShallowCircle,
            Square,
            Cross
        }

        public List<PointsDependant> Dependants = new List<PointsDependant>();

        #endregion

        #region Drawing

        public override void  Draw(bool throwError = false)
        {
            Bitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            Graphics = Graphics.FromImage(Bitmap);
            Graphics.SmoothingMode = MainFunctions.Smoothing;
            Graphics.CompositingQuality = MainFunctions.Composition;

            try
            {
                if (x_variabled || y_variabled)
                {
                    if (Is_Selected)
                    {
                        Color color;
                        if (MainFunctions.IsColorDark(Pen.Color))
                            color = Color.FromArgb(255, 255, 255, 255);
                        else
                            color = Color.FromArgb(255, 0, 0, 0);

                        Graphics.FillEllipse(new SolidBrush(Color.FromArgb(100, color.R, color.G, color.B)), Convert.ToInt32(GraphSetting.xChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2) - 4, Convert.ToInt32(GraphSetting.yChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2) - 4, Size + 8, Size + 8);
                        Graphics.FillEllipse(new SolidBrush(color), Convert.ToInt32(GraphSetting.xChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2), Convert.ToInt32(GraphSetting.yChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2), Size, Size);

                        return;
                    }
                    switch (Shape)
                    {
                        case PointShape.ShallowCircle:
                            {
                                Graphics.FillEllipse(new SolidBrush(Color.FromArgb(100, Pen.Color.R, Pen.Color.G, Pen.Color.B)), Convert.ToInt32(GraphSetting.xChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2) - 4, Convert.ToInt32(GraphSetting.yChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2) - 4, Size + 8, Size + 8);
                                Graphics.DrawEllipse(new Pen(Pen.Color, 3), Convert.ToInt32(GraphSetting.xChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2), Convert.ToInt32(GraphSetting.yChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2), Size, Size);
                                break;
                            }
                        case PointShape.SolidCircle:
                            {
                                Graphics.FillEllipse(new SolidBrush(Color.FromArgb(100, Pen.Color.R, Pen.Color.G, Pen.Color.B)), Convert.ToInt32(GraphSetting.xChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2) - 4, Convert.ToInt32(GraphSetting.yChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2) - 4, Size + 8, Size + 8);
                                Graphics.FillEllipse(new SolidBrush(Pen.Color), Convert.ToInt32(GraphSetting.xChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2), Convert.ToInt32(GraphSetting.yChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2), Size, Size);
                                break;
                            }
                        case PointShape.Square:
                            {
                                break;
                            }
                        case PointShape.Cross:
                            {
                                break;
                            }
                    }
                }
                else
                {
                    if (Is_Selected)
                    {
                        Color color;
                        if (MainFunctions.IsColorDark(Pen.Color))
                            color = Color.FromArgb(255, 255, 255, 255);
                        else
                            color = Color.FromArgb(255, 0, 0, 0);

                        Graphics.FillEllipse(new SolidBrush(color), Convert.ToInt32(GraphSetting.xChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2), Convert.ToInt32(GraphSetting.yChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2), Size, Size);
                        return;
                    }
                    switch (Shape)
                    {
                        case PointShape.ShallowCircle:
                            {
                                Graphics.DrawEllipse(new Pen(Pen.Color, 3), Convert.ToInt32(GraphSetting.xChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2), Convert.ToInt32(GraphSetting.yChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2), Size, Size);
                                break;
                            }

                        case PointShape.SolidCircle:
                            {
                                Graphics.FillEllipse(new SolidBrush(Pen.Color), Convert.ToInt32(GraphSetting.xChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2), Convert.ToInt32(GraphSetting.yChangeToPixel(Get_X_Value(), Get_Y_Value()) - Size / 2), Size, Size);
                                break;
                            }
                        case PointShape.Square:
                            {
                                break;
                            }
                        case PointShape.Cross:
                            {
                                break;
                            }
                    }
                }
            }

            catch (Exception ex)
            {
                if (throwError)
                    throw ex;
                ShowError(ex.Message);
            }
        }
    
        #endregion

        #region X and Y

        public bool x_variabled { get; set; }

        public bool y_variabled { get; set; }

        public bool x_static { get; set; }

        public bool y_static { get; set; }

        /// <summary>
        /// For the properities below
        /// </summary>
        Node X_Value_ = new Constant(0), Y_Value_ = new Constant(0);

        public Node X_Value
        {
            get
            {
                return X_Value_;
            }
            set
            {
                if (value.ContainsVaraible)
                {
                    if (value is MathPackage.Operations.Variable)
                    {
                        /// This means that the value is <see cref="MathPackage.Operations.Variable"> 
                        /// and when the value is changed in the future the value of this <see cref="MathPackage.Operations.Variable"> will be modified.
                        x_variabled = true;
                        X_Value_ = value;
                    }
                    else
                    {
                        /// This means that the position of this point in the coordinates won't be modified unless you adjust it from the point window editing
                        X_Value_ = value;
                        x_static = true;
                    }
                }
                /// value is double
                else
                {
                    /// if variabled : change the value of the <see cref="MathPackage.Operations.Variable"> or the slider
                    if (x_variabled)
                    {
                        for (int i = 0; i < GraphSetting.Sketch.Sliders.Count; i++)
                        {
                            if (GraphSetting.Sketch.Sliders.Item(i).Name == ((MathPackage.Operations.Variable)X_Value_).Name.Name)
                            {
                                GraphSetting.Sketch.Sliders.Item(i).SetValue(value.Calculate(GraphSetting.CalculationSetting, new Dictionary<Loyc.Symbol, Node>()));
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (!x_static)
                        {
                            X_Value_ = value;
                            x_variabled = false;
                        }
                    }
                }
            }
        }

        public Node Y_Value
        {
            get
            {
                return Y_Value_;
            }
            set
            {
                if (value.ContainsVaraible)
                {
                    if (value is MathPackage.Operations.Variable)
                    {
                        /// This means that the value is <see cref="MathPackage.Operations.Variable"> 
                        /// and when the value is changed in the future the value of this <see cref="MathPackage.Operations.Variable"> will be modified.
                        y_variabled = true;
                        Y_Value_ = value;
                    }
                    else
                    {
                        /// This means that the position of this point in the coordinates won't be modified unless you adjust it from the point window editing
                        Y_Value_ = value;
                        y_static = true;
                    }
                }
                /// value is double
                else
                {
                    /// if variabled : change the value of the <see cref="MathPackage.Operations.Variable"> or the slider
                    if (y_variabled)
                    {
                        for (int i = 0; i < GraphSetting.Sketch.Sliders.Count; i++)
                        {
                            if (GraphSetting.Sketch.Sliders.Item(i).Name == ((MathPackage.Operations.Variable)Y_Value_).Name.Name)
                            {
                                GraphSetting.Sketch.Sliders.Item(i).SetValue(value.Calculate(GraphSetting.CalculationSetting, new Dictionary<Loyc.Symbol, Node>()));
                                return;
                            }
                        }
                    }
                    else
                    {
                        if (!y_static)
                        {
                            Y_Value_ = value;
                            y_variabled = false;
                        }
                    }
                }
            }
        }

        public double Get_X_Value()
        {
            return X_Value.Calculate(GraphSetting.CalculationSetting, new Dictionary<Loyc.Symbol, Node>());
        }

        public double Get_Y_Value()
        {
            return Y_Value.Calculate(GraphSetting.CalculationSetting, new Dictionary<Loyc.Symbol, Node>());
        }

        #endregion

        #region Methods

        public override void SetName(string Name_)
        {
            base.SetName(Name_);
            //adjust the name
            foreach (var pd in Dependants)
                pd.Control.Discription.Text = pd.DiscriptionString();
        }
        public void UpdatePDsDiscription()
        {
            foreach (var pd in Dependants)
                pd.Control.Discription.Text = pd.DiscriptionString();
        }
        public override string ToString()
        {
            string returned = string.IsNullOrEmpty(Name) ? $"({X_Value.ToString()}, {Y_Value.ToString()})" : $"{Name} = ({X_Value.ToString()}, {Y_Value.ToString()})";
            return returned;
        }
        public override string DiscriptionString()
        {
            return "";
        }
        public override void UpdateScriptText()
        {
            this.Control.Script.Text = this.ToString();
        }
        public override System.Xml.Linq.XElement GetAsXml()
        {
            XElement element = new XElement("Point");
            element.SetAttributeValue("Name", Name);

            if (!Visible) element.SetAttributeValue("Visible", Visible);
            if (Size != 10) element.SetAttributeValue("Size", Size);
            if (Pen.Color != Color.RoyalBlue) element.SetAttributeValue("Color", MainFunctions.GetColorAsString(Pen.Color));
            if (Shape != PointShape.SolidCircle) element.SetAttributeValue("Shape", Shape.ToString());

            XElement node = new XElement("x");
            node.SetAttributeValue("Value", X_Value.ToString());
            element.Add(node);
            node = new XElement("y");
            node.SetAttributeValue("Value", Y_Value.ToString());
            element.Add(node);
            return element;
        }
        public override void Delete(bool removeControl)
        {
            while (Dependants.Count > 0)
            {
                Dependants[0].Delete(true);
            }
            GraphSetting.Sketch.Objects.Delete(this, removeControl);
        }

        #endregion

    }
}
