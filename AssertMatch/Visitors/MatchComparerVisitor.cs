﻿using System;
using System.Linq.Expressions;

namespace AssertMatch.Visitors
{
    class MatchComparerVisitor<T> : ExpressionVisitor
    {
        private string _parameterName;
        private MatchComparer<T> _comparer;

        public static MatchComparer<T> BuildComparer(Expression<Func<T, bool>> expression)
        {
            var visitor = new MatchComparerVisitor<T>();
            visitor._parameterName = expression.Parameters[0].Name;
            visitor._comparer = new MatchComparer<T>(visitor._parameterName);
            visitor.Visit(expression);
            return visitor._comparer;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Equal)
            {
                var isActualOnLeft = IsParameterAccessExpression(node.Left);

                var valueReader = GetParameterValueReader(isActualOnLeft ?  node.Left : node.Right);

                var expectedValueExpression = isActualOnLeft ? node.Right : node.Left;
                var expectedValue = EvalualteExpression(expectedValueExpression);

                _comparer.RegisterItem(valueReader, new ExpectedValue(expectedValue, expectedValueExpression));
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