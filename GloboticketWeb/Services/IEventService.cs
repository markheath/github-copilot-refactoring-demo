using GloboticketWeb.Models;
namespace GloboticketWeb.Services;

public interface IEventService
{
    Task<List<Event>> GetTopEventsAsync(int count = 10);

    bool AreSeatsAvailable(int eventId, int numberOfSeats);
    Event GetEvent(int eventId);
}