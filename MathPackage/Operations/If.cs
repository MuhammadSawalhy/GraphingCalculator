using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class If : Node
    {
        public override int ArgumentNumber => 3;
        public override SyntaxType Syntax_Type => SyntaxType.Function;
        public override string Type => "If";

        public Node Condition;
        public Node ifTrue, ifFalse;

        public If(Node condition, Node iftrue, Node iffalse)
        {
            ifTrue = iftrue;
            ifFalse = iffalse;
            Condition = condition;
        }

        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {

            if (Condition.Calculate(CalculationSetting, tempVars) == 1)
            {
                return ifTrue.Calculate(CalculationSetting, tempVars);
            }
            else if (ifFalse != null)
            {
                return ifFalse.Calculate(CalculationSetting, tempVars);
            }
            return double.NaN;
        }

        public override string ToString()
        {
            if(ifFalse != null)
                return $"if({Condition}, {ifTrue}, {ifFalse})";
            else
                return $"if({Condition}, {ifTrue})";
        }

    }
}
