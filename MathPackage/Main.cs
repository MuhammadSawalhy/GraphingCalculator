using System;
using MathPackage.Operations;
using Microsoft.VisualBasic;
using number = System.Double;	// Change this line to make a calculator for a different data type 
using Loyc.Syntax;
using Loyc.Syntax.Les;
using Loyc;
using System.Collections.Generic;


namespace MathPackage
{

    public static class Main
    {


        public static Language Lang
        {
            get;
            set;
        }

        public enum Language
        {
            AR = 0,
            EN = 1
        }


        public enum AngleType
        {
            Degree = 0,
            Radian = 1,
            Grads = 2
        }

        public static bool IsNumeric(string text)
        {

            if (text.Contains("."))
            {
                if (text.Replace(".", "").Length + 1 != text.Length)
                    return false;
            }

            text = text.Replace(".", "");
            if (text.Substring(0, 1) == "-")
            {
                text = text.Substring(1, text.Length - 1);
            }

            for (int i = 0; i < text.Length; i++)
            {
                int c = 0;
                if (!int.TryParse(text[i].ToString(), out c))
                    return false;
            }

            return true;
        }

        public static double CalculateString(string str, CalculationSetting calculationSetting)
        {
            Node n = Transformer.GetNodeFromString(str);
            return n.Calculate(calculationSetting, new Dictionary<Symbol, Node>());
        }
        public static double CalculateLNode(LNode node, CalculationSetting calculationSetting)
        {
            Node n = Transformer.GetNodeFromLoycNode(node);

            return n.Calculate(calculationSetting, new Dictionary<Symbol, Node>());
        }

        ///<param name="decimal_len">if you what to get three decimal digits you will input "###" and so on.</param>
        public static double Approximate(string value, string up = "", string down = "", string decimal_len = "")
        {
            if (IsNumeric(value))
            {
                if (value != "" && value.Contains("."))
                {
                    string[] str_ = new string[2];
                    str_[0] = value.Split('.')[0];
                    str_[1] = ClipStr(value.Split('.')[1], decimal_len.Length);

                    if (value.Contains("." + up) && up != "")
                        return Math.Round(double.Parse(value.Replace("." + up, ".9")));
                    else if (value.Contains("." + down) && down != "")
                        return Math.Round(double.Parse(value.Replace("." + down, ".0")));
                    else
                        return double.Parse(str_[0] + '.' + str_[1]);

                }
                else
                    return double.Parse(value);
            }
            else
                return double.NaN;
        }
        ///<param name="decimal_len">if you what to get three decimal digits you will input "###" and so on.</param>
        public static double Approximate(double value, string up = "", string down = "", string decimal_len = "")
        {
            if (value.ToString().Contains("."))
            {
                string[] str_ = new string[2];
                str_[0] = value.ToString().Split('.')[0];
                str_[1] = ClipStr(value.ToString().Split('.')[1], decimal_len.Length);
                if (value.ToString().Contains("." + up) && up != "")
                    return Math.Round(double.Parse(value.ToString().Replace("." + up, ".9")));
                else if (value.ToString().Contains("." + down) && down != "")
                    return Math.Round(double.Parse(value.ToString().Replace("." + down, ".0")));
                else
                    return double.Parse(str_[0] + '.' + str_[1]);
            }
            return value;
        }

        internal static string ClipStr(string str, int len, int start = 0)
        {
            if (str.Length >= len + start)
            {
                return str.Substring(start, len);
            }
            else
            {
                return str.Substring(start, str.Length - start);
            }
        }

        ///<param name="decimal_len">if you what to get three decimal digits you will input "###" and so on.</param>
        public static double Approximate(double value, string decimal_len)
        {
            if (!double.IsInfinity(value) && !double.IsNaN(value) && value.ToString().Contains("."))
            {
                string str_ = value.ToString();
                string str = "";
                if (str_.Contains("E"))
                {
                    str_ = Get_x10Values(value);
                    str = str_.Substring(0, str_.IndexOf('.')) + "." + ClipStr(str_, decimal_len.Length, str_.IndexOf('.') + 1);
                }
                else
                    str = str_.Substring(0, str_.IndexOf('.')) + "." + ClipStr(str_, decimal_len.Length, str_.IndexOf('.')+1);
                if (str != "")
                    return double.Parse(str);
                else
                    return 0;
            }
            else
                return value;
        }
        static string Get_x10Values(double value)
        {
            string[] str = value.ToString().Split('E');
            int exponent = int.Parse(str[1]);
            if (exponent < 0)
            {

                string[] str_ = str[0].Split('.');
                do
                {
                    str_[1] = str_[1].Insert(0, str_[0].Substring(str_[0].Length - 1));
                    str_[0] = str_[0].Remove(str_[0].Length - 1);
                    exponent += 1;
                } while ((str_[0].Length > 0 && str_[0] != "-") && exponent < 0);
                if(exponent < 0)
                {
                    string zeros = "";
                    for (int i = 0; i < -exponent; i++)
                    {
                        zeros += "0";
                    }
                    return $"{str_[0]}.{zeros}{str_[1]}";
                }
                else
                {
                    return $"{str_[0]}.{str_[1]}";
                }

            }
            else
            {

            }
            return "";
        }
        public static number Eval(LNode expr, Dictionary<Symbol, LNode> Vars)
        {
            Func<Symbol, number> lookup = null;
            lookup = name => CalculateLNode(Vars[name], lookup);
            return CalculateLNode(expr, lookup);
        }

        // Evaluates an expression
        public static number CalculateLNode(LNode expr, Func<Symbol, number> lookup)
        {
            if (expr.IsLiteral)
            {
                if (expr.Value is number)
                    return (number)expr.Value;
                else
                    return (number)Convert.ToDouble(expr.Value);
            }
            if (expr.IsId)
                return lookup(expr.Name);
            // expr must be a function or operator
            if (expr.ArgCount == 2)
            {
                {
                    LNode a, b, hi, lo, tmp_10, tmp_11 = null;
                    if (expr.Calls(CodeSymbols.Add, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return CalculateLNode(a, lookup) + CalculateLNode(b, lookup);
                    else if (expr.Calls((Symbol)"log", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return Math.Log(CalculateLNode(a, lookup), CalculateLNode(b, lookup));
                    else if (expr.Calls(CodeSymbols.Mul, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return CalculateLNode(a, lookup) * CalculateLNode(b, lookup);
                    else if (expr.Calls(CodeSymbols.Sub, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return CalculateLNode(a, lookup) - CalculateLNode(b, lookup);
                    else if (expr.Calls(CodeSymbols.Div, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return CalculateLNode(a, lookup) / CalculateLNode(b, lookup);
                    else if (expr.Calls(CodeSymbols.Mod, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return CalculateLNode(a, lookup) % CalculateLNode(b, lookup);
                    else if (expr.Calls(CodeSymbols.Exp, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return (number)Math.Pow(CalculateLNode(a, lookup), CalculateLNode(b, lookup));
                    else if (expr.Calls(CodeSymbols.Shr, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return (number)G.ShiftRight(CalculateLNode(a, lookup), (int)CalculateLNode(b, lookup));
                    else if (expr.Calls(CodeSymbols.Shl, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return (number)G.ShiftLeft(CalculateLNode(a, lookup), (int)CalculateLNode(b, lookup));

                    else if (expr.Calls(CodeSymbols.GT, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return CalculateLNode(a, lookup) > CalculateLNode(b, lookup) ? (number)1 : (number)0;
                    else if (expr.Calls(CodeSymbols.LT, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return CalculateLNode(a, lookup) < CalculateLNode(b, lookup) ? (number)1 : (number)0;
                    else if (expr.Calls(CodeSymbols.GE, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return CalculateLNode(a, lookup) >= CalculateLNode(b, lookup) ? (number)1 : (number)0;
                    else if (expr.Calls(CodeSymbols.LE, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return CalculateLNode(a, lookup) <= CalculateLNode(b, lookup) ? (number)1 : (number)0;
                    else if (expr.Calls(CodeSymbols.Eq, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return CalculateLNode(a, lookup) == CalculateLNode(b, lookup) ? (number)1 : (number)0;
                    else if (expr.Calls(CodeSymbols.Neq, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return CalculateLNode(a, lookup) != CalculateLNode(b, lookup) ? (number)1 : (number)0;
                    else if (expr.Calls(CodeSymbols.AndBits, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return (number)((long)CalculateLNode(a, lookup) & (long)CalculateLNode(b, lookup));
                    else if (expr.Calls(CodeSymbols.OrBits, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return (number)((long)CalculateLNode(a, lookup) | (long)CalculateLNode(b, lookup));
                    else if (expr.Calls(CodeSymbols.NullCoalesce, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null)
                    {
                        var a2 = CalculateLNode(a, lookup); return double.IsNaN(a2) | double.IsInfinity(a2) ? CalculateLNode(b, lookup) : a2;
                    }
                    else if (expr.Calls(CodeSymbols.And, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null || expr.Calls((Symbol)"'and", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return CalculateLNode(a, lookup) != (number)0 ? CalculateLNode(b, lookup) : (number)0;
                    else if (expr.Calls(CodeSymbols.Or, 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null || expr.Calls((Symbol)"'or", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return CalculateLNode(a, lookup) == (number)0 ? CalculateLNode(b, lookup) : (number)1;
                    else if (expr.Calls((Symbol)"'xor", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return (CalculateLNode(a, lookup) != 0) != (CalculateLNode(b, lookup) != 0) ? (number)1 : (number)0;
                    else if (expr.Calls((Symbol)"xor", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return (number)((long)CalculateLNode(a, lookup) ^ (long)CalculateLNode(b, lookup));
                    else if (expr.Calls((Symbol)"min", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return Math.Min(CalculateLNode(a, lookup), CalculateLNode(b, lookup));
                    else if (expr.Calls((Symbol)"max", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return Math.Max(CalculateLNode(a, lookup), CalculateLNode(b, lookup));
                    else if (expr.Calls((Symbol)"mod", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null || expr.Calls((Symbol)"'MOD", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return Mod(CalculateLNode(a, lookup), CalculateLNode(b, lookup));
                    else if (expr.Calls((Symbol)"atan", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return Math.Atan2(CalculateLNode(a, lookup), CalculateLNode(b, lookup));
                    else if (expr.Calls((Symbol)"log", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return Math.Log(CalculateLNode(a, lookup), CalculateLNode(b, lookup));
                    else if (expr.Calls((Symbol)"'in", 2) && (a = expr.Args[0]) != null && (tmp_10 = expr.Args[1]) != null && tmp_10.Calls(CodeSymbols.Tuple, 2) && (lo = tmp_10.Args[0]) != null && (hi = tmp_10.Args[1]) != null) return G.IsInRange(CalculateLNode(a, lookup), CalculateLNode(lo, lookup), CalculateLNode(hi, lookup)) ? (number)1 : (number)0;
                    else if (expr.Calls((Symbol)"'clamp", 2) && (a = expr.Args[0]) != null && (tmp_11 = expr.Args[1]) != null && tmp_11.Calls(CodeSymbols.Tuple, 2) && (lo = tmp_11.Args[0]) != null && (hi = tmp_11.Args[1]) != null || expr.Calls((Symbol)"clamp", 3) && (a = expr.Args[0]) != null && (lo = expr.Args[1]) != null && (hi = expr.Args[2]) != null) return G.PutInRange(CalculateLNode(a, lookup), CalculateLNode(lo, lookup), CalculateLNode(hi, lookup));
                    else if (expr.Calls((Symbol)"'P", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null || expr.Calls((Symbol)"P", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return P((int)Math.Round(CalculateLNode(a, lookup)), (int)Math.Round(CalculateLNode(b, lookup)));
                    else if (expr.Calls((Symbol)"'C", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null || expr.Calls((Symbol)"C", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return C((ulong)Math.Round(CalculateLNode(a, lookup)), (ulong)Math.Round(CalculateLNode(b, lookup)));

                }
            }
            {
                LNode a, b, c, tmp_12;
                if (expr.Calls(CodeSymbols.Sub, 1) && (a = expr.Args[0]) != null) return -CalculateLNode(a, lookup);
                else if (expr.Calls(CodeSymbols.Add, 1) && (a = expr.Args[0]) != null) return Math.Abs(CalculateLNode(a, lookup));
                else if (expr.Calls(CodeSymbols.Not, 1) && (a = expr.Args[0]) != null) return CalculateLNode(a, lookup) == 0 ? (number)1 : (number)0;
                else if (expr.Calls(CodeSymbols.NotBits, 1) && (a = expr.Args[0]) != null) return (number)~(long)CalculateLNode(a, lookup);
                else if (expr.Calls(CodeSymbols.QuestionMark, 2) && (c = expr.Args[0]) != null && (tmp_12 = expr.Args[1]) != null && tmp_12.Calls(CodeSymbols.Colon, 2) && (a = tmp_12.Args[0]) != null && (b = tmp_12.Args[1]) != null)
                    return CalculateLNode(c, lookup) != (number)0 ? CalculateLNode(a, lookup) : CalculateLNode(b, lookup);
                else if (expr.Calls((Symbol)"square", 1) && (a = expr.Args[0]) != null)
                {
                    var n = CalculateLNode(a, lookup); return n * n;
                }
                else if (expr.Calls((Symbol)"sqrt", 1) && (a = expr.Args[0]) != null) return Math.Sqrt(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"sin", 1) && (a = expr.Args[0]) != null) return Math.Sin(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"cos", 1) && (a = expr.Args[0]) != null) return Math.Cos(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"tan", 1) && (a = expr.Args[0]) != null) return Math.Tan(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"asin", 1) && (a = expr.Args[0]) != null) return Math.Asin(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"acos", 1) && (a = expr.Args[0]) != null) return Math.Acos(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"atan", 1) && (a = expr.Args[0]) != null) return Math.Atan(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"sec", 1) && (a = expr.Args[0]) != null) return 1 / Math.Cos(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"csc", 1) && (a = expr.Args[0]) != null) return 1 / Math.Sin(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"cot", 1) && (a = expr.Args[0]) != null) return 1 / Math.Tan(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"exp", 1) && (a = expr.Args[0]) != null) return Math.Exp(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"ln", 1) && (a = expr.Args[0]) != null) return Math.Log(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"log", 1) && (a = expr.Args[0]) != null) return Math.Log10(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"ceil", 1) && (a = expr.Args[0]) != null) return Math.Ceiling(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"floor", 1) && (a = expr.Args[0]) != null) return Math.Floor(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"sign", 1) && (a = expr.Args[0]) != null) return Math.Sign(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"abs", 1) && (a = expr.Args[0]) != null) return Math.Abs(CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"rnd", 0)) return (number)_r.NextDouble();
                else if (expr.Calls((Symbol)"rnd", 1) && (a = expr.Args[0]) != null) return (number)_r.Next((int)CalculateLNode(a, lookup));
                else if (expr.Calls((Symbol)"rnd", 2) && (a = expr.Args[0]) != null && (b = expr.Args[1]) != null) return (number)_r.Next((int)CalculateLNode(a, lookup), (int)CalculateLNode(b, lookup));
                else if (expr.Calls((Symbol)"fact", 1) && (a = expr.Args[0]) != null) return Factorial(CalculateLNode(a, lookup));
            }
            throw new ArgumentException("Expression not understood: {0}".Localized(expr));
        }

        static double Mod(double x, double y)
        {
            double m = x % y;
            return m + (m < 0 ? y : 0);
        }
        static System.Random _r = new System.Random();
        static double Factorial(double n) =>
        n <= 1 ? 1 : n * Factorial(n - 1);
        static double C(ulong n, ulong k)
        {
            if (k > n)
                return 0;
            k = Math.Min(k, n - k);
            double result = 1;
            for (ulong d = 1; d <= k; ++d)
            {
                result *= n--;
                result /= d;
            }
            return result;
        }
        static double P(int n, int k) =>
        k <= 0 ? 1 : k > n ? 0 : n * P(n - 1, k - 1);

    }

}
