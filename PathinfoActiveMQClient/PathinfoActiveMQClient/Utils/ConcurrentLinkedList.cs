using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PathinfoActiveMQClient.Utils
{
    class ConcurrentLinkedList<T>
    {
        private LinkedList<T> list;

        public ConcurrentLinkedList() 
        {
            list = new LinkedList<T>();
        }

        public void AddLast(T value)
        {
            Monitor.Enter(list);
            list.AddLast(value);
            Monitor.Exit(list);
        }

        public T RemoveFirst()
        {
            Monitor.Enter(list);
            T value = list.First();
            list.RemoveFirst();
            Monitor.Exit(list);
            return value;
        }

        public void Clear()
        {
            Monitor.Enter(list);
            list.Clear();
            Monitor.Exit(list);
        }

        public int Count
        {
            get { return list.Count; }
        }
    }
}
