using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client.MessagePatterns;
using RabbitMQ.Client.Events;

namespace TechDays.Messaging.RabbitMQ
{
    public class Consumer : ConnectToRabbitMQ
    {
        // internal delegate to run the consuming queue on a seperate thread  
        private delegate void ConsumeDelegate();

        protected bool isConsuming;
        protected String QueueName;

        // used to pass messages back to UI for processing  
        public delegate void onReceiveMessage(byte[] message);
        public event onReceiveMessage onMessageReceived;

        protected Subscription mSubscription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="exchange"></param>
        /// <param name="exchangeType"></param>
        public Consumer(String server, String exchange, String exchangeType)
            : base(server, exchange, exchangeType)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartConsuming()
        {
            Model.BasicQos(0, 1, false);
            QueueName = Model.QueueDeclare();
            Model.QueueBind(QueueName, ExchangeName, "");
            isConsuming = true;
            ConsumeDelegate c = new ConsumeDelegate(Consume);
            c.BeginInvoke(null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Consume()
        {
            bool autoAck = false;
            //create a subscription  
            mSubscription = new Subscription(Model, QueueName, autoAck);
            while (isConsuming)
            {
                BasicDeliverEventArgs e = mSubscription.Next();
                byte[] body = e.Body;
                onMessageReceived(body);
                mSubscription.Ack(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            isConsuming = false;
            base.Dispose();
        }
    }
}
