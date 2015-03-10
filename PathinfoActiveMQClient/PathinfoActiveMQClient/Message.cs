using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PathinfoActiveMQClient
{
    interface Message
    {
        void SetId(String id);

        String GetId();

        IMessage ToMessage(ISession session);

        void FromMessage(IMessage message);
    }
}
