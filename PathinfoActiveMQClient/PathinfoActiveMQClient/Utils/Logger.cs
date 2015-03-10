using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PathinfoActiveMQClient.Utils
{
    class Logger
    {
        public void Log(String message)
        {
            Console.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + ": " + message);
        }
    }
}
