using System;
using System.Collections.Generic;
using Loyc;
using Loyc.Collections;
using Loyc.Syntax;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Xml.Linq;
using System.Text;
using System.Linq;
using number = System.Double;	// Change this line to make a calculator for a different data type 

namespace Graphing.Objects
{

    public class ParametricFunc : Function
    {
        public ParametricFunc(string name, Symbol parameter, MathPackage.Node start, MathPackage.Node end, MathPackage.Node step, MathPackage.Node x_expression, MathPackage.Node y_expression, GraphSetting GraphSetting_, Controls.GraphControl Control = null) : base(GraphSetting_, Control)
        {
            x_Expression = x_expression;
            y_Expression = y_expression;
            Parameter = parameter;
            Start = start;
            End = end;
            Step = step;
            SetName(name);
        }
        public ParametricFunc(GraphSetting GraphSetting_, Controls.GraphControl Control = null) : base(GraphSetting_, Control)
        {
            Parameter = (Symbol)"t";
            Start = new MathPackage.Operations.Constant(-10);
            End = new MathPackage.Operations.Constant(10);
            Step = new MathPackage.Operations.Constant(0.01);
        }

        public override string Get_Type => "ParametricFunc";

        #region Calculation

        public MathPackage.Node x_Expression { get; set; }
        public MathPackage.Node y_Expression { get; set; }
        protected Symbol parameter_;
        public Symbol Parameter {
            get {
                return parameter_;
            }
            set {
                parameter_ = value;
                dic = new Dictionary<Symbol, MathPackage.Node>();
                dic.Add(value, null);
            }
        }
        public MathPackage.Node Start, End;
        protected MathPackage.Node Step_;
        public MathPackage.Node Step
        {
            get
            {
                return Step_;
            }
            set
            {
                if (value != null)
                {
                    Step_ = value;
                    StepIsAssigned = true;
                }
                else
                {
                    StepIsAssigned = false;
                    Step_ = new MathPackage.Operations.Constant((End.Calculate(GraphSetting.CalculationSetting, new Dictionary<Symbol, MathPackage.Node>()) - Start.Calculate(GraphSetting.CalculationSetting, new Dictionary<Symbol, MathPackage.Node>())) / 10000);
                }
            }
        }
        protected bool StepIsAssigned = false;
    
        /// <summary>
        /// the dictionary containing the <see cref="Parameter"/> that is considered as a temporary variable passed throw the Calculation method.
        /// </summary>
        protected Dictionary<Symbol, MathPackage.Node> dic;
        public double Get_X(double parameter)
        {
            dic[parameter_] = new MathPackage.Operations.Constant(parameter);
            return x_Expression.Calculate(GraphSetting.CalculationSetting, dic);
        }
        public double Get_Y(double parameter)
        {
            dic[parameter_] = new MathPackage.Operations.Constant(parameter);
            return y_Expression.Calculate(GraphSetting.CalculationSetting, dic);
        }

        #endregion

        #region "Drawing"

        public override void Draw(bool throwError = false)
        {
            Bitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            Graphics = Graphics.FromImage(Bitmap);
            Graphics.SmoothingMode = MainFunctions.Smoothing;
            Graphics.CompositingQuality = MainFunctions.Composition;
            try
            {
                if (!IsSelected)
                {
                    Render();
                }
                else
                {
                    RenderSelected();
                }
            }
            catch (Exception ex)
            {
                ShowError(ex.Message);
            }
        }

        public void RenderSelected()
        {
            Pen pen_ = new Pen(Color.FromArgb(100, Pen.Color.R, Pen.Color.G, Pen.Color.B), Pen.Width + 4);
            List<AuxiliaryPoints> aPoints = new List<AuxiliaryPoints>();

            int S1 = 0, S2;
            bool lastIdentified = false;

            List<PointF> points = new List<PointF>();
            number start_ = Start.Calculate(GraphSetting.CalculationSetting, new Dictionary<Symbol, MathPackage.Node>());
            number end_ = End.Calculate(GraphSetting.CalculationSetting, new Dictionary<Symbol, MathPackage.Node>());
            number step_ = Step.Calculate(GraphSetting.CalculationSetting, new Dictionary<Symbol, MathPackage.Node>());
            double x, y;
            PointF current;
            PointF mid, previous;
            x = Get_X(start_ + step_ / 2); y = Get_Y(start_ + step_ / 2);
            mid = new PointF(GraphSetting.xChangeToPixel(x, y), GraphSetting.yChangeToPixel(x, y));
            x = Get_X(start_); y = Get_Y(start_);
            previous = new PointF(GraphSetting.xChangeToPixel(x, y), GraphSetting.yChangeToPixel(x, y));
            for (number i = start_ + step_; i <= end_; i += step_)
            {
                Line:
         
                #region  Getting points

                //Now, I have x and y of the function.
                x = Get_X(i); y = Get_Y(i);
                current = new PointF(GraphSetting.xChangeToPixel(x, y), GraphSetting.yChangeToPixel(x, y));
                x = Get_X(i - step_ / 2); y = Get_Y(i - step_ / 2);
                mid = new PointF(GraphSetting.xChangeToPixel(x, y), GraphSetting.yChangeToPixel(x, y));

                #endregion

                #region  shearching for problems
                if ((float.IsNaN(current.X) || float.IsInfinity(current.X) ||
                    (float.IsNaN(current.Y) || float.IsInfinity(current.Y) ||
                    Math.Abs(current.X) > 1000000) ||
                    Math.Abs(current.Y) > 1000000))
                {
                    //DrawSlice
                    {
                        if (points.Count > 1)
                        {
                             Graphics.DrawLines(Pen, points.ToArray());
                           Graphics.DrawLines(pen_, points.ToArray());
                        }
                        else if (points.Count == 1)
                        {
                             Graphics.DrawLines(Pen, new[] { points[0], new PointF(points[0].X + 0.1f, points[0].Y) });
                           Graphics.DrawLines(pen_, new[] { points[0], new PointF(points[0].X + 0.1f, points[0].Y) });
                        }
                        points.Clear();
                    };
                    if (i < end_)
                    {
                        i += step_;
                        previous = current;
                        lastIdentified = false;
                        goto Line;
                    }
                    else
                        goto line2;
                }
                #endregion

                #region  shearching for AuxiliaryPoints


                if (lastIdentified)
                {
                    ///* if the first derivative (dy/dx) chandes its sign (+ or -) then there is a local extremum or if the dx/dy makes the same thing, we will look at the sloop of the line going throw the current and the previous point, if the last mentions sloop is bigger than 100 - is great, then the functions is not continous at this point *///
                    if (Math.Sign(mid.X - previous.X) != Math.Sign(current.X - mid.X))
                    {
                        if (Math.Abs((current.X - previous.X) / (current.Y - previous.Y)) > 100) // dx/dy > 100
                        {
                            //DrawSlice
                            {
                                if (points.Count > 1)
                                {
                                    Graphics.DrawLines(Pen, points.ToArray());
                                    Graphics.DrawLines(pen_, points.ToArray());
                                }
                                else if (points.Count == 1)
                                {
                                    Graphics.DrawLines(Pen, new[] { points[0], new PointF(points[0].X + 0.1f, points[0].Y) });
                                    Graphics.DrawLines(pen_, new[] { points[0], new PointF(points[0].X + 0.1f, points[0].Y) });
                                }
                                points.Clear();
                            };
                            if (i < end_)
                            {
                                i += step_;
                                previous = current;
                                lastIdentified = false;
                                goto Line;
                            }
                            else
                                goto line2;
                        }
                        else // dx/dy = 0
                        {
                            /// Extremum , Adding Auxilary point
                            /// Pay attention to that: x, y belongs to mid point as a coor point (cartisian point).
                            AuxiliaryPoints ap = new AuxiliaryPoints(x, y, new PointF(mid.X, mid.Y), "Extremum");
                            aPoints.Add(ap);
                        }
                    }
                    else if(Math.Sign(mid.Y - previous.Y) != Math.Sign(current.Y - mid.Y))
                    {
                        if (Math.Abs((current.Y - previous.Y) / (current.X - previous.X)) > 100) // dy/dx > 100
                        {
                            //DrawSlice
                            {
                                if (points.Count > 1)
                                {
                                    Graphics.DrawLines(Pen, points.ToArray());
                                    Graphics.DrawLines(pen_, points.ToArray());
                                }
                                else if (points.Count == 1)
                                {
                                    Graphics.DrawLines(Pen, new[] { points[0], new PointF(points[0].X + 0.1f, points[0].Y) });
                                    Graphics.DrawLines(pen_, new[] { points[0], new PointF(points[0].X + 0.1f, points[0].Y) });
                                }
                                points.Clear();
                            };
                            if (i < end_)
                            {
                                i += step_;
                                previous = current;
                                lastIdentified = false;
                                goto Line;
                            }
                            else
                                goto line2;
                        }
                        else // dy/dx = 0
                        {
                            /// Extremum , Adding Auxilary point
                            /// Pay attention to that: x, y belongs to mid point as a coor point (cartisian point).
                            AuxiliaryPoints ap = new AuxiliaryPoints(x, y, new PointF(mid.X, mid.Y), "Extremum");
                            aPoints.Add(ap);
                        }
                    }

                }
                #endregion

                points.Add(current);
                previous = current;
                lastIdentified = true;
            }
            line2:
            if (points.Count > 1)
            {
                Graphics.DrawLines(Pen, points.ToArray());
                Graphics.DrawLines(pen_, points.ToArray());
            }
            else if (points.Count == 1)
            {
                Graphics.DrawLines(Pen, new[] { points[0], new PointF(points[0].X + 0.1f, points[0].Y) });
                Graphics.DrawLines(pen_, new[] { points[0], new PointF(points[0].X + 0.1f, points[0].Y) });
            }

            GraphSetting.Sketch.AuxiliaryPoints.AddRange(aPoints.ToArray());
        }

        public void Render()
        {
            List<PointF> points = new List<PointF>();
            number start_ = Start.Calculate(GraphSetting.CalculationSetting, new Dictionary<Symbol, MathPackage.Node>());
            number end_ = End.Calculate(GraphSetting.CalculationSetting, new Dictionary<Symbol, MathPackage.Node>());
            number step_ = Step.Calculate(GraphSetting.CalculationSetting, new Dictionary<Symbol, MathPackage.Node>());
            double x, y;
            PointF current;
            PointF mid, previous;
            x = Get_X(start_); y = Get_Y(start_);
            previous = new PointF(GraphSetting.xChangeToPixel(x, y), GraphSetting.yChangeToPixel(x, y));
            for (number i = start_ + step_; i <= end_; i += step_)
            {
                Line:
                //Now, I have x and y of the function.
                x = Get_X(i); y = Get_Y(i);
                current = new PointF(GraphSetting.xChangeToPixel(x, y), GraphSetting.yChangeToPixel(x, y));
                x = Get_X(i - step_/2); y = Get_Y(i - step_ / 2);
                mid = new PointF(GraphSetting.xChangeToPixel(x, y), GraphSetting.yChangeToPixel(x, y));
                if ((float.IsNaN(current.X) || float.IsInfinity(current.X) ||
                    (float.IsNaN(current.Y) || float.IsInfinity(current.Y) ||
                    Math.Abs(current.X) > 1000000) || 
                    Math.Abs(current.Y) > 1000000) ||
                    (Math.Sign(mid.X - previous.X) != Math.Sign(current.X - mid.X) && (Math.Abs((current.X - previous.X) / (current.Y - previous.Y)) > 100)) ||
                    (Math.Sign(mid.Y - previous.Y) != Math.Sign(current.Y - mid.Y) && (Math.Abs((current.Y - previous.Y) / (current.X - previous.X)) > 100))
                    ///* if the first derivative (dy/dx) chandes its sign (+ or -) then there is a local extremum or if the dx/dy makes the same thing, we will look at the sloop of the line going throw the current and the previous point, if the last mentions sloop is bigger than 100 - is great, then the functions is not continous at this point *///
                    )
                {
                    //DrawSlice
                    {
                        if (points.Count > 1)
                        {
                            Graphics.DrawLines(Pen, points.ToArray());
                        }
                        else if (points.Count == 1)
                        {
                            Graphics.DrawLines(Pen, new[] { points[0], new PointF(points[0].X + 0.1f, points[0].Y) });
                        }
                        points.Clear();
                    };
                    if (i < end_)
                    {
                        i += step_;
                        previous = current;
                        goto Line;
                    }
                    else
                        goto line2;
                }
                points.Add(current);
                previous = current;
            }
            line2:
            if (points.Count > 1)
            {
                Graphics.DrawLines(Pen, points.ToArray());
            }
            else if (points.Count == 1)
            {
                Graphics.DrawLines(Pen, new[] { points[0], new PointF(points[0].X + 0.1f, points[0].Y) });
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            if (StepIsAssigned)
                return $"{Name} = Curve({Parameter}, {Start}, {End}, {Step}, {x_Expression}, {y_Expression})";
            else
                return $"{Name} = Curve({Parameter}, {Start}, {End}, {x_Expression}, {y_Expression})";
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
            XElement element = new XElement("ParametricFunc");

            element.SetAttributeValue("Name", Name);
            if (!Visible) element.SetAttributeValue("Visible", Visible);

            XElement node = new XElement("Start");
            node.SetAttributeValue("Value", Start.ToString());
            element.Add(node);
            node = new XElement("End");
            node.SetAttributeValue("Value", End.ToString());
            element.Add(node);
            node = new XElement("Step");
            node.SetAttributeValue("Value", Step.ToString());
            element.Add(node);
            node = new XElement("x_Expression");
            node.SetAttributeValue("Value", x_Expression.ToString());
            element.Add(node);
            node = new XElement("y_Expression");
            node.SetAttributeValue("Value", y_Expression.ToString());
            element.Add(node);
            node = new XElement("Pen");
            node.SetAttributeValue("Value", MainFunctions.GetPenAsString(Pen));
            element.Add(node);

            return element;
        }
        public override void Delete(bool removeControl)
        {
            GraphSetting.Sketch.Objects.Delete(this, removeControl);
        }
      
        #endregion

    }

}
