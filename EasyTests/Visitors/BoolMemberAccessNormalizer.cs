using System;
using System.Linq.Expressions;

namespace EasyTests.Visitors
{
    public class BoolMemberAccessNormalizer<T> : ExpressionVisitor
    {
        private readonly string _parameterName;
        private static readonly Type _boolType = typeof(bool);
        private Expression lastNotExpressionTarget = null;
        private Expression lastEqualLeftExp = null;
        private Expression lastEqualRightExp = null;
        
        public BoolMemberAccessNormalizer(string parameterName)
        {
            _parameterName = parameterName;
        }

        public static Expression Normalize(Expression<Func<T, bool>> expression)
        {
            return new BoolMemberAccessNormalizer<T>(expression.Parameters[0].Name).Visit(expression);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.Equal)
            {
                lastEqualLeftExp = node.Left;
                lastEqualRightExp = node.Right;
            }
            return base.VisitBinary(node);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Not)
            {
                lastNotExpressionTarget = node.Operand;
            }
            return base.VisitUnary(node);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (IsParameterAccessExpression(node) && IsBoolProperty(node) && IsNotInsideEqual(node))
            {
                var expectedValues = node == lastNotExpressionTarget ? false : true;
                return Expression.MakeBinary(ExpressionType.Equal, node, Expression.Constant(expectedValues));
            }
            return base.VisitMember(node);
        }

        private bool IsNotInsideEqual(MemberExpression node)
        {
            return node != lastEqualLeftExp && node != lastEqualRightExp;
        }

        private bool IsBoolProperty(MemberExpression node)
        {
            return MemberAccessTypeReader.Read(node) == _boolType;
        }

        private bool IsParameterAccessExpression(Expression expression)
        {
            var visitor = new IsParameterAccessExpressionVisitor(_parameterName);
            visitor.Visit(expression);
            return visitor.Result;
        }
    }
}