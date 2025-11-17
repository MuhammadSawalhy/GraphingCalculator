using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class Not : Boolean
    {
        public override int ArgumentNumber => 1;

        public override string Type => "Not";
        public Not(Node children) : base(children) { }

        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            if (Children[0].Calculate(CalculationSetting, tempVars) == 0)
                return 1;
            return 0;
        }
        public override string ToString()
        {
            string child = "";
            if (Children[0].Syntax_Type == Node.SyntaxType.Literal || Children[0].Syntax_Type == Node.SyntaxType.Function)
                child = Children[0].ToString();
            else
                child = "(" + Children[0].ToString() + ")";
            return $"!{child}";
        }

    }
}
