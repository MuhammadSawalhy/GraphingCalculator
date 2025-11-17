using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Loyc.Syntax;
using Loyc.Syntax.Les;
using Loyc;
using System.Xml.Linq;
using MathPackage.Operations;
using number = System.Double;	// Change this line to make a calculator for a different data type 

namespace MathPackage
{
    public class Transformer
    {
        private static readonly string[] AR_Nums = "٠,١,٢,٣,٤,٥,٦,٧,٨,٩".Split(',');

        public static string ToArNumbers(string str, string TextToAppend = "")
        {
            str = str.Replace("0", AR_Nums[0]).Replace("1", AR_Nums[1]).Replace("2", AR_Nums[2]).Replace("3", AR_Nums[3]).Replace("4", AR_Nums[4]).Replace("5", AR_Nums[5]).Replace("6", AR_Nums[6]).Replace("7", AR_Nums[7]).Replace("8", AR_Nums[8]).Replace("9", AR_Nums[9]);
            if (str.Contains("-"))
                return str.Replace("-", "") + TextToAppend + "-";
            return str + TextToAppend;
        }
        public static string ToArNumbers(double val, string TextToAppend = "")
        {
            String str = val.ToString();
            str = str.Replace("0", AR_Nums[0]).Replace("1", AR_Nums[1]).Replace("2", AR_Nums[2]).Replace("3", AR_Nums[3]).Replace("4", AR_Nums[4]).Replace("5", AR_Nums[5]).Replace("6", AR_Nums[6]).Replace("7", AR_Nums[7]).Replace("8", AR_Nums[8]).Replace("9", AR_Nums[9]);
            if (str.Contains("-"))
                return str.Replace("-", "") + TextToAppend + "-";
            return str + TextToAppend;
        }

        /// <summary>
        /// This will convert from real numbers to degrees (angle) like 3.23 => 3˚ 13' 48'' 
        /// </summary>
        /// <param name="text_"></param>
        /// <param name="lang"></param>
        /// <returns></returns>
        public static string AngleDegrees(string text_, Main.Language lang = Main.Language.EN)
        {
            string deg, min, sec;
            sec = "";
            if (Main.IsNumeric(text_) && text_.Contains("."))
            {
                string a, s;
                s = text_;
                a = text_.Substring(0, text_.IndexOf("."));
                deg = a;
                a = (double.Parse("0." + (long.Parse(s.Substring(s.IndexOf(".") + 1))) * 60).ToString()).ToString();
                if (a.Contains("."))
                {
                    s = a;
                    a = s.Substring(0, s.ToString().IndexOf("."));
                    min = a;
                    a = (double.Parse("0." + (long.Parse(s.Substring(s.IndexOf(".") + 1))) * 60).ToString()).ToString();
                    sec = (Main.Approximate(a, "9", "0", "##")).ToString();
                    if (double.Parse(sec) == 60)
                    {
                        if (double.Parse(min) + 1 == 60)
                            return deg + 1 + "˚";
                        else
                            return deg + "˚ " + Main.Approximate(min + 1, "##").ToString();
                    }
                    else
                        return deg + "˚ " + Main.Approximate(min, "##").ToString() + "' " + Main.Approximate(sec, "##").ToString() + "''";
                }
                else
                {
                    min = (Main.Approximate(a, "9", "0", "##")).ToString();
                    if (double.Parse(min) == 60)
                        return deg + 1 + "˚";
                    else
                        return deg + "˚ " + Main.Approximate(min, "##").ToString() + "'";
                }
            }
            else
                return text_ + "˚";
        }
        public static bool IsAlpha(char C)
        {
            if (Main.IsNumeric(C.ToString()) || IsSymbol(C))
                return false;
            else
                return true;
        }
        public static bool IsSymbol(char C)
        {
            string str = "_÷×+-*|/^%!@#$&*(){}/>:,;.~`÷×®©\"";
            if (str.Contains(C))
                return true;
            else
                return false;
        }
        public static bool IsAlpha(string S)
        {

            if (Main.IsNumeric(S))
                return false;
            else
            {
                bool is_symbol = false;

                foreach (char c in S)
                {
                    if (IsSymbol(c))
                    {
                        is_symbol = true;
                        break;
                    }
                }

                return !is_symbol;
            }
        }

        #region "Reforming"

        /// <summary>
        /// Get the string before the bracket like: cos,sin,log......
        /// </summary>
        /// <returns></returns>

        private static string replace(string str)
        {

            str = System.Text.RegularExpressions.Regex.Replace(str, @"([-+*/%^&*|<>=?.])([-~!+])", "$1 $2");
            str = System.Text.RegularExpressions.Regex.Replace(str, @"\^", "**");

            return str;
        }

        private string ReplaceFunctions(string str, Main.Language fromLang, Main.Language toLang)
        {
            switch (fromLang)
            {
                case Main.Language.AR:
                    {
                        switch (toLang)
                        {
                            case Main.Language.EN:
                                {
                                    str = str.Replace("جا", "sin");
                                    str = str.Replace("جتا", "cos");
                                    str = str.Replace("ظا", "tan");
                                    str = str.Replace("هـ", "e");
                                    str = str.Replace("طـ", "pi");
                                    str = str.Replace("π", "pi");
                                    str = str.Replace("جا_ع", "asin");
                                    str = str.Replace("جتا_ع", "acos");
                                    str = str.Replace("ظا_ع", "atan");
                                    str = str.Replace("مطلق", "abs");
                                    str = str.Replace("لو", "log");
                                    str = str.Replace("لو_ط", "ln");
                                    str = str.Replace("أ_ط", "exp");
                                    break;
                                }
                        }

                        break;
                    }

                case Main.Language.EN:
                    {
                        switch (toLang)
                        {
                            case Main.Language.AR:
                                {
                                    str = str.Replace("sin", "جا");
                                    str = str.Replace("cos", "جتا");
                                    str = str.Replace("tan", "ظا");
                                    str = str.Replace("e", "هـ");
                                    str = str.Replace("pi", "طـ");
                                    str = str.Replace("π", "طـ");
                                    str = str.Replace("asin", "جا_ع");
                                    str = str.Replace("acos", "جتا_ع");
                                    str = str.Replace("atan", "ظا_ع");
                                    str = str.Replace("abs", "مطلق");
                                    str = str.Replace("log", "لو");
                                    str = str.Replace("ln", "لو_ط");
                                    str = str.Replace("exp", "أ_ط");
                                    break;
                                }
                        }

                        break;
                    }
            }
            return str;
        }

        public static List<LNode> ParseExprsList(string text, string fieldName = "")
        {
            // Separate things like *- into two separate operators (* -), and change ^ to **
            text = replace(text);
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
            return Les3LanguageService.Value.Parse(text, errorHandler).ToList();
        }

        public static LNode ParseExprs(string text, string fieldName = "")
        {

            // Separate things like *- into two separate operators (* -), and change ^ to **
            text = System.Text.RegularExpressions.Regex.Replace(text, @"([-+*/%^&*|<>=?.])([-~!+])", "$1 $2");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\^", "**");

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
                throw new Exception("This expression is not valid.");
            }

            return Les3LanguageService.Value.Parse(text, errorHandler).ToList()[0];

        }

        public static LNode ParseCondition(string text, string fieldName = "")
        {

            // Separate things like *- into two separate operators (* -), and change ^ to **
            text = System.Text.RegularExpressions.Regex.Replace(text, @"([-+*/%^&*|<>=?.])([-~!+])", "$1 $2");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\^", "**");

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
                throw new Exception("This expression is not valid.");
            }

            LNode node = Les3LanguageService.Value.Parse(text, errorHandler).ToList()[0];
            if (!ExprIsCondition(node))
            {
                throw new Exception("This expression is not conditional.");
            }

            return node;
        }

        #endregion

        #region "Get"

        public static Expression GetExpressionFromString(string str)
        {
            LNode str_ = GetLoycNodeFromString(str);
            return new Expression(str_);
        }

        ///this is not for ths Nodes without Childrens
        public static Node GetNodeByType(string type, Node[] Children)
        {
            switch (type)
            {
                case "add":
                    return new Add(Children[0], Children[1]);
                case "subtract":
                    return new Subtract(Children[0], Children[1]);
                case "multiply":
                    return new Multiply(Children[0], Children[1]);
                case "divide":
                    return new Divide(Children[0], Children[1]);
                case "power":
                    return new Power(Children[0], Children[1]);
                case "mod":
                    return new Mod(Children[0], Children[1]);
                case "log":
                    Log l = new Log(Children[0]);
                    if (Children.Length > 1)
                        l.Children[1] = Children[1];
                    return l;
                case "sin":
                    return new Sin(Children[0]);
                case "cos":
                    return new Cos(Children[0]);
                case "tan":
                    return new Tan(Children[0]);
                case "asin":
                    return new Asin(Children[0]);
                case "acos":
                    return new Acos(Children[0]);
                case "atan":
                    return new Atan(Children[0]);
                case "atan2":
                    return new Atan2(Children[0], Children[1]);
                case "csc":
                    return new Csc(Children[0]);
                case "sec":
                    return new Sec(Children[0]);
                case "cot":
                    return new Cot(Children[0]);
                case "abs":
                    return new Abs(Children[0]);
                case "fact":
                    return new Factorial(Children[0]);
                case "exp":
                    return new Exp(Children[0]);
                case "sqrt":
                    return new Sqrt(Children[0]);
                case "sign":
                    return new Sign(Children[0]);
                case "root":
                    return new Root(Children[0], Children[1]);
                case "neg":
                    return new Neg(Children[0]);
                case "min":
                    return new Min(Children[0], Children[1]);
                case "max":
                    return new Max(Children[0], Children[1]);
                case "ln":
                    return new Ln(Children[0]);
                case "floor":
                    return new Floor(Children[0]);
                case "clamp":
                    return new Clamp(Children);
                case "ceil":
                    return new Ceil(Children[0]);
                case "c":
                    return new C(Children[0], Children[1]);
                case "p":
                    return new P(Children[0], Children[1]);
                case "and":
                    return new And(Children[0], Children[1]);
                case "equal":
                    return new Equal(Children[0], Children[1]);
                case "greaterthan":
                    return new GreaterThan(Children[0], Children[1]);
                case "greaterequal":
                    return new GreaterEqual(Children[0], Children[1]);
                case "if":
                    return new If((Operations.Boolean)Children[0], Children[1], Children.Count() == 2 ? Children[2] : null);
                case "in":
                    return new In(Children);
                case "lowerqual":
                    return new LowerEqual(Children[0], Children[1]);
                case "lowerthan":
                    return new LowerThan(Children[0], Children[1]);
                case "not":
                    return new Not(Children[0]);
                case "notequal":
                    return new NotEqual(Children[0], Children[1]);
                case "or":
                    return new Or(Children[0], Children[1]);
                case "xor":
                    return new Xor(Children[0], Children[1]);
                case "band":
                    return new BAnd(Children[0], Children[1]);
                case "bnot":
                    return new BNot(Children[0]);
                case "bor":
                    return new BOr(Children[0], Children[1]);
                case "bxor":
                    return new BXor(Children[0], Children[1]);
                case "nullcoalesce":
                    return new NullCoalesce(Children[0], Children[1]);
                case "shiftright":
                    return new ShiftLeft(Children[0], Children[1]);
                case "shiftleft":
                    return new ShiftLeft(Children[0], Children[1]);
            }
            return null;
        }

        #region "Get GetXml"

        public static Node GetNodeFromXml(XElement Node)
        {
            switch (Node.Name.ToString().ToLower())
            {
                case "varaible":
                    return new Variable(Node.Value);
                case "constant":
                    if (!Transformer.GetNodeFromString(Node.Value).ContainsVaraible)
                    {
                        return new Constant(Transformer.GetNodeFromString(Node.Value).Calculate(new CalculationSetting(), new Dictionary<Symbol, Node>()));
                    }
                    else
                        throw new Exception("");

               default:
                        List<Node> Children = new List<Node>();
                        List<XElement> XChildren = new List<XElement>();
                        XChildren.AddRange(Node.Elements().ToList());
                        for (int i = 0; i < XChildren.Count; i++) { Children.Add(GetNodeFromXml(XChildren[i])); }
                        return GetNodeByType(Node.Name.ToString(), Children.ToArray());
            }
        }

        public static string GetNodeAsXml(Node node)
        {
            XElement str = XElement.Parse(Get_xmlNode(node));
            return str.ToString();
        }

        private static string Get_xmlNode(Node Node)
        {

            StringBuilder str = new StringBuilder();
            switch (Node)
            {
                case Log l:
                    str.Append("<Log>");
                    str.Append(Get_xmlNode(l.Children[0]));
                    if (l.Children[1] != null)
                        str.Append(Get_xmlNode(l.Children[1]));
                    str.Append("</Log>");
                    return str.ToString();
                case Variable variable:
                    str.Append("<Variable>");
                    str.Append(variable.Name);
                    str.Append("</Variable>");
                    return str.ToString();
                case Constant constant:
                    str.Append("<Constant>");
                    str.Append(constant.Value);
                    str.Append("</Constant>");
                    return str.ToString();
                default:
                    str.Append('<' + Node.Type.ToString() + '>');
                    foreach (Node child in Node.Children)
                    {
                        str.Append(Get_xmlNode(child));
                    }
                    str.Append("</" + Node.Type.ToString() + '>');
                    return str.ToString();
            }
        }

        #endregion

        #region "Get GetMathMl"

        //public string GetNodeAsMathMl(Node node)
        //{
        //    System.Xml.Linq.XElement str = XElement.Parse(GetAsMathMl_(node));
        //    return str.ToString();
        //}

        //public string GetAsMathMl_(Node Node)
        //{
        //    StringBuilder str = new StringBuilder();
        //    switch (Node.Syntax_Type)
        //    {
        //        case SyntaxType.Operator:
        //            str.Append("<mrow>");
        //            if (((Node.Children[0].GetNodeType() == "Constant") & (Node.Children[1].GetNodeType() == "Variable")) || ((Node.Children[0].GetNodeType() == "Variable") & (Node.Children[1].GetNodeType() == "Constant")))
        //            {
        //                str.AppendLine(GetAsMathMl_(Node.Children[0]));
        //                str.AppendLine("<mo>*</mo>");
        //                str.AppendLine(GetAsMathMl_(Node.Children[1]));
        //            }
        //            else if ((Node.Children[0].GetNodeType() == "Variable") || (Node.Children[0].GetNodeType() == "Constant"))
        //            {
        //                str.AppendLine(GetAsMathMl_(Node.Children[0]));
        //                if (Node.Children[1].Syntax_Type == SyntaxType.Function)
        //                {
        //                    str.AppendLine(GetAsMathMl_(Node.Children[1]));
        //                }
        //                else
        //                {
        //                    str.AppendLine("<mi>(</mi>");
        //                    str.AppendLine(GetAsMathMl_(Node.Children[1]));
        //                    str.AppendLine("<mi>)</mi>");
        //                }
        //            }
        //            else if ((Node.Children[1].GetNodeType() == "Variable") || (Node.Children[1].GetNodeType() == "Constant"))
        //            {
        //                str.AppendLine(GetAsMathMl_(Node.Children[1]));
        //                if (Node.Children[0].Syntax_Type == SyntaxType.Function)
        //                {
        //                    str.AppendLine(GetAsMathMl_(Node.Children[0]));
        //                }
        //                else
        //                {
        //                    str.AppendLine("<mi>(</mi>");
        //                    str.AppendLine(GetAsMathMl_(Node.Children[0]));
        //                    str.AppendLine("<mi>)</mi>");
        //                }
        //            }
        //            else
        //            {
        //                if (Node.Children[1].Syntax_Type == SyntaxType.Function)
        //                {
        //                    str.AppendLine(GetAsMathMl_(Node.Children[0]));
        //                    str.AppendLine("<mi>(</mi>");
        //                    str.AppendLine(GetAsMathMl_(Node.Children[1]));
        //                    str.AppendLine("<mi>)</mi>");
        //                }
        //                else if (Children[1].IsCommonFunc)
        //                {
        //                    str.AppendLine(GetAsMathMl_(Node.Children[1]));
        //                    str.AppendLine("<mi>(</mi>");
        //                    str.AppendLine(GetAsMathMl_(Node.Children[0]));
        //                    str.AppendLine("<mi>)</mi>");
        //                }
        //                else
        //                {
        //                    str.AppendLine("<mi>(</mi>");
        //                    str.AppendLine(GetAsMathMl_(Node.Children[0]));
        //                    str.AppendLine("<mi>)</mi>");
        //                    str.AppendLine("<mi>(</mi>");
        //                    str.AppendLine(GetAsMathMl_(Node.Children[1]));
        //                    str.AppendLine("<mi>)</mi>");
        //                }
        //            }
        //            str.Append("</mrow>");
        //            return str.ToString();
        //        case SyntaxType.Literal:
        //            str.Append("<mi>" + variable.Name + "</mi>");
        //            return str.ToString();
        //        case SyntaxType.Function:
        //            str.Append("<mrow>");
        //            str.Append("<mi>" + Node.GetNodeType() + "</mi>");
        //            str.Append("<mi>(</mi>");
        //            str.Append(GetAsMathMl_(Node.Children[0]));
        //            str.Append("<mi>)</mi>");
        //            str.Append("</mrow>");
        //            return str.ToString();
        //    }
        //    return null;
        //}

        #endregion

        public static LNode GetLoycNodeFromString(string text)
        {
            // Separate things like *- into two separate operators (* -), and change ^ to **
            text = System.Text.RegularExpressions.Regex.Replace(text, @"([-+*/%^&*|<>=?.])([-~!+])", "$1 $2");
            text = System.Text.RegularExpressions.Regex.Replace(text, @"\^", "**");

            var errorHandler = MessageSink.FromDelegate((severity, ctx, fmt, args) =>
            {
                if (severity >= Severity.Error)
                {
                    var msg = fmt.Localized(args);
                    if (ctx is SourceRange)
                        msg += $"\r\n{text}\r\n{new string('-', ((SourceRange)ctx).Start.PosInLine - 1)}^";
                    throw new LogException(severity, ctx, msg);
                }
            });
            return Les3LanguageService.Value.Parse(text, errorHandler).ToList()[0];
        }

        public static Node GetNodeFromLoycNode(LNode node)
        {
            LNode expr = node;
            if (expr.IsLiteral)
            {
                if (Main.IsNumeric(expr.Value.ToString()))
                    return new Constant(double.Parse(expr.Value.ToString()));
            }
            if (expr.IsId)
                return new Variable(expr.Name.ToString());
            
            else if (expr.ArgCount == 2)
            {
                {
                    LNode a, b, hi, lo, tmp_10, tmp_11 = null;

                    if (expr.Calls(CodeSymbols.And, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null
                        || expr.Calls((Symbol)"'and", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new And(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls(CodeSymbols.Or, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null
                        || expr.Calls((Symbol)"'or", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new Or(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls(CodeSymbols.GT, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new GreaterThan(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls(CodeSymbols.LT, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new LowerThan(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls(CodeSymbols.GE, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new GreaterEqual(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls(CodeSymbols.LE, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new LowerEqual(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if ((expr.Calls(CodeSymbols.Eq, 2) || expr.Calls(CodeSymbols.Assign, 2)) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new Equal(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls(CodeSymbols.Neq, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new NotEqual(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls(CodeSymbols.AndBits, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new BAnd(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls(CodeSymbols.OrBits, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new BOr(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls(CodeSymbols.NullCoalesce, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new NullCoalesce(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls((Symbol)"'xor", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new Xor(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls((Symbol)"xor", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new BXor(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));


                    else if(expr.Calls(CodeSymbols.Add, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new Add(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls(CodeSymbols.Mul, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new Multiply(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls(CodeSymbols.Sub, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new Subtract(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls(CodeSymbols.Div, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new Divide(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls(CodeSymbols.Mod, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new Mod(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls(CodeSymbols.Exp, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new Power(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls(CodeSymbols.Shr, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new ShiftRight(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls(CodeSymbols.Shl, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new ShiftLeft(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));


                    else if (expr.Calls((Symbol)"min", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new Min(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls((Symbol)"max", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new Max(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));

                    else if (expr.Calls((Symbol)"mod", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null
                        || expr.Calls((Symbol)"'mod", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new Mod(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls((Symbol)"atan", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new Atan2(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls((Symbol)"log", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new Log(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));

                    else if (expr.Calls((Symbol)"'in", 2) && (a = expr.Args[0]) != null && (tmp_10 = expr.Args[1]) != null && tmp_10.Calls(CodeSymbols.Tuple, 2) && (lo = tmp_10.Args[0]) != null && (hi = tmp_10.Args[1]) != null)
                        return new In(new[] { GetNodeFromLoycNode(a), GetNodeFromLoycNode(lo), GetNodeFromLoycNode(hi) });

                    else if (expr.Calls((Symbol)"'in", 2) && (a = expr.Args[0]) != null && (tmp_10 = expr.Args[1]) != null && (tmp_10.Calls((Symbol)"'[]", 2) || tmp_10.Calls((Symbol)"'[]", 2)) && (lo = tmp_10.Args[0]) != null && (hi = tmp_10.Args[1]) != null)
                        return new In(new[] { GetNodeFromLoycNode(a), GetNodeFromLoycNode(lo), GetNodeFromLoycNode(hi) });

                    else if (expr.Calls((Symbol)"'clamp", 2) && (a = expr.Args[0]) != null && (tmp_11 = expr.Args[1]) != null && ( tmp_11.Calls(CodeSymbols.Tuple, 2) || tmp_11.Calls((Symbol)"'[]", 2)) && (lo = tmp_11.Args[0]) != null && (hi = tmp_11.Args[1]) != null
                        || expr.Calls((Symbol)"clamp", 3) && (a = expr.Args[0]) != null && (lo = expr.Args[1]) != null && (hi = expr.Args[2]) != null)
                        return new Clamp(new[] { GetNodeFromLoycNode(a), GetNodeFromLoycNode(lo), GetNodeFromLoycNode(hi) });

                    else if (expr.Calls((Symbol)"'P", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null 
                        || expr.Calls((Symbol)"P", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new P(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
                    else if (expr.Calls((Symbol)"'C", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null
                        || expr.Calls((Symbol)"C", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return new C(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));

                }
            }
            {
                LNode a, b, c, tmp_12, tmp_11;
                if (expr.Calls(CodeSymbols.Sub, 1) && (a = expr.Args[0]) != null) return new Neg(GetNodeFromLoycNode(a));
                else if (expr.Calls(CodeSymbols.Add, 1) && (a = expr.Args[0]) != null) return new Abs(GetNodeFromLoycNode(a));
                else if (expr.Calls(CodeSymbols.Not, 1) && (a = expr.Args[0]) != null) return new Not(GetNodeFromLoycNode(a));
                else if (expr.Calls(CodeSymbols.NotBits, 1) && (a = expr.Args[0]) != null) return new BNot(GetNodeFromLoycNode(a));

                else if (expr.Calls(CodeSymbols.QuestionMark, 2) && (a = expr.Args[0]) != null && (tmp_12 = expr.Args[1]) != null && tmp_12.Calls(CodeSymbols.Colon, 2) && (b = tmp_12.Args[0]) != null && (c = tmp_12.Args[1]) != null)
                    return new If(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b), GetNodeFromLoycNode(c));
                else if (expr.Calls(CodeSymbols.QuestionMark, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null)
                    return new If(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b), null);
                else if (expr.Calls((Symbol)"if", 3) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null && (c = expr.Args[2]) != null)
                    return new If((Operations.Boolean)GetNodeFromLoycNode(a), GetNodeFromLoycNode(b), GetNodeFromLoycNode(c));
                else if (expr.Calls((Symbol)"if", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null)
                    return new If(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b), null);

                else if (expr.Calls((Symbol)"sqrt", 1) && (a = expr.Args[0]) != null) return new Sqrt(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"sin", 1) && (a = expr.Args[0]) != null) return new Sin(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"cos", 1) && (a = expr.Args[0]) != null) return new Cos(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"tan", 1) && (a = expr.Args[0]) != null) return new Tan(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"asin", 1) && (a = expr.Args[0]) != null) return new Asin(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"acos", 1) && (a = expr.Args[0]) != null) return new Acos(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"atan", 1) && (a = expr.Args[0]) != null) return new Atan(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"sec", 1) && (a = expr.Args[0]) != null) return new Sec(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"csc", 1) && (a = expr.Args[0]) != null) return new Csc(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"cot", 1) && (a = expr.Args[0]) != null) return new Cot(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"exp", 1) && (a = expr.Args[0]) != null) return new Exp(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"ln", 1) && (a = expr.Args[0]) != null) return new Ln(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"log", 1) && (a = expr.Args[0]) != null) return new Log(GetNodeFromLoycNode(a), new Constant(10));
                else if (expr.Calls((Symbol)"ceil", 1) && (a = expr.Args[0]) != null) return new Ceil(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"floor", 1) && (a = expr.Args[0]) != null) return new Floor(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"sign", 1) && (a = expr.Args[0]) != null) return new Sign(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"abs", 1) && (a = expr.Args[0]) != null) return new Abs(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"fact", 1) && (a = expr.Args[0]) != null) return new Factorial(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"'!", 1) && (a = expr.Args[0]) != null) return new Factorial(GetNodeFromLoycNode(a));
                else if (expr.Calls((Symbol)"random"))
                    return _getRandom(expr);
                else if (expr.Calls((Symbol)"sum", 4) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null && (c = expr.Args[2]) != null && (tmp_12 = expr.Args[3]) != null)
                {
                    if (a.IsId)
                    {
                        //Here temp vars will be added
                        return new Sum(a.Name, new[] { GetNodeFromLoycNode(b), GetNodeFromLoycNode(c), new Constant(1), GetNodeFromLoycNode(tmp_12) });
                    }
                }
                else if (expr.Calls((Symbol)"sum", 5) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null && (c = expr.Args[2]) != null && (tmp_12 = expr.Args[3]) != null && (tmp_11 = expr.Args[4]) != null)
                {
                    if (a.IsId)
                    {
                        //Here temp vars will be added
                        return new Sum(a.Name, new[] { GetNodeFromLoycNode(b), GetNodeFromLoycNode(c), GetNodeFromLoycNode(tmp_11), GetNodeFromLoycNode(tmp_12) });
                    }
                }
            }

            // to get functions
            if (expr.Kind == LNodeKind.Call && expr.Args.Count > 0)
            {
                if (node.Calls((Loyc.Symbol)"'."))
                {
                    return new Function(expr.Args[0].Name.ToString(), new[] { new Variable(expr.Args[1].Name.ToString()) });
                }
                else if(IsFuncId(node))
                {
                    List<Node> args = new List<Node>();
                    for (int i = 0; i < expr.Args.Count; i++)
                    {
                        args.Add(GetNodeFromLoycNode(expr.Args[i]));
                    }
                    return new Function(expr.Name.ToString(), args.ToArray());
                }
            }

            throw new ArgumentException("Expression not understood: {0}".Localized(expr));

        }
        static Operations.Random _getRandom(LNode expr)
        {
            Operations.Random.RType type = Operations.Random.RType.Int;
            bool TypeAssigned = false;
            if(expr.Args.Count >0)
                if (expr.Args.Last.IsId)
                {
                    if (expr.Args.Last.Name.Name == "double")
                    {
                        type = Operations.Random.RType.Double;
                        TypeAssigned = true;
                    }
                    else if (expr.Args.Last.Name.Name == "int")
                    {
                        type = Operations.Random.RType.Int;
                        TypeAssigned = true;
                    }
                }

            if (TypeAssigned)
            {
                expr.Args.RemoveAt(expr.Args.Count - 1);
            }
            //
            //return new Operations.Random(GetNodeFromLoycNode(a), GetNodeFromLoycNode(b));
            if (expr.Args.Count == 0)
            {
                return new Operations.Random(type);
            }
            if (expr.Args.Count == 1)
            {
                return new Operations.Random(GetNodeFromLoycNode(expr.Args[0]), type);
            }
            if (expr.Args.Count == 2)
            {
                return new Operations.Random(GetNodeFromLoycNode(expr.Args[0]), GetNodeFromLoycNode(expr.Args[1]), type);
            }
            return null;
        }
        static bool IsFuncId(LNode node)
        {
            if (node.Style == (NodeStyle.PrefixNotation | NodeStyle.OneLiner) || node.Style == NodeStyle.PrefixNotation)
            {
                return true;
            }
            return false;
        }

        public static bool ExprIsCondition(LNode expr)
        {
            LNode a, b;
            if (expr.Calls(CodeSymbols.And, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null || expr.Calls((Symbol)"'and", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null)
            {
                if (ExprIsCondition(a) && ExprIsCondition(b)) { return true; }
            }
            else if (expr.Calls(CodeSymbols.Or, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null || expr.Calls((Symbol)"'or", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null)
            {
                if (ExprIsCondition(a) && ExprIsCondition(b)) { return true; }
            }
            else if (expr.Calls(CodeSymbols.GT, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null)
            {
                return true;
            }
            else if (expr.Calls(CodeSymbols.LT, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null)
            {
                return true;
            }
            else if (expr.Calls(CodeSymbols.GE, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null)
            {
                return true;
            }
            else if (expr.Calls(CodeSymbols.LE, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null)
            {
                return true;
            }
            else if (expr.Calls(CodeSymbols.Eq, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null)
            {
                return true;
            }
            else if (expr.Calls(CodeSymbols.Neq, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null)
            {
                return true;
            }
            return false;
        }

        public static Node GetNodeFromString(string str)
        {
            LNode str_ = GetLoycNodeFromString(str);
            return GetNodeFromLoycNode(str_);
        }

        public static LNode GetLoycNodeFromNode(Node node)
        {
            return GetLoycNodeFromString(node.ToString());
        }

        #endregion

    }
}
