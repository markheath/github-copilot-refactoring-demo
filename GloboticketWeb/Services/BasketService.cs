using GloboticketWeb.Models;
namespace GloboticketWeb.Services;

public class BasketService : IBasketService
{
    private readonly GloboticketDbContext _context;
    private readonly Dictionary<string, List<Ticket>> _sessionBaskets = new();

    public BasketService(GloboticketDbContext context)
    {
        _context = context;
    }

    public async Task<List<Ticket>> GetBasketItems(string sessionId)
    {
        if (_sessionBaskets.TryGetValue(sessionId, out var basketItems))
        {
            return basketItems;
        }
        return new List<Ticket>();
    }

    public async Task AddToBasket(string sessionId, int eventId, int numberOfSeats)
    {
        if (!_sessionBaskets.ContainsKey(sessionId))
        {
            _sessionBaskets[sessionId] = new List<Ticket>();
        }
        var @event = await _context.Events.FindAsync(eventId);

        var ticket = new Ticket
        {
            Event = @event,
            EventId = @event.Id,
            NumberOfSeats = numberOfSeats,
        };

        _sessionBaskets[sessionId].Add(ticket);
    }

    public async Task RemoveFromBasket(string sessionId, int ticketId)
    {
        if (_sessionBaskets.TryGetValue(sessionId, out var basketItems))
        {
            var ticketToRemove = basketItems.FirstOrDefault(t => t.Id == ticketId);
            if (ticketToRemove != null)
            {
                basketItems.Remove(ticketToRemove);
            }
        }
    }

    public async Task ClearBasket(string sessionId)
    {
        _sessionBaskets.Remove(sessionId);
    }

    public async Task<decimal> GetBasketTotal(string sessionId)
    {
        if (_sessionBaskets.TryGetValue(sessionId, out var basketItems))
        {
            return basketItems.Sum(item => item.Price * item.NumberOfSeats);
        }
        return 0;
    }

    public async Task<int> GetBasketItemCount(string sessionId)
    {
        if (_sessionBaskets.TryGetValue(sessionId, out var basketItems))
        {
            return basketItems.Sum(item => item.NumberOfSeats);
        }
        return 0;
    }
}