using System;
using System.Collections.Generic;
using System.Text;
using Loyc;

namespace Graphing
{
    public class Variable
    {
        public Variable(Symbol name, MathPackage.Node value , Controls.GraphControl control = null )
        {
            Name = name;
            Value = value;
            Control = control;
        }
        public Symbol Name;
        MathPackage.Node Value_;
        public MathPackage.Node Value
        {
            get
            {
                return Value_;
            }
            set
            {
                Value_ = value;
                if (Control != null)
                {
                    Control.Script.Text = Name.Name + " = " + value.ToString();
                }
            }
        }
        public Controls.GraphControl Control;
        public override string ToString()
        {
            return $"{Name.ToString()} = {Value.ToString()}";
        }

        public System.Xml.Linq.XElement GetAsXml()
        {
            System.Xml.Linq.XElement element = new System.Xml.Linq.XElement("Variable");
            element.SetAttributeValue("Name", Name.Name);
            element.SetAttributeValue("Value", Value_.ToString());
            return element;
        }

    }
}
