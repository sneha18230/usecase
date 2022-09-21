using Auction.Common.Exception;
using Auction.Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace eAuction.BuyerApi.Controllers
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class BuyerController : ControllerBase
    {
        private readonly ILogger<BuyerController> _logger;

        private readonly IBuyerService _service;

        public BuyerController(ILogger<BuyerController> logger, IBuyerService service)
        {
            this._logger = logger;

            this._service = service;
        }

        [HttpPost("place-bid")]
        [Produces(typeof(Auction.Commom.Model.Buyer))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public virtual async Task<ActionResult<Auction.Commom.Model.Buyer>> AddBid(Auction.Commom.Model.Buyer bid)
        {
            _logger.LogInformation("Method Started : " + MethodBase.GetCurrentMethod().Name);

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _service.AddBid(bid).ConfigureAwait(false);

                return Ok(response);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                if (ex is ProductException)
                {
                    return NotFound(ex.Message);
                }

                else if (ex is BuyerException || ex is ValidationException)
                {
                    return BadRequest(ex.Message);
                }

                else
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }

            finally
            {
                _logger.LogInformation("Method Ended : " + MethodBase.GetCurrentMethod().Name);
            }
        }

        [HttpPut("update-bid/{productId}/{buyerEmailld}/{newBidAmount}")]
        [Produces(typeof(Auction.Commom.Model.Buyer))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public virtual async Task<ActionResult<Auction.Commom.Model.Buyer>> UpdateBid(int productId, string buyerEmailld, double newBidAmount)
        {
            _logger.LogInformation("Method Started : " + MethodBase.GetCurrentMethod().Name);

            try
            {
                var response = await _service.UpdateBid(productId, buyerEmailld, newBidAmount).ConfigureAwait(false);

                return Ok(response);
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                if (ex is ProductException || ex is BuyerException)
                {
                    return NotFound(ex.Message);
                }

                else if (ex is ValidationException)
                {
                    return BadRequest(ex.Message);
                }

                else
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }

            finally
            {
                _logger.LogInformation("Method Ended : " + MethodBase.GetCurrentMethod().Name);
            }
        }


    }
}
