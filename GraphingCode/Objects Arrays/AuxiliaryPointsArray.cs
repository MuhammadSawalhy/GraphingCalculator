using System;
using System.Collections.Generic;
using Graphing.Objects;


namespace Graphing.Objects.Arrays
{

    public class AuxiliaryPointsArray : ObjectArray<AuxiliaryPoints>
    {

        public override void Clear()
        {
            Items.Clear();
        }

        public override int Count => Items.Count;
        public override AuxiliaryPoints[] ToArray()
        {
            return Items.ToArray();
        }

        public override List<AuxiliaryPoints> ToList()
        {
            return Items;
        }

        public override void AddRange(AuxiliaryPoints[] Item)
        {
            Items.AddRange(Item);
        }

        public override AuxiliaryPoints Item(int index)
        {
            return Items[index];
        }

        public override void Add(AuxiliaryPoints Item)
        {
            Items.Add(Item);
        }

        public override void Delete(AuxiliaryPoints Item,bool RemoveControl)
        {
            Items.Remove(Item);
        }


    }
    

}
