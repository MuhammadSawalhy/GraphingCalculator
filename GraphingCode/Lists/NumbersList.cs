using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;

namespace Graphing.Lists
{
    public class NumbersList : GList<double>
    {
        public NumbersList(string name, Loyc.Syntax.LNode script,MathPackage.Node start , MathPackage.Node end, MathPackage.Node step, GraphSetting graphSetting, Controls.GraphControl graphControl = null) : base(name, script, null, graphSetting, graphControl)
        {
            Items = null;
            Start = start;
            End = end;
            Step = step;
        }
        public NumbersList(string name, Loyc.Syntax.LNode script, List<double> items, GraphSetting graphSetting, Controls.GraphControl graphControl = null) : base(name, script, items, graphSetting, graphControl)
        {
        }

        public MathPackage.Node Start, End, Step;

        public override LType ListType => LType.NumbersList;

        public override int Count {
            get
            {
                return (int)Math.Floor((End.Calculate(GraphSetting.CalculationSetting) - Start.Calculate(GraphSetting.CalculationSetting)) / Step.Calculate(GraphSetting.CalculationSetting) + 1);
            }
        }

        public override double GetItem(int index)
        {
            if (Items == null)
                if (index <= Count)
                    return Start.Calculate(GraphSetting.CalculationSetting) + Step.Calculate(GraphSetting.CalculationSetting) * index;
                else throw new Exception($"Index { index } is out of the range { "[" + Start.Calculate(GraphSetting.CalculationSetting) + "," + End.Calculate(GraphSetting.CalculationSetting) + "]" }");
            else
                return Items[index];
        }

    }
}
