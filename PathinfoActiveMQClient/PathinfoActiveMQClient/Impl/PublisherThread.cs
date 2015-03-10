using Apache.NMS;
using PathinfoActiveMQClient.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace PathinfoActiveMQClient.Impl
{
    class PublisherThread
    {
        Logger logger = new Logger();
        private NMSConnectionFactory factory;
        private ConcurrentLinkedList<Message> messageSource;
        private String topic;
        private String user;
        private String password;

        public PublisherThread(NMSConnectionFactory factory, ConcurrentLinkedList<Message> messageSource, String topic, String user, String password)
        {
            this.factory = factory;
            this.messageSource = messageSource;
            this.topic = topic;
            this.user = user;
            this.password = password;
        }

        public void ThreadRun()
        {
            logger.Log("Publishing thread started.");
            while (true)
            {
                // Assume that outside PublisherThread messageSource's size can only
                // be increased. No remove actions outside PublisherThread.
                if (messageSource.Count > 0)
                {
                    // The first element is valid since messageSource only increase
                    // never decrease outside PublisherThread.
                    Message request = messageSource.RemoveFirst();

                    logger.Log("Start connection " + request.GetId() + " ...");
                    IConnection connection = factory.CreateConnection(user, password);
                    connection.Start();
                    ISession session = connection.CreateSession(AcknowledgementMode.AutoAcknowledge);
                    IDestination dest = session.GetTopic(topic);
                    IMessageProducer producer = session.CreateProducer(dest);
                    producer.DeliveryMode = MsgDeliveryMode.NonPersistent;
                    logger.Log("Send message " + request.GetId() + " on topic " + topic + "...");
                    producer.Send(request.ToMessage(session));
                    connection.Close();
                }
            }
        }
    }
}
