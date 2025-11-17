using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class C : Node
    {
        public override int ArgumentNumber => 2;
        public override SyntaxType Syntax_Type => SyntaxType.Operator;
        public override string Type => "C";
        public C(Node children1, Node children2) : base(children1, children2)
        {
        }
        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            return C_((ulong)Children[0].Calculate(CalculationSetting, tempVars), (ulong)Children[1].Calculate(CalculationSetting, tempVars));
        }
        static double C_(ulong n, ulong k)
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
        public override string ToString()
        {
            string child1 = "", child2 = "";

            if (Children[0].Syntax_Type == Node.SyntaxType.Literal || Children[0].Syntax_Type == Node.SyntaxType.Function)
                child1 = Children[0].ToString();
            else
                child1 = "(" + Children[0].ToString() + ")";

            if (Children[1].Syntax_Type == Node.SyntaxType.Literal || Children[1].Syntax_Type == Node.SyntaxType.Function)
                child2 = Children[1].ToString();
            else
                child2 = "(" + Children[1].ToString() + ")";


            return child1 + " C " + child2;
        }

    }
}
