using GloboticketWeb.Models;

public interface IDiscountService
{
    bool IsValidDiscountCode(string discountCode);
    decimal ApplyDiscount(decimal price, string discountCode);
}

public interface IEventService
{
    bool AreSeatsAvailable(int eventId, int numberOfSeats);
    Event GetEvent(int eventId);
}

public class OrderProcessor
{
    private readonly IDiscountService _discountService;
    private readonly IEventService _eventService;

    public OrderProcessor(IDiscountService discountService, IEventService eventService)
    {
        _discountService = discountService;
        _eventService = eventService;
    }

    public async Task<bool> ProcessOrder(Order order)
    {
        if (!ValidateOrder(order))
        {
            return false;
        }

        decimal total = 0;
        foreach (var ticket in order.Tickets)
        {
            total += ticket.Price;
        }

        if (!string.IsNullOrEmpty(order.DiscountCode) && _discountService.IsValidDiscountCode(order.DiscountCode))
        {
            total = _discountService.ApplyDiscount(total, order.DiscountCode);
        }

        if (order.Tickets.Count > 10)
        {
            // Apply bulk discount
            total *= 0.9m;
        }

        order.TotalPrice = total;

        // Simulate payment processing
        await Task.Delay(1000);

        // Simulate saving to database
        await Task.Delay(1000);

        return true;
    }

    private bool ValidateOrder(Order order)
    {
        if (order == null)
        {
            return false;
        }

        if (order.CustomerDetails == null || string.IsNullOrEmpty(order.CustomerDetails.Email) || string.IsNullOrEmpty(order.CustomerDetails.FirstName) || string.IsNullOrEmpty(order.CustomerDetails.LastName))
        {
            return false;
        }

        if (order.PaymentInfo == null || string.IsNullOrEmpty(order.PaymentInfo.PaymentMethod) || string.IsNullOrEmpty(order.PaymentInfo.CreditCardName))
        {
            return false;
        }

        if (order.Tickets == null || order.Tickets.Count == 0)
        {
            return false;
        }

        foreach (var ticket in order.Tickets)
        {
            if (ticket.Event == null)
            {
                return false;
            }

            var eventDetails = _eventService.GetEvent(ticket.Event.Id);
            if (eventDetails == null)
            {
                return false;
            }

            if (eventDetails.Date < DateTime.Now)
            {
                return false;
            }

            if (!_eventService.AreSeatsAvailable(ticket.Event.Id, 1))
            {
                return false;
            }
        }

        return true;
    }
}