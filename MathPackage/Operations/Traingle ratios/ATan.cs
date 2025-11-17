using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class Atan : Node
    {
        public override int ArgumentNumber => 1;
        public override SyntaxType Syntax_Type => SyntaxType.Function;
        public override string Type => "Atan";
        public Atan(Node children) : base(children)
        {
        }
        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            switch (CalculationSetting.AngleType)
            {
                case Main.AngleType.Degree:
                    return Math.Atan(Children[0].Calculate(CalculationSetting, tempVars)) * 180 / Math.PI;
                case Main.AngleType.Grads:
                    return Math.Atan(Children[0].Calculate(CalculationSetting, tempVars)) * 200 / Math.PI;
                default:
                    return Math.Atan(Children[0].Calculate(CalculationSetting, tempVars));
            }
        }
        public override string ToString()
        {
            return Type.ToLower() + "(" + Children[0].ToString() + ")";
        }

    }
}
