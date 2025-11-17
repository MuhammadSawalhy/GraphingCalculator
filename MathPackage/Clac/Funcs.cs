using System;
using Loyc;
using Loyc.Syntax;
using System.Collections.Generic;
using System.Linq;


namespace MathPackage.Operations
{

    public class Func : object
    {

        public Symbol Name;
        public Node Process { get; set; }
        private Dictionary<Loyc.Symbol, Node> ArgsDic;
        protected List<Symbol> args_;
        public List<Symbol> Args
        {
            get
            {
                return args_;
            }
            set
            {
                args_ = value;
                if(value != null)
                    foreach (Symbol sy in value)
                    {
                        ArgsDic = new Dictionary<Loyc.Symbol, Node>();
                        ArgsDic.Add(sy, null);
                    }
            }
        }
        public Func(Symbol name, List<Symbol> argsNames, Node process)
        {
            Name = name;
            Args = argsNames;
            Process = process;
        }

        public double Calculate(Node[] argsValues, CalculationSetting CalculationSetting, Dictionary<Loyc.Symbol, Node> tempVars)
        {
         
            #region Preparing tempVars
          
            for(int i = 0; i < Args.Count;i++)
            {
                ArgsDic[Args[i]] = new Constant(argsValues[i].Calculate(CalculationSetting, tempVars));
            }

            #endregion

            return Process.Calculate(CalculationSetting, ArgsDic);

        }

        public System.Xml.Linq.XElement GetAsXml()
        {
            System.Xml.Linq.XElement element = new System.Xml.Linq.XElement("Func");
            element.SetAttributeValue("Name", Name);
            string args = "";
            foreach (Symbol arg in Args)
            {
                if(args == "" )
                    args += arg.ToString();
                else
                    args += ", " + arg.ToString();
            }
            element.SetAttributeValue("Args", "{" + args + "}");
            element.SetAttributeValue("Process", Process.ToString());
            return element;
        }
        public override string ToString()
        {
            string args = "";
            foreach (Symbol arg in Args)
            {
                if (args == "")
                    args += arg.ToString();
                else
                    args += ", " + arg.ToString();
            }
            return $"{Name.ToString()}({args}) = {Process.ToString()}";
        }
    }
}
