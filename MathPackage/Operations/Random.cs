using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class Random : Node
    {
        public override int ArgumentNumber => 1;
        public override SyntaxType Syntax_Type => SyntaxType.Function;
        public override string Type => "Random";
        /// <summary>
        ///  0 is UnLimited, 1 is EndAssigned, 2 Limited from the beginning to the end.
        /// </summary>
        double Limits = 0;

        public RType myType = RType.Int;

        public enum RType
        {
            Int,
            Double,
        }

        public Random(RType type = RType.Int) : base() { myType = type; Limits = 0; }
        public Random(Node children1, Node children2, RType type = RType.Int) : base(children1, children2) { myType = type; Limits = 2; }
        public Random(Node children1, RType type = RType.Int) : base(children1) { myType = type; Limits = 1; }

        private System.Random r = new System.Random();
        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            switch(myType)
            {
                case RType.Double:
                    if (Limits == 0)
                    {
                        int ran = r.Next();
                        return (r.Next() % 2 == 0) ? ran + r.NextDouble() : -ran + r.NextDouble();
                    }
                    else if(Limits == 1)
                    {
                        int end= (int)Children[0].Calculate(CalculationSetting, tempVars)-1;
                        int ran = r.Next(end);
                        return (r.Next() % 2 == 0) ? ran + r.NextDouble() : -ran + r.NextDouble();
                    }
                    else if(Limits == 2)
                    {
                        int start = (int)Children[0].Calculate(CalculationSetting, tempVars), 
                            end = (int)Children[1].Calculate(CalculationSetting, tempVars) - 1;
                        if (end > start)
                            return r.Next(start, end) + r.NextDouble();
                        else if(Children[1].Calculate(CalculationSetting, tempVars) < Children[0].Calculate(CalculationSetting, tempVars))
                            return double.NaN;
                        else if (start == end)
                        {
                            ///Getting a random Divisor.
                            double ranDivisor = r.Next(end) * r.Next(end);
                            return Children[0].Calculate(CalculationSetting, tempVars) + (Children[1].Calculate(CalculationSetting, tempVars) - Children[0].Calculate(CalculationSetting, tempVars)) / ranDivisor ;
                        }
                    }
                    break;
                case RType.Int:
                    if (Limits == 2)
                    {
                        int start = (int)Children[0].Calculate(CalculationSetting, tempVars),
                            end = (int)Children[1].Calculate(CalculationSetting, tempVars) - 1;
                        if (start > end)
                            return double.NaN;
                        int ran = r.Next();
                        return r.Next() % 2 == 0 ? ran : -ran;
                    }
                    else if (Limits == 1)
                    {
                        int end = (int)Children[0].Calculate(CalculationSetting, tempVars);
                        int ran = r.Next(end);
                        return r.Next() % 2 == 0 ? ran : -ran;
                    }
                    else if (Limits == 0)
                    {
                        int ran = r.Next();
                        return r.Next() % 2 == 0 ? ran : -ran;
                    }
                    break;
            }
            return double.NaN;
        }

        public override string ToString()
        {
            if (Limits == 0)
            {
                string type = myType == RType.Int ? "" : myType.ToString();
                return $"random({type})";
            }
            else if (Limits == 1)
            {
                string type = myType == RType.Int ? "" : ", " + myType.ToString();
                return $"random({Children[0]}{type})";
            }
            else
            {
                string type = myType == RType.Int ? "" : ", " + myType.ToString();
                return Type.ToLower() + "(" + Children[0].ToString() + ", " + Children[1].ToString() + $"{type})";
            }
        }


    }
}
