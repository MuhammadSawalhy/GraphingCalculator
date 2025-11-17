using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class Atan2 : Node
    {
        public override int ArgumentNumber => 1;
        public override SyntaxType Syntax_Type => SyntaxType.Function;
        public override string Type => "Atan";
        public Atan2(Node children1, Node children2) : base(children1, children2){}
        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            switch (CalculationSetting.AngleType)
            {
                case Main.AngleType.Degree:
                    return Math.Atan2(Children[0].Calculate(CalculationSetting, tempVars), Children[1].Calculate(CalculationSetting, tempVars)) * 180 / Math.PI;
                case Main.AngleType.Grads:
                    return Math.Atan2(Children[0].Calculate(CalculationSetting, tempVars), Children[1].Calculate(CalculationSetting, tempVars)) * 200 / Math.PI;
                default:
                    return Math.Atan2(Children[0].Calculate(CalculationSetting, tempVars), Children[1].Calculate(CalculationSetting, tempVars));
            }
        }
        public override string ToString()
        {
            return Type.ToLower() + "(" + Children[0].ToString() + ", " + Children[1].ToString() + ")";
        }

    }
}
