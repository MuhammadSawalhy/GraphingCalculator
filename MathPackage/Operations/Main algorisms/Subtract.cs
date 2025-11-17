using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class Subtract : Node
    {
        public override int ArgumentNumber => 2;
        public override SyntaxType Syntax_Type => SyntaxType.Operator;
        public override string Type => "Subtract";
        public Subtract(Node children1, Node children2) : base(children1, children2)
        {
        }
        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            return Children[0].Calculate(CalculationSetting, tempVars) - Children[1].Calculate(CalculationSetting, tempVars);
        }
        public override string ToString()
        {
            string child1 = "", child2 = "";

            child1 = Children[0].ToString();

            if (Children[1].Syntax_Type == Node.SyntaxType.Literal || Children[1].Syntax_Type == Node.SyntaxType.Function)
                child2 = Children[1].ToString();
            else
                child2 = "(" + Children[1].ToString() + ")";

            return child1 + " - " + child2;
        }
    }
}
