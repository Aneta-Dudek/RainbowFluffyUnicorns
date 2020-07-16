using System;
using System.Collections.Generic;

namespace Questore.MockingExamples
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<IEnumerable<T>> Iterate<T>(this IEnumerable<T> enumerable, Func<IEnumerable<T>, IEnumerable<T>> functor)
        {
            while (true)
            {
                yield return enumerable;
                enumerable = functor(enumerable);
            }
        }
    }
}
