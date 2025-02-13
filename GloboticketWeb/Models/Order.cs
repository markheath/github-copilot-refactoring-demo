namespace GloboticketWeb.Models;

public class Order
{
    public int Id { get; set; }
    public List<Ticket> Tickets { get; set; } = new();
    public decimal TotalPrice { get; set; }
    public int CustomerDetailsId { get; set; }
    public required Customer CustomerDetails { get; set; }
    public int PaymentInfoId { get; set; }
    public required Payment PaymentInfo { get; set; }
    public string? DiscountCode { get; set; }
    public OrderStatus Status { get; set; }
}

public enum OrderStatus
{
    Pending,
    Confirmed,
    Paid,
    Shipped,
    Delivered,
    Cancelled
}

public class Event
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Artist { get; set; }
    public DateTime Date { get; set; }
    public required string Venue { get; set; }
    public required string ImageUrl { get; set; }
}

public class Ticket
{
    public int Id { get; set; }
    public int NumberOfSeats { get; set; }
    public decimal Price { get; set; }
    public int EventId { get; set;}
    public required Event Event { get; set; }
}

public class Customer
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}

public class Payment
{
    public int Id { get; set; }
    public required string PaymentMethod { get; set; }
    public string? CreditCardNumber { get; set; }
    public string? CreditCardType { get; set; }
    public DateOnly? CreditCardExpiry { get; set; }
    public string? CreditCardName { get; set; }
    public string? PaypalAddress { get; set; }
    public string? GooglePayEmail { get; set; }
}
