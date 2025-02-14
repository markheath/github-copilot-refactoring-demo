using GloboticketWeb.Models;
namespace GloboticketWeb.Services;

public class OrderService : IOrderService
{
    private readonly IDiscountService _discountService;
    private readonly IEventService _eventService;
    private readonly GloboticketDbContext dbContext;
    private readonly IPaymentProcessor paymentProcessor;

    public OrderService(IDiscountService discountService, 
        IEventService eventService,
        IPaymentProcessor paymentProcessor,
        GloboticketDbContext dbContext)
    {
        _discountService = discountService;
        _eventService = eventService;
        this.dbContext = dbContext;
        this.paymentProcessor = paymentProcessor;
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
            if (ticket.NumberOfSeats < 0)
            {
                return false;
            }
            if (ticket.NumberOfSeats > 10)
            {
                // Limit of 10 tickets per order
                return false;
            }
            total += ticket.NumberOfSeats * ticket.Price;
        }

        if (!string.IsNullOrEmpty(order.DiscountCode) && _discountService.IsValidDiscountCode(order.DiscountCode))
        {
            total = _discountService.ApplyDiscount(total, order.DiscountCode);
        }

        order.TotalPrice = total;
        order.Status = OrderStatus.Confirmed;
        
        // payment processing
        await paymentProcessor.ProcessPayment(order.TotalPrice, order.PaymentInfo);

        order.Status = OrderStatus.Paid;

        // save to database
        dbContext.Add(order);
        await dbContext.SaveChangesAsync();

        // send confirmation email
        var emailService = new EmailService();
        await emailService.SendEmailAsync(order.CustomerDetails.Email, 
            "Order Confirmation", 
            $"Thank you for your order {order.Id}. " +
            $"Your order total is {order.TotalPrice}");

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

            if (!_eventService.AreSeatsAvailable(ticket.Event.Id, ticket.NumberOfSeats))
            {
                return false;
            }
        }

        return true;
    }
}
