using System.Collections.Generic;

namespace EasyTests
{
    public class Asserts
    {
        public static Matcher<T> Expect<T>(T source)
        {
            return new Matcher<T>(source);
        }

        public static CollectionEquivalentMatcher<T> ExpectCollection<T>(IEnumerable<T> source)
        {
            return new CollectionEquivalentMatcher<T>(source);
        }
    }
}