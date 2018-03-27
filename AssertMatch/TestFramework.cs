using System;
using System.Linq;
using System.Reflection;

namespace AssertMatch
{
    public class TestFramework
    {
        static Lazy<ITestFrameworkAdapter> _adapterLoader = new Lazy<ITestFrameworkAdapter>(LoadAdapter);

        static ITestFrameworkAdapter _adapter;

        static ITestFrameworkAdapter Adapter => _adapter ?? (_adapter = _adapterLoader.Value);

        public static void Fail(string message)
        {
            Adapter.Fail(message);
        }

        public static void Ok()
        {
            Adapter.Ok();
        }

        public static void SetAdapter(ITestFrameworkAdapter adapter)
        {
            _adapter = adapter;
        }

        private static ITestFrameworkAdapter LoadAdapter()
        {
            var assembly = Assembly.Load("AssertMatch.MSTestV2Adapter");
            var adapter = assembly.GetTypes().First(x => typeof(ITestFrameworkAdapter).IsAssignableFrom(x));
            return (ITestFrameworkAdapter)Activator.CreateInstance(adapter);
        }
    }
}