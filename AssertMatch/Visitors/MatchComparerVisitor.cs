using System;
using System.Linq.Expressions;

namespace AssertMatch.Visitors
{
    class MatchComparerVisitor<T> : ExpressionVisitor
    {
        private string _parameterName;
        private MatchComparer<T> _comparer;

        public MatchComparer<T> BuildComparer(Expression<Func<T, bool>> expression)
        {
            _parameterName = expression.Parameters[0].Name;
            _comparer = new MatchComparer<T>(_parameterName);
            this.Visit(expression);
            return _comparer;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Equal)
            {
                var isActualOnLeft = IsParameterAccessExpression(node.Left);

                var valueReader = GetParameterValueReader(isActualOnLeft ?  node.Left : node.Right);
                var expectedValue = EvalualteExpression(isActualOnLeft ? node.Right : node.Left);
                
                _comparer.RegisterExpectedValue(valueReader, expectedValue);
                return node;
            }

            if (node.NodeType == ExpressionType.AndAlso)
            {
                return base.VisitBinary(node);
            }

            throw new Exception($"Not supported binary operation {node.NodeType}.\n Expression: ${node}");
        }

        private ValueReader<T> GetParameterValueReader(Expression node)
        {
            var memberAccessVisitor = new ValueReaderVisitor<T>(_parameterName);
            memberAccessVisitor.Visit(node);
            var valueReader = memberAccessVisitor.ValueReader;
            return valueReader;
        }

        private bool IsParameterAccessExpression(Expression expression)
        {
            var visitor = new IsParameterAccessExpressionVisitor(_parameterName);
            visitor.Visit(expression);
            return visitor.Result;
        }

        private object EvalualteExpression(Expression expression)
        {
            return Expression.Lambda(expression).Compile().DynamicInvoke();
        }
    }
}