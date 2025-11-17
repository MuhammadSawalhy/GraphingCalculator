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

    public class XFunction : Function
    {
        public XFunction(string name, MathPackage.Node expression, GraphSetting GraphSetting_, Controls.GraphControl Control = null) : base(GraphSetting_, Control)
        {
            Expression = expression;
            SetName(name);
        }
        public XFunction(GraphSetting GraphSetting_, Controls.GraphControl Control = null) : base(GraphSetting_, Control)
        {
        }

        public override string Get_Type => "XFunction";

        public double Get(double x)
        {
            GraphSetting.CalculationSetting.Vars[0].Value = new MathPackage.Operations.Constant(x);
            return Expression.Calculate(GraphSetting.CalculationSetting, new Dictionary<Symbol, MathPackage.Node>());
        }

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
                   RenderXFunc();
                }
                else
                {
                    RenderXFuncSelected();
                }
            }
            catch (Exception ex)
            {
                if (throwError)
                    throw ex;
                ShowError(ex.Message);
            }
        }

        #region "XFunction"

        public void RenderXFuncSelected()
        {
            List<PointF> points = new List<PointF>();
            number x = GraphSetting.XRange.Lo;
            number y = 0;
            number midY;
            Pen pen_ = new Pen(Color.FromArgb(100, Pen.Color.R, Pen.Color.G, Pen.Color.B), Pen.Width + 4);
            List<AuxiliaryPoints> aPoints = new List<AuxiliaryPoints>();
            double previous_y = Get(GraphSetting.XRange.Lo);
            bool lastIdentified = false;
            for (double i = GraphSetting.XRange.Lo + GraphSetting.XRange.StepSize; i <= GraphSetting.XRange.Hi - GraphSetting.XRange.StepSize; i += GraphSetting.XRange.StepSize)
            {
                Line:
                //Now, I am getting x and y of the function.
                y = Get(x);
                midY = Get(x - GraphSetting.XRange.StepSize / 2);
                //if the point isn't supported
                if ((double.IsNaN(y) || double.IsInfinity(y)) ||
                    (double.IsNaN(previous_y) || double.IsInfinity(previous_y)) ||
                    (Math.Abs(GraphSetting.yChangeToPixel(x, y)) > 1000000) ||
                    (Math.Sign(midY - previous_y) != Math.Sign(y - midY) && Math.Abs((y - previous_y) / GraphSetting.XRange.StepSize) > 100)
                    )
                {

                    //DrawSlice
                    {
                        if (points.Count > 1)
                        {
                            Graphics.DrawLines(pen_, points.ToArray());
                            Graphics.DrawLines(Pen, points.ToArray());
                            points.Clear();
                            lastIdentified = false;
                        }
                        else if (points.Count == 1)
                        {
                            Graphics.DrawLines(pen_, new[] { points[0], new PointF(points[0].X + 0.1f, points[0].Y) });
                            Graphics.DrawLines(Pen, new[] { points[0], new PointF(points[0].X + 0.1f, points[0].Y) });
                            points.Clear();
                            lastIdentified = false;
                        }
                    };

                    if (i < GraphSetting.XRange.Hi - GraphSetting.XRange.StepSize)
                    {
                        x += GraphSetting.XRange.StepSize;
                        i += GraphSetting.XRange.StepSize;
                        previous_y = y;
                        goto Line;
                    }
                    else
                        goto line2;

                }

                //if the point is supported
                {
                    //Adding extremum points
                    if (lastIdentified) { 
                        if (Math.Sign(y - midY) != Math.Sign(midY - previous_y))
                        {
                            aPoints.Add(new AuxiliaryPoints(x, y, new PointF(GraphSetting.xChangeToPixel(x, y), GraphSetting.yChangeToPixel(x, y)), "Extremum Point"));
                        }
                    }
                    points.Add(new PointF(GraphSetting.xChangeToPixel(x, y), GraphSetting.yChangeToPixel(x, y)));
                    x += GraphSetting.XRange.StepSize;
                    previous_y = y;
                    lastIdentified = true;
                };
            }
            line2:
            {
                if (points.Count > 1)
                {
                    Graphics.DrawLines(pen_, points.ToArray());
                    Graphics.DrawLines(Pen, points.ToArray());
                }
                else if (points.Count == 1)
                {
                    Graphics.DrawLines(pen_, new[] { points[0], new PointF(points[0].X + 0.1f, points[0].Y) });
                    Graphics.DrawLines(Pen, new[] { points[0], new PointF(points[0].X + 0.1f, points[0].Y) });
                }
            };
            GraphSetting.Sketch.AuxiliaryPoints.AddRange(aPoints.ToArray());
        }

        public void RenderXFunc()
        {
            List<PointF> points = new List<PointF>();
            number x = GraphSetting.XRange.Lo;
            number y = 0;
            number midY;
            double previous_y = Get(GraphSetting.XRange.Lo);
            for (double i = GraphSetting.XRange.Lo + GraphSetting.XRange.StepSize; i <= GraphSetting.XRange.Hi; i += GraphSetting.XRange.StepSize)
            {
                Line:
                //Now, I have x and y of the function.
                y = Get(x);
                midY = Get(x - GraphSetting.XRange.StepSize / 2);
                if ((double.IsNaN(y) || double.IsInfinity(y)) ||
                    (double.IsNaN(previous_y) || double.IsInfinity(previous_y)) || 
                    (Math.Abs(GraphSetting.yChangeToPixel(x, y)) > 1000000) ||
                    (Math.Sign(midY - previous_y) != Math.Sign(y - midY) && Math.Abs((y - previous_y) / GraphSetting.XRange.StepSize )> 100) 
                    // that means that the function is not continous at this point.
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

                    if (i < GraphSetting.XRange.Hi)
                    {
                        x += GraphSetting.XRange.StepSize;
                        i += GraphSetting.XRange.StepSize;
                        previous_y = y;
                        goto Line;
                    }
                    else
                        goto line2;

                }

                points.Add(new PointF(GraphSetting.xChangeToPixel(x, y), GraphSetting.yChangeToPixel(x, y)));
                x += GraphSetting.XRange.StepSize;
                previous_y = y;
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

        #endregion

        #region Methods

        public override string ToString()
        {
            return $"{Name}({GraphSetting.sy_x}) = {Expression.ToString()}";
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
            XElement element = new XElement("XFunction");

            element.SetAttributeValue("Name", Name);
            if (!Visible) element.SetAttributeValue("Visible", Visible);

            XElement expr = new XElement("Expression");
            expr.SetAttributeValue("Value", Expression.ToString());
            element.Add(expr);

            XElement pen_ = new XElement("Pen");
            pen_.SetAttributeValue("Value", MainFunctions.GetPenAsString(Pen));
            element.Add(pen_);

            return element;
        }
        public override void Delete(bool removeControl)
        {
            GraphSetting.Sketch.Objects.Delete(this, removeControl);
        }
      
        #endregion

    }

}
