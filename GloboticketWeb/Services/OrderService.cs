using GloboticketWeb.Models;
namespace GloboticketWeb.Services;

public class OrderService : IOrderService
{
    private readonly IDiscountService discountService;
    private readonly IEventService eventService;
    private readonly GloboticketDbContext globoticketDbContext;
    private readonly IPaymentProcessor paymentProcessor;
    private readonly IEmailService emailService;

    public OrderService(
        IDiscountService discountService, 
        IEventService eventService,
        IPaymentProcessor paymentProcessor,
        GloboticketDbContext globoticketDbContext,
        IEmailService emailService)
    {
        this.discountService = discountService;
        this.eventService = eventService;
        this.paymentProcessor = paymentProcessor;
        this.globoticketDbContext = globoticketDbContext;
        this.emailService = emailService;
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

        if (!string.IsNullOrEmpty(order.DiscountCode) && discountService.IsValidDiscountCode(order.DiscountCode))
        {
            total = discountService.ApplyDiscount(total, order.DiscountCode);
        }

        order.TotalPrice = total;
        order.Status = OrderStatus.Confirmed;
        
        // payment processing
        await paymentProcessor.ProcessPaymentAsync(order.TotalPrice, order.PaymentInfo);

        order.Status = OrderStatus.Paid;

        // save to database
        globoticketDbContext.Add(order);
        await globoticketDbContext.SaveChangesAsync();

        // send confirmation email
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

            var eventDetails = eventService.GetEvent(ticket.Event.Id);
            if (eventDetails == null)
            {
                return false;
            }

            if (eventDetails.Date < DateTime.Now)
            {
                return false;
            }

            if (!eventService.AreSeatsAvailable(ticket.Event.Id, ticket.NumberOfSeats))
            {
                return false;
            }
        }

        return true;
    }
}
