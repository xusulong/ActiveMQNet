using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;

namespace ActiveMQNet
{
    class Program
    {
        static IConnectionFactory _factory = null;
        static IConnection _connection = null;
        static ITextMessage _message = null;
        static void Main(string[] args)
        {
            _factory = new ConnectionFactory("tcp://127.0.0.1:61616/");
            try
            {
                using (_connection = _factory.CreateConnection())
                {
                    using (ISession session = _connection.CreateSession())
                    {
                        IDestination destination = new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("topic");
                        IMessageProducer producer = session.CreateProducer(destination);
                        Console.WriteLine("Sending: ");
                        _message = producer.CreateTextMessage("Hello ActiveMQ...");
                        //发送消息
                        producer.Send(_message,MsgDeliveryMode.NonPersistent,MsgPriority.Normal,TimeSpan.MinValue);
                        while (true)
                        {
                            var msg = Console.ReadLine();
                            _message = producer.CreateTextMessage(msg);
                            producer.Send(_message, MsgDeliveryMode.NonPersistent, MsgPriority.Normal, TimeSpan.MinValue);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            Console.ReadLine();
        }
    }
}
