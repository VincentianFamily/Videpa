using System.Collections.Generic;
using System.Linq;

namespace Videpa.Core
{
    public class Maybe<T>
    {
        private readonly IEnumerable<T> _values;

        public Maybe()
        {
            _values = new T[0];
        }

        public Maybe(T value)
        {
            _values = value == null ? new T[0] : new[] { value };
        }

        public bool IsEmpty => !_values.Any();
        public bool HasValue => _values.Any();
        public T Value => _values.FirstOrDefault();
    }

    // Jon Skeet already coded this for us @ http://stackoverflow.com/a/2601501
}
