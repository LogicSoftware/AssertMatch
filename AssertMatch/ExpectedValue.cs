namespace AssertMatch
{
    class ExpectedValue<T>
    {
        public ValueReader<T> ValueReader { get; }

        public object Expected { get; }

        public ExpectedValue(ValueReader<T> valueReader, object expected)
        {
            ValueReader = valueReader;
            Expected = expected;
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

        public string GetMessage(T actual)
        {
            var name = ValueReader.GetMemberName();
            if (IsEqual(actual))
            {
                return $"✓ {name} == {FormatValue(Expected)}";
            }

            var actualValue = ValueReader.GetValue(actual, out var isCantReadProperiesChain);
            var actualValueMsg = FormatValue(actualValue);
            if (isCantReadProperiesChain)
            {
                actualValueMsg = $"{ValueReader.GetNameBeforeNullInPropsChain(actual)} is NULL";
            }
            
            return $"✘ {name} == {FormatValue(Expected)}, Actual: {actualValueMsg}";
        }

        private string FormatValue(object value)
        {
            if(value == null)
            {
                return "NULL";
            }

            if(value is string)
            {
                return $"\"{value}\"";
            }

            return value.ToString();
        }
    }
}