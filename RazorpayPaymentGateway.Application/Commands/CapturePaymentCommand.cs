using MediatR;
using RazorpayPaymentGateway.Infrastructure.Services.Interfaces;

namespace RazorpayPaymentGateway.Application.Commands
{
    /// <summary>
    /// Represents the request to capture a payment.
    /// </summary>
    public class CapturePaymentCommand
    {
        /// <summary>
        /// Command to capture a payment.
        /// </summary>
        public class CommandRequest : IRequest<string>
        {
            public string PaymentId { get; set; }
            public string OrderId { get; set; }
            public string Signature { get; set; }

        }

        /// <summary>
        /// Handles the command to capture a payment.
        /// </summary>
        public class Handler : IRequestHandler<CommandRequest, string>
        {
            private readonly IPaymentService _paymentService;

            public Handler(IPaymentService paymentService)
            {
                _paymentService = paymentService;
            }

            /// <summary>
            /// Handles the request to capture a payment.
            /// </summary>
            /// <param name="request">The command containing payment details.</param>
            /// <param name="cancellationToken">Cancellation token.</param>
            /// <returns>The captured payment ID.</returns>
            public async Task<string> Handle(CommandRequest request, CancellationToken cancellationToken)
            {
               
                return await _paymentService.CapturePaymentAsync(request.PaymentId, request.OrderId, request.Signature);
                
            }
        }
    }
}
