using System;
using System.Collections.Generic;
using System.Text;

namespace MathPackage.Operations
{
    public sealed class Log : Node
    {
        public override int ArgumentNumber => 2;

        public override SyntaxType Syntax_Type => SyntaxType.Function;
        public override string Type => "Log";
        public Log(Node children, Node _base = null) : base(children, _base)
        {
            if (_base == null)
            {
                Children[1] = new Constant(10);
            }
        }
        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            return Math.Log(Children[0].Calculate(CalculationSetting, tempVars), Children[1].Calculate(CalculationSetting, tempVars));
        }
        public override string ToString()
        {
            if(Children[1] is Constant && ((Constant)Children[1]).Value == 10)
            {
                return Type.ToLower() + "(" + Children[0].ToString() + ")";
            }
            else if (Children[1] is Constant && ((Constant)Children[1]).Value == Math.E)
            {
                return "ln(" + Children[0].ToString() + ", " + Children[1].ToString() + ")";
            }
            else
                 return Type.ToLower() + "(" + Children[0].ToString() + ", " + Children[1].ToString() + ")";
        }


    }
}
