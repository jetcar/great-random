using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GreatRandom
{
    public class MyThreadPool<T>
    {
        static IList<ThreadWrapper> myThreads = new List<ThreadWrapper>();
        static IDictionary<int, MyQueu<T>> queues = new Dictionary<int, MyQueu<T>>();
        private static int counter = 0;

        public static void Foreach(SortableObservableCollection<T> source, Action<T> action)
        {
            if (myThreads.Count == 0)
            {
                for (int i = 0; i < Environment.ProcessorCount-1; i++)
                {
                    var thread = new Thread(ProcessThreadQueue);
                    var wrapper = new ThreadWrapper() {Thread = thread, Id = counter++};
                    myThreads.Add(wrapper);
                    queues.Add(wrapper.Id, new MyQueu<T>());
                }
                foreach (var myThread in myThreads)
                {
                    myThread.Thread.Start(myThread);
                }
            }
            for (int i = 0; i < source.Count(); i++)
            {
                var thread = myThreads[i % (myThreads.Count - 1)];
                var queue = queues[thread.Id];
                queue.Add(action, source[i]);
                if (i == source.Count - 1)
                {
                    while (queues.Any(x => x.Value.HaveItem == true))
                    {
                        Thread.Sleep(1);
                    }
                }
            }
            //foreach (var myThread in myThreads)
            //{
            //    myThread.Thread.Abort();
            //}
            //myThreads.Clear();
        }

        private static void ProcessThreadQueue(object ob)
        {
            ThreadWrapper threadWrapper = (ThreadWrapper) ob;
            while (true)
            {
                var queue = queues[threadWrapper.Id];
                while (queue.HaveItem)
                {
                    var wrapper = queue.Get();
                    wrapper.action.Invoke(wrapper.value);
                }
            }
        }
    }

    internal class ThreadWrapper
    {
        public Thread Thread;
        public int Id;
    }
}
