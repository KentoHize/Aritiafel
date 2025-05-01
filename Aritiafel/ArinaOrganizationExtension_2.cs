using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Aritiafel
{
    public static partial class ArinaOrganizationExtension
    {

        public static void ParallelForEach<TSource, TLocal>(this OrderablePartitioner<TSource> p, ParallelOptions options, Func<TLocal> initFunction, Func<TSource, ParallelLoopState, long, TLocal, TLocal> bodyFunction, Action<TLocal> finalFunction)
        {
            Parallel.ForEach(p, options, initFunction, bodyFunction, finalFunction);
        }

        public static void ParallelForEach<TSource, TLocal>(this Partitioner<TSource> p, ParallelOptions options, Func<TLocal> initFunction, Func<TSource, ParallelLoopState, TLocal, TLocal> bodyFunction, Action<TLocal> finalFunction)
        {
            Parallel.ForEach(p, options, initFunction, bodyFunction, finalFunction);
        }

        public static void ParallelForEach<T>(this IEnumerable<T> e, ParallelOptions options, Action<T> action)
        {
            Parallel.ForEach(e, options, action);
        }

        public static void ParallelForEach<T>(this IEnumerable<T> e, ParallelOptions options, Action<T, ParallelLoopState> action)
        {
            Parallel.ForEach(e, options, action);
        }

        public static void ParallelForEach<T>(this IEnumerable<T> e, ParallelOptions options, Action<T, ParallelLoopState, long> action)
        {
            Parallel.ForEach(e, options, action);
        }

        public static void ParallelForEach<T>(this OrderablePartitioner<T> e, ParallelOptions options, Action<T, ParallelLoopState, long> action)
        {
            Parallel.ForEach(e, options, action);
        }

        public static void ParallelForEach<T>(this Partitioner<T> e, ParallelOptions options, Action<T> action)
        {
            Parallel.ForEach(e, options, action);
        }

        public static void ParallelForEach<T>(this Partitioner<T> e, ParallelOptions options, Action<T, ParallelLoopState> action)
        {
            Parallel.ForEach(e, options, action);
        }

        public static void ParallelForEach<T>(this IEnumerable<T> e, Action<T, ParallelLoopState, int> action)
            => e.ParallelForEach(new ParallelOptions(), (m, pls, i) => action(m, pls, (int)i));
        public static void ParallelForEach<T>(this IEnumerable<T> e, Action<T, ParallelLoopState, long> action)
            => e.ParallelForEach(new ParallelOptions(), action);
        public static void ParallelForEach<T>(this IEnumerable<T> e, Action<T, ParallelLoopState> action)
            => e.ParallelForEach(new ParallelOptions(), action);
        public static void ParallelForEach<T>(this IEnumerable<T> e, Action<T> action)
            => e.ParallelForEach(new ParallelOptions(), action);


        public static void ParallelFor(this IEnumerable e, long startIndex, long endIndex, ParallelOptions options, Action<long, ParallelLoopState> action)
        {
            Parallel.For(startIndex, endIndex, options, action);
        }
        public static void ParallelFor(this IEnumerable e, long startIndex, long endIndex, ParallelOptions options, Action<long> action)
        {
            Parallel.For(startIndex, endIndex, options, action);
        }
        public static void ParallelFor(this IEnumerable e, int startIndex, int endIndex, ParallelOptions options, Action<int> action)
        {
            Parallel.For(startIndex, endIndex, options, action);
        }

        public static void ParallelFor(this ICollection e, Action<int> action)
            => e.ParallelFor(0, e.Count, new ParallelOptions(), action);
        public static void ParallelFor(this ICollection e, Action<long> action)
            => e.ParallelFor(0, e.Count, new ParallelOptions(), action);
        public static void ParallelFor(this ICollection e, Action<long, ParallelLoopState> action)
            => e.ParallelFor(0, e.Count, new ParallelOptions(), action);
        public static void ParallelFor(this ICollection e, long startIndex, long endIndex, ParallelOptions options, Action<long, ParallelLoopState> action)
            => e.ParallelFor(startIndex, endIndex, options, action);
        public static void ParallelFor(this IEnumerable e, long endIndex, ParallelOptions options, Action<long, ParallelLoopState> action)
            => e.ParallelFor(0, endIndex, options, action);
        public static void ParallelFor(this IEnumerable e, long endIndex, Action<long, ParallelLoopState> action)
            => e.ParallelFor(0, endIndex, new ParallelOptions(), action);

        //public static void ParallelFor(this Array e, Action<long, ParallelLoopState> action)
        //    => e.ParallelFor(0, e.Count, null, action);


        //public static void ParallelFor( Action<int, ParallelLoopState> action)
        //{

        //}            

        //public static void ParallelFor(int startIndex, int endIndex, ParallelOptions options, Action<int, ParallelLoopState> action)
        //{   
        //    Parallel.For(startIndex, endIndex, options, action);
        //}

        //public static void ParallelFor<T>(long startIndex, long endIndex, ParallelOptions options, Func<T> initFunction, Func<long, ParallelLoopState, T, T> bodyFunction, Action<T> finalFunction)
        //{
        //    Parallel.For(startIndex, endIndex, options, initFunction, bodyFunction, finalFunction);
        //}
    }
}
