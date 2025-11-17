using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class Floor : Node
    {
        public override int ArgumentNumber => 1;
        public override SyntaxType Syntax_Type => SyntaxType.Function;
        public override string Type => "Floor";

        public Floor(Node children) : base(children)
        {
        }
        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            return Math.Floor(Children[0].Calculate(CalculationSetting, tempVars));
        }
        public override string ToString()
        {
            return Type.ToLower() + "(" + Children[0].ToString() + ")";
        }

    }
}
