using System;
using System.Collections.Generic;

using System.Linq;

namespace Graphing.Objects.Arrays
{

    public class PointsArray : ObjectArray<Points>
    {

        public override void Clear()
        {
            Items.Clear();
        }
        public override int Count => Items.Count;
        public override Points[] ToArray()
        {
            return Items.ToArray();
        }
        public override List<Points> ToList()
        {
            return Items;
        }
        public override void AddRange(Points[] Item)
        {
            Items.AddRange(Item);
        }

        public override Points Item(int index)
        {
            return Items[index];
        }



        public override void Add(Points Point)
        {
            Items.Add(Point);
        }

        public override void Delete(Points Point, bool RemoveControl)
        {
            while (Point.Dependants.Count > 0)
                Point.GraphSetting.Sketch.Objects.Delete(Point.Dependants[0], true);
            Items.Remove(Point);
            if (RemoveControl)
                Point.Control.Parent.Controls.Remove(Point.Control);
        }

    }

}
