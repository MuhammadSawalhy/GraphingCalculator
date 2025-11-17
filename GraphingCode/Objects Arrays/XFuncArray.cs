using System;
using System.Collections.Generic;
using Graphing.Objects;

namespace Graphing.Objects.Arrays
{
    
    public class XFuncArray : ObjectArray<XFunction>
    {

        public override void Clear()
        {
            Items.Clear();
        }
      
        public override int Count => Items.Count;
     
        public override XFunction[] ToArray()
        {
            return Items.ToArray();
        }
        public override List<XFunction> ToList()
        {
            return Items;
        }
        public override void AddRange(XFunction[] Item)
        {
            Items.AddRange(Item);
        }

        public override XFunction Item(int index)
        {
            return Items[index];
        }



        public override void Add(XFunction Func)
        {
            Items.Add(Func);
        }

        public override void Delete(XFunction Func, bool RemoveControl)
        {
            Items.Remove(Func);
            if (RemoveControl)
                Func.Control.Parent.Controls.Remove(Func.Control);
        }


    }

}
