using GloboticketWeb.Models;

namespace GloboticketWeb.Services;

public interface IPaymentProcessor
{
    Task ProcessPaymentAsync(decimal totalPrice, Payment paymentInfo);
}
