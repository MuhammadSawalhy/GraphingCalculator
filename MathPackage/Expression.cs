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
    public class Expression
    {

        public LNode LRoot { get; set; }
        public Node Root { get; set; }

        public string Text { get; set; }

        public Expression(LNode root)
        {
            LRoot = root;
            Root = Transformer.GetNodeFromLoycNode(LRoot);
            Text = root.ToString();
        }
        public Expression(Node root)
        {
            LRoot = Transformer.GetLoycNodeFromNode(root);
            Root = root;
            Text = root.ToString();
        }
        public Expression(string str)
        {
            LRoot = Transformer.GetLoycNodeFromString(str);
            Root = Transformer.GetNodeFromLoycNode(LRoot);
            Text = str;
        }
        public Expression(XDocument doc)
        {
            Root = Transformer.GetNodeFromXml(doc.Root);
        }

        public override bool Equals(object obj)
        {

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return Root.Equals(((Expression)obj).Root);
        }

        public override int GetHashCode()
        {
            var hash = 17;
            hash = hash * 23 + Root.GetHashCode();
            return hash;
        }
    }
}
