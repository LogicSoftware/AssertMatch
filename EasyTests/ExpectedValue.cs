﻿using System;
using System.Linq.Expressions;
using EasyTests.Visitors;

namespace EasyTests
{
    class ExpectedValue
    {
        public object Value { get; }
        public Expression Expression { get; }

        public ExpectedValue(object value, Expression expression)
        {
            Value = value;
            Expression = expression;
        }

        public object GetFormattedValue()
        {
            var closureName = ExpectedValueNameReader.GetName(Expression);
            var value = Helper.FormatValue(Value);

            return string.IsNullOrEmpty(closureName) ?
                                value :
                                $"{value} ({closureName})";
        }
    }
}