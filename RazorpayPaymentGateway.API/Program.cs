using Microsoft.OpenApi.Models;
using RazorpayPaymentGateway.Application;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();

// Injects application-level dependencies from a separate module.
builder.Services.InjectApplicationDependencies(builder.Configuration);

// Configures CORS to allow requests from specific origins.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        policy => policy.WithOrigins("http://localhost:*/").AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
});
// Enables Swagger only in the development environment for API documentation.
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen(options =>
    {
        options.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Razorpay Payment Gateway API",
            Version = "v1",
            Description = "API for managing Razorpay Payment"
        });
    });
}

var app = builder.Build();

// Applies the CORS policy to the application.
app.UseCors("AllowSpecificOrigins");

// Swagger is enabled only in the development environment.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Razorpay Payment Gateway API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
