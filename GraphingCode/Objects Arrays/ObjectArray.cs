using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphing.Objects.Arrays
{
  public abstract class ObjectArray<T>
    {
        protected List<T> Items = new List<T>();

        public abstract void Clear();

        public abstract int Count { get; }

        public abstract T[] ToArray();

        public abstract List<T> ToList();

        public abstract void Add(T Obj);

        public abstract void AddRange(T[] Obj);

        public abstract T Item(int index);

        public abstract void Delete(T Obj, bool RemoveControl);
    }
}
