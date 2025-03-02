using RazorpayPaymentGateway.Domain.Entities;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Razorpay.Api;
using Org.BouncyCastle.Asn1.Ocsp;

namespace RazorpayPaymentGateway.Infrastructure.Services.Interfaces.Implementation
{
    /// <summary>
    /// Service for handling payments using Razorpay.
    /// </summary>
    public class PaymentService : IPaymentService
    {

        private readonly string _key;
        private readonly string _secret;

        public PaymentService(IConfiguration configuration)
        {
            _key = Convert.ToString(configuration["Razorpay:key_id"]);
            _secret = Convert.ToString(configuration["Razorpay:key_secret"]);
        }

        /// <summary>
        /// Creates a new order in Razorpay with the specified amount.
        /// </summary>
        /// <param name="amount">The amount for the order (in INR).</param>
        /// <returns>An <see cref="OrderResponse"/> object containing the order details.</returns>
        public async Task<OrderResponse> CreateOrderAsync(decimal amount)
        {
            try
            {
                // Initialize Razorpay client
                var client = new RazorpayClient(_key, _secret);

                // Create order parameters
                var options = new Dictionary<string, object>
                {
                    { "amount", (int)(amount * 100) }, // Convert to paise
                    { "currency", "INR" },
                    { "receipt", Guid.NewGuid().ToString() }
                };

                // Create order in Razorpay
                var order = client.Order.Create(options);

                // Return order response
                return new OrderResponse
                {
                    OrderId = order["id"].ToString(),
                    RazorpayKey = _key,
                    Amount = amount
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while creating order for amount {amount}: {ex.Message}");
                throw new Exception("An unexpected error occurred while processing your order.");
            }
        }
        /// <summary>
        /// Captures a payment after verifying its authenticity.
        /// </summary>
        /// <param name="paymentId">The Razorpay payment ID.</param>
        /// <param name="orderId">The Razorpay order ID.</param>
        /// <param name="signature">The Razorpay signature for verification.</param>
        /// <returns>The status of the captured payment.</returns>
        public async Task<string> CapturePaymentAsync(string paymentId, string orderId, string signature)
        {
            try
            {
                // Validate Signature
                string generatedSignature = GenerateSignature(orderId, paymentId, _secret);
                if (generatedSignature != signature)
                {
                    return "Payment verification failed"; // If signature does not match
                }

                // Capture Payment
                var client = new Razorpay.Api.RazorpayClient(_key, _secret);

                // Fetch payment details
                var payment = client.Payment.Fetch(paymentId);

                if (payment == null)
                {
                    return "Payment not found";
                }

                // Convert dynamic object to JObject safely
                JObject paymentData = JObject.FromObject(payment);

                // Ensure the attribute exists
                if (!paymentData.ContainsKey("Attributes"))
                {
                    return "Invalid payment data: attribute missing";
                }

                // Extract the attribute JSON string
                string attributeJson = Convert.ToString(paymentData["Attributes"]);

                if (string.IsNullOrEmpty(attributeJson))
                {
                    return "Attribute is empty";
                }

                // Parse the attribute JSON safely
                JObject attributeData;
                try
                {
                    attributeData = JObject.Parse(attributeJson);
                }
                catch (Exception)
                {
                    return "Invalid JSON format in attribute";
                }

                // Retrieve the status value safely
                var status = attributeData.ContainsKey("status") ? attributeData["status"]?.ToString() ?? "Unknown Status" : "Status not found";

                return status;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Capture Payment Error: {ex.Message}");
                return "Payment processing error";
            }
        }

        /// <summary>
        /// Generates a HMAC-SHA256 signature for payment verification.
        /// </summary>
        /// <param name="orderId">The Razorpay order ID.</param>
        /// <param name="paymentId">The Razorpay payment ID.</param>
        /// <param name="secret">The secret key for signing the request.</param>
        /// <returns>A hexadecimal string representing the generated signature.</returns>
        private string GenerateSignature(string orderId, string paymentId, string secret)
        {
            try
            {
                string payload = $"{orderId}|{paymentId}";

                using (var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(secret)))
                {
                    byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                    return BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error in GenerateSignature: {ex.Message}");
                throw new Exception("An error occurred while generating the payment signature.");
            }
        }

    }
}
