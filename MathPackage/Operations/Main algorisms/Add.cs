using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class Add : Node
    {
        public override int ArgumentNumber => 2;
        public override SyntaxType Syntax_Type => SyntaxType.Operator;

        public override string Type => "Add";
        public Add(Node children1, Node children2) : base(children1, children2)
        {
        }
        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            return Children[0].Calculate(CalculationSetting, tempVars) + Children[1].Calculate(CalculationSetting, tempVars);
        }
        public override string ToString()
        {
            string child1 = "", child2 = "";
            child1 = Children[0].ToString();
            child2 = Children[1].ToString();
            return child1 + " + " + child2;
        }
    }
}
