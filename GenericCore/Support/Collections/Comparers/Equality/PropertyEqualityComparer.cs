using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenericCore.Support.Collections.Comparers.Equality
{
    public class PropertyEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _eqComparer;
        private readonly Func<T, int> _hashFunction;

        public PropertyEqualityComparer(Func<T, object> propertyFx)
        {
            _eqComparer = (x, y) => propertyFx(x).Equals(propertyFx(y));
            _hashFunction = (x) => propertyFx(x).GetHashCode();
        }

        public bool Equals(T x, T y)
        {
            return _eqComparer(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _hashFunction(obj);
        }
    }
}
