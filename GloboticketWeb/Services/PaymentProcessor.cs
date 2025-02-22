using System.Text;
using System.Text.Json;
using GloboticketWeb.Models;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Retry;

namespace GloboticketWeb.Services;

public class PaymentProcessor : IPaymentProcessor
{
    private const string paymentApiUrl = "http://paymentgateway.example/api/process";
    private readonly HttpClient _httpClient;
    private readonly ILogger<PaymentProcessor> _logger;
    private readonly AsyncRetryPolicy<HttpResponseMessage> _retryPolicy;

    public PaymentProcessor(HttpClient httpClient, ILogger<PaymentProcessor> logger)
    {
        _httpClient = httpClient;
        _logger = logger;

        _retryPolicy = Policy
            .Handle<HttpRequestException>()
            .OrResult<HttpResponseMessage>(r => !r.IsSuccessStatusCode)
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                (outcome, timespan, retryCount, context) =>
                {
                    if (outcome.Exception != null)
                    {
                        _logger.LogWarning($"Request failed with exception: {outcome.Exception.Message}. Waiting {timespan} before next retry. Retry attempt {retryCount}");
                    }
                    else
                    {
                        _logger.LogWarning($"Request failed with {outcome.Result.StatusCode}. Waiting {timespan} before next retry. Retry attempt {retryCount}");
                    }
                });
    }

    public async Task ProcessPaymentAsync(decimal totalPrice, Payment paymentInfo)
    {
        _logger.LogInformation("Starting payment processing.");

        // Retrieve the API key from the environment variable
        string? apiKey = Environment.GetEnvironmentVariable("PAYMENT_API_KEY");
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

        // Send the request asynchronously with retry policy
        HttpResponseMessage response = await _retryPolicy.ExecuteAsync(() => _httpClient.SendAsync(request));
        response.EnsureSuccessStatusCode();
        _logger.LogInformation("Payment processed successfully.");
    }
}