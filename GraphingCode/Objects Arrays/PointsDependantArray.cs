using System;
using System.Collections.Generic;
using Graphing.Objects;
namespace Graphing.Objects.Arrays
{

    public class PointsDependantArray: ObjectArray<PointsDependant>
    {

        public override void Clear()
        {
            Items.Clear();
        }
        public override int Count => Items.Count;
        public override PointsDependant[] ToArray()
        {
            return Items.ToArray();
        }
        public override List<PointsDependant> ToList()
        {
            return Items;
        }
        public override void AddRange(PointsDependant[] Item)
        {
            Items.AddRange(Item);
        }

        public override PointsDependant Item(int index)
        {
            return Items[index];
        }



        public override void Add(PointsDependant pd)
        {
            Items.Add(pd);
        }

        public override void Delete(PointsDependant Item, bool RemoveControl)
        {
            foreach (Points point in Item.Points)
                point.Dependants.Remove(Item);
            Items.Remove(Item);
            if (RemoveControl)
                Item.Control.Parent.Controls.Remove(Item.Control);
        }
    }


}
