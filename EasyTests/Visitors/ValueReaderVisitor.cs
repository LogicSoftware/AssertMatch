using System;
using System.Linq.Expressions;

namespace EasyTests.Visitors
{
    class ValueReaderVisitor<T> : ExpressionVisitor
    {
        private readonly string _parameterName;
        private bool _isFirstVisitMember = true;

        public ValueReader<T> ValueReader { get; }

        public ValueReaderVisitor(string parameterName)
        {
            _parameterName = parameterName;
            ValueReader = new ValueReader<T>();
        }

        public override Expression Visit(Expression node)
        {
            if(node.NodeType != ExpressionType.MemberAccess &&
               node.NodeType != ExpressionType.Parameter)
            {
                throw new Exception($"Not supported node. {node}");
            }
            return base.Visit(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var result = base.VisitMember(node);

            if (_isFirstVisitMember)
            {
                EnsureParameterIsCorrect(node.Expression);
                _isFirstVisitMember = false;
            }
            ValueReader.AddMember(node.Member);

            return result;
        }

        private void EnsureParameterIsCorrect(Expression node)
        {
            if(((ParameterExpression)node)?.Name != _parameterName)
            {
                throw new Exception($"Incorrect parameter. Expr ${node}");
            }
        }
    }
}