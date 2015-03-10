using Apache.NMS;
using PathinfoActiveMQClient.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PathinfoActiveMQClient.MessageImpl
{
    abstract class BaseMessage : Message
    {
        protected Dictionary<String, String> properties = new Dictionary<String, String>();

        protected String GetProperty(String key)
        {
            return properties[key];
        }

        protected void SetPropertiy(String key, String value)
        {
            properties.Add(key, value);
        }

        public void SetId(string id)
        {
            properties.Add(Strings.ID, id);
        }

        public string GetId()
        {
            if (properties.ContainsKey(Strings.ID))
            {
                return properties[Strings.ID];
            }
            else
            {
                return String.Empty;
            }
        }
        protected void SetMessageProperties(IMessage message)
        {
            foreach (KeyValuePair<String, String> entry in properties)
            {
                message.Properties.SetString(entry.Key, entry.Value);
            }
            message.NMSMessageId = GetId();
        }
        public abstract IMessage ToMessage(ISession session);

        public abstract void FromMessage(IMessage message);
    }
}
