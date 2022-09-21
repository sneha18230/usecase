using System.Threading.Tasks;

namespace eAuction.Common.Interfaces
{
    public interface IAzureMsgService
    {
        Task PushMessage(string connectionString, string queueName, string message);

        Task<string> ReceiveMessage(string connectionString, string queueName);
    }
}