using GloboticketWeb.Models;
namespace GloboticketWeb.Services;

public interface IOrderService
{
    Task<bool> ProcessOrder(Order order);
}