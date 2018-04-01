using System;
using System.Linq.Expressions;
using EasyTests.Visitors;

namespace EasyTests
{
    public class Matcher<T>
    {
        private readonly T _actual;

        public Matcher(T actual)
        {
            _actual = actual;
        }

        internal bool IsMatchTo(Expression<Func<T, bool>> expression)
        {
            var matchComarer = MatchComparerVisitor<T>.BuildComparer(expression);
            return matchComarer.IsMatch(_actual);
        }

        internal string GetFailMessage(Expression<Func<T, bool>> expression)
        {
            var matchComarer = MatchComparerVisitor<T>.BuildComparer(expression);
            return matchComarer.GetFailMessage(_actual);
        }

        public void MatchTo(Expression<Func<T, bool>> expectedValuesExpression)
        {
            if(IsMatchTo(expectedValuesExpression))
            {
                TestFramework.Ok();
            }
            else
            {
                var msg = GetFailMessage(expectedValuesExpression);
                TestFramework.Fail(msg);
            }
        }
    }
}