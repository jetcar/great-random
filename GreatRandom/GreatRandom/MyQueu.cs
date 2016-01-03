using System;
using System.Collections.Generic;

namespace GreatRandom
{
    internal class MyQueu<T>
    {
        Queue<MyActionWrapper<T>> internalQueue = new Queue<MyActionWrapper<T>>();
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
                return internalQueue.Dequeue();
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