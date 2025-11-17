using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class In : Boolean
    {
        public override int ArgumentNumber => 3;

        public override string Type => "Equal";

        public In(Node[] children) : base(children) {}

        private bool BetWeen(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            if (Children[0].Calculate(CalculationSetting, tempVars) > Children[1].Calculate(CalculationSetting, tempVars) && Children[0].Calculate(CalculationSetting, tempVars) < Children[2].Calculate(CalculationSetting, tempVars))
                 return true;
            return false;
        }

        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            if (BetWeen(CalculationSetting, tempVars) || (Children[0].Calculate(CalculationSetting, tempVars) == Children[1].Calculate(CalculationSetting, tempVars)) || (Children[0].Calculate(CalculationSetting, tempVars) == Children[2].Calculate(CalculationSetting, tempVars))) 
                return 1;
            return 0;
        }
        public override string ToString()
        {
            return null;
        }

    }
}
