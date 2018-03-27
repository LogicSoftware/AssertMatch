using System.Collections.Generic;
using AssertMatch.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static AssertMatch.Match;

namespace AssertMatch.Tests
{
    [TestClass]
    public class CollectionEquivalentMatcher_GetFailMessage_Tests
    {
        [TestMethod]
        public void Actual_collection_is_null()
        {
            IEnumerable<Person> persons = null;

            var msg = ExpectCollection(persons).GetFailMessage(
                x => x.Age == 25
            );

            Assert.AreEqual(@"
Actual collection is NULL.
", msg);
        }

        [TestMethod]
        public void Single_item_not_match_check()
        {
            var persons = new[]
            {
                new Person { Age = 20 }
            };

            var msg = ExpectCollection(persons).GetFailMessage(
                x => x.Age == 25
            );

            Assert.AreEqual(@"
Actual collection is not equivalent to specifed items.
Expected items:
    ✘ { Age = 25 }

Actual items:
    ✘ { Age = 20 }
", msg);
        }

        [TestMethod]
        public void One_item_match_and_one_is_not_match_check()
        {
            var persons = new[]
            {
                new Person { Age = 20 },
                new Person { Age = 25 },
            };

            var msg = ExpectCollection(persons).GetFailMessage(
                x => x.Age == 25,
                x => x.Age == 18
            );

            Assert.AreEqual(@"
Actual collection is not equivalent to specifed items.
Expected items:
    ✓ { Age = 25 }
    ✘ { Age = 18 }

Actual items:
    ✘ { Age = 20 }
    ✓ { Age = 25 }
", msg);
        }

    }
}