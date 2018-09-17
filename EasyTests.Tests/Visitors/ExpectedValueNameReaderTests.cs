using EasyTests.Tests.Entities;
using EasyTests.Visitors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EasyTests.Tests.Visitors
{
    [TestClass]
    public class ExpectedValueNameReaderTests
    {
        Person PersonProperty => new Person();
        static Person StaticPersonProperty => new Person();

        [TestMethod]
        public void GetName_shuld_extract_name_of_local_variable()
        {
            var value = 5;

            Assert.AreEqual("value", GetName(() => value));
        }

        [TestMethod]
        public void GetName_shuld_extract_name_when_there_is_only_members_access_on_local_variable()
        {
            var obj = new { Name = "Test" };

            Assert.AreEqual("obj.Name", GetName(() => obj.Name));
        }

        [TestMethod]
        public void GetName_shuld_extract_name_of_owner_property()
        {
#pragma warning disable 219
            var value = 5;
#pragma warning restore 219

            Assert.AreEqual("PersonProperty", GetName(() => PersonProperty));
        }

        [TestMethod]
        public void GetName_shuld_extract_name_when_there_is_only_members_access_on_owner_property()
        {
            Assert.AreEqual("PersonProperty.Pet.Name", GetName(() => PersonProperty.Pet.Name));
        }

        [TestMethod]
        [Ignore]
        public void GetName_shuld_extract_name_for_static_property_access()
        {
            Assert.AreEqual("DateTime.Now", GetName(() => DateTime.Now));
        }

        [TestMethod]
        public void GetName_shuld_extract_name_for_local_static_property_access()
        {
            Assert.AreEqual("StaticPersonProperty.Name", GetName(() => StaticPersonProperty.Name));
        }

        [TestMethod]
        public void GetName_should_return_nothing_for_literals()
        {
            AssertEmptyName(() => 25);
        }

        [TestMethod]
        public void GetName_should_return_nothing_for_complex_expressions()
        {
            AssertEmptyName(() => PersonProperty.Name + "AAA");
        }

        [TestMethod]
        public void GetName_should_return_nothing_for_new_object_expression_with_member_access()
        {
            AssertEmptyName(() => new { Name = "Test" }.Name);
        }

        private void AssertEmptyName<T>(Expression<Func<T>> expression)
        {
            var result = GetName(expression);
            Assert.IsNull(result, $"But was: {result}");
        }

        private string GetName<T>(Expression<Func<T>> expression)
        {
            return ExpectedValueNameReader.GetName(expression.Body);
        }
    }
}
