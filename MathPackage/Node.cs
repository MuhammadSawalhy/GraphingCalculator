using System;
using System.Collections.Generic;
using System.Text;
using MathPackage.Operations;
using System.Xml.Linq;
using Loyc.Syntax;
using Loyc.Syntax.Les;
using Loyc;

namespace MathPackage
{
    public abstract class Node : object,IDisposable
    {


        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            var equals = true;
            for (int i = 0; i < Children.Length; i++)
            {
                if (!Children[i].Equals(((Node)obj).Children[i]))
                {
                    equals = false;
                }
            }
            return equals;
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + ArgumentNumber.GetHashCode();
            hash = hash * 23 + Children.GetHashCode();
            return hash;
        }

        #region "Variables"

        public abstract string Type { get; }
        public abstract SyntaxType Syntax_Type { get; }
        public abstract int ArgumentNumber { get; }

        public bool IsBoolean
        {
            get
            {
                if(this is Operations.Boolean)
                {
                    return true;
                }
                return false;
            }
        }

        public enum SyntaxType
        {
            Operator,
            Function,
            Literal
        }

        public Node() { }

        protected Node(Node[] children)
        {
            if (children == null)
            {
                if (ArgumentNumber != 0)
                {
                    throw new Exception("Wrong number of arguments");
                }
                Children = children;
                return;
            }
            if (ArgumentNumber > 0 && children.Length != ArgumentNumber)
            {
                throw new Exception("Wrong number of arguments");
            }
            Children = children;
        }
        protected Node(Node children1, Node children2)
        {
            if (children1 == null || children2 == null)
            {
                if (ArgumentNumber != 0)
                {
                    throw new Exception("Wrong number of arguments");
                }
                Children = null;
                return;
            }

            Children = new Node[] { children1, children2 };
        }

        protected Node(Node children)
        {
            if (children == null)
            {
                if (ArgumentNumber != 0)
                {
                    throw new Exception("Wrong number of arguments");
                }
                Children = null;
                return;
            }
            Children = new Node[] { children };
        }

        public string GetNodeType()
        {
            switch (this)
            {

                case Variable variable:
                    return "Variable";
                case Constant constant:
                    return "Constant";
                default:
                    return Type;
            }
        }


        public void Dispose()
        {

        }

        /// <summary>
        /// It is the arguments inside an equation looks like "cos( {} ), sin( {} ), tan( {} ), log( {} ), exp( {} ), ...."
        /// </summary>
        public Node[] Children { get; set; }

        #endregion

        public virtual double Calculate(CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars )
        {
            return double.NaN;
        }

        public double Calculate(CalculationSetting CalculationSetting)
        {
            return this.Calculate(CalculationSetting, new Dictionary<Loyc.Symbol, Node>());
        }

        public abstract override string ToString();
        
        public bool ContainsVaraible
        {
            get
            {
                if (this.GetNodeType() == "Variable")
                {
                    return true;
                }
                if (this.GetNodeType() == "Constant")
                {
                    return false;
                }
                foreach (Node child in Children)
                {
                    if (child.ContainsVaraible)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool ContainsAddOrSub
        {
            get
            {
                foreach (Node child in Children)
                {
                    switch (child)
                    {
                        case Add _:
                            return true;
                        case Subtract _:
                            return true;
                    }
                }
                return false;
            }
        }

        public bool IsConstantOrVariable
        {
            get
            {
                switch (this)
                {
                    case Constant _:
                        return true;
                    case Variable _:
                        return true;
                }
                return false;
            }
        }

        public virtual string GetAsXml()
        {
            return "";
        }

    }
}