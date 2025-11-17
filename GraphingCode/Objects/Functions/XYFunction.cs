using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;
using System.Xml.Linq;
using MathPackage.Operations;
namespace Graphing.Objects
{
    public class XYFunction : Function
    {

        public XYFunction(MathPackage.Node expression, GraphSetting GraphSetting_, Controls.GraphControl Control = null) : base(GraphSetting_)
        {
            Expression = expression;
        }
        public XYFunction(GraphSetting GraphSetting_, Controls.GraphControl Control = null) : base(GraphSetting_, Control) { }

        MathPackage.Node expression, realExpression;
        public override MathPackage.Node Expression
        {
            get => realExpression;
            set
            {
                if (!(value is MathPackage.Operations.Boolean))
                    throw new Exception($"Your Expression is not valid for XYFunction.\n{value.ToString()}");
                switch (value)
                {
                    case Equal _:
                        Func_Type = FuncType.Equation;
                        expression = new Subtract(value.Children[0], value.Children[1]);
                        break;
                    case GreaterEqual _:
                        Func_Type = FuncType.Alternation;
                        expression = new GreaterEqual(new Subtract(value.Children[0], value.Children[1]), new Constant(0));
                        break;
                    case GreaterThan _:
                        Func_Type = FuncType.Alternation;
                        expression = new GreaterThan(new Subtract(value.Children[0], value.Children[1]), new Constant(0));
                        break;
                    case LowerEqual _:
                        Func_Type = FuncType.Alternation;
                        expression = new LowerEqual(new Subtract(value.Children[0], value.Children[1]), new Constant(0));
                        break;
                    case LowerThan _:
                        Func_Type = FuncType.Alternation;
                        expression = new LowerThan(new Subtract(value.Children[0], value.Children[1]), new Constant(0));
                        break;
                    default:
                        Func_Type = FuncType.Alternation;
                        expression = value;
                        break;
                }
                realExpression = value;
            }
        }
        public double Get(double x, double y)
        {
            GraphSetting.CalculationSetting.Vars[0].Value = new MathPackage.Operations.Constant(x);
            GraphSetting.CalculationSetting.Vars[1].Value = new MathPackage.Operations.Constant(y);
            return expression.Calculate(GraphSetting.CalculationSetting, new Dictionary<Loyc.Symbol, MathPackage.Node>());
        }

        #region Drawing

        public FuncType Func_Type { get; private set; }
        public enum FuncType
        {
            Equation,
            Alternation
        }
        public override void Draw(bool throwError = false)
        {

            Bitmap = new Bitmap(GraphSetting.Width, GraphSetting.Height);
            Graphics = Graphics.FromImage(Bitmap);
            Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            try
            {
                if (IsSelected)
                    RenderSelected();
                else
                    Render();
            }
            catch (Exception ex)
            {
                if (throwError)
                    throw ex;
                ShowError(ex.Message);
            }

        }
        // Scanning the skecth {Coordinates}.
        //.........................
        //.........................
        //.........................
        //.........................
        //.........................
        //.........................
        protected void RenderSelected()
        {
            double[,] previousValues = Get_xz_values(GraphSetting.YRange.Hi), currentValues;
            double step = (GraphSetting.YRange.Hi - GraphSetting.YRange.Lo) / GraphSetting.YRange.PxCount;
            List<List<PointF>> pathsList = new List<List<PointF>>();
            List<PointF> currentPath;
            for (double y = GraphSetting.YRange.Hi - step; y > GraphSetting.YRange.Lo + step; y -= step)
            {
                if (Func_Type == FuncType.Equation)
                {
                    currentValues = Get_xz_values(y);
                    if (pathsList.Count < 20)
                    {
                        currentPath = new List<PointF>();
                        for (int i = 0; i < currentValues.GetLength(1) - 1; i++)
                        {
                            // if the point belong to the XYFunction Curve
                            if (double.IsNaN(currentValues[1, i]) || double.IsInfinity(currentValues[1, i]))
                                continue;
                            int sign = Math.Sign(currentValues[1, i]);
                            if (sign == 0 || sign != Math.Sign(currentValues[1, i + 1]) ||
                                sign != Math.Sign(previousValues[1, i]) ||
                                sign != Math.Sign(previousValues[1, i + 1]))
                            {
                                currentPath.Add(new PointF(GraphSetting.xChangeToPixel(currentValues[0, i], y), GraphSetting.yChangeToPixel(currentValues[0, i], y)));
                            }
                            else if (currentPath.Count > 0)
                            {
                                AddPixels(pathsList, currentPath.ToArray());
                                currentPath.Clear();
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < currentValues.GetLength(1) - 1; i++)
                        {
                            // if the point belong to the XYFunction Curve
                            if (double.IsNaN(currentValues[1, i]) || double.IsInfinity(currentValues[1, i]))
                                continue;
                            int sign = Math.Sign(currentValues[1, i]);
                            if (sign == 0 || sign != Math.Sign(currentValues[1, i + 1]) ||
                                sign != Math.Sign(previousValues[1, i]) ||
                                sign != Math.Sign(previousValues[1, i + 1]))
                            {
                                int x_ = (int)GraphSetting.xChangeToPixel(currentValues[0, i], y), y_ = (int)GraphSetting.yChangeToPixel(currentValues[0, i], y);
                                Bitmap.SetPixel(x_, y_, Pen.Color);
                                // faded pixels
                                for (int ii = 1; ii <= Pen.Width; ii++)
                                {
                                    Graphics.FillRectangle(new SolidBrush(Color.FromArgb((int)((Pen.Width - ii) / Pen.Width) * 150, Pen.Color)),
                                        x_ - ii, y_ - ii, 2 * ii + 1, 2 * ii + 1
                                        );
                                }
                            }
                        }
                    }
                    previousValues = currentValues;
                }
                else if (Func_Type == FuncType.Alternation)
                {
                    double xstep = (GraphSetting.XRange.Hi - GraphSetting.XRange.Lo) / GraphSetting.XRange.PxCount;
                    for (double x = GraphSetting.XRange.Lo; x < GraphSetting.XRange.Hi - xstep; x += xstep)
                    {
                        if (Get(x, y) == 1)
                        {
                            // Adding a {Point}
                            Bitmap.SetPixel(
                              (int)GraphSetting.xChangeToPixel(x, y),
                              (int)GraphSetting.yChangeToPixel(x, y),
                              Color.FromArgb(150, Pen.Color.R, Pen.Color.G, Pen.Color.B)
                            );
                        }
                    }
                }
            }
            DrawPaths(pathsList, true);
        }
        protected void Render()
        {
            double[,] previousValues = Get_xz_values(GraphSetting.YRange.Hi), currentValues;
            double step = (GraphSetting.YRange.Hi - GraphSetting.YRange.Lo) / GraphSetting.YRange.PxCount;
            List<List<PointF>> pathsList = new List<List<PointF>>();
            List<PointF> currentPath;
            for (double y = GraphSetting.YRange.Hi - step; y > GraphSetting.YRange.Lo + step; y -= step)
            {
                if (Func_Type == FuncType.Equation)
                {
                    currentValues = Get_xz_values(y);
                    if (pathsList.Count < 20)
                    {
                        currentPath = new List<PointF>();
                        for (int i = 0; i < currentValues.GetLength(1) - 1; i++)
                        {
                            // if the point belong to the XYFunction Curve
                            if (double.IsNaN(currentValues[1, i]) || double.IsInfinity(currentValues[1, i]))
                                continue;
                            int sign = Math.Sign(currentValues[1, i]);
                            if (sign == 0 || sign != Math.Sign(currentValues[1, i + 1]) ||
                                sign != Math.Sign(previousValues[1, i]) ||
                                sign != Math.Sign(previousValues[1, i + 1]))
                            {
                                currentPath.Add(new PointF(GraphSetting.xChangeToPixel(currentValues[0, i], y), GraphSetting.yChangeToPixel(currentValues[0, i], y)));
                            }
                            else if (currentPath.Count > 0)
                            {
                                AddPixels(pathsList, currentPath.ToArray());
                                currentPath.Clear();
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < currentValues.GetLength(1) - 1; i++)
                        {
                            // if the point belong to the XYFunction Curve
                            if (double.IsNaN(currentValues[1, i]) || double.IsInfinity(currentValues[1, i]))
                                continue;
                            int sign = Math.Sign(currentValues[1, i]);
                            if (sign == 0 || sign != Math.Sign(currentValues[1, i + 1]) ||
                                sign != Math.Sign(previousValues[1, i]) ||
                                sign != Math.Sign(previousValues[1, i + 1]))
                            {
                                int x_ = (int)GraphSetting.xChangeToPixel(currentValues[0, i], y), y_ = (int)GraphSetting.yChangeToPixel(currentValues[0, i], y);
                                Bitmap.SetPixel(x_, y_, Pen.Color);
                                // faded pixels
                                //for (int ii = 1; ii <= Pen .Width; ii++)
                                //{
                                //    Graphics.FillRectangle(new SolidBrush(Color.FromArgb((int)((Pen.Width - ii) / Pen.Width) * 150, Pen.Color)),
                                //        x_ - ii,y_ - ii , 2*ii + 1, 2*ii+1
                                //        );
                                //}
                            }
                        }
                    }
                    previousValues = currentValues;
                }
                else if (Func_Type == FuncType.Alternation)
                {
                    double xstep = (GraphSetting.XRange.Hi - GraphSetting.XRange.Lo) / GraphSetting.XRange.PxCount;
                    for (double x = GraphSetting.XRange.Lo; x < GraphSetting.XRange.Hi - xstep; x += xstep)
                    {
                        if (Get(x, y) == 1)
                        {
                            // Adding a {Point}
                            Bitmap.SetPixel(
                              (int)GraphSetting.xChangeToPixel(x, y),
                              (int)GraphSetting.yChangeToPixel(x, y),
                              Color.FromArgb(150, Pen.Color.R, Pen.Color.G, Pen.Color.B)
                            );
                        }
                    }
                }
            }
            DrawPaths(pathsList, false);
        }
        protected void DrawPaths(List<List<PointF>> pathsList, bool selected)
        {
            if (selected)
            {
                Pen _pen = new Pen(Color.FromArgb(100, Pen.Color), Pen.Width + 4);
                _pen.DashStyle = Pen.DashStyle;
                for (int i = 0; i < pathsList.Count; i++)
                {
                    if (pathsList[i].Count > 1)
                    {
                        Graphics.DrawCurve(_pen, pathsList[i].ToArray());
                        Graphics.DrawCurve(Pen, pathsList[i].ToArray());
                    }
                    else
                    {
                        Color fadedColor = Color.FromArgb(100, Pen.Color);
                        if (IsIn((int)pathsList[i][0].X, (int)pathsList[i][0].Y))
                            Bitmap.SetPixel((int)pathsList[i][0].X, (int)pathsList[i][0].Y, Pen.Color);
                        /// Faded pixels
                        {
                            if (IsIn((int)pathsList[i][0].X - 1, (int)pathsList[i][0].Y - 1))
                                Bitmap.SetPixel((int)pathsList[i][0].X - 1, (int)pathsList[i][0].Y - 1, Pen.Color);
                            if (IsIn((int)pathsList[i][0].X - 1, (int)pathsList[i][0].Y))
                                Bitmap.SetPixel((int)pathsList[i][0].X - 1, (int)pathsList[i][0].Y, Pen.Color);
                            if (IsIn((int)pathsList[i][0].X - 1, (int)pathsList[i][0].Y + 1))
                                Bitmap.SetPixel((int)pathsList[i][0].X - 1, (int)pathsList[i][0].Y + 1, Pen.Color);

                            if (IsIn((int)pathsList[i][0].X, (int)pathsList[i][0].Y - 1))
                                Bitmap.SetPixel((int)pathsList[i][0].X, (int)pathsList[i][0].Y - 1, Pen.Color);
                            if (IsIn((int)pathsList[i][0].X, (int)pathsList[i][0].Y))
                                Bitmap.SetPixel((int)pathsList[i][0].X, (int)pathsList[i][0].Y, Pen.Color);
                            if (IsIn((int)pathsList[i][0].X, (int)pathsList[i][0].Y + 1))
                                Bitmap.SetPixel((int)pathsList[i][0].X, (int)pathsList[i][0].Y + 1, Pen.Color);

                            if (IsIn((int)pathsList[i][0].X + 1, (int)pathsList[i][0].Y - 1))
                                Bitmap.SetPixel((int)pathsList[i][0].X + 1, (int)pathsList[i][0].Y - 1, Pen.Color);
                            if (IsIn((int)pathsList[i][0].X + 1, (int)pathsList[i][0].Y))
                                Bitmap.SetPixel((int)pathsList[i][0].X + 1, (int)pathsList[i][0].Y, Pen.Color);
                            if (IsIn((int)pathsList[i][0].X + 1, (int)pathsList[i][0].Y + 1))
                                Bitmap.SetPixel((int)pathsList[i][0].X + 1, (int)pathsList[i][0].Y + 1, Pen.Color);
                        }
                    }
                }
                return;
            }
            for (int i = 0; i < pathsList.Count; i++)
            {
                if (pathsList[i].Count > 1)
                    Graphics.DrawCurve(Pen, pathsList[i].ToArray());
                else
                    Bitmap.SetPixel((int)pathsList[i][0].X, (int)pathsList[i][0].Y, Pen.Color);
            }
        }
        protected bool IsIn(int x, int y)
        {
            if (x >= 0 && x <= Bitmap.Width && y >= 0 && y <= Bitmap.Height)
                return true;
            return false;
        }
        protected double[,] Get_xz_values(double y)
        {
            double[,] zValues = new double[2, GraphSetting.XRange.PxCount];
            double step = (GraphSetting.XRange.Hi - GraphSetting.XRange.Lo) / GraphSetting.XRange.PxCount;
            int i = 0;
            for (double x = GraphSetting.XRange.Lo; x < GraphSetting.XRange.Hi - step; x += step)
            {
                zValues[0, i] = x;
                zValues[1, i] = Get(x, y);
                i++;
            }
            return zValues;
        }
        protected void AddPixels(List<List<PointF>> pathsList, PointF[] point)
        {
            for (int i = 0; i < pathsList.Count; i++)
            {
                byte belong = _belong(pathsList[i], point);
                // attach the first with the end
                if (belong == 10)
                {
                    pathsList[i].AddRange(point);
                    return;
                }
                // attach the end with the first
                else if (belong == 01)
                {
                    pathsList[i].InsertRange(0, point);
                    return;
                }
                // attach the first with the first
                else if (belong == 00)
                {
                    pathsList[i].AddRange(point.Reverse());
                    return;
                }
                // attach the end with the end
                else if (belong == 11)
                {
                    pathsList[i].InsertRange(0, point.Reverse());
                    return;
                }
            }
            /// this will be excuted if the <see cref="point"/> does not belong to any of <see cref="pathsList"/> paths
            pathsList.Add(new List<PointF>(point));
        }
        /// <summary>
        /// this to know if the point is adjacent to ont of the two edge points of the path (the start and the beginning) 
        /// 0 is {add at the end}, 1 is {add at the begining}
        /// </summary>
        /// <param name="pathsList"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        protected byte _belong(List<PointF> path, PointF[] point)
        {
            /// ...
            /// .+.
            /// ...

            /**/
            if ((int)point.First().X - 1 <= (int)path.First().X && (int)path.First().X <= (int)point.First().X + 1 &&
                (int)point.First().Y - 1 <= (int)path.First().Y && (int)path.First().Y <= (int)point.First().Y + 1)
                return 11;
            else if ((int)point.First().X - 1 <= (int)path.Last().X && (int)path.Last().X <= (int)point.First().X + 1 &&
                     (int)point.First().Y - 1 <= (int)path.Last().Y && (int)path.Last().Y <= (int)point.First().Y + 1)
                return 10;
            else if ((int)point.Last().X - 1 <= (int)path.First().X && (int)path.First().X <= (int)point.Last().X + 1 &&
                     (int)point.Last().Y - 1 <= (int)path.First().Y && (int)path.First().Y <= (int)point.Last().Y + 1)
                return 01;
            else if ((int)point.Last().X - 1 <= (int)path.Last().X && (int)path.Last().X <= (int)point.Last().X + 1 &&
                     (int)point.Last().Y - 1 <= (int)path.Last().Y && (int)path.Last().Y <= (int)point.Last().Y + 1)
                return 00;



            else return 2;
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return realExpression.ToString();
        }
        public override string DiscriptionString()
        {
            return "";
        }
        public override void UpdateScriptText()
        {
            this.Control.Script.Text = this.ToString();
        }
        public override XElement GetAsXml()
        {
            XElement element = new XElement("XYFunction");

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
