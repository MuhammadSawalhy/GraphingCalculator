using System;
using System.Collections.Generic;
using Graphing.Objects;

namespace Graphing.Objects.Arrays
{

    public class TextArray:ObjectArray<Text>
    {

        public override void Clear()
        {
            Items.Clear();
        }

        public override int Count => Items.Count;

        public override Text[] ToArray()
        {
            return Items.ToArray();
        }
        public override List<Text> ToList()
        {
            return Items;
        }
        public override void AddRange(Text[] Item)
        {
            Items.AddRange(Item);
        }

        public override Text Item(int index)
        {
            return Items[index];
        }



        public override void Add(Text text)
        {
            Items.Add(text);
        }
        public override void Delete(Text text, bool RemoveControl)
        {
            Items.Remove(text);
        }
    }


}
