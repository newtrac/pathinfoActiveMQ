using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace PathinfoActiveMQClient
{
    interface Client
    {
        Message SendAndWait(Message request);
        void WaitAndResponse(MessageHandler handler);
    }
}
