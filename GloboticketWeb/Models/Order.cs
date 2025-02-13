namespace GloboticketWeb.Models;

public class Order
{
    public required string Id { get; set; }
    public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    public decimal TotalPrice { get; set; }
    public required Customer CustomerDetails { get; set; }
    public required Payment PaymentInfo { get; set; }
    public string? DiscountCode { get; set; }
}

public class Event
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Artist { get; set; }
    public DateTime Date { get; set; }
    public required string Venue { get; set; }
}

public class Ticket
{
    public required string SeatNumber { get; set; }
    public decimal Price { get; set; }
    public required Event Event { get; set; }
}

public class Customer
{
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
}

public class Payment
{
    public required string PaymentMethod { get; set; }
    public required string PaymentReference { get; set; }
}
