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

    public Task<List<Ticket>> GetBasketItems(string sessionId)
    {
        if (_sessionBaskets.TryGetValue(sessionId, out var basketItems))
        {
            return Task.FromResult(basketItems);
        }
        return Task.FromResult(new List<Ticket>());
    }

    public async Task AddToBasket(string sessionId, int eventId, int numberOfSeats)
    {
        if (!_sessionBaskets.ContainsKey(sessionId))
        {
            _sessionBaskets[sessionId] = new List<Ticket>();
        }
        var @event = await _context.Events.FindAsync(eventId);
        if (@event == null)
        {
            throw new ArgumentException($"Event {eventId} not found");
        }

        var ticket = new Ticket
        {
            Event = @event,
            EventId = @event.Id,
            NumberOfSeats = numberOfSeats,
        };

        _sessionBaskets[sessionId].Add(ticket);
    }

    public Task RemoveFromBasket(string sessionId, int ticketId)
    {
        if (_sessionBaskets.TryGetValue(sessionId, out var basketItems))
        {
            var ticketToRemove = basketItems.FirstOrDefault(t => t.Id == ticketId);
            if (ticketToRemove != null)
            {
                basketItems.Remove(ticketToRemove);
            }
        }

        return Task.CompletedTask;
    }

    public Task ClearBasket(string sessionId)
    {
        _sessionBaskets.Remove(sessionId);
        return Task.CompletedTask;
    }

    public Task<decimal> GetBasketTotal(string sessionId)
    {
        if (_sessionBaskets.TryGetValue(sessionId, out var basketItems))
        {
            return Task.FromResult(basketItems.Sum(item => item.Price * item.NumberOfSeats));
        }
        return Task.FromResult<decimal>(0);
    }

    public Task<int> GetBasketItemCount(string sessionId)
    {
        if (_sessionBaskets.TryGetValue(sessionId, out var basketItems))
        {
            return Task.FromResult(basketItems.Sum(item => item.NumberOfSeats));
        }
        return Task.FromResult(0);
    }
}