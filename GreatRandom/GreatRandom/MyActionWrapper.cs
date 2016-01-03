using System;

namespace GreatRandom
{
    public class MyActionWrapper<T>
    {
        public Action<T> action;
        public T value;
        public MyActionWrapper(Action<T> action, object value)
        {
            this.action = action;
            this.value = (T) value;
        }

    }
}