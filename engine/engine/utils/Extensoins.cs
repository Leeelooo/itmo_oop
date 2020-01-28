using System;
using System.Collections.Generic;

namespace engine.utils
{
    public static class Extensoins
    {
        public static void OnEach<T>(this IEnumerable<T> enumerable, Action<T> block)
        {
            foreach (var element in enumerable)
                block(element);
        }
    }
}
