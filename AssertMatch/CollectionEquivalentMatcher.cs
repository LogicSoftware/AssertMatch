using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using AssertMatch.Visitors;

namespace AssertMatch
{
    public class CollectionEquivalentMatcher<T>
    {
        private readonly List<T> _actual;

        public CollectionEquivalentMatcher(IEnumerable<T> actual)
        {
            _actual = actual?.ToList();
        }

        internal bool IsEquivalentTo(params Expression<Func<T, bool>>[] expectedItems)
        {
            var result = GetResult(expectedItems);
            return result.AreEquivalent;
        }

        internal string GetFailMessage(params Expression<Func<T, bool>>[] expectedItems)
        {
            var result = GetResult(expectedItems);
            var msg = new StringBuilder("\r\n");

            if (_actual == null)
            {
                msg.AppendLine("Actual collection is NULL.");
                return msg.ToString();
            }

            msg.AppendLine("Actual collection is not equivalent to specifed items.");
            msg.AppendLine("Expected items:");
            foreach (var item in result.ExpectedItems)
            {
                var resSign = item.IsProcessed ? Constants.PassSign : Constants.FailSign;
                msg.AppendLine($"    {resSign} {item.Value.FormatExpectedObj()}");
            }

            //TODO: 
            msg.AppendLine("");
            msg.AppendLine("Actual items:");
            var valueReader = result.ExpectedItems.First().Value.ActualValueReaders.First();
            foreach (var item in result.ActualItems)
            {
                var resSign = item.IsProcessed ? Constants.PassSign : Constants.FailSign;
                var actualObj = $"{{ {valueReader.GetMemberName()} = {Helper.FormatValue(valueReader.GetValue(item.Value, out var _))} }}";
                msg.AppendLine($"    {resSign} {actualObj}");
            }

            return msg.ToString();
        }

        private Result GetResult(params Expression<Func<T, bool>>[] expectedItems)
        {
            var expected = expectedItems.Select(MatchComparerVisitor<T>.BuildComparer)
                                         .Select(ProcessingItem.Create)
                                         .ToList();

            var actualItems = _actual?.Select(ProcessingItem.Create).ToList();
            var result = new Result
            {
                ActualItems = actualItems,
                ExpectedItems = expected
            };

            if (result.ActualItems == null)
            {
                return result;
            }

            foreach (var expectedItem in result.ExpectedItems)
            {
                var actual = result.ActualItems.NotProcessed()
                                   .FirstOrDefault(x => expectedItem.Value.IsMatch(x.Value));

                if(actual != null)
                {
                    actual.MarkAsProcessed();
                    expectedItem.MarkAsProcessed();
                }
            }

            return result;
        }

        public void EquivalentTo(params Expression<Func<T, bool>>[] expectedItems)
        {
            if (IsEquivalentTo(expectedItems))
            {
                TestFramework.Ok();
            }
            else
            {
                var msg = GetFailMessage(expectedItems);
                TestFramework.Fail(msg);
            }
        }

        class Result
        {
            public List<ProcessingItem<T>> ActualItems { get; set; }

            public List<ProcessingItem<MatchComparer<T>>> ExpectedItems { get; set; }

            public bool AreEquivalent => ActualItems.IsAllProcessed() && ExpectedItems.IsAllProcessed();
        }
    }
}