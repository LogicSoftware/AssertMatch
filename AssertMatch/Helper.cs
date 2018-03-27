namespace AssertMatch
{
    class Helper
    {
        public static string FormatValue(object value)
        {
            if (value == null)
            {
                return "NULL";
            }

            if (value is string)
            {
                return $"\"{value}\"";
            }

            return value.ToString();
        }
    }
}