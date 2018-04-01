using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EasyTests
{
    class MatchComparer<T>
    {
        //name of match func argument.
        //for example: x => x.Name == "test", then actualArgName is x
        private readonly string _actualArgName;

        private List<MatchComparerItems<T>> _items = new List<MatchComparerItems<T>>();

        public IEnumerable<ValueReader<T>> ActualValueReaders => _items.Select(x => x.ValueReader);

        public MatchComparer(string actualArgName)
        {
            _actualArgName = actualArgName;
        }

        public bool IsMatch(T obj)
        {
            if(_items.Count == 0)
            {
                throw new Exception("At least one expected value should be registered");
            }

            return _items.All(x => x.IsEqual(obj));
        }

        public string GetFailMessage(T actual)
        {
            var result = new StringBuilder("\r\n");
            result.AppendLine("Match is failed:");
            foreach (var expectedValue in _items)
            {
                result.AppendLine($"    {expectedValue.GetMessage(actual, _actualArgName)}");
            }

            return result.ToString();
        }

        public void RegisterItem(ValueReader<T> valueReader, ExpectedValue expectedValue)
        {
            _items.Add(new MatchComparerItems<T>(valueReader, expectedValue)); 
        }

        public string FormatExpectedObj()
        {
            var values = string.Join(", ", _items.Select(x => x.GetExpectedValueMsg()));
            return $"{{ {values} }}";
        }
    }
}