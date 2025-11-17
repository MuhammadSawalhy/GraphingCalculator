using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class ShiftRight : Binary
    {

        public override string Type => "ShiftRight";
        public ShiftRight(Node children1, Node children2) : base(children1, children2) { }


        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            return Loyc.G.ShiftRight(Children[0].Calculate(CalculationSetting, tempVars), (int)Children[1].Calculate(CalculationSetting, tempVars));
        }
        public override string ToString()
        {
            return null;
        }

    }
}
