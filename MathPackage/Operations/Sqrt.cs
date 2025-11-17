using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class Sqrt : Node
    {
        public override int ArgumentNumber => 1;
        public override SyntaxType Syntax_Type => SyntaxType.Function;
        public override string Type => "Sqrt";

        public Sqrt(Node children) : base(children)
        {
        }
        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            return Math.Sqrt(Children[0].Calculate(CalculationSetting, tempVars));
        }

        public override string ToString()
        {
            return Type.ToLower() + "(" + Children[0].ToString() + ")";
        }

    }
}
