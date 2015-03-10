using Apache.NMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PathinfoActiveMQClient.MessageImpl
{
    class MessageFactory
    {
        public static Message WrapMessage(IMessage message) {
            Message adaptor = null;
		if (CameraCaptureRequest.TYPE.Equals(message.NMSType)) {
			adaptor = new CameraCaptureRequest();
		} else if (CameraCaptureResponse.TYPE.Equals(message.NMSType)) {
			adaptor = new CameraCaptureResponse();
		} else {
			throw new NMSException("Unknown JMS type: " + message.NMSType);
		}
		
		adaptor.FromMessage(message);
		return adaptor;
	}
    }
}
