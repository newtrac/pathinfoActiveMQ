using Apache.NMS;
using PathinfoActiveMQClient.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PathinfoActiveMQClient.MessageImpl
{
    class CameraCaptureResponse : BaseMessage
    {
        public static String TYPE = "2";

        public override IMessage ToMessage(ISession session)
        {
            // TODO
            IMessage message = session.CreateMessage();
            SetMessageProperties(message);
            message.NMSType = TYPE;
            return message;
        }


        public override void FromMessage(IMessage message)
        {
            // TODO
            this.SetId(message.Properties.GetString(Strings.ID));

        }
    }
}
