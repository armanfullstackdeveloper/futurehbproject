using System;
using System.Collections.Generic;
using System.Threading;

namespace Boundary.Helper
{
    static class MyExtensions
    {
        public static class ThreadSafeRandom
        {
            [ThreadStatic]
            private static Random _local;

            public static Random ThisThreadsRandom => _local ?? (_local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId)));
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}