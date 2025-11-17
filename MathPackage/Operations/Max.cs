using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class Max : Node
    {
        public override int ArgumentNumber => 2;
        public override SyntaxType Syntax_Type => SyntaxType.Function;
        public override string Type => "Max";

        public Max(Node children1, Node children2) : base(children1, children2){}
        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            return Math.Max(Children[0].Calculate(CalculationSetting, tempVars), Children[1].Calculate(CalculationSetting, tempVars));
        }
        public override string ToString()
        {
            return Type.ToLower() + "(" + Children[0].ToString() + ", " + Children[1].ToString() + ")";
        }

    }
}
