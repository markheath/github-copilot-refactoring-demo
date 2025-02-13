using GloboticketWeb.Models;

namespace GloboticketWeb.Services;

public interface IBasketService
{
    Task<List<Ticket>> GetBasketItems(string sessionId);
    Task AddToBasket(string sessionId, int eventId, int numberOfSeats);
    Task RemoveFromBasket(string sessionId, int ticketId);
    Task ClearBasket(string sessionId);
    Task<decimal> GetBasketTotal(string sessionId);
    Task<int> GetBasketItemCount(string sessionId);
}