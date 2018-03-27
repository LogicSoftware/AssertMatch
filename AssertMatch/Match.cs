using System.Collections.Generic;

namespace AssertMatch
{
    public class Match
    {
        public static Matcher<T> Expect<T>(T source)
        {
            return new Matcher<T>(source);
        }

        public static CollectionMatcher<T> ExpectCollection<T>(IEnumerable<T> source)
        {
            return new CollectionMatcher<T>(source);
        }
    }
}