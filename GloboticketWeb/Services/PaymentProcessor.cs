using System.Text;
using System.Text.Json;
using GloboticketWeb.Models;
using Microsoft.Extensions.Logging;

namespace GloboticketWeb.Services;

public class PaymentProcessor : IPaymentProcessor
{
    private const string paymentApiUrl = "http://paymentgateway.example/api/process";
    private readonly HttpClient _httpClient;
    private readonly ILogger<PaymentProcessor> _logger;

    public PaymentProcessor(HttpClient httpClient, ILogger<PaymentProcessor> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task ProcessPaymentAsync(decimal totalPrice, Payment paymentInfo)
    {
        _logger.LogInformation("Starting payment processing.");

        // Retrieve the API key from the environment variable
        string apiKey = Environment.GetEnvironmentVariable("PAYMENT_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            _logger.LogError("Payment API key not found in environment variables.");
            throw new Exception("Payment API key not found in environment variables.");
        }

        // Build a JSON payload using JsonSerializer
        var payload = new
        {
            creditCardNumber = paymentInfo.CreditCardNumber,
            expiryDate = paymentInfo.CreditCardExpiry,
            totalPrice = totalPrice
        };
        string jsonPayload = JsonSerializer.Serialize(payload);

        // Create HttpRequestMessage
        var request = new HttpRequestMessage(HttpMethod.Post, paymentApiUrl)
        {
            Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
        };
        request.Headers.Add("API_KEY", apiKey);

        // Send the request asynchronously
        HttpResponseMessage response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        _logger.LogInformation("Payment processed successfully.");
    }
}