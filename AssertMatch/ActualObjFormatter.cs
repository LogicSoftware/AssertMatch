using System.Collections.Generic;
using System.Linq;

namespace AssertMatch
{
    class ActualObjFormatter<T>
    {
        private List<ValueReader<T>> _valueReaders;

        public ActualObjFormatter(IEnumerable<ValueReader<T>> valueReaders)
        {
            _valueReaders = valueReaders.Distinct(ValueReader<T>.PropertiesComparer).ToList();
        }

        public string Format(T actual)
        {
            if(actual == null)
            {
                return "NULL";
            }

            var values = _valueReaders.Select(x => $"{x.GetMemberName()} = {x.GetFormattedValue(actual)}");
            return $"{{ {string.Join(", ", values)} }}";
        }
    }
}