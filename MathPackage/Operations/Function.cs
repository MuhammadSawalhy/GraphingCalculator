using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathPackage.Operations
{
    public class Function : Node
    {
        public override int ArgumentNumber => 0;
        public override SyntaxType Syntax_Type => SyntaxType.Function;
        public override string Type => "Function";

        public string Name = "";

        public Function(string name, Node[] args)
        {
            Children = args;
            Name = name;
        }

        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {

            // Searching in the {Functions}
            for (int i = 0; i < CalculationSetting.Funcs.Count; i++)
            {
                if (CalculationSetting.Funcs[i].Name.Name == Name)
                {
                    return CalculationSetting.Funcs[i].Calculate(Children, CalculationSetting, tempVars);
                }
            }
            // Searching in the {NumberLists}
            for (int i = 0; i < CalculationSetting.GraphSetting.Sketch.NumbersLists.Count; i++)
            {
                if (CalculationSetting.GraphSetting.Sketch.NumbersLists.ElementAt(i).Name == Name)
                {
                    return CalculationSetting.GraphSetting.Sketch.NumbersLists.ElementAt(i).GetItem((int)Children[0].Calculate(CalculationSetting, tempVars));
                }
            }
            // Searching in the {GraphObjects}
            for (int i = 0; i < CalculationSetting.GraphSetting.Sketch.Objects.Count; i++)
            {
                if (CalculationSetting.GraphSetting.Sketch.Objects.Item(i).Name == Name)
                {
                    if (CalculationSetting.GraphSetting.Sketch.Objects.Item(i) is Graphing.Objects.XFunction)
                        return ((Graphing.Objects.XFunction)CalculationSetting.GraphSetting.Sketch.Objects.Item(i)).Get(Children[0].Calculate(CalculationSetting, tempVars));
                    else if (CalculationSetting.GraphSetting.Sketch.Objects.Item(i) is Graphing.Objects.Points)
                    {
                        if (Children[0].ToString() == CalculationSetting.GraphSetting.sy_x.ToString())
                            return ((Graphing.Objects.Points)CalculationSetting.GraphSetting.Sketch.Objects.Item(i)).Get_X_Value();
                        else if (Children[0].ToString() == CalculationSetting.GraphSetting.sy_y.ToString())
                            return ((Graphing.Objects.Points)CalculationSetting.GraphSetting.Sketch.Objects.Item(i)).Get_Y_Value();
                    }

                }
            }

            throw new Exception($"Your function { Name } is not exist.");
        }
        public override string ToString()
        {
            string args = "";
            for (int i = 0; i < Children.Count(); i++)
            {
                if (args == "")
                {
                    args += Children[i].ToString();
                }
                else
                {
                    args += ", " + Children[i].ToString();
                }
            }
            return Name + "(" + args + ")";
        }


    }
}
