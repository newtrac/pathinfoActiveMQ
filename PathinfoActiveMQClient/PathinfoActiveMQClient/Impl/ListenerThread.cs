using Apache.NMS;
using PathinfoActiveMQClient.MessageImpl;
using PathinfoActiveMQClient.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace PathinfoActiveMQClient.Impl
{
    class ListenerThread
    {
        private Logger logger = new Logger();
        private NMSConnectionFactory factory;
        private ConcurrentDictionary<String, Message> messageResponse;
        private String topic;
        private String user;
        private String password;

        public ListenerThread(NMSConnectionFactory factory, ConcurrentDictionary<String, Message> messageResponse, String topic, String user, String password)
        {

            this.factory = factory;
            this.messageResponse = messageResponse;
            this.topic = topic;
            this.user = user;
            this.password = password;
        }
        public void ThreadRun()
        {
            logger.Log("Listening thread started.");

            IConnection connection = factory.CreateConnection(user, password);
            connection.Start();
            ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
            IDestination dest = session.GetTopic(topic);

            IMessageConsumer consumer = session.CreateConsumer(dest);

            logger.Log("Waiting for messages...");
            while (true)
            {
                IMessage message = null;
                String id = null;
                try
                {
                    message = consumer.Receive();
                    id = message.Properties.GetString(Strings.ID);
                } 
                catch{
                    MessageBox.Show("Error receiving message in consumer");
                }
                
                if (id == null)
                {
                    logger.Log("Property ID is null. Message is " + message);
                }
                else
                {
                    messageResponse
                            .Put(id, MessageFactory.WrapMessage(message));
                    logger.Log("Message " + message.Properties.GetString(Strings.ID) + " received on topic " + topic + ".");
                }
            }

        }
    }
}
