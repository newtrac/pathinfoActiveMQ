using PathinfoActiveMQClient.MessageImpl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PathinfoActiveMQClient.Impl
{
    class CaptureRequestHandler : MessageHandler
    {
        public Message Handle(Message request)
        {
            CameraCaptureResponse response = new CameraCaptureResponse();
            response.SetId(request.GetId());
            // TODO send image back
            return response;
        }
    }
}
