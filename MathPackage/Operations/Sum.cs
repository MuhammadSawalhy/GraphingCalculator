using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathPackage.Operations
{
    public class Sum : Node
    {
        public override int ArgumentNumber => 0;
        public override SyntaxType Syntax_Type => SyntaxType.Function;
        public override string Type => "Sum";

        public Loyc.Symbol Parameter;
        /// <summary>
        /// Your Children contains 
        /// Node Start      index 0;
        /// Node End        index 1;
        /// Node Process    index 2;
        /// Node Step       index 3;
        /// </summary>
        /// <param name="param"></param>
        public Sum(Loyc.Symbol param, Node[] children)
        {
            Parameter = param;
            Children = children;
        }

        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            double sum = 0,
                start = Children[0].Calculate(CalculationSetting, tempVars), 
                end = Children[1].Calculate(CalculationSetting, tempVars),
                step = Children[2].Calculate(CalculationSetting, tempVars),
                valueToAdd;

            #region Preparing tempVars

            Dictionary<Loyc.Symbol, Node> tempvars = new Dictionary<Loyc.Symbol, Node>();

            for (int i = 0; i < tempVars.Count; i++)
            {
                tempvars.Add(tempVars.ElementAt(i).Key, tempVars.ElementAt(i).Value);
            }

            tempvars.Add(Parameter, null);

            #endregion

            for (double i = start; i <= end; i += step)
            {
                tempvars[Parameter] = new Constant(i);
                valueToAdd = Children[3].Calculate(CalculationSetting, tempvars);
                if (valueToAdd == double.NaN)
                    return double.NaN;
                sum += valueToAdd;
            }

            return sum;
        }

        public override  string ToString()
        {
            if(Children[2].ToString() == "1")
                return Type.ToLower() + "(" + Parameter.ToString() + ", " + Children[0].ToString() + ", " + Children[1].ToString() + ", " + Children[3].ToString() + ")";
            else
                return Type.ToLower() + "(" + Parameter.ToString() + ", " + Children[0].ToString() + ", " + Children[1].ToString() + ", " + Children[2].ToString() + ", " + Children[3].ToString() + ")";
        }

    }
}
