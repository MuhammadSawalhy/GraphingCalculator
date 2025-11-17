using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Globalization;
using Loyc.Syntax;
using Loyc.Syntax.Les;
using Loyc;
using System.Linq;
using Graphing.Controls;
using Graphing.Objects;

internal static class ColorExtensions
{
    public static float GetBrightness(this Color color)
    {
        float num = ((float)color.R) / 255f;
        float num2 = ((float)color.G) / 255f;
        float num3 = ((float)color.B) / 255f;
        float num4 = num;
        float num5 = num;
        if (num2 > num4)
            num4 = num2;
        if (num3 > num4)
            num4 = num3;
        if (num2 < num5)
            num5 = num2;
        if (num3 < num5)
            num5 = num3;
        return ((num4 + num5) / 2f);
    }

    public static float GetHue(this Color color)
    {
        if ((color.R == color.G) && (color.G == color.B))
            return 0f;
        float num = ((float)color.R) / 255f;
        float num2 = ((float)color.G) / 255f;
        float num3 = ((float)color.B) / 255f;
        float num7 = 0f;
        float num4 = num;
        float num5 = num;
        if (num2 > num4)
            num4 = num2;
        if (num3 > num4)
            num4 = num3;
        if (num2 < num5)
            num5 = num2;
        if (num3 < num5)
            num5 = num3;
        float num6 = num4 - num5;
        if (num == num4)
            num7 = (num2 - num3) / num6;
        else if (num2 == num4)
            num7 = 2f + ((num3 - num) / num6);
        else if (num3 == num4)
            num7 = 4f + ((num - num2) / num6);
        num7 *= 60f;
        if (num7 < 0f)
            num7 += 360f;
        return num7;
    }

    public static float GetSaturation(this Color color)
    {
        float num = ((float)color.R) / 255f;
        float num2 = ((float)color.G) / 255f;
        float num3 = ((float)color.B) / 255f;
        float num7 = 0f;
        float num4 = num;
        float num5 = num;
        if (num2 > num4)
            num4 = num2;
        if (num3 > num4)
            num4 = num3;
        if (num2 < num5)
            num5 = num2;
        if (num3 < num5)
            num5 = num3;
        if (num4 == num5)
            return num7;
        float num6 = (num4 + num5) / 2f;
        if (num6 <= 0.5)
            return ((num4 - num5) / (num4 + num5));
        return ((num4 - num5) / ((2f - num4) - num5));
    }
}

namespace Graphing
{
    public static class MainFunctions
    {

        public static System.Drawing.Drawing2D.SmoothingMode Smoothing = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
        public static System.Drawing.Drawing2D.CompositingQuality Composition = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
        public static Pen AdjustPen(Pen Pen)
        {
            Edit_Pen form = new Edit_Pen(Pen);
            form.ShowDialog();
            return form.Pen;
        }

        #region Varaibles

        public static List<string> AvaliableFunctions()
        {
            List<string> list = new List<string>(new string[]
            {
                "sin","cos","tan","sqrt","asin","acos","atan","sec","csc","cot"
                ,"exp","ln","log","ceil","floor","sign","abs","random","fact"
                ,"sum","square","sinh","cosh","tanh","asinh","acosh","atanh"
                ,"if"
            });
            return list;
        }
        public static List<string> PDsTypes()
        {
            return new List<string>(new string[] {
                "Circle","Circle2","Angle","Distance","SemiCircle",
                "SemiCircle2","Polygone","Line"
            });
        }
        public static List<string> RestrictedNames()
        {
            return new List<string>(new string[] {
                "nth", "pi", "phi"
            });
        }

        #region default
        public static Pen DefaultPen = new Pen(Color.FromArgb(43, 44, 51), 2);
        public static Font DefaultFont = new Font("Tahoma",10);
        #endregion
        
        #endregion

        #region Language

        public static MathPackage.Main.Language GetLanguage()
        {
            switch (Thread.CurrentThread.CurrentCulture.ToString())
            {
                case "en-EN":
                    return MathPackage.Main.Language.EN;
                case "ar-AR":
                    return MathPackage.Main.Language.AR;
            }
            return MathPackage.Main.Language.EN;
        }

        public static void SetLanguage(MathPackage.Main.Language Lang)
        {
            CultureInfo ci = new CultureInfo(Lang.ToString());
            Thread.CurrentThread.CurrentCulture = ci;
            Thread.CurrentThread.CurrentUICulture = ci;
        }

        #endregion

        #region ParsingExpressions

        public static LNode ParseExprs(string text, string fieldName = "")
        {
            // Separate things like *- into two separate operators (* -), and change ^ to **
            text = System.Text.RegularExpressions.Regex.Replace(text, @"([-+*/%^&*|<>=?.])([-~!+])", "$1 $2");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\^", "**");
            text = text.Replace(",...,", ",,");
            var errorHandler = MessageSink.FromDelegate((severity, ctx, fmt, args) =>
            {
                if (severity >= Severity.Error)
                {
                    var msg = "";
                    if (fieldName != "" && fieldName != null) { msg = fieldName + ": " + fmt.Localized(args); }
                    else { msg = fmt.Localized(args); }
                    if (ctx is SourceRange)
                        msg += $"\r\n{text}\r\n{new string('-', ((SourceRange)ctx).Start.PosInLine - 1)}^";
                    throw new LogException(severity, ctx, msg);
                }
            });
            if (Les3LanguageService.Value.Parse(text, errorHandler).ToList().Count != 1)
            {
                throw new Exception($"This expression \"{text}\" is not valid.");
            }
            else
            {
                LNode node = Les3LanguageService.Value.Parse(text, errorHandler).ToList()[0];
                return node;
            }
        }

        public static bool ExprContainVariable(LNode expr)
        {

            if (expr.IsId)
            {
                return true;
            }
            else if (expr.IsLiteral)
            {
                if (MathPackage.Main.IsNumeric(expr.Value.ToString()))
                {
                    return false;
                }
            }
            else if (expr.IsCall && expr.Args.Count > 0)
            {
                for (int i = 0; i < expr.Args.Count; i++)
                {
                    if (ExprContainVariable(expr.Args[i]))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        #endregion

        #region Get a GraphObject

        /// <summary>
        /// To get any supported object throw a mere string.
        /// this will not add anything to the sketch and the grahpsetting ( except you pass true for EnrollName { To add the name to the GraphSetting } ) 
        /// </summary>
        /// <param name="script">the string that represents the object.</param>
        /// <param name="SelectName">If the object's name is not assigned in the script we will select a one from the avaliable names.</param>
        /// <param name="EnrollName">If you want to SetName for this Object ( this will add the name to the GraphSetting ) </param>
        /// <returns></returns>
        public static object GetObject(string script, GraphSetting GraphSetting, bool SelectName, bool EnrollName)
        {
            LNode node = ParseExprs(script);
            /// like a*x^2 , a = {1,2,...,5} then return true.
            bool ContainsNumListSymbol = ContainsListSymbol(node, GraphSetting.Sketch.NumbersLists);

            if (node.Calls(CodeSymbols.Assign, 2))
            {

                LNode left = null, right = null;
                left = node.Args[0]; right = node.Args[1];
            
                #region points_dependant
                //such : a = pdType(...)
                if (left.IsId && IsFuncId(right) && PDsTypes().Contains(right.Target.Name.ToString()))
                {
                    LNode b = right, a = left;
                    PointsDependant pd = GetPD(b, GraphSetting, false, false);
                    if (pd == null)
                        goto Line1;
                    pd.SetName(a.Name.Name);
                    return pd;
                }
                //such : pdType(...) = a
                else if (right.IsId && IsFuncId(left) && PDsTypes().Contains(left.Target.Name.ToString()))
                {
                    LNode a = right, b = left;
                    PointsDependant pd = GetPD(b, GraphSetting, false, false);
                    if (pd == null)
                        goto Line1;
                    pd.SetName(a.Name.Name);
                    return pd;
                }
                #endregion

                #region explicit functions
                // to add function  e.g. " y = sin(x) "
                else if (left.IsId && left.Name == GraphSetting.sy_y && !ContainsSymbol(right, GraphSetting.sy_y))
                {

                    XFunction f = new XFunction(GraphSetting)
                    {
                        Expression = MathPackage.Transformer.GetNodeFromLoycNode(right)
                    };
                    if (SelectName)
                    {
                        if (EnrollName)
                        {
                            f.SetName(GraphSetting.SelectName());
                        }
                        else
                        {
                            f.Name = GraphSetting.SelectName();
                        }
                    }
                    return f;

                }
                // to add function  e.g. " sin(x) = y "
                else if (right.IsId && right.Name == GraphSetting.sy_y && !ContainsSymbol(left, GraphSetting.sy_y))
                {
                    XFunction f = new XFunction(GraphSetting)
                    {
                        Expression = MathPackage.Transformer.GetNodeFromLoycNode(left)
                    };
                    if (SelectName)
                    {
                        if (EnrollName)
                        {
                            f.SetName(GraphSetting.SelectName());
                        }
                        else
                        {
                            f.Name = GraphSetting.SelectName();
                        }
                    }
                    return f;
                }
                //such : a = c * x^2 + sin( x )
                else if (left.IsId && left.Name != GraphSetting.sy_x && MathExpression(right) && ContainsSymbol(right, GraphSetting.sy_x) && !ContainsSymbol(right, GraphSetting.sy_y))
                {
                    LNode b = right, a = left;

                    XFunction f = new XFunction(GraphSetting)
                    {
                        Expression = MathPackage.Transformer.GetNodeFromLoycNode(b)
                    };
                    if (EnrollName)
                    {
                        f.SetName(a.Name.ToString());
                    }
                    else
                    {
                        f.Name = a.Name.ToString();
                    }

                    return f;

                }
                //such : c * x^2 + sin( x ) = a
                else if (right.IsId && right.Name != GraphSetting.sy_x && MathExpression(left) && ContainsSymbol(left, GraphSetting.sy_x) && !ContainsSymbol(left, GraphSetting.sy_y))
                {
                    LNode a = right, b = left;

                    XFunction f = new XFunction(GraphSetting)
                    {
                        Expression = MathPackage.Transformer.GetNodeFromLoycNode(b)
                    };
                    if (EnrollName)
                    {
                        f.SetName(a.Name.ToString());
                    }
                    else
                    {
                        f.Name = a.Name.ToString();
                    }

                    return f;
                }
                
                //such : area( h , b , theta) = 0.5 * h * b * sin( theta )
                //such : f(x) = x^2
                else if (IsFuncId(left) && !AvaliableFunctions().Contains(left.Target.Name.Name))
                {
                    // such : f(x) = x^2
                    if (left.Args.Count == 1 && left.Args[0].IsId && left.Args[0].Name == GraphSetting.sy_x && !ContainsSymbol(right, GraphSetting.sy_y))
                    {
                        XFunction f = new XFunction(GraphSetting);
                        if (MathExpression(right))
                        {
                            f.Expression = MathPackage.Transformer.GetNodeFromLoycNode(right);
                        }
                        else
                        {
                            throw new Exception("The function expression is not valid.");
                        }
                        if (EnrollName)
                        {
                            f.SetName(left.Target.Name.Name);
                        }
                        else
                        {
                            f.Name = left.Target.Name.Name;
                        }
                        return f;
                    }
                    //such : area( h , b , theta) = 0.5 * h * b * sin( theta )
                    else
                    {
                        Symbol name = null;
                        List<Symbol> args = new List<Symbol>();
                        LNode Process = null;
                        if (EnrollName)
                        {
                            if (GraphSetting.IsNameUsed(left.Target.Name.Name))
                            {
                                throw new Exception("This name has been used before.");
                            }
                            name = left.Target.Name;
                        }
                        else
                        {
                            name = left.Target.Name;
                        }
                        foreach (LNode arg in left.Args)
                        {
                            if (arg.IsId)
                            {
                                args.Add(arg.Name);
                            }
                            else
                            {
                                throw new Exception($"The argument {arg.ToString().Substring(0, arg.ToString().Length - 1)} of the function \"{name.ToString()}\" is invalid");
                            }
                        }
                        if (MathExpression(right))
                        {
                            Process = right;
                        }
                        else
                        {
                            throw new Exception($"The process {right.ToString().Substring(0, right.ToString().Length - 1)} of the function \"{name.ToString()}\" is invalid");
                        }


                        MathPackage.Operations.Func func = new MathPackage.Operations.Func(name, args, MathPackage.Transformer.GetNodeFromLoycNode(Process));
                        return func;
                    }
                }
                //such : 0.5 * h * b * sin( theta ) = area( h , b , theta)
                //such : x^2 = f(x) 
                else if (IsFuncId(right) && !AvaliableFunctions().Contains(right.Target.Name.Name))
                {
                    LNode a = right, b = left;
                    // such : f(x) = x^2
                    if (a.Args.Count == 1 && a.Args[0].IsId && a.Args[0].Name == GraphSetting.sy_x && !ContainsSymbol(left, GraphSetting.sy_y))
                    {
                        XFunction f = new XFunction(GraphSetting);
                        if (MathExpression(b))
                        {
                            f.Expression = MathPackage.Transformer.GetNodeFromLoycNode(b);
                        }
                        else
                        {
                            throw new Exception("The function expression is not valid.");
                        }
                        if (EnrollName)
                        {
                            f.SetName(a.Target.Name.Name);
                        }
                        else
                        {
                            f.Name = a.Target.Name.Name;
                        }
                        return f;
                    }
                    //such : area( h , b , theta) = 0.5 * h * b * sin( theta )
                    else
                    {
                        Symbol name = null;
                        List<Symbol> args = new List<Symbol>();
                        LNode Process = null;
                        if (EnrollName)
                        {
                            if (GraphSetting.IsNameUsed(b.Target.Name.Name))
                            {
                                throw new Exception("This name has been used before.");
                            }
                            name = b.Target.Name;
                        }
                        else
                        {
                            name = b.Target.Name;
                        }
                        foreach (LNode arg in a.Args)
                        {
                            if (a.IsId)
                            {
                                args.Add(arg.Name);
                            }
                            else
                            {
                                throw new Exception($"The argument {arg.ToString().Substring(0, arg.ToString().Length - 1)} of the function \"{name.ToString()}\" is invalid");
                            }
                        }
                        if (MathExpression(right))
                        {
                            Process = b;
                        }
                        else
                        {
                            throw new Exception($"The process {b.ToString().Substring(0, b.ToString().Length - 1)} of the function \"{name.ToString()}\" is invalid");
                        }


                        MathPackage.Operations.Func func = new MathPackage.Operations.Func(name, args, MathPackage.Transformer.GetNodeFromLoycNode(Process));
                        return func;
                    }
                }
                #endregion

                #region implicit functions
                // to add function  e.g. " y = sin(x)*y "
                else if (ContainsSymbol(right, GraphSetting.sy_y) ||
                    ContainsSymbol(left, GraphSetting.sy_y) ||
                    ContainsSymbol(right, GraphSetting.sy_x) ||
                    ContainsSymbol(left, GraphSetting.sy_x))
                {

                    XYFunction f = new XYFunction(GraphSetting)
                    {
                        Expression = MathPackage.Transformer.GetNodeFromLoycNode(node)
                    };
                    if (SelectName)
                    {
                        if (EnrollName)
                        {
                            f.SetName(GraphSetting.SelectName());
                        }
                        else
                        {
                            f.Name = GraphSetting.SelectName();
                        }
                    }
                    return f;

                }
                #endregion

                #region points
                //such : a = (2, 3)
                else if (left.IsId && right.Calls(CodeSymbols.Tuple))
                {
                    LNode a = right, b = left;
                    Points point = GetPointFromTuple(a, GraphSetting);
                    if (EnrollName)
                    {
                        point.SetName(b.Name.ToString());
                    }
                    else
                    {
                        point.Name = b.Name.ToString();
                    }
                    return point;
                }
                //such : (2, 3) = a
                else if (right.IsId && left.Calls(CodeSymbols.Tuple))
                {
                    LNode a = left, b = right;
                    Points point = GetPointFromTuple(a, GraphSetting);
                    if (EnrollName)
                    {
                        point.SetName(b.Name.ToString());
                    }
                    else
                    {
                        point.Name = b.Name.ToString();
                    }
                    return point;
                }
                #endregion

                #region varaibles
                //such : a = 2 * c + sin( k )
                else if (left.IsId && MathExpression(right))
                {
                    LNode b = right, a = left;
                    return new Variable(a.Name, MathPackage.Transformer.GetNodeFromLoycNode(b));
                }
                //such : 2 * c + sin( k ) = a
                else if (right.IsId && MathExpression(left))
                {
                    LNode a = right, b = left;
                    return new Variable(a.Name, MathPackage.Transformer.GetNodeFromLoycNode(b));
                }
                #endregion



                Line1:;

            }
            /// like 2+3*x = sin(y)^2
            else if ((IsBool(node)) && (ContainsSymbol(node, GraphSetting.sy_x) || ContainsSymbol(node, GraphSetting.sy_y)))
            {
                XYFunction f = new XYFunction(GraphSetting)
                {
                    Expression = MathPackage.Transformer.GetNodeFromLoycNode(node)
                };
                if (SelectName)
                {
                    if (EnrollName)
                    {
                        f.SetName(GraphSetting.SelectName());
                    }
                    else
                    {
                        f.Name = GraphSetting.SelectName();
                    }
                }
                return f;
            }
            /// to add a point like (1, 2)
            else if (node.Calls(CodeSymbols.Tuple))
            {
                Points point = GetPointFromTuple(node, GraphSetting);
                if (SelectName)
                {
                    if (EnrollName)
                    {
                        point.SetName(GraphSetting.SelectName());
                    }
                    else
                    {
                        point.Name = GraphSetting.SelectName();
                    }
                }
                return point;
            }
            /// to add function like : x^2
            else if (MathExpression(node) && ContainsSymbol(node, GraphSetting.sy_x) && !ContainsSymbol(node, GraphSetting.sy_y))
            {
                XFunction f = new XFunction(GraphSetting)
                {
                    Expression = MathPackage.Transformer.GetNodeFromLoycNode(node)
                };
                if (SelectName)
                {
                    if (EnrollName)
                    {
                        f.SetName(GraphSetting.SelectName());
                    }
                    else
                    {
                        f.Name = GraphSetting.SelectName();
                    }
                }
                return f;
            }
            else
            {
                #region "Add PointsDependant"
                if(PDsTypes().Contains(node.Target.Name.ToString()))
                {
                    PointsDependant pd = GetPD(node, GraphSetting, SelectName, EnrollName);
                    if (pd != null)
                        return pd;
                }
                #endregion

                if (node.Target.Name.ToString() == "Curve")
                {
                    if(node.Args.Count == 5)
                    {
                        // Checking for problems
                        if (!node.Args[0].IsId || !MathExpression(node.Args[1]) || !MathExpression(node.Args[2]) || !MathExpression(node.Args[3]) || !MathExpression(node.Args[4]))
                            throw new Exception($"the arguments is not valid.\n{node.ToString().Remove(0, node.ToString().Length - 1)}");
                        // Getting the object
                        ParametricFunc func = new ParametricFunc(GraphSetting)
                        {
                            Parameter = node.Args[0].Name,
                            Start = MathPackage.Transformer.GetNodeFromLoycNode(node.Args[1]),
                            End = MathPackage.Transformer.GetNodeFromLoycNode(node.Args[2]),
                            x_Expression = MathPackage.Transformer.GetNodeFromLoycNode(node.Args[3]),
                            y_Expression = MathPackage.Transformer.GetNodeFromLoycNode(node.Args[4]),
                        };
                        func.Step = null;
                        if (SelectName)
                        {
                            if (EnrollName)
                                func.SetName(GraphSetting.SelectName());
                            else
                                func.Name = GraphSetting.SelectName();
                        }
                        return func;
                    }
                    else if (node.Args.Count == 6)
                    {
                        // Checking for problems
                        if (!node.Args[0].IsId || !MathExpression(node.Args[1]) || !MathExpression(node.Args[2]) || !MathExpression(node.Args[3]) || !MathExpression(node.Args[4]) || !MathExpression(node.Args[5]))
                            throw new Exception($"the arguments is not valid.\n{node.ToString().Remove(0, node.ToString().Length - 1)}");
                        // Getting the object
                        ParametricFunc func = new ParametricFunc(GraphSetting)
                        {
                            Parameter = node.Args[0].Name,
                            Start = MathPackage.Transformer.GetNodeFromLoycNode(node.Args[1]),
                            End = MathPackage.Transformer.GetNodeFromLoycNode(node.Args[2]),
                            Step = MathPackage.Transformer.GetNodeFromLoycNode(node.Args[3]),
                            x_Expression = MathPackage.Transformer.GetNodeFromLoycNode(node.Args[4]),
                            y_Expression = MathPackage.Transformer.GetNodeFromLoycNode(node.Args[5]),
                        };
                        if (SelectName)
                        {
                            if (EnrollName)
                                func.SetName(GraphSetting.SelectName());
                            else
                                func.Name = GraphSetting.SelectName();
                        }
                        return func;
                    }
                }

            }

            throw new Exception("Your script is not valid.");
        }

        static PointsDependant GetPD(LNode node, GraphSetting GraphSetting, bool SelectName, bool EnrollName)
        {
            if (node.Target.Name.ToString() == "Circle" && node.Args.Count == 3)
            {
                // Checking the points
                for (int i = 0; i < node.Args.Count; i++)
                {
                    if (!IsPoints(node.Args[i], GraphSetting))
                    {
                        throw new Exception($"Your arguments is not valid for {node.Target.Name.ToString()}.");
                    }
                }
                // adding the points
                ThreePCircle obj = new ThreePCircle(GraphSetting);
                for (int i = 0; i < node.Args.Count; i++)
                {
                    if (node.Args[i].IsId)
                    {
                        obj.Points.Add((Points)GraphSetting.GetObjByName(node.Args[i].Name.ToString()));
                    }
                    else
                    {
                        obj.Points.Add(GetPointFromTuple(node.Args[i], GraphSetting));
                    }
                }
                if (SelectName)
                {
                    if (EnrollName)
                    {
                        obj.SetName(GraphSetting.SelectName());
                    }
                    else
                    {
                        obj.Name = GraphSetting.SelectName();
                    }
                }
                return obj;
            }
            else if (node.Target.Name.ToString() == "Circle" && node.Args.Count == 2)
            {
                // Checking the points
                for (int i = 0; i < node.Args.Count; i++)
                {
                    if (!IsPoints(node.Args[i], GraphSetting))
                    {
                        throw new Exception($"Your arguments is not valid for {node.Target.Name.ToString()}.");
                    }
                }
                // adding the points
                TwoPCircle obj = new TwoPCircle(GraphSetting);
                for (int i = 0; i < node.Args.Count; i++)
                {
                    if (node.Args[i].IsId)
                    {
                        obj.Points.Add((Points)GraphSetting.GetObjByName(node.Args[i].Name.ToString()));
                    }
                    else
                    {
                        obj.Points.Add(GetPointFromTuple(node.Args[i], GraphSetting));
                    }
                }
                if (SelectName)
                {
                    if (EnrollName)
                    {
                        obj.SetName(GraphSetting.SelectName());
                    }
                    else
                    {
                        obj.Name = GraphSetting.SelectName();
                    }
                }
                return obj;
            }
            else if (node.Target.Name.ToString() == "Circle2" && node.Args.Count == 2)
            {
                // Checking the points
                for (int i = 0; i < node.Args.Count; i++)
                {
                    if (!IsPoints(node.Args[i], GraphSetting))
                    {
                        throw new Exception($"Your arguments is not valid for {node.Target.Name.ToString()}.");
                    }
                }
                // adding the points
                TwoPCircle2 obj = new TwoPCircle2(GraphSetting);
                for (int i = 0; i < node.Args.Count; i++)
                {
                    if (node.Args[i].IsId)
                    {
                        obj.Points.Add((Points)GraphSetting.GetObjByName(node.Args[i].Name.ToString()));
                    }
                    else
                    {
                        obj.Points.Add(GetPointFromTuple(node.Args[i], GraphSetting));
                    }
                }
                if (SelectName)
                {
                    if (EnrollName)
                    {
                        obj.SetName(GraphSetting.SelectName());
                    }
                    else
                    {
                        obj.Name = GraphSetting.SelectName();
                    }
                }
                return obj;
            }
            else if (node.Target.Name.ToString() == "SemiCircle" && node.Args.Count == 2)
            {
                // Checking the points
                for (int i = 0; i < node.Args.Count; i++)
                {
                    if (!IsPoints(node.Args[i], GraphSetting))
                    {
                        throw new Exception($"Your arguments is not valid for {node.Target.Name.ToString()}.");
                    }
                }
                // adding the points
                TwoPSemiCircle obj = new TwoPSemiCircle(GraphSetting);
                for (int i = 0; i < node.Args.Count; i++)
                {
                    if (node.Args[i].IsId)
                    {
                        obj.Points.Add((Points)GraphSetting.GetObjByName(node.Args[i].Name.ToString()));
                    }
                    else
                    {
                        obj.Points.Add(GetPointFromTuple(node.Args[i], GraphSetting));
                    }
                }
                if (SelectName)
                {
                    if (EnrollName)
                    {
                        obj.SetName(GraphSetting.SelectName());
                    }
                    else
                    {
                        obj.Name = GraphSetting.SelectName();
                    }
                }
                return obj;
            }
            else if (node.Target.Name.ToString() == "Curve")
            {
                // Checking the points
                for (int i = 0; i < node.Args.Count; i++)
                {
                    if (!IsPoints(node.Args[i], GraphSetting))
                    {
                        return null;
                    }
                }
                // adding the points
                Curve obj = new Curve(GraphSetting);
                for (int i = 0; i < node.Args.Count; i++)
                {
                    if (node.Args[i].IsId)
                    {
                        obj.Points.Add((Points)GraphSetting.GetObjByName(node.Args[i].Name.ToString()));
                    }
                    else
                    {
                        obj.Points.Add(GetPointFromTuple(node.Args[i], GraphSetting));
                    }
                }
                if (SelectName)
                {
                    if (EnrollName)
                    {
                        obj.SetName(GraphSetting.SelectName());
                    }
                    else
                    {
                        obj.Name = GraphSetting.SelectName();
                    }
                }
                return obj;
            }
            else if (node.Target.Name.ToString() == "Polygone")
            {
                // Checking the points
                for (int i = 0; i < node.Args.Count; i++)
                {
                    if (!IsPoints(node.Args[i], GraphSetting))
                    {
                        throw new Exception($"Your arguments is not valid for {node.Target.Name.ToString()}.");
                    }
                }
                // adding the points
                Polygone obj = new Polygone(GraphSetting);
                for (int i = 0; i < node.Args.Count; i++)
                {
                    if (node.Args[i].IsId)
                    {
                        obj.Points.Add((Points)GraphSetting.GetObjByName(node.Args[i].Name.ToString()));
                    }
                    else
                    {
                        obj.Points.Add(GetPointFromTuple(node.Args[i], GraphSetting));
                    }
                }
                if (SelectName)
                {
                    if (EnrollName)
                    {
                        obj.SetName(GraphSetting.SelectName());
                    }
                    else
                    {
                        obj.Name = GraphSetting.SelectName();
                    }
                }
                return obj;

            }
            else if (node.Target.Name.ToString() == "Line" && node.Args.Count == 2)
            {
                // Checking the points
                for (int i = 0; i < node.Args.Count; i++)
                {
                    if (!IsPoints(node.Args[i], GraphSetting))
                    {
                        throw new Exception($"Your arguments is not valid for {node.Target.Name.ToString()}.");
                    }
                }
                // adding the points
                Line obj = new Line(GraphSetting);
                for (int i = 0; i < node.Args.Count; i++)
                {
                    if (node.Args[i].IsId)
                    {
                        obj.Points.Add((Points)GraphSetting.GetObjByName(node.Args[i].Name.ToString()));
                    }
                    else
                    {
                        obj.Points.Add(GetPointFromTuple(node.Args[i], GraphSetting));
                    }
                }
                if (SelectName)
                {
                    if (EnrollName)
                    {
                        obj.SetName(GraphSetting.SelectName());
                    }
                    else
                    {
                        obj.Name = GraphSetting.SelectName();
                    }
                }
                return obj;

            }
            return null;
        }

        public static bool IsPoints(LNode node, GraphSetting GraphSetting)
        {
            if (node.Calls(CodeSymbols.Tuple, 2))
            {
                if (MathExpression(node.Args[0]) && MathExpression(node.Args[1]))
                {
                    return true;
                }
            }
            else if (node.IsId)
            {
                if (GraphSetting.GetObjByName(node.Name.ToString()) is Points)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsBool(LNode node)
        {
            if (node.Name.IsOneOf(CodeSymbols.Eq,
                                  CodeSymbols.GT, CodeSymbols.GE, CodeSymbols.LT,
                                  CodeSymbols.LE, CodeSymbols.Eq, CodeSymbols.Not
                                  , CodeSymbols.And, CodeSymbols.Or, CodeSymbols.Neq
                                  , CodeSymbols.Xor
                                 )
               )
                return true;
            return false;
        }

        public static bool ContainsSymbol(LNode node, params Symbol[] symbol)
        {
            if (node.IsId)
            {
                foreach (Symbol sy in symbol)
                {
                    if (node.Name.Name == sy.Name)
                        return true;
                }
                return false;
            }
            if (node.IsLiteral)
            {
                return false;
            }
            for (int i = 0; i < node.Args.Count; i++)
            {
                if (node.Target.Name != (Symbol)"'." && ContainsSymbol(node.Args[i], symbol))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool ContainsListSymbol(LNode node, List<Graphing.Lists.NumbersList> numsLists)
        {
            if (node.IsId)
            {
                foreach (Graphing.Lists.NumbersList nums_list in numsLists)
                {
                    if (nums_list.Name == node.Name.Name)
                        return true;
                }
                return false;
            }
            if (node.IsLiteral)
            {
                return false;
            }
            for (int i = 0; i < node.Args.Count; i++)
            {
                if (node.Target.Name != (Symbol)"'." && ContainsListSymbol(node.Args[i], numsLists))
                    return true;
            }
            return false;
        }

        public static bool MathExpression(LNode node)
        {
            if (node.IsId || node.IsLiteral)
            {
                return true;
            }
            else if (node.Calls(CodeSymbols.Tuple))
            {
                return false;
            }
            foreach (LNode node_ in node.Args)
            {
                if (!MathExpression(node_))
                {
                    return false;
                }
            }
            return true;
        }

        public static bool IsFuncId(LNode node)
        {
            if (node.Style == (NodeStyle.PrefixNotation | NodeStyle.OneLiner) || node.Style == NodeStyle.PrefixNotation)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// you should pass a {Tuple} LNode : e.g. : (2, 5*r+6)
        /// </summary>
        public static Points GetPointFromTuple(LNode node, GraphSetting GraphSetting)
        {
            return new Points(MathPackage.Transformer.GetNodeFromLoycNode(node.Args[0]), MathPackage.Transformer.GetNodeFromLoycNode(node.Args[1]), GraphSetting);
        }


        #endregion

        #region Changing Names

        public static void CheckName(string Name)
        {
            if (string.IsNullOrEmpty(Name) || !MathPackage.Transformer.IsAlpha(Name.Replace("_","")) || RestrictedNames().Contains(Name))
                throw new Exception($"\"{Name}\" is not avaliable to use.");
        }

        #endregion

        #region Colors

        public static bool IsColorDark(System.Drawing.Color color)
        {
            if (color.GetBrightness() > 0.35)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static Color RandomColor()
        {
            Random r = new Random();
            return Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));
        }
        public static Color RandomDarkColor()
        {
            Random r = new Random();
            Color c = Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));
            do { c = Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255)); } while (!IsColorDark(c));
            return c;
        }
        public static Color RandomLightColor()
        {
            Random r = new Random();
            Color c = Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255));
            do { c = Color.FromArgb(255, r.Next(255), r.Next(255), r.Next(255)); } while (IsColorDark(c));
            return c;
        }

        #endregion

        #region Drawing

        public static double GetSloop(Points origin, Points point)
        {
            double sloop;
            double x1, y1, x2, y2;
            x1 = point.Get_X_Value();
            y1 = point.Get_Y_Value();
            x2 = origin.Get_X_Value();
            y2 = origin.Get_Y_Value();

            try
            {
                sloop = Math.Atan((y1 - y2) / (x1 - x2));
                if (x1 < x2)
                    sloop += Math.PI;
                else if (x1 > x2 && y1 < y2)
                    sloop += 2 * Math.PI;
            }
            catch
            {
                sloop = Math.PI / 2;
                if (y1 < y2)
                    sloop += Math.PI;
            }


            if (sloop == double.NaN)
                throw new Exception("NaN");

            return sloop;
        }

        public static void DrawText(Graphics g, string text, Font Font, PointF Location, Color Color, Color BackColor, StringAlignment hAlign = StringAlignment.Center, StringAlignment vAlign = StringAlignment.Center)
        {
            using (Brush brush = new SolidBrush(Color))
            {
                SizeF size = g.MeasureString(text, Font);
                float x = Align(Location.X, size.Width, hAlign);
                float y = Align(Location.X, size.Height, vAlign);
                using (var rectbrush = new SolidBrush(BackColor))
                    g.FillRectangle(rectbrush, x, y, size.Width, size.Height);
                g.DrawString(text, Font, Brushes.White, x - 1, y);
                g.DrawString(text, Font, Brushes.White, x + 1, y);
                g.DrawString(text, Font, Brushes.White, x, y - 1);
                g.DrawString(text, Font, Brushes.White, x, y + 1);
                g.DrawString(text, Font, brush, x, y);
            }
        }
        static float Align(float x, float size, StringAlignment align) =>
        align == StringAlignment.Center ? x - size / 2 :
        align == StringAlignment.Far ? x - size : x;

        /// <summary>
        ///         ''' Draw an arc.
        ///         ''' </summary>
        ///         ''' <param name="start_angle">In degree</param>
        ///         ''' <param name="end_angle">In degree</param>
        ///         ''' <param name="circle_center">As a System.Drawing.Point</param>
        ///         ''' <param name="Xradius">As number of pixels.</param>
        ///         ''' <param name="Yradius">As number of pixels.</param>
        ///         ''' <param name="clockWise"></param>
        public static void Draw_arc(Graphics g, Pen Pen, double start_angle, double sweep_angle, PointF circle_center, double Xradius, double Yradius, bool clockWise = false)
        {

            double end, step;
            if (clockWise)
            {
                end = start_angle - sweep_angle;
                step = -0.05;
                List<PointF> points = new List<PointF>();
                for (double angle = start_angle; angle >= start_angle - sweep_angle * Math.Sign(start_angle - end); angle += step)
                    points.Add(new PointF((float)(circle_center.X + Xradius * Math.Cos(angle)), (float)(circle_center.Y - Yradius * Math.Sin(angle))));

                if (points.Count > 1)
                {
                    g.DrawLines(Pen, points.ToArray());
                }
            }
            else
            {
                end = start_angle + sweep_angle;
                step = 0.05;
                List<PointF> points = new List<PointF>();

                for (double angle = start_angle; angle <= start_angle + sweep_angle * Math.Sign(end - start_angle); angle += step)
                    points.Add(new PointF((float)(circle_center.X + Xradius * Math.Cos(angle)), (float)(circle_center.Y - Yradius * Math.Sin(angle))));

                if (points.Count > 1)
                {
                    g.DrawLines(Pen, points.ToArray());
                }
            }


        }

        /// <summary>
        ///         ''' Draw an arc.
        ///         ''' </summary>
        ///         ''' <param name="start_angle">In degree</param>
        ///         ''' <param name="end_angle">In degree</param>
        ///         ''' <param name="circle_center">As a System.Drawing.Point</param>
        ///         ''' <param name="Xradius">As number of pixels.</param>
        ///         ''' <param name="Yradius">As number of pixels.</param>
        ///         ''' <param name="clockWise"></param>
        public static void DrawClosed_arc(Graphics g, Pen Pen, double start_angle, double sweep_angle, PointF circle_center, double Xradius, double Yradius, bool clockWise = false)
        {
            double end, step;
            if (clockWise)
            {
                end = start_angle - sweep_angle;
                step = -0.05;
                List<PointF> points = new List<PointF>();
                for (double angle = start_angle; angle >= start_angle - sweep_angle * Math.Sign(start_angle - end); angle += step)
                    points.Add(new PointF((float)(circle_center.X + Xradius * Math.Cos(angle)), (float)(circle_center.Y - Yradius * Math.Sin(angle))));
                points.Add(circle_center);

                if (points.Count > 1)
                {
                    points.Add(points.First());
                    g.DrawLines(Pen, points.ToArray());
                }
            }
            else
            {
                end = start_angle + sweep_angle;
                step = 0.05;
                List<PointF> points = new List<PointF>();

                for (double angle = start_angle; angle <= start_angle + sweep_angle * Math.Sign(end - start_angle); angle += step)
                    points.Add(new PointF((float)(circle_center.X + Xradius * Math.Cos(angle)), (float)(circle_center.Y - Yradius * Math.Sin(angle))));
                points.Add(circle_center);

                if (points.Count > 1)
                {
                    points.Add(points.First());
                    g.DrawLines(Pen, points.ToArray());
                }
            }

        }

        /// <summary>
        ///         ''' Draw filled arc.
        ///         ''' </summary>
        ///         ''' <param name="start_angle">In degree</param>
        ///         ''' <param name="end_angle">In degree</param>
        ///         ''' <param name="circle_center">As a System.Drawing.Point</param>
        ///         ''' <param name="Xradius">As number of pixels.</param>
        ///         ''' <param name="Yradius">As number of pixels.</param>
        ///         ''' <param name="clockWise"></param>
        public static void Fill_arc(Graphics g, Pen Pen, double start_angle, double sweep_angle, PointF circle_center, double Xradius, double Yradius, bool clockWise)
        {
            double end, step;
            if (clockWise)
            {
                end = start_angle - sweep_angle;
                step = -0.05;
                List<PointF> points = new List<PointF>();
                for (double angle = start_angle; angle >= start_angle - sweep_angle * Math.Sign(start_angle - end); angle += step)
                    points.Add(new PointF((float)(circle_center.X + Xradius * Math.Cos(angle)), (float)(circle_center.Y - Yradius * Math.Sin(angle))));
                points.Add(circle_center);

                if (points.Count > 1)
                {
                    g.FillClosedCurve(new SolidBrush(Color.FromArgb(150, Pen.Color.R, Pen.Color.G, Pen.Color.B)), points.ToArray());
                    points.Add(points.First());
                    g.DrawLines(Pen, points.ToArray());
                }
            }
            else
            {
                end = start_angle + sweep_angle;
                step = 0.05;
                List<PointF> points = new List<PointF>();

                for (double angle = start_angle; angle <= start_angle + sweep_angle * Math.Sign(end - start_angle); angle += step)
                    points.Add(new PointF((float)(circle_center.X + Xradius * Math.Cos(angle)), (float)(circle_center.Y - Yradius * Math.Sin(angle))));
                points.Add(circle_center);

                if (points.Count > 1)
                {
                    g.FillClosedCurve(new SolidBrush(Color.FromArgb(150, Pen.Color.R, Pen.Color.G, Pen.Color.B)), points.ToArray());
                    points.Add(points.First());
                    g.DrawLines(Pen, points.ToArray());
                }
            }
        }

        #endregion

        #region Get
     
        // {Color= Argb(255,3,126,125) ,Width=2 , Style = Dash}"
        public static string GetPenAsString(Pen pen)
        {
            string str = "";
            str += $"Color={GetColorAsString(pen.Color)}, ";
            str += $"Width={pen.Width}, ";
            str += $"Style={pen.DashStyle}";
            return "{" + str + "}";
        }
        // "{Color= Argb(255,3,126,125) ,Width=2 , Style = Dash}"
        public static Pen GetPenFromString(string str, MathPackage.CalculationSetting calculationSetting)
        {
            Pen pen = new Pen(Color.White);
            LNode value = ParseExprs(str);
            if (!value.Calls((Symbol)"'{}"))
                throw new Exception($"Pen is not valid.\n{str}");
            for (int i = 0; i < value.Args.Count; i++)
            {
                if(value.Args[i].ToString() == ";")
                {
                    switch (i)
                    {
                        case 0:
                            pen.Color = DefaultPen.Color;
                            break;
                        case 1:
                            pen.Width = DefaultPen.Width;
                            break;
                        case 2:
                            pen.DashStyle = DefaultPen.DashStyle;
                            break;
                    }
                }
                else if (value.Args[i].Calls(CodeSymbols.Assign))
                {
                    string str_ = value.Args[i].Args[1].ToString().Remove(value.Args[i].Args[1].ToString().Length - 1, 1);
                    switch (value.Args[i].Args[0].ToString().Remove(value.Args[i].Args[0].ToString().Length - 1, 1))
                    {
                        case "Color":
                            pen.Color = GetColorFromString(str_, calculationSetting);
                            break;
                        case "Width":
                            pen.Width = (int)MathPackage.Main.CalculateString(str_, calculationSetting);
                            break;
                        case "Style":
                            pen.DashStyle = PenDashStyle(str_);
                            break;
                        default:
                            throw new Exception($"Your pen is not valid.\n{str}");
                    }
                }
                else
                {
                    string str_ = value.Args[i].ToString().Remove(value.Args[i].ToString().Length - 1, 1);
                    switch (i)
                    {
                        case 0:
                            pen.Color = GetColorFromString(str_, calculationSetting);
                            break;
                        case 1:
                            pen.Width = (int)MathPackage.Main.CalculateString(str_, calculationSetting);
                            break;
                        case 2:
                            pen.DashStyle = PenDashStyle(str_);
                            break;
                    }
                }
            }
            return pen;
        }
        static System.Drawing.Drawing2D.DashStyle PenDashStyle(string str)
        {
            switch (str)
            {
                case "Dash":
                    return System.Drawing.Drawing2D.DashStyle.Dash;
                case "DashDot":
                    return System.Drawing.Drawing2D.DashStyle.DashDot;
                case "DashDotDot":
                    return System.Drawing.Drawing2D.DashStyle.DashDotDot;
                case "Solid":
                    return System.Drawing.Drawing2D.DashStyle.Solid;
                case "Dot":
                    return System.Drawing.Drawing2D.DashStyle.Dot;
            }
            return System.Drawing.Drawing2D.DashStyle.Solid;
        }

        public static string GetColorAsString(Color color)
        {
            return $"Argb({color.A},{color.R},{color.G},{color.B})";
        }
        public static Color GetColorFromString(string str, MathPackage.CalculationSetting calculationSetting)
        {
            LNode value = ParseExprs(str);
            List<int> color_nums = new List<int>();
            foreach (LNode node in value.Args)
            {
                color_nums.Add((int)MathPackage.Main.CalculateLNode(node, calculationSetting));
            }
            if (value.Calls((Symbol)"Argb") || value.Calls((Symbol)"argb"))
            {
                return Color.FromArgb(color_nums[0], color_nums[1], color_nums[2], color_nums[3]);
            }
            else if (value.Calls((Symbol)"rgb") || value.Calls((Symbol)"Rgb"))
            {
                return Color.FromArgb(255, color_nums[1], color_nums[2], color_nums[3]);
            }
            else if (value.IsLiteral && value.ToString().Contains('#'))
            {
                string str_ = value.ToString().Remove(value.ToString().Length - 1, 1);
                return ColorTranslator.FromHtml(str_);  
            }
            else
                throw new Exception($"Your color is not valid.\n{str}");
        }

        public static string GetFontAsString(Font Font)
        {
            string str = "";
            str += Font.FontFamily.ToString().Replace("[FontFamily: Name=", "/").Split('/')[1].Replace("]", "");
            str += ",";
            str += Font.Size.ToString();
            str += ",";
            str += Font.Bold.ToString();
            str += ",";
            str += Font.Italic.ToString();

            return str;
        }
        public static Font GetFontFromString(string Str)
        {
            string[] str_ = Str.Split(',');
            Font font = new Font(str_[0], float.Parse((str_[1])));
            if (str_[2].ToLower() == "true")
                font = new Font(str_[0], Convert.ToSingle(double.Parse(str_[1])), FontStyle.Bold);
            if (str_[3].ToLower() == "true")
                font = new Font(str_[0], Convert.ToSingle(double.Parse(str_[1])), FontStyle.Italic);
            return font;
        }
   
        #endregion

    }
}