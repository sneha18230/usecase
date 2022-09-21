using Auction.Commom.Model;
using eAuction.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace eAuction.ServiceBus.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageProcessController : ControllerBase
    {
        private readonly ILogger<MessageProcessController> _logger;

        private readonly IAzureMsgService _service;

        private readonly IConfiguration _config;

        private readonly IHttpClientService _httpClientService;

        public MessageProcessController(ILogger<MessageProcessController> logger, IAzureMsgService service, IConfiguration config, IHttpClientService httpClientService)
        {
            this._logger = logger;

            this._service = service;

            this._config = config;

            this._httpClientService = httpClientService;
        }

        [HttpPost("add-bidmsg")]
        [Produces(typeof(bool))]
        public virtual async Task<ActionResult<bool>> AddBidMsg(Buyer bid)
        {
            _logger.LogInformation("Method Started : " + MethodBase.GetCurrentMethod().Name);

            try
            {
                var bidJsonMsg = JsonConvert.SerializeObject(bid);

                var sbConnString = _config.GetSection("AzureSBConnectionString").Value;

                var queueName = _config.GetSection("AzureSBBidQueueName").Value;

                await _service.PushMessage(sbConnString, queueName, bidJsonMsg).ConfigureAwait(false);

                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            finally
            {
                _logger.LogInformation("Method Ended : " + MethodBase.GetCurrentMethod().Name);
            }
        }

        [HttpPost("add-productidmsg")]
        [Produces(typeof(bool))]
        public virtual async Task<ActionResult<bool>> AddProductIdMsg(int productId)
        {
            _logger.LogInformation("Method Started : " + MethodBase.GetCurrentMethod().Name);

            try
            {
                var productJsonMsg = JsonConvert.SerializeObject(productId);

                var sbConnString = _config.GetSection("AzureSBConnectionString").Value;

                var queueName = _config.GetSection("AzureSBProductQueueName").Value;

                await _service.PushMessage(sbConnString, queueName, productJsonMsg).ConfigureAwait(false);

                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            finally
            {
                _logger.LogInformation("Method Ended : " + MethodBase.GetCurrentMethod().Name);
            }
        }

        [HttpGet("execute-addbidmsg")]
        [Produces(typeof(Buyer))]
        public virtual async Task<ActionResult<Buyer>> ReadAndExecuteAddBidMsg()
        {
            _logger.LogInformation("Method Started : " + MethodBase.GetCurrentMethod().Name);

            try
            {
                var sbConnString = _config.GetSection("AzureSBConnectionString").Value;

                var queueName = _config.GetSection("AzureSBBidQueueName").Value;

                var bidJsonMsg = await _service.ReceiveMessage(sbConnString, queueName).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(bidJsonMsg))
                {
                    return NotFound("Message not found!");
                }

                var addBidUrl = _config.GetSection("AddBidUrl").Value;

                var buyer = JsonConvert.DeserializeObject<Buyer>(bidJsonMsg);

                var buyerResult = await _httpClientService.ExecutePost<Buyer>(addBidUrl, buyer);

                return buyerResult ?? new Buyer();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            finally
            {
                _logger.LogInformation("Method Ended : " + MethodBase.GetCurrentMethod().Name);
            }
        }

        [HttpGet("execute-getbuyersmsg")]
        [Produces(typeof(List<Buyer>))]
        public virtual async Task<ActionResult<List<Buyer>>> ReadAndExecuteGetBuyersMsg()
        {
            _logger.LogInformation("Method Started : " + MethodBase.GetCurrentMethod().Name);

            try
            {
                var sbConnString = _config.GetSection("AzureSBConnectionString").Value;

                var queueName = _config.GetSection("AzureSBProductQueueName").Value;

                var productJsonMsg = await _service.ReceiveMessage(sbConnString, queueName).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(productJsonMsg))
                {
                    return NotFound("Message not found!");
                }

                var apiUrl = _config.GetSection("ProductUrl").Value;

                var productId = JsonConvert.DeserializeObject<int>(productJsonMsg);

                var getProductUrl = string.Format("{0}/{1}", apiUrl, productId);

                var product = await _httpClientService.ExecuteGet<Product>(getProductUrl);

                return product?.Buyers ?? new List<Buyer>();
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            finally
            {
                _logger.LogInformation("Method Ended : " + MethodBase.GetCurrentMethod().Name);
            }
        }

        [HttpGet("execute-deleteProduct")]
        [Produces(typeof(bool))]
        public virtual async Task<ActionResult<bool>> ReadAndExecuteDeleteProductMsg()
        {
            _logger.LogInformation("Method Started : " + MethodBase.GetCurrentMethod().Name);

            try
            {
                var sbConnString = _config.GetSection("AzureSBConnectionString").Value;

                var queueName = _config.GetSection("AzureSBProductQueueName").Value;

                var productJsonMsg = await _service.ReceiveMessage(sbConnString, queueName).ConfigureAwait(false);

                if (string.IsNullOrWhiteSpace(productJsonMsg))
                {
                    return NotFound("Message not found!");
                }

                var apiUrl = _config.GetSection("ProductUrl").Value;

                var productId = JsonConvert.DeserializeObject<int>(productJsonMsg);

                var deleteProductUrl = string.Format("{0}/{1}", apiUrl, productId);

                var result = await _httpClientService.ExecuteDelete(deleteProductUrl);

                return result;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            finally
            {

                _logger.LogInformation("Method Ended : " + MethodBase.GetCurrentMethod().Name);
            }
        }
    }
}
