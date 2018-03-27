using AssertMatch.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static AssertMatch.Match;

namespace AssertMatch.Tests
{
    [TestClass]
    public class Matcher_IsMatchTo_Tests
    {
        [TestMethod]
        public void Simple_property_match_check()
        {
            var person = new Person { Name = "Jack" };

            var result = Expect(person).IsMatchTo(x => x.Name == "Jack");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Simple_property_match_check_when_actual_and_expected_are_nulls()
        {
            var person = new Person { Name = null };

            var result = Expect(person).IsMatchTo(x => x.Name == null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Simple_property_match_check_when_actual_is_null()
        {
            var person = new Person { Name = null };

            var result = Expect(person).IsMatchTo(x => x.Name == "Jack");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Simple_property_match_check_when_expected_is_null()
        {
            var person = new Person { Name = "Jack" };

            var result = Expect(person).IsMatchTo(x => x.Name == null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Simple_property_not_match_check()
        {
            var person = new Person { Name = "Jack" };

            var result = Expect(person).IsMatchTo(x => x.Name == "Not Jack");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Two_properties_match_check()
        {
            var person = new Person { Name = "Jack", Age = 20 };

            var result = Expect(person).IsMatchTo(x => x.Name == "Jack" && 
                                                      x.Age == 20);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Two_properties_not_match_if_first_one_is_not_match()
        {
            var person = new Person { Name = "Jack", Age = 20 };

            var result = Expect(person).IsMatchTo(x => x.Name == "Not Jack" &&
                                                       x.Age == 20);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Two_properties_not_match_if_second_one_is_not_match()
        {
            var person = new Person { Name = "Jack", Age = 20 };

            var result = Expect(person).IsMatchTo(x => x.Name == "Jack" &&
                                                       x.Age == 21);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Match_for_property_navigation_check()
        {
            var person = new Person { Pet = new Pet { Name = "Sharik" } };

            var result = Expect(person).IsMatchTo(x => x.Pet.Name == "Sharik");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void NotMatch_for_property_navigation_check()
        {
            var person = new Person { Pet = new Pet { Name = "Sharik" } };

            var result = Expect(person).IsMatchTo(x => x.Pet.Name == "NotSharik");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Simple_property_match_check_when_expected_value_on_the_left_side()
        {
            var person = new Person { Name = "Jack" };

            var result = Expect(person).IsMatchTo(x => "Jack" == x.Name);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Match_for_property_navigation_check_when_nested_object_is_null()
        {
            var person = new Person { Pet = null };

            var result = Expect(person).IsMatchTo(x => x.Pet.Name == "Sharik");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Match_for_property_navigation_check_when_nested_object_is_null_and_expected_value_is_null()
        {
            var person = new Person { Pet = null };

            var result = Expect(person).IsMatchTo(x => x.Pet.Name == null);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Match_for_property_navigation_check_when_nested_object_is_not_null_and_expected_value_is_null()
        {
            var person = new Person { Pet = new Pet { Name = null } };

            var result = Expect(person).IsMatchTo(x => x.Pet.Name == null);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Should_not_match_if_actual_object_is_null()
        {
            Person person = null;

            var result = Expect(person).IsMatchTo(x => x.Name == "Jack");

            Assert.IsFalse(result);
        }
    }
}