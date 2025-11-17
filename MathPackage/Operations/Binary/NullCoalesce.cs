using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class NullCoalesce : Binary
    {

        public override string Type => "NullCoalesce";

        public NullCoalesce(Node children1, Node children2) : base(children1, children2) { }

        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            var a2 = Children[0].Calculate(CalculationSetting, tempVars);
            return double.IsNaN(a2) || double.IsInfinity(a2) ? 
                Children[1].Calculate(CalculationSetting, tempVars) :
                a2;
        }
        public override string ToString()
        {
            return null;
        }

    }
}
