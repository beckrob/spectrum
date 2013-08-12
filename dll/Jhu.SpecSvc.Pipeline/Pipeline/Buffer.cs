using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Jhu.SpecSvc.Pipeline
{
    public static class ParallelQueryExtensions
    {
        public static IEnumerable<T> Buffer<T>(this IEnumerable<T> source, int boundedCapacity)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (boundedCapacity < 1)
            {
                throw new ArgumentOutOfRangeException("boundedCapacity");
            }

            BlockingCollection<T> buffer = new BlockingCollection<T>(boundedCapacity);

            // start worker thread
            Task t = Task.Factory.StartNew(() => BufferWorker(source, buffer));

            return buffer.GetConsumingEnumerable();
        }

        private static void BufferWorker<T>(IEnumerable<T> source, BlockingCollection<T> buffer)
        {
            foreach (T item in source)
            {
                buffer.Add(item);
            }

            buffer.CompleteAdding();
        }
    }
}
