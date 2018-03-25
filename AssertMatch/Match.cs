namespace AssertMatch
{
    public class Match
    {
        public static Matcher<T> Expect<T>(T source)
        {
            return new Matcher<T>(source);
        }
    }
}