using System.Collections.Generic;
using System.Linq;
using EasyTests.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static EasyTests.Asserts;

namespace EasyTests.Tests
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

        [TestMethod]
        public void Actual_is_empty_check()
        {
            var persons = Enumerable.Empty<Person>();

            var msg = ExpectCollection(persons).GetFailMessage(
                x => x.Age == 10
            );

            Assert.AreEqual(@"
Actual collection is not equivalent to specifed items.
Expected items:
    ✘ { Age = 10 }

Actual items:
    EMPTY
", msg);
        }

        [TestMethod]
        public void Expected_is_empty_check()
        {
            var persons = new[]
            {
                new Person { Age = 20 }
            };

            var msg = ExpectCollection(persons).GetFailMessage(

            );

            Assert.AreEqual(@"
Actual collection is not equivalent to specifed items.
Expected items:
    EMPTY

Actual items:
    ✘ {  }
", msg);
        }

        [TestMethod]
        public void Actual_items_should_be_formatted_with_all_expected_values()
        {
            var persons = new[]
            {
                new Person { Age = 20, Name = "Jack" }
            };

            var msg = ExpectCollection(persons).GetFailMessage(
                x => x.Age == 19,
                x => x.Name == "NotJack"
            );

            Assert.AreEqual(@"
Actual collection is not equivalent to specifed items.
Expected items:
    ✘ { Age = 19 }
    ✘ { Name = ""NotJack"" }

Actual items:
    ✘ { Age = 20, Name = ""Jack"" }
", msg);
        }

        [TestMethod]
        public void Expected_object_with_multiple_properties_formattig_check()
        {
            var obj = new { Name = "NotJack" };
            var persons = new[]
            {
                new Person { Age = 20, Name = "Jack" }
            };

            var msg = ExpectCollection(persons).GetFailMessage(
                x => x.Age == 20 && x.Name == obj.Name
            );

            Assert.AreEqual(@"
Actual collection is not equivalent to specifed items.
Expected items:
    ✘ { Age = 20, Name = ""NotJack"" (obj.Name) }

Actual items:
    ✘ { Age = 20, Name = ""Jack"" }
", msg);
        }

        [TestMethod]
        public void Navigation_properties_check()
        {
            var persons = new[]
            {
                new Person { Pet = new Pet { Name = "Sharik"} }
            };

            var msg = ExpectCollection(persons).GetFailMessage(
                x => x.Pet.Name == "Jack"
            );

            Assert.AreEqual(@"
Actual collection is not equivalent to specifed items.
Expected items:
    ✘ { Pet.Name = ""Jack"" }

Actual items:
    ✘ { Pet.Name = ""Sharik"" }
", msg);

        }
    }
}