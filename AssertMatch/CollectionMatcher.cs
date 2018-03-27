using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AssertMatch.Visitors;

namespace AssertMatch
{
    public class CollectionMatcher<T>
    {
        private readonly List<T> _actual;

        public CollectionMatcher(IEnumerable<T> actual)
        {
            _actual = actual?.ToList();
        }

        internal bool IsEquivalentTo(params Expression<Func<T, bool>>[] expectedItems)
        {
            if(_actual == null)
            {
                return false;
            }

            var comparers = expectedItems.Select(MatchComparerVisitor<T>.BuildComparer).ToList();
            var leftActualItems = new List<T>(_actual);
            foreach (var comparer in comparers)
            {
                var index = leftActualItems.FindIndex(comparer.IsMatch);
                if(index == -1)
                {
                    return false;
                }
                leftActualItems.RemoveAt(index);
            }

            return leftActualItems.Count == 0;
        }

        public void EquivalentTo(params Expression<Func<T, bool>>[] expectedItems)
        {
            
        }
    }
}