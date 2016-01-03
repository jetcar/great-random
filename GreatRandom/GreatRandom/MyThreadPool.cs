using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GreatRandom
{
    public class MyThreadPool<T>
    {
        static IList<ThreadWrapper> myThreads = new List<ThreadWrapper>();
        static IDictionary<int, ConcurrentQueue<MyActionWrapper<T>>> queues = new Dictionary<int, ConcurrentQueue<MyActionWrapper<T>>>();
        private static int counter = 0;
        private static int _processedCount;


        public static void Foreach(SortableObservableCollection<T> source, Action<T> action)
        {
            if (myThreads.Count == 0)
            {
                for (int i = 0; i < Environment.ProcessorCount - 1; i++)
                {
                    var thread = new Thread(ProcessThreadQueue);
                    var wrapper = new ThreadWrapper() { Thread = thread, Id = counter++ };
                    myThreads.Add(wrapper);
                    queues.Add(wrapper.Id, new ConcurrentQueue<MyActionWrapper<T>>());
                }
                foreach (var myThread in myThreads)
                {
                    myThread.Thread.Start(myThread);
                }
            }
            Debug.Assert(_processedCount == 0);
            for (int i = 0; i < source.Count(); i++)
            {
                Interlocked.Increment(ref _processedCount);
                var index = i % (myThreads.Count);
                var thread = myThreads[index];
                var queue = queues[thread.Id];
                queue.Enqueue(new MyActionWrapper<T>(action, source[i]));
            }
            while (_processedCount > 0)
            {
                Thread.Sleep(1);
            }
            //foreach (var myThread in myThreads)
            //{
            //    myThread.Thread.Abort();
            //}
            //myThreads.Clear();
        }

        private static void ProcessThreadQueue(object ob)
        {
            ThreadWrapper threadWrapper = (ThreadWrapper)ob;
            while (true)
            {
                var queue = queues[threadWrapper.Id];
                while (!queue.IsEmpty)
                {
                    MyActionWrapper<T> wrapper;
                    while (!queue.TryDequeue(out wrapper))
                    {
                        Thread.Sleep(1);
                    }
                    wrapper.action.Invoke(wrapper.value);
                    Interlocked.Decrement(ref _processedCount);
                }
                Thread.Sleep(1);
            }
        }
    }

    internal class ThreadWrapper
    {
        public Thread Thread;
        public int Id;
    }
}
