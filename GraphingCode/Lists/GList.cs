using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Graphing.Lists
{
    public abstract class GList<T>
    {
        public GList(string name, Loyc.Syntax.LNode script, List<T> items, GraphSetting graphSetting, Controls.GraphControl control = null)
        {
            Script = script;
            Items = items;
            GraphSetting = graphSetting;
            Control = control;
            SetName(name);
        }

        public Controls.GraphControl Control;

        #region "Name"

        public string Name { get; internal set; }

        public virtual void SetName(string Name_)
        {
            MainFunctions.CheckName(Name_);

            if (GraphSetting.IsNameUsed(Name_, new List<string>(new[] { Name })))
            {
                throw new Exception($"This name \"{Name_}\" has already been used.");
            }

            Name = Name_;
        }

        #endregion

        public GraphSetting GraphSetting;

        public abstract LType ListType { get; }

        public enum LType
        {
            NumbersList,
            GraphObjectsList
        }

        public Loyc.Syntax.LNode Script;

        public List<T> Items = new List<T>();

        public XElement GetAsXml()
        {
            XElement element = new XElement(ListType.ToString());

            XElement script = new XElement("Script");
            script.SetAttributeValue("Value" , Script.ToString().Remove(Script.ToString().Length -1,1));
            element.Add(script);

            script = new XElement("Items");
            script.SetAttributeValue("Value", null);
            element.Add(script);

            return element; 
        }

        public abstract T GetItem(int index);

        public abstract int Count { get; }

    }
}
