using Api.Core.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Api.Core.Commands;
using PaymentGateway.Api.Core.Data.Dtos;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using ControllerBase = Api.Core.Controller.ControllerBase;

namespace PaymentGateway.Api.Controllers
{
    /// <summary>
    /// PaymentGatewayController 
    /// </summary>

    [Route("api/Payments")]
    public class PaymentGatewayController : ControllerBase
    {
        /// <summary>
        /// Payment Gateway Controller
        /// </summary>
        /// <param name="commandService"></param>
        public PaymentGatewayController(ICommandService commandService) : base(commandService)
        {
        }

        /// <summary>
        /// Api to post carde details for making payments
        /// </summary>
        /// <param name="cardDetails"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(CardInformation), 200)]
        [ProducesErrorResponseType(typeof(ErrorMessage))]
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(typeof(ErrorMessage), 404)]
        public Task<ActionResult> Post([FromBody] CardInformation cardDetails) =>
            Run<ProcessPaymentsCommand, CardInformation, Response>(cardDetails);



        /// <summary>
        /// retrieve details of a previously made payment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PaymentRecord), 200)]
        [ProducesErrorResponseType(typeof(ErrorMessage))]        
        [ProducesResponseType(typeof(ErrorMessage), 400)]
        [ProducesResponseType(typeof(ErrorMessage), 404)]
        public Task<ActionResult> GetById([Required] string id) =>
            Run<FetchPaymentsByIdCommand, string, PaymentRecord>(id);


        /// <summary>
        /// Health end point to check the elastic search cluster health.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("health")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public Task<ActionResult> Health() => Run<HealthCommand, Health, Health>(null);
    }
}
