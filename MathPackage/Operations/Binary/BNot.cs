using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class BNot : Binary
    {
        public override int ArgumentNumber => 1;
        public override string Type => "BAnd";
        public BNot(Node children) : base(children) { }

        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            return ~((long)Children[0].Calculate(CalculationSetting, tempVars));
        }
        public override string ToString()
        {
            return null;
        }

    }
}
