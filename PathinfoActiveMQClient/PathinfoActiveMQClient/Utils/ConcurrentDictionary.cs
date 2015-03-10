using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PathinfoActiveMQClient.Utils
{
    class ConcurrentDictionary<A, B>
    {
        private Dictionary<A, B> dictionary = new Dictionary<A, B>();

        public B Remove(A key)
        {
            B value = default(B);
            Monitor.Enter(dictionary);
            if (dictionary.ContainsKey(key))
            {
                value = dictionary[key];
                dictionary.Remove(key);
            }
            Monitor.Exit(dictionary);
            return value;
        }

        public int Count
        {
            get { return dictionary.Count; }
        }

        public KeyValuePair<A, B> PopNext()
        {
            KeyValuePair<A, B> target = default(KeyValuePair<A, B>);
            if (dictionary.Count == 0)
            {
                return target;
            }
            Monitor.Enter(dictionary);
            foreach (KeyValuePair<A, B> pair in dictionary)
            {
                target = pair;
                break;
            }
            dictionary.Remove(target.Key);
            Monitor.Exit(dictionary);
            return target;
        }

        public void Put(A key, B value)
        {

            Monitor.Enter(dictionary);
            dictionary.Add(key, value);
            Monitor.Exit(dictionary);
        }
    }
}
