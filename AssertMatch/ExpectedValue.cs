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
    }
}