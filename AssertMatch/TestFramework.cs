using System;
using System.Linq;
using System.Reflection;

namespace AssertMatch
{
    public class TestFramework
    {
        static Lazy<ITestFrameworkAdapter> _adapter = new Lazy<ITestFrameworkAdapter>(GetAdapter);

        public static void Fail(string message)
        {
            _adapter.Value.Fail(message);
        }

        public static void Ok()
        {
            _adapter.Value.Ok();
        }

        private static ITestFrameworkAdapter GetAdapter()
        {
            var assembly = Assembly.Load("AssertMatch.MSTestV2Adapter");
            var adapter = assembly.GetTypes().First(x => typeof(ITestFrameworkAdapter).IsAssignableFrom(x));
            return (ITestFrameworkAdapter)Activator.CreateInstance(adapter);
        }
    }
}