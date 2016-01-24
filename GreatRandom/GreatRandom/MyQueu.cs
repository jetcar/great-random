using System;
using System.Collections.Concurrent;
using System.Threading;

namespace GreatRandom
{
    internal class MyQueu<T>
    {
        ConcurrentQueue<MyActionWrapper<T>> internalQueue = new ConcurrentQueue<MyActionWrapper<T>>();
        object locker = new object();

        public bool HaveItem
        {
            get
            {
                lock (locker)
                {
                    return internalQueue.Count > 0;
                }
            }
        }

        public MyActionWrapper<T> Get()
        {
            lock (locker)
            {
                MyActionWrapper<T> result;
                while (!internalQueue.TryDequeue(out result))
                {
                    Thread.Sleep(1);
                }
                
                return result;
            }
        }




        public void Add(object action, object o)
        {
            lock (locker)
            {
                internalQueue.Enqueue(new MyActionWrapper<T>((Action<T>) action,o));
            }
        }
    }
}