using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class BXor : Binary
    {

        public override string Type => "Or";
        public BXor(Node children1, Node children2) : base(children1, children2) { }

        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            return ((long)Children[0].Calculate(CalculationSetting, tempVars) ^ (long)Children[1].Calculate(CalculationSetting, tempVars));
        }
        public override string ToString()
        {
            return null;
        }

    }
}
