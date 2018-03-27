using System.Linq.Expressions;

namespace AssertMatch.Visitors
{
    public class ExpectedValueNameReader : ExpressionVisitor
    {
        string _result;

        public static string GetName(Expression expression)
        {
            var nameReader = new ExpectedValueNameReader();
            nameReader.Visit(expression);
            return nameReader._result;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var visitResult = base.VisitMember(node);

            _result += (_result == null ? "" : ".") + node.Member.Name;

            return visitResult;
        }
    }
}