using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace TechDays.Messaging.RabbitMQ
{
    public abstract class ConnectToRabbitMQ : IDisposable
    {
        protected IModel Model { get; set; }
        protected IConnection Connection { get; set; }

        public String Server { get; set; }
        public String ExchangeName { get; set; }
        public String ExchangeTypeName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="server"></param>
        /// <param name="exchange"></param>
        /// <param name="exchangeType"></param>
        public ConnectToRabbitMQ(String server, String exchange, String exchangeType)
        {
            Server = server;
            ExchangeName = exchange;
            ExchangeTypeName = exchangeType;
        }
        
        /// <summary>
        /// Create the connection, Model and Exchange(if one is required)  
        /// </summary>
        /// <returns></returns>
        public virtual bool ConnectTo()
        {
            try
            {
                var connectionFactory = new ConnectionFactory();
                connectionFactory.HostName = Server;
                Connection = connectionFactory.CreateConnection();
                Model = Connection.CreateModel();
                bool durable = true;
                if (!String.IsNullOrEmpty(ExchangeName))
                    Model.ExchangeDeclare(ExchangeName, ExchangeTypeName, durable);
                return true;
            }
            catch (BrokerUnreachableException e)
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Disconnect()
        { 
            if (Connection != null) 
            {
                if (Connection.IsOpen) 
                { 
                    Connection.Close(); 
                }
                Connection.Dispose(); 
            }
            Connection = null;

            if (Model != null)
                Model.Abort();
            Model = null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Disconnect();
        }
    }
}
