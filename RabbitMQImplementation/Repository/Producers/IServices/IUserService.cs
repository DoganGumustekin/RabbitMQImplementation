using RabbitMQImplementation.Models;

namespace RabbitMQImplementation.Repository.Producers.IServices
{
    public interface IUserService
    {
        void register(UserModel user);
    }
}
