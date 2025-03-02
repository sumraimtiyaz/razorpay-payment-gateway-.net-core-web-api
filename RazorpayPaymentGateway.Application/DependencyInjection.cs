using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RazorpayPaymentGateway.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RazorpayPaymentGateway.Application
{
    public static class DependencyInjection
    {
        /// <summary>
        /// Configures and registers application-layer dependencies, including MediatR and infrastructure dependencies.
        /// </summary>
        public static IServiceCollection InjectApplicationDependencies(this IServiceCollection services, IConfiguration Configuration)
        {
            // Register Infrastructure Layer Dependencies
            services.InjectInfrastructureDependencies(Configuration);

            // Register MediatR for CQRS pattern
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            return services;
        }
    }
}
