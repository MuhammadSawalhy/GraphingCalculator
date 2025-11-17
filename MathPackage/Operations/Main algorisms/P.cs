using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class P : Node
    {
        public override int ArgumentNumber => 2;
        public override SyntaxType Syntax_Type => SyntaxType.Operator;
        public override string Type => "P";
        public P(Node children1, Node children2) : base(children1, children2){}
        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            return P_((int)Children[0].Calculate(CalculationSetting, tempVars), (int)Children[1].Calculate(CalculationSetting, tempVars));
        }
        static double P_(int n, int k) =>
        k <= 0 ? 1 : k > n ? 0 : n * P_(n - 1, k - 1);
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


            return child1 + " P " + child2;
        }
    }
}
