using RazorpayPaymentGateway.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RazorpayPaymentGateway.Infrastructure.Services.Interfaces
{
    public interface IPaymentService
    {
        /// <summary>
        /// Creates a new order in Razorpay with the specified amount.
        /// </summary>
        /// <param name="amount">The amount for the order (in INR).</param>
        /// <returns>An <see cref="OrderResponse"/> object containing the order details.</returns>
        Task<OrderResponse> CreateOrderAsync(decimal amount);
        /// <summary>
        /// Captures a payment after verifying its authenticity.
        /// </summary>
        /// <param name="paymentId">The Razorpay payment ID.</param>
        /// <param name="orderId">The Razorpay order ID.</param>
        /// <param name="signature">The Razorpay signature for verification.</param>
        /// <returns>The status of the captured payment.</returns>
        Task<string> CapturePaymentAsync(string paymentId, string orderId, string signature);
    }
}
