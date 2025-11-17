using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public class GreaterEqual : Boolean
    {

        public override string Type => "GreaterEqual";
        public GreaterEqual(Node children1, Node children2) : base(children1, children2) { }

        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            if (Children[0].Calculate(CalculationSetting, tempVars) >= Children[1].Calculate(CalculationSetting, tempVars))
                return 1;
            return 0;
        }

        public override string ToString()
        {
            string child1 = "", child2 = "";

            if (Children[0].Syntax_Type == Node.SyntaxType.Literal || Children[0].Syntax_Type == Node.SyntaxType.Function)
                child1 = Children[0].ToString();
            else
                child1 = "(" + Children[0].ToString() + ")";

            if (Children[1].Syntax_Type == Node.SyntaxType.Literal || Children[1].Syntax_Type == Node.SyntaxType.Function)
                child2 = Children[1].ToString();
            else
                child2 = "(" + Children[1].ToString() + ")";


            return child1 + " >= " + child2;
        }
    }
}
