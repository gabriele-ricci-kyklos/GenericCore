using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.Support.EqualityComparers
{
    public class LinqEqualityComparer<T> : EqualityComparer<T>
    {
        private readonly Func<T, T, bool> _eqComparer;
        private readonly Func<T, int> _hashFunction;

        public LinqEqualityComparer(Func<T, T, bool> eqComparer)
            : this(eqComparer, x => x.GetHashCode())
        {
        }

        public LinqEqualityComparer(Func<T, T, bool> eqComparer, Func<T, int> hashFunction)
        {
            _eqComparer = eqComparer;
            _hashFunction = hashFunction;
        }

        public override bool Equals(T x, T y)
        {
            return _eqComparer(x, y);
        }

        public override int GetHashCode(T obj)
        {
            return _hashFunction(obj);
        }
    }
}
