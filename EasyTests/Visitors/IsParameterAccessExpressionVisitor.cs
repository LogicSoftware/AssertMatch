using System.Linq.Expressions;

namespace EasyTests.Visitors
{
    class IsParameterAccessExpressionVisitor : ExpressionVisitor
    {
        private readonly string _parameterName;
        public bool Result { get; private set; }

        public IsParameterAccessExpressionVisitor(string parameterName)
        {
            _parameterName = parameterName;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if(node.Name == _parameterName)
            {
                Result = true;
            }
            return base.VisitParameter(node);
        }
    }
}