using GloboticketWeb.Models;
namespace GloboticketWeb.Services;

public interface IPaymentProcessor
{
    void ProcessPayment(decimal totalPrice, Payment paymentInfo);
}
