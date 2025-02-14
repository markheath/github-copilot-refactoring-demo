using GloboticketWeb.Models;
namespace GloboticketWeb.Services;

public interface IPaymentProcessor
{
    Task ProcessPayment(decimal totalPrice, Payment paymentInfo);
}