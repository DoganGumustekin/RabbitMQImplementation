using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQImplementation.Models;
using RabbitMQImplementation.Repository.Producers.ServicesManager;
using System.Text;
using System.Threading.Channels;

namespace RabbitMQImplementation.Repository.Consumers
{
    public class UserWorker : BackgroundService
    {
        public readonly ILogger<UserManager> _logger;
        //bağlantı nesnesi, queues, exchange, channel
        static IConnection connection;

        private static readonly string userRegister = "user_register_queue";

        static IModel? _channel;
        static IModel channel => _channel ?? (_channel = GetChannel());
        public UserWorker(ILogger<UserManager> logger)
        {
            _logger = logger;
            connection = GetConnection();
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                //_logger.LogInformation("Worker is running");
                
                await Task.Delay(5000);
                //consumer oluştur.
                var consumerEvent = new EventingBasicConsumer(channel);

                //queue ya bir şey geldiği zaman bu method tetiklenir(Received).
                //burada kuyruğa mesaj geldiğinde istediğimiz işlemleri yapabileceğiz. ch=kanal, ea=event argument
                consumerEvent.Received += (ch, ea) =>
                {
                    var modelJson = Encoding.UTF8.GetString(ea.Body.ToArray());
                    var model = JsonConvert.DeserializeObject<UserModel>(modelJson);
                    Console.WriteLine("Gelen Data: " + modelJson);
                    SendMail(model.UserID,model.UserName);
                };

                //Console.WriteLine($"{userRegister} listening...");

                
                //consume et
                channel.BasicConsume(userRegister, true, consumerEvent);

                
            }
        }


        private async Task SendMail(int userId, string userName)
        {
            //Mail Gönderme Kodları...

            await Console.Out.WriteLineAsync($"{userId}, {userName} Kullanıcısına Mail gönderildi...");
        }



        //rabbitmq bağlanmak için fonksiyonu
        private static IConnection GetConnection()
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            return connectionFactory.CreateConnection();
        }



        //kanal oluşturma fonksiyonu.
        private static IModel GetChannel()
        {
            return connection.CreateModel();
        }
    }
}
