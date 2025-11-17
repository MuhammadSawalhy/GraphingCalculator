using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{ // This class return a value between two numbers if the parameter is not between the two numbers -boundaries- it will return the nearset value of these two.
    // E.g.     2.5 Clam [2,3] => 2.5     -0.5 Clam [2,3] => 2
    public sealed class Clamp : Node
    {
        public override int ArgumentNumber => 3;
        public override SyntaxType Syntax_Type => SyntaxType.Function;
        public override string Type => "Clamp";

        public Clamp(Node[] children) : base(children) {}

        private bool BetWeen(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            if (Children[0].Calculate(CalculationSetting, tempVars) > Children[1].Calculate(CalculationSetting, tempVars) && Children[0].Calculate(CalculationSetting, tempVars) < Children[2].Calculate(CalculationSetting, tempVars))
                 return true;
            return false;
        }

        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            double a1 = Children[0].Calculate(CalculationSetting, tempVars), a2 = Children[1].Calculate(CalculationSetting, tempVars), a3 = Children[2].Calculate(CalculationSetting, tempVars);
            if (BetWeen(CalculationSetting, tempVars))
                return a1;
            else if (a1 <= a2)
                return a2;
            else 
                return a3;
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


            return child1 + " clamp " + child2;
        }

    }
}
