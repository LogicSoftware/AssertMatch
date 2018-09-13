using System;
using System.Linq;
using System.Reflection;

namespace EasyTests
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
            var adapter = MSTestAdapterBuilder.Build();
            if (adapter == null)
            {
                throw new NotSupportedException(
                    "Project's test framework is not supported yet. " +
                    "Use TestFramework.SetAdapter method for using this lib with current test framework");
            }

            return adapter;
        }
    }
}