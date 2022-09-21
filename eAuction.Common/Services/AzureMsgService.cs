using Azure.Messaging.ServiceBus;
using eAuction.Common.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace eAuction.Common.Services
{
    [ExcludeFromCodeCoverage]
    public class AzureMsgService : IAzureMsgService
    {
        private readonly ILogger<AzureMsgService> _logger;

        public AzureMsgService(ILogger<AzureMsgService> logger)
        {
            this._logger = logger;
        }

        public async Task PushMessage(string connectionString, string queueName, string message)
        {
            try
            {
                await using var client = new ServiceBusClient(connectionString);
                
                ServiceBusSender sender = client.CreateSender(queueName);

                var serviceBusMessage = new ServiceBusMessage(message);

                await sender.SendMessageAsync(serviceBusMessage);

                _logger.LogInformation("Message sent to Queue!");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task<string> ReceiveMessage(string connectionString, string queueName)
        {
          try
            {
                await using var client = new ServiceBusClient(connectionString);

                ServiceBusReceiverOptions options = new ServiceBusReceiverOptions() { ReceiveMode = ServiceBusReceiveMode.ReceiveAndDelete};
              
                ServiceBusReceiver receiver = client.CreateReceiver(queueName, options);

                ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

                string body = receivedMessage.Body.ToString();

                _logger.LogWarning("Azuremsg file body : " + body);


                return body;
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, ex.Message);

                return "";
            }
        }
    }
}