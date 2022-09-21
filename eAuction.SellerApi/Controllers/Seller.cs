using Auction.Commom.Model;
using Auction.Common.Exception;
using Auction.Common.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;

namespace eAuction.SellerApi.Controllers
{
    [Route("api/v1/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [ExcludeFromCodeCoverage]
    public class SellerController : ControllerBase
    {
        private readonly ILogger<SellerController> _logger;

        private readonly ISellerService _service;

        public SellerController(ILogger<SellerController> logger, ISellerService service)
        {
            this._logger = logger;

            this._service = service;
        }

        [HttpGet("{id}")]
        [Produces(typeof(Product))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public virtual async Task<ActionResult<Product>> GetProduct(int id)
        {
            _logger.LogInformation("Method Started : " + MethodBase.GetCurrentMethod().Name);

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _service.GetProductBids(id).ConfigureAwait(false);

                return response != null ? Ok(response) : NotFound("No Product with given Id");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                if (ex is ProductException)
                {
                    return NotFound(ex.Message);
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

        [HttpPost("add-product")]
        [Produces(typeof(Product))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public virtual async Task<ActionResult<Product>> AddProduct([FromBody] Product product)
        {
            _logger.LogInformation("Method Started : " + MethodBase.GetCurrentMethod().Name);

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _service.AddProduct(product).ConfigureAwait(false);

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                if (ex is ProductException || ex is ValidationException)
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

        [HttpDelete("{id}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public virtual async Task<ActionResult<bool>> DeleteProduct(int id)
        {
            _logger.LogInformation("Method Started : " + MethodBase.GetCurrentMethod().Name);

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _service.DeleteProduct(id).ConfigureAwait(false);

                if (response)
                {
                    return Ok(response);
                }
                else
                {
                    return new StatusCodeResult(StatusCodes.Status500InternalServerError);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                if (ex is ProductException || ex is ValidationException)
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

        [HttpGet("GetProducts")]
        [Produces(typeof(Product))]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public virtual async Task<ActionResult<Product>> GetProducts()
        {
            _logger.LogInformation("Method Started : " + MethodBase.GetCurrentMethod().Name);

            try
            {
                var response = await _service.GetProducts().ConfigureAwait(false);

                if (response == null || response.Count == 0)
                {
                    return NotFound("No Data Found");
                }

                return Ok(response);
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