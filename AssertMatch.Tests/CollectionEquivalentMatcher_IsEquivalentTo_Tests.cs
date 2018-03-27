using System.Collections.Generic;
using AssertMatch.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static AssertMatch.Match;

namespace AssertMatch.Tests
{
    [TestClass]
    public class CollectionEquivalentMatcher_IsEquivalentTo_Tests
    {
        [TestMethod]
        public void Single_item_match_check()
        {
            var persons = new[]
            {
                new Person { Name = "Jack" }
            };

            var result = ExpectCollection(persons).IsEquivalentTo(
                x => x.Name == "Jack"
            );

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Single_item_not_match_check()
        {
            var persons = new[]
            {
                new Person { Name = "Jack" }
            };

            var result = ExpectCollection(persons).IsEquivalentTo(
                x => x.Name == "NotJack"
            );

            Assert.IsFalse(result);
        }
        
        [TestMethod]
        public void Two_expected_items_match_to_the_same_item_in_actual()
        {
            var persons = new[]
            {
                new Person { Name = "Jack" },
                new Person { Name = "NotJack" },
            };

            var result = ExpectCollection(persons).IsEquivalentTo(
                x => x.Name == "Jack",
                x => x.Name == "Jack"
            );

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Expected_items_is_subset_of_actual_items()
        {
            var persons = new[]
            {
                new Person { Name = "Jack" },
                new Person { Name = "NotJack" },
            };

            var result = ExpectCollection(persons).IsEquivalentTo(
                x => x.Name == "Jack"
            );

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Actual_items_is_subset_of_expected_items()
        {
            var persons = new[]
            {
                new Person { Name = "Jack" }
            };

            var result = ExpectCollection(persons).IsEquivalentTo(
                x => x.Name == "Jack",
                x => x.Name == "NotJack"
            );

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Actual_is_null_check()
        {
            IEnumerable<Person> persons = null;

            var result = ExpectCollection(persons).IsEquivalentTo(
                x => x.Name == "Jack"
            );

            Assert.IsFalse(result);
        }
    }
}