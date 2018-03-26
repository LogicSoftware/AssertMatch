using Microsoft.VisualStudio.TestTools.UnitTesting;
using static AssertMatch.Match;

namespace MSTestV2Example
{
    [TestClass]
    public class ObjectMatchExampleTests
    {
        [TestMethod]
        public void Ok_sample()
        {
            var obj = new
            {
                Name = "Jack",
                Age = 25,
                Pet = new { Name = "Sharik" }
            };

            Expect(obj).MatchTo(x => x.Name == "Jack" &&
                                     x.Age == 25 &&
                                     x.Pet.Name == "Sharik");
        }

        [TestMethod]
        public void Single_prop_fail()
        {
            var obj = new
            {
                Name = "Jack",
                Age = 25
            };

            Expect(obj).MatchTo(x => x.Name == "NotJack" &&
                                     x.Age == 25);
        }

        [TestMethod]
        public void Multiple_props_fail()
        {
            var obj = new
            {
                Name = "Jack",
                Age = 25,
                Department = "Dev"
            };

            Expect(obj).MatchTo(x => x.Name == "NotJack" &&
                                     x.Age == 25 &&
                                     x.Department == "QA");
        }

        [TestMethod]
        public void Navigation_prop_fail_sample()
        {
            var obj = new
            {
                Name = "Jack",
                Pet = new { Name = "Sharik" }
            };

            Expect(obj).MatchTo(x => x.Pet.Name == "Jack");
        }

        [TestMethod]
        public void Navigation_prop_fail_when_nested_obj_is_null()
        {
            var obj = new
            {
                Inner = (string)null
            };

            Expect(obj).MatchTo(x => x.Inner.Length == 25);
        }
    }
}