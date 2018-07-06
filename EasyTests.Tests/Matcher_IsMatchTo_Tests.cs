using System;
using EasyTests.Tests.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static EasyTests.Asserts;

namespace EasyTests.Tests
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

        [TestMethod]
        public void Should_work_with_explicit_bool_comparison()
        {
            var test = new { Value = false };

            var result = Expect(test).IsMatchTo(x => x.Value == false);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Should_work_with_explicit_bool_comparison_with_expected_value_on_the_left()
        {
            var test = new { Value = false };

            var result = Expect(test).IsMatchTo(x => false == x.Value);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Should_be_false_for_False_bool_value_without_explicity_comparing_to_true()
        {
            var test = new { Value = false };

            var result = Expect(test).IsMatchTo(x => x.Value);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Should_work_for_bool_properties_with_property_navigation_expressio_without_explicity_comparing_to_true()
        {
            var test = new { Value = new { NestedValue = false } };

            var result = Expect(test).IsMatchTo(x => x.Value.NestedValue);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Should_be_true_for_True_value_without_explicity_comparing_to_true()
        {
            var test = new { Value = true };

            var result = Expect(test).IsMatchTo(x => x.Value);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Should_be_true_for_inverted_False_bool_value()
        {
            var test = new { Value = false };

            var result = Expect(test).IsMatchTo(x => !x.Value);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Should_be_false_for_inverted_True_value_without_explicity_comparing_to_true()
        {
            var test = new { Value = true };

            var result = Expect(test).IsMatchTo(x => !x.Value);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Should_throw_error_for_unsupported_expression()
        {
            var person = new Person();
            Func<bool> myFunc = () => true;

            Assert.ThrowsException<Exception>(() =>
            {
                Expect(person).IsMatchTo(x => x.Age == 25 && myFunc());
            });
        }

        [TestMethod]
        public void Should_throw_error_for_unsupported_expression_on_the_left_expression()
        {
            var person = new Person();
            Func<bool> myFunc = () => true;

            Assert.ThrowsException<Exception>(() =>
            {
                Expect(person).IsMatchTo(x => myFunc() && x.Age == 25);
            });
        }

        [TestMethod]
        public void More_then_two_comparisions_should_be_supported()
        {
            var person = new Person {Age = 25, Name = "test", Pet = new Pet {Name = "testpet"}};

            var result = Expect(person).IsMatchTo(x => x.Age == 25 && x.Name == "test" && x.Pet.Name == "testpet");

            Assert.IsTrue(result);
        }

        [DataTestMethod]
        [DataRow(Sex.Female, Sex.Female, true, DisplayName = "when match")]
        [DataRow(Sex.Female, Sex.Male, false, DisplayName = "when not match")]
        [DataRow(null, Sex.Male, false, DisplayName = "when actual is null and expected is not null")]
        [DataRow(null, null, true, DisplayName = "when both null")]
        [DataRow(Sex.Male, null, false, DisplayName = "when actual is not null and expected is null")]
        public void Should_work_for_nullable_enums(Sex? actual, Sex? expected, bool expectedResult)
        {
            var person = new Person { Name = "test", Sex = actual };

            var result = Expect(person).IsMatchTo(x => x.Sex == expected);

            Assert.AreEqual(expectedResult, result);
        }

        [DataTestMethod]
        [DataRow(Sex.Female, Sex.Female, true, DisplayName = "when match")]
        [DataRow(Sex.Female, Sex.Male, false, DisplayName = "when not match")]
        public void Should_work_for_enums(Sex actual, Sex expected, bool expectedResult)
        {
            var person = new { Sex = actual };

            var result = Expect(person).IsMatchTo(x => x.Sex == expected);

            Assert.AreEqual(expectedResult, result);
        }

        [DataTestMethod]
        [DataRow(Sex.Female, Sex.Female, true, DisplayName = "when match")]
        [DataRow(Sex.Female, Sex.Male, false, DisplayName = "when not match")]
        public void Should_work_for_enums_with_navigation_properties(Sex actual, Sex expected, bool expectedResult)
        {
            var person = new { Inner = new { Sex = actual } };

            var result = Expect(person).IsMatchTo(x => x.Inner.Sex == expected);

            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Convert_to_int_should_not_throw_exception()
        {
            var person = new { Sex = (object)0};

            var result = Expect(person).IsMatchTo(x => (int)x.Sex == 0);

            Assert.IsTrue(result);
        }
        
        [TestMethod]
        public void Convert_to_string_should_not_throw_exception()
        {
            var person = new { Sex = (object)"Male"};

            var result = Expect(person).IsMatchTo(x => (string)x.Sex == "Male");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Convert_to_enum_should_not_throw_exception()
        {
            var person = new { Sex = (object)Sex.Male};

            var result = Expect(person).IsMatchTo(x => (Sex)x.Sex == Sex.Male);

            Assert.IsTrue(result);
        }
    }
}