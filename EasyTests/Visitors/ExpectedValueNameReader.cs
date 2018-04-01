using System.Linq.Expressions;

namespace EasyTests.Visitors
{
    public class ExpectedValueNameReader : ExpressionVisitor
    {
        string _result;
        int _visitedNodesCount;
        int _knownVisitedNodesCount;

        string Result => _visitedNodesCount == _knownVisitedNodesCount ? _result : null;

        public static string GetName(Expression expression)
        {
            var nameReader = new ExpectedValueNameReader();
            nameReader.Visit(expression);
            return nameReader.Result;
        }

        public override Expression Visit(Expression node)
        {
            if(node != null)
            {
                _visitedNodesCount++;
            }
            
            return base.Visit(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _knownVisitedNodesCount++;
            return base.VisitConstant(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _knownVisitedNodesCount++;
            var visitResult = base.VisitMember(node);

            AppendToName(node.Member.Name);

            return visitResult;
        }

        private void AppendToName(string name)
        {
            _result += (_result == null ? "" : ".") + name;
        }
    }
}