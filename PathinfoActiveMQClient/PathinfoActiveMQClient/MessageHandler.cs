using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PathinfoActiveMQClient
{
    interface MessageHandler
    {
        Message Handle(Message request);
    }
}
