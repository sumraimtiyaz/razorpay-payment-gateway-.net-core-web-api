using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RazorpayPaymentGateway.Infrastructure.Services.Interfaces;
using RazorpayPaymentGateway.Infrastructure.Services.Interfaces.Implementation;

namespace RazorpayPaymentGateway.Infrastructure
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Configures and registers infrastructure-layer services and repositories dependencies.
        /// </summary>
        public static IServiceCollection InjectInfrastructureDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            // Registers the PaymentService class as an implementation of the IPaymentService interface
            services.AddScoped<IPaymentService, PaymentService>();

            return services;
        }
    }
}
