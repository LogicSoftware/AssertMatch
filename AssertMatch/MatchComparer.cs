using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssertMatch
{
    class MatchComparer<T>
    {
        private List<ExpectedValue<T>> _expectedValues = new List<ExpectedValue<T>>();

        public bool IsMatch(T obj)
        {
            if(_expectedValues.Count == 0)
            {
                throw new Exception("At least one expected value should be registered");
            }

            return _expectedValues.All(x => x.IsEqual(obj));
        }

        public string GetFailMessage(T actual)
        {
            var result = new StringBuilder("\r\n");
            result.AppendLine("Match is failed:");
            foreach (var expectedValue in _expectedValues)
            {
                result.AppendLine($"    {expectedValue.GetMessage(actual)}");
            }

            return result.ToString();
        }

        public void RegisterExpectedValue(ValueReader<T> valueReader, object expectedValue)
        {
            _expectedValues.Add(new ExpectedValue<T>(valueReader, expectedValue)); 
        }
    }
}