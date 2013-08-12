#region Written by László Dobos (dobos@complex.elte.hu)
/*
 * 
 * VoService.Spectrum.Lib classes are designed for processing
 * astonomical spectra
 * 
 * See bottom of file for revision history
 * 
 * Current revision:
 *   ID:          $Id: ParallelExecuter.cs,v 1.1 2008/01/08 21:37:05 dobos Exp $
 *   Revision:    $Revision: 1.1 $
 *   Date:        $Date: 2008/01/08 21:37:05 $
 */
#endregion
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace VoServices.SpecSvc.Lib
{
    public class ParallelExecuter<T, P, R>
    {
        public delegate R Worker(T t, P p);
        public delegate R Accumulator(R r, T t, P p);
        public delegate R Merger(R r1, R r2, P p);

        private IEnumerable<T> inQueue;
        private Queue<R> outQueue;
        private int maxQueueSize;
        private int poolSize;
        private P parameters;
        private Worker worker;
        private Accumulator accumulator;
        private Merger merger;

        private Thread[] threadPool;
        private int exitCount;
        private EventWaitHandle[] waitHandles;

        private class ExecutionState
        {
            public EventWaitHandle waitHandle;
            public IEnumerator<T> items;

            public ExecutionState(EventWaitHandle waitHandle, IEnumerator<T> items)
            {
                this.waitHandle = waitHandle;
                this.items = items;
            }
        }

        public IEnumerable<T> InQueue
        {
            set { inQueue = value; }
        }

        public int MaxQueueSize
        {
            get { return maxQueueSize; }
            set { maxQueueSize = value; }
        }

        public int PoolSize
        {
            get { return poolSize; }
            set { poolSize = value; }
        }

        public P Parameters
        {
            get { return parameters; }
            set { parameters = value; }
        }

        public Worker WorkerFunction
        {
            set { this.worker = value; }
        }

        public Accumulator AccumulatorFunction
        {
            set { accumulator = value; }
        }

        public Merger MergerFunction
        {
            set { merger = value; }
        }

        public ParallelExecuter()
        {
            poolSize = Environment.ProcessorCount;
            maxQueueSize = 100;
        }


        private void StartWorkerThreads(ParameterizedThreadStart dotask)
        {
            // Initialize output queue
            outQueue = new Queue<R>(maxQueueSize);

            // Create a set of EventWaitHangles
            // The started thread will wait for all these Events to be set before exiting
            waitHandles = new EventWaitHandle[poolSize];
            threadPool = new Thread[poolSize];
            exitCount = 0;

            IEnumerator<T> items = inQueue.GetEnumerator();

            for (int i = 0; i < poolSize; i++)
            {
                waitHandles[i] = new EventWaitHandle(true, EventResetMode.AutoReset);
                waitHandles[i].Reset();
            }

            for (int i = 0; i < poolSize; i++)
            {
                ExecutionState s = new ExecutionState(waitHandles[i], items);

                threadPool[i] = new Thread(new ParameterizedThreadStart(dotask));
                threadPool[i].Name = "Worker thread #" + i.ToString();
                threadPool[i].IsBackground = true;
                threadPool[i].CurrentCulture = Thread.CurrentThread.CurrentCulture;
                threadPool[i].Start(s);
            }
        }

        public IEnumerable<R> RunParallelizer()
        {
            StartWorkerThreads(new ParameterizedThreadStart(DoTaskParallelizer));

            while (true)
            {
                // wait for any of the events to be signaled
                WaitHandle.WaitAny(waitHandles);

                while (true)
                {
                    R res;
                    lock (outQueue)
                    {
                        if (outQueue.Count > 0)
                            res = outQueue.Dequeue();
                        else
                            break;
                    }

                    yield return res;
                }

                if (exitCount == poolSize) break;
            }

            // flush output queue
            while (outQueue.Count > 0)
                yield return outQueue.Dequeue();
        }

        private void DoTaskParallelizer(object state)
        {
            ExecutionState s = (ExecutionState)state;
            T item;

            // Read next item from the input queue

            while (true)
            {
                lock (s.items)
                {
                    if (s.items.MoveNext())
                        item = s.items.Current;
                    else break;
                }

                //Console.WriteLine(Thread.CurrentThread.Name + " start");

                // process item and enque results in the output queue
                R res = worker(item, parameters);

                //Console.WriteLine(Thread.CurrentThread.Name + " end");

                while (true)
                {
                    lock (outQueue)
                    {
                        if (outQueue.Count < maxQueueSize)
                        {
                            outQueue.Enqueue(res);
                            break;
                        }
                    }
                    Thread.Sleep(100);
                }

                // signal event
                s.waitHandle.Set();
            }


            // no more items
            lock (outQueue)
            {
                exitCount++;
            }

            s.waitHandle.Set();
        }

        public R RunAccumulator()
        {
            StartWorkerThreads(new ParameterizedThreadStart(DoTaskAccumulator));

            // wait for any of the events to be signaled
            EventWaitHandle.WaitAll(waitHandles);

            R res = default(R);

            while (outQueue.Count > 0)
            {
                R r = outQueue.Dequeue();
                res = merger(res, r, parameters);
            }

            return res;
        }

        private void DoTaskAccumulator(object state)
        {
            ExecutionState s = (ExecutionState)state;
            T item;

            R res = default(R);

            // Read next item from the input queue
            while (true)
            {
                lock (s.items)
                {
                    if (s.items.MoveNext())
                        item = s.items.Current;
                    else break;
                }

                // process item and enque results in the output queue
                res = accumulator(res, item, parameters);
            }

            // no more items
            lock (outQueue)
            {
                outQueue.Enqueue(res);
                exitCount++;
            }

            s.waitHandle.Set();
        }

    }
}
#region Revision History
/* Revision History

        $Log: ParallelExecuter.cs,v $
        Revision 1.1  2008/01/08 21:37:05  dobos
        Initial checkin


*/
#endregion