using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace EasyTests
{
    class MSTestAdapterBuilder
    {
        public static ITestFrameworkAdapter Build()
        {
            var assemlby = GetFrameworkAssembly();
            if (assemlby == null)
            {
                return null;
            }
            var assertType = assemlby.GetType("Microsoft.VisualStudio.TestTools.UnitTesting.Assert");
            
            var failAction = BuildFailAction(assertType);
            var okAction = BuildOkAction(assertType);

            return new DelegateTestFrameworkAdapter(okAction, failAction);
        }

        private static Action BuildOkAction(Type assertType)
        {
            var isTrueMethod = assertType.GetMethod("IsTrue", new[] {typeof(bool)});
            Expression body = Expression.Call(isTrueMethod, Expression.Constant(true));
            return Expression.Lambda<Action>(body).Compile();
        }

        private static Action<string> BuildFailAction(Type assertType)
        {
            var failMethod = assertType.GetMethod("Fail", new[] {typeof(string)});
            ParameterExpression msgParameter = Expression.Parameter(typeof(string), "msg");
            Expression body = Expression.Call(failMethod, msgParameter);
            return Expression.Lambda<Action<string>>(body, msgParameter).Compile();
        }

        static Assembly GetFrameworkAssembly()
        {
            try
            {
                return Assembly.Load("Microsoft.VisualStudio.TestPlatform.TestFramework");
            }
            catch (FileNotFoundException)
            {
                // assembly doesn't exists
                return null;
            }
        }

        class DelegateTestFrameworkAdapter : ITestFrameworkAdapter
        {
            private readonly Action _okAction;
            private readonly Action<string> _failAction;

            public DelegateTestFrameworkAdapter(Action okAction, Action<string> failAction)
            {
                _okAction = okAction;
                _failAction = failAction;
            }

            public void Fail(string message)
            {
                _failAction(message);
            }

            public void Ok()
            {
                _okAction();
            }
        }
    }
}
