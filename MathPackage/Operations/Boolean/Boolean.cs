using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathPackage.Operations
{
    public abstract class Boolean : Node
    {
        public override int ArgumentNumber => 2;
        public override SyntaxType Syntax_Type => SyntaxType.Operator;
        public override string Type => "Boolean";

        public Boolean(Node children1, Node children2) : base(children1, children2) { }
        public Boolean(Node children) : base(children) { }
        public Boolean(Node[] children) : base(children) { }

    }
}
