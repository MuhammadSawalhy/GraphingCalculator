using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class Neg : Node
    {
        public override int ArgumentNumber => 1;
        public override SyntaxType Syntax_Type => SyntaxType.Function;
        public override string Type => "Neg";

        public Neg(Node children) : base(children){}
        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            return -(Children[0].Calculate(CalculationSetting, tempVars));
        }
        public override string ToString()
        {
            string child = "";
            if (Children[0].Syntax_Type == Node.SyntaxType.Literal || Children[0].Syntax_Type == Node.SyntaxType.Function)
                child = Children[0].ToString();
            else
                child = "(" + Children[0].ToString() + ")";
            return $"-{child}";
        }

    }
}
