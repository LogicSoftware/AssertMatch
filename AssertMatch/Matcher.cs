using System;
using System.Linq.Expressions;
using AssertMatch.Visitors;

namespace AssertMatch
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
            var matchComarer = BuildComparer(expression);
            return matchComarer.IsMatch(_actual);
        }

        internal string GetFailMessage(Expression<Func<T, bool>> expression)
        {
            var matchComarer = BuildComparer(expression);
            return matchComarer.GetFailMessage(_actual);
        }

        public void MatchTo(Expression<Func<T, bool>> expectedValuesExpression)
        {
            if(!IsMatchTo(expectedValuesExpression))
            {
                throw new Exception("error");
            }
        }

        private MatchComparer<T> BuildComparer(Expression<Func<T, bool>> expression)
        {
            var visitor = new MatchComparerVisitor<T>(expression.Parameters[0]
                                                                .Name);
            visitor.Visit(expression);
            var matchComarer = visitor.Comparer;
            return matchComarer;
        }
    }
}