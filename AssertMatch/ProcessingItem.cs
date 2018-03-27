using System.Collections.Generic;
using System.Linq;

namespace AssertMatch
{
    class ProcessingItem
    {
        public static ProcessingItem<T> Create<T>(T value) => new ProcessingItem<T> { Value = value };
    }

    class ProcessingItem<T>
    {
        public T Value { get; set; }

        public bool IsProcessed { get; set; }

        public void MarkAsProcessed() => IsProcessed = true;
    }

    static class ProcessingItemExtensions
    {
        public static IEnumerable<ProcessingItem<T>> NotProcessed<T>(this IEnumerable<ProcessingItem<T>> source)
        {
            return source.Where(x => !x.IsProcessed);
        }

        public static bool IsAllProcessed<T>(this IEnumerable<ProcessingItem<T>> source)
        {
            if(source == null)
            {
                return false;
            }
            return source.All(x => x.IsProcessed);
        }
    }
}