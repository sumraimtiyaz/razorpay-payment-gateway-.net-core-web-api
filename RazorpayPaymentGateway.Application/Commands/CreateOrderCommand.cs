using MediatR;
using RazorpayPaymentGateway.Domain.Entities;
using RazorpayPaymentGateway.Infrastructure.Services.Interfaces;

namespace RazorpayPaymentGateway.Application.Commands
{

    /// <summary>
    /// Represents the request to create a new Razorpay order.
    /// </summary>
    public class CreateOrderCommand
    {
        /// <summary>
        /// Command to create an order.
        /// </summary>
        public class Command : IRequest<OrderResponse>
        {
            public decimal Amount { get; set; }
        }

        /// <summary>
        /// Handles the command to create an order.
        /// </summary>
        public class Handler : IRequestHandler<Command, OrderResponse>
        {
            private readonly IPaymentService _paymentService;

            public Handler(IPaymentService paymentService)
            {
                _paymentService = paymentService;
            }

            /// <summary>
            /// Handles the request to create an order in Razorpay.
            /// </summary>
            /// <param name="request">The command containing the order amount.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>The created order details.</returns>
            public async Task<OrderResponse> Handle(Command request, CancellationToken cancellationToken)
            {
                return await _paymentService.CreateOrderAsync(request.Amount);
            }
        }
    }

}
