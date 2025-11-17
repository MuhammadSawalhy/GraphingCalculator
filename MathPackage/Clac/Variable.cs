using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace MathPackage.Operations
{
    public sealed class Variable : Node

    {

        public override int ArgumentNumber => 0;
        public override SyntaxType Syntax_Type => SyntaxType.Literal;
        public override string Type => "Variable" ;

        public Loyc.Symbol Name { get; }

        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            /// Searching for temporary variables, like the variables that passed from a segma notation or passed into a function.
            for (int i = 0; i < tempVars.Count; i++)
            {
                if(tempVars.ElementAt(i).Key == Name)
                    return tempVars.ElementAt(i).Value.Calculate(CalculationSetting, tempVars);
            }
            /// Searching for existing varaibles in the <see cref="MathPackage.CalculationSetting">
            for (int i= 0; i < CalculationSetting.Vars.Count; i++)
            { if (CalculationSetting.Vars[i].Name == Name)
                    return CalculationSetting.Vars[i].Value.Calculate(CalculationSetting);
            }
            throw new Exception($"\"{Name}\" doesn't exist. :(");
        }

        public override string ToString()
        {
            return Name.Name;
        }

        public Variable(string name, double value = 0) 
        {
            Name = (Loyc.Symbol)name;
        }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Name.Equals(((Variable)obj).Name);
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + ArgumentNumber.GetHashCode();
            hash = hash * 23 + Children.GetHashCode();
            hash = hash * 23 + Name.GetHashCode();
            return hash;
        }

    }


}
