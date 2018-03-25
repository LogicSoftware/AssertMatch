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
            var visitor = new MatchComparerVisitor<T>(expression.Parameters[0].Name);
            visitor.Visit(expression);

            return visitor.Comparer.IsMatch(_actual);
        }

        public void MatchTo(Expression<Func<T, bool>> expectedValuesExpression)
        {
            if(!IsMatchTo(expectedValuesExpression))
            {
                throw new Exception("error");
            }
        }
    }
}