using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathPackage.Operations
{
    public abstract class Binary : Node
    {
        public override int ArgumentNumber => 2;
        public override SyntaxType Syntax_Type => SyntaxType.Function;
        public override string Type => "Binary";

        public Binary(Node children1, Node children2) : base(children1, children2) { }
        public Binary(Node children) : base(children) { }
        public Binary(Node[] children) : base(children) { }

        public override double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
            return double.NaN;
        }

    }
}
