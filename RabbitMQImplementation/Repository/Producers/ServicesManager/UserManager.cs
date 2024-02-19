using RabbitMQ.Client;
using Newtonsoft.Json;
using RabbitMQImplementation.Models;
using System.Text;
using RabbitMQ.Client.Events;
using RabbitMQImplementation.Repository.Producers.IServices;

namespace RabbitMQImplementation.Repository.Producers.ServicesManager
{
    public class UserManager : IUserService
    {
        public readonly ILogger<UserManager> _logger;

        public UserManager(ILogger<UserManager> logger)
        {
            _logger = logger;
        }

        //bağlantı nesnesi, queues, exchange, channel
        IConnection connection;

        private readonly string userRegister = "user_register_queue";
        private readonly string userRegisterExchange = "user_register_exchange";

        static IModel _channel;

        //channel yoksa oluştur.
        IModel channel => _channel ?? (_channel = GetChannel());


        //controller methodu
        public void register(UserModel user)
        {
            //burada register işlemleri olacak...


            //bağlantı kurduk rabbitmq ya
            connection = GetConnection();

            //exchange oluştur.
            channel.ExchangeDeclare(userRegisterExchange, "direct");

            //queues oluştur. oluşturulan queue ile exchange yi bağladık.
            channel.QueueDeclare(userRegister, false, false, false);
            channel.QueueBind(userRegister, userRegisterExchange, userRegister);

            //queue ya yazdır.
            WriteToQueue(userRegister, user);
        }

        //rabbitmq bağlanmak için fonksiyonu
        private IConnection GetConnection()
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            return connectionFactory.CreateConnection();
        }

        //kuyruğa yazdır.
        private void WriteToQueue(string queueName, UserModel model)
        {
            var messageArr = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(model));
            channel.BasicPublish(userRegisterExchange, queueName, null, messageArr);

            _logger.LogInformation("Message Published");
        }
        //kanal oluşturma fonksiyonu.
        private IModel GetChannel()
        {
            return connection.CreateModel();
        }
    }
}
