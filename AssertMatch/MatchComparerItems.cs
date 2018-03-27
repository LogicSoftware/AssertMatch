namespace AssertMatch
{
    class MatchComparerItems<T>
    {
        private readonly ExpectedValue _expectedValue;
        public ValueReader<T> ValueReader { get; }

        public object Expected => _expectedValue.Value;

        public MatchComparerItems(ValueReader<T> valueReader, ExpectedValue expectedValue)
        {
            _expectedValue = expectedValue;
            ValueReader = valueReader;
        }

        public bool IsEqual(T obj)
        {
            var actualValue = ValueReader.GetValue(obj, out var cantReadProperiesChain);
            if (cantReadProperiesChain)
            {
                return false;
            }

            if(actualValue == null && Expected == null)
            {
                return true;
            }

            if(actualValue == null || Expected == null)
            {
                return false;
            }

            return Expected.Equals(actualValue);
        }

        public string GetMessage(T actual, string actualArgName)
        {
            var name = $"{actualArgName}.{ValueReader.GetMemberName()}";
            var expectedText = _expectedValue.GetFormattedValue();
            if (IsEqual(actual))
            {
                return $"{Constants.PassSign} {name} == {expectedText}";
            }

            var actualValue = ValueReader.GetValue(actual, out var isCantReadProperiesChain);
            var actualValueMsg = Helper.FormatValue(actualValue);
            if (isCantReadProperiesChain)
            {
                var nullPathName = actualArgName;
                var pathBeforeNull = ValueReader.GetNameBeforeNullInPropsChain(actual);
                if (!string.IsNullOrEmpty(pathBeforeNull))
                {
                    nullPathName += $".{pathBeforeNull}";
                }
                actualValueMsg = $"{nullPathName} is NULL";
            }
            
            return $"{Constants.FailSign} {name} == {expectedText}, Actual: {actualValueMsg}";
        }

        public string GetExpectedValueMsg()
        {
            return $"{ValueReader.GetMemberName()} = {_expectedValue.GetFormattedValue()}";
        }
    }
}