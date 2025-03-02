using MediatR;
using Microsoft.AspNetCore.Mvc;
using RazorpayPaymentGateway.Application;
using RazorpayPaymentGateway.Application.Commands;

namespace RazorpayPaymentGateway.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Creates a new payment order.
        /// </summary>
        /// <param name="command">Command containing order details.</param>
        /// <returns>Returns the created order details.</returns>
        [HttpPost("CreateOrder")]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand.Command command)
        {
            var result = await _mediator.Send(command);

            return Ok(result);
        }


        /// <summary>
        /// Captures a payment for a given order.
        /// </summary>
        /// <param name="command">Command containing payment details.</param>
        /// <returns>Returns the payment capture status.</returns>
        [HttpPost("CapturePayment")]
        public async Task<IActionResult> CapturePayment([FromBody] CapturePaymentCommand.CommandRequest command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
