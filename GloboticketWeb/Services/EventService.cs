using GloboticketWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace GloboticketWeb.Services;

public class EventService : IEventService
{
    private readonly GloboticketDbContext _context;

    public EventService(GloboticketDbContext context)
    {
        _context = context;
    }

    public bool AreSeatsAvailable(int eventId, int numberOfSeats)
    {
        throw new NotImplementedException();
    }

    public Event GetEvent(int eventId)
    {
        return _context.Events.Find(eventId) ?? throw new KeyNotFoundException();
    }

    public async Task<List<Event>> GetTopEventsAsync(int count = 10)
    {
        return await _context.Events
            .OrderBy(e => e.Date)
            .Take(count)
            .ToListAsync();
    }
}