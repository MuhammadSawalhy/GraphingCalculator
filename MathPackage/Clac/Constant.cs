using System;
using System.Collections.Generic;

namespace MathPackage.Operations
{

    public sealed class Constant : Node
    {

        public override int ArgumentNumber => 0;
        public override SyntaxType Syntax_Type => SyntaxType.Literal;
        public override string Type => "Constant";

        public double  Value { get; set; }

        public string Name { get; set; }

        public Constant(double value, string name_ = null)
        {
            Name = name_;
            Value = value;
        }

        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            return Value;
        }
        

        public override string ToString()
        { 
           return Value.ToString();
        }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Value.Equals(((Constant)obj).Value);
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + ArgumentNumber.GetHashCode();
            hash = hash * 23 + Children.GetHashCode();
            hash = hash * 23 + Value.GetHashCode();
            return hash;
        }

    }
}
