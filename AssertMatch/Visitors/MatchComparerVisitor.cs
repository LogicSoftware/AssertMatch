using System;
using System.Linq.Expressions;

namespace AssertMatch.Visitors
{
    class MatchComparerVisitor<T> : ExpressionVisitor
    {
        private readonly string _parameterName;
        public MatchComparer<T> Comparer { get; }

        public MatchComparerVisitor(string parameterName)
        {
            _parameterName = parameterName;
            Comparer = new MatchComparer<T>();
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Equal)
            {
                var isActualOnLeft = IsParameterAccessExpression(node.Left);

                var valueReader = GetParameterValueReader(isActualOnLeft ?  node.Left : node.Right);
                var expectedValue = EvalualteExpression(isActualOnLeft ? node.Right : node.Left);
                
                Comparer.RegisterExpectedValue(valueReader, expectedValue);
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