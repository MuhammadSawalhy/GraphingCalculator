using System;
using System.Collections.Generic;
using Graphing.Objects;

namespace Graphing.Objects.Arrays
{
    
    public class ObjectsArray : ObjectArray<GraphObject>
    {

        public override void Clear()
        {
            Items.Clear();
        }   
        public override int Count => Items.Count;
        public override GraphObject[] ToArray()
        {
            return Items.ToArray();
        }
        public override List<GraphObject> ToList()
        {
            return Items;
        }
        public override void AddRange(GraphObject[] Item)
        {
            Items.AddRange(Item);
        }
        public override GraphObject Item(int index)
        {
            return Items[index];
        }

        public override void Add(GraphObject obj)
        {
            Items.Add(obj);
        }

        public override void Delete(GraphObject obj, bool RemoveControl)
        {
            Items.Remove(obj);
            if (RemoveControl)
                if(obj.Control != null && obj.Control.Parent != null)
                    obj.Control.Parent.Controls.Remove(obj.Control);
        }


    }

}
