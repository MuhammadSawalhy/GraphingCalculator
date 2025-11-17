using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class Factorial : Node
    {

        public override int ArgumentNumber => 1;
        public override SyntaxType Syntax_Type => SyntaxType.Literal;
        public override string Type => "Fact";

        public Factorial(Node children) : base(children){}

        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {

            return F(Children[0].Calculate(CalculationSetting, tempVars));

        }

        static double F(double n) =>
        n <= 1 ? 1 : n * F(n - 1);
        public override string ToString()
        {
            return null;
        }

    }
}
