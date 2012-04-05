using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace TechDays.Messaging.RabbitMQ
{
    public class Producer : ConnectToRabbitMQ  
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="exchange"></param>
        /// <param name="exchangeType"></param>
        public Producer(String server, String exchange, String exchangeType) 
            : base(server, exchange, exchangeType)
        {  
        }  

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SendMessage(byte[] message)  
        {  
            IBasicProperties basicProperties = Model.CreateBasicProperties();  
            basicProperties.SetPersistent(true);  
            Model.BasicPublish(ExchangeName, String.Empty, basicProperties, message);  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        public void SendMessage<T>(T message)
        {
            IBasicProperties basicProperties = Model.CreateBasicProperties();
            basicProperties.SetPersistent(true);  
            byte[] messageBody = message.ToByteArray();
            Model.BasicPublish(ExchangeName, String.Empty, basicProperties, messageBody);
        }
    }
}
