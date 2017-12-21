using Apache.NMS;
using Apache.NMS.ActiveMQ;
using System;

namespace ActiveMQNetCustomer
{
    class Program
    {
        static IConnectionFactory _factory = null;
        static void Main(string[] args)
        {
            try
            {
                _factory = new ConnectionFactory("tcp://127.0.0.1:61616/");
                using (IConnection conn = _factory.CreateConnection())
                {
                    conn.Start();
                    using (ISession session = conn.CreateSession())
                    {
                        var topic = new Apache.NMS.ActiveMQ.Commands.ActiveMQTopic("topic");
                        IMessageConsumer consumer = session.CreateDurableConsumer(topic,"Customer",null,false);
                        consumer.Listener += new MessageListener(consumer_Listener);
                        //没有这句话，Session会话就会被关闭
                        Console.Read();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        static void consumer_Listener(IMessage message)
        {
            ITextMessage msg = (ITextMessage)message;
            Console.WriteLine("Receive: "+msg.Text);
        }
    }
}
