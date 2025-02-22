using System.Net;
using GloboticketWeb.Models;
namespace GloboticketWeb.Services;

public class PaymentProcessor : IPaymentProcessor
{
    private const string paymentApiUrl = "http://paymentgateway.example/api/process";
    public void ProcessPayment(decimal totalPrice, Payment paymentInfo)
    {
        // Retrieve the API key from the environment variable
        string apiKey = Environment.GetEnvironmentVariable("PAYMENT_API_KEY");
        if (string.IsNullOrEmpty(apiKey))
        {
            throw new Exception("Payment API key not found in environment variables.");
        }

        // Manually build a JSON payload
        string jsonPayload = "{" +
            "\"creditCardNumber\":\"" + paymentInfo.CreditCardNumber + "\"," +
            "\"expiryDate\":\"" + paymentInfo.CreditCardExpiry + "\"," +
            "\"totalPrice\":" + totalPrice.ToString(System.Globalization.CultureInfo.InvariantCulture) +
        "}";

        // Use WebClient for a synchronous call
        using (var client = new WebClient())
        {
            client.Headers.Add("API_KEY", apiKey);
            client.Headers.Add("Content-Type", "application/json");

            // Call payment API endpoint
            // will throw a WebException if the request fails
            string response = client.UploadString(
                paymentApiUrl, "POST", jsonPayload);
            
            // check for success
            if (!response.Contains("SUCCESS"))
            {
                throw new Exception("Payment processing failed.");
            }

        }

    }

}