using GloboticketWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace GloboticketWeb.Services;

public static class EventDataSeeder
{
    public static async Task SeedEvents(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GloboticketDbContext>();

        if (!await context.Events.AnyAsync())
        {
            var events = new List<Event>
            {
                new() {
                    Name = "Cosmic Synthesizer Summit",
                    Description = "Join Luna Wave and her electronic ensemble for an evening of otherworldly synthesizer music",
                    Artist = "Luna Wave",
                    Date = DateTime.Now.AddDays(7),
                    Venue = "Digital Dreams Arena",
                    TicketPrice = 45.00M,
                    ImageUrl = "/images/placeholder.jpg"
                },
                new() {
                    Name = "Thunder Dragons Rock Experience",
                    Description = "High-energy rock performance featuring the breakthrough band of the year",
                    Artist = "Thunder Dragons",
                    Date = DateTime.Now.AddDays(14),
                    Venue = "Mountain Peak Stadium",
                    TicketPrice = 65.00M,
                    ImageUrl = "/images/placeholder.jpg"
                },
                new() {
                    Name = "Jazz Fusion Night",
                    Description = "An innovative blend of jazz and world music by the renowned quartet",
                    Artist = "Quantum Jazz Collective",
                    Date = DateTime.Now.AddDays(21),
                    Venue = "Blue Note Theater",
                    TicketPrice = 55.00M,
                    ImageUrl = "/images/placeholder.jpg"
                },
                new() {
                    Name = "Folk Tales Festival",
                    Description = "A cozy evening of storytelling through folk music",
                    Artist = "The Wandering Minstrels",
                    Date = DateTime.Now.AddDays(28),
                    Venue = "Garden Grove Amphitheater",
                    TicketPrice = 35.00M,
                    ImageUrl = "/images/placeholder.jpg"
                },
                new() {
                    Name = "Digital Beats Revolution",
                    Description = "Cutting-edge electronic music showcase featuring emerging artists",
                    Artist = "Binary Beats Collective",
                    Date = DateTime.Now.AddDays(35),
                    Venue = "Cyber Club Matrix",
                    TicketPrice = 40.00M,
                    ImageUrl = "/images/placeholder.jpg"
                },
                new() {
                    Name = "Classical Fusion Evening",
                    Description = "Modern interpretations of classical masterpieces",
                    Artist = "Neo Classical Orchestra",
                    Date = DateTime.Now.AddDays(42),
                    Venue = "Grand Harmony Hall",
                    TicketPrice = 75.00M,
                    ImageUrl = "/images/placeholder.jpg"
                },
                new() {
                    Name = "Indie Rock Showcase",
                    Description = "Featuring the best up-and-coming indie rock bands",
                    Artist = "The Pixel Rebels",
                    Date = DateTime.Now.AddDays(49),
                    Venue = "Underground Sound Factory",
                    TicketPrice = 30.00M,
                    ImageUrl = "/images/placeholder.jpg"
                },
                new() {
                    Name = "Future Pop Experience",
                    Description = "Innovative pop music with futuristic elements",
                    Artist = "Crystal Dynamics",
                    Date = DateTime.Now.AddDays(56),
                    Venue = "Neon Dreams Arena",
                    TicketPrice = 50.00M,
                    ImageUrl = "/images/placeholder.jpg"
                },
                new() {
                    Name = "World Music Fusion",
                    Description = "A celebration of global music traditions",
                    Artist = "Global Harmony Ensemble",
                    Date = DateTime.Now.AddDays(63),
                    Venue = "Cultural Arts Center",
                    TicketPrice = 45.00M,
                    ImageUrl = "/images/placeholder.jpg"
                },
                new() {
                    Name = "Alternative Metal Night",
                    Description = "Heavy riffs meet melodic compositions",
                    Artist = "Steel Phoenix Rising",
                    Date = DateTime.Now.AddDays(70),
                    Venue = "Metal Haven",
                    TicketPrice = 55.00M,
                    ImageUrl = "/images/placeholder.jpg"
                }
            };

            await context.Events.AddRangeAsync(events);
            await context.SaveChangesAsync();
        }
    }
    
    public static async Task SeedCustomersAndOrders(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<GloboticketDbContext>();

        // Seed customers if none exist
        if (!await context.Customers.AnyAsync())
        {
            var customers = new List<Customer>
            {
                new() 
                {
                    FirstName = "Michael",
                    LastName = "Williams",
                    Email = "michael.williams@example.com"
                },
                new() 
                {
                    FirstName = "Sophia",
                    LastName = "Brown",
                    Email = "sophia.brown@example.com"
                },
                new()
                {
                    FirstName = "Priya",
                    LastName = "Patel",
                    Email = "priya.patel@example.com"
                },
                new() 
                {
                    FirstName = "Wei",
                    LastName = "Zhang",
                    Email = "wei.zhang@example.com"
                },
            };

            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }

        // Seed orders for Priya Patel if none exist
        if (!await context.Orders.AnyAsync())
        {
            // Get Priya Patel
            var customer = await context.Customers
                .FirstOrDefaultAsync(c => c.FirstName == "Priya" && c.LastName == "Patel");
                
            if (customer != null)
            {
                // Get some events
                var events = await context.Events.Take(5).ToListAsync();
                
                if (events.Any())
                {
                    var orders = new List<Order>();
                    
                    // Create payment info
                    var payment1 = new Payment 
                    {
                        PaymentMethod = "Credit Card",
                        CreditCardNumber = "************4567",
                        CreditCardType = "Visa",
                        CreditCardExpiry = new DateOnly(2026, 5, 1),
                        CreditCardName = "Priya Patel"
                    };
                    
                    var payment2 = new Payment 
                    {
                        PaymentMethod = "PayPal",
                        PaypalAddress = "priya.patel@example.com"
                    };
                    
                    // Order 1: Two tickets for first event
                    var order1 = new Order
                    {
                        CustomerDetails = customer,
                        PaymentInfo = payment1,
                        Status = OrderStatus.Paid,
                        DiscountCode = "SUMMER2025",
                        Tickets = new List<Ticket> 
                        {
                            new Ticket 
                            {
                                Event = events[0],
                                NumberOfSeats = 2,
                                Price = events[0].TicketPrice * 2
                            }
                        }
                    };
                    order1.TotalPrice = order1.Tickets.Sum(t => t.Price);
                    orders.Add(order1);
                    
                    // Order 2: One ticket for second event
                    var order2 = new Order
                    {
                        CustomerDetails = customer,
                        PaymentInfo = payment2,
                        Status = OrderStatus.Confirmed,
                        Tickets = new List<Ticket> 
                        {
                            new Ticket 
                            {
                                Event = events[1],
                                NumberOfSeats = 1,
                                Price = events[1].TicketPrice
                            }
                        }
                    };
                    order2.TotalPrice = order2.Tickets.Sum(t => t.Price);
                    orders.Add(order2);
                    
                    // Order 3: Multiple tickets for multiple events
                    var order3 = new Order
                    {
                        CustomerDetails = customer,
                        PaymentInfo = payment1,
                        Status = OrderStatus.Paid,
                        Tickets = new List<Ticket> 
                        {
                            new Ticket 
                            {
                                Event = events[2],
                                NumberOfSeats = 2,
                                Price = events[2].TicketPrice * 2
                            },
                            new Ticket 
                            {
                                Event = events[3],
                                NumberOfSeats = 1,
                                Price = events[3].TicketPrice
                            }
                        }
                    };
                    order3.TotalPrice = order3.Tickets.Sum(t => t.Price);
                    orders.Add(order3);
                    
                    // Order 4: Pending order
                    var order4 = new Order
                    {
                        CustomerDetails = customer,
                        PaymentInfo = payment2,
                        Status = OrderStatus.Pending,
                        Tickets = new List<Ticket> 
                        {
                            new Ticket 
                            {
                                Event = events[4],
                                NumberOfSeats = 4,
                                Price = events[4].TicketPrice * 4
                            }
                        }
                    };
                    order4.TotalPrice = order4.Tickets.Sum(t => t.Price);
                    orders.Add(order4);
                    
                    // Order 5: Cancelled order
                    var order5 = new Order
                    {
                        CustomerDetails = customer,
                        PaymentInfo = payment1,
                        Status = OrderStatus.Cancelled,
                        Tickets = new List<Ticket> 
                        {
                            new Ticket 
                            {
                                Event = events[0],
                                NumberOfSeats = 1,
                                Price = events[0].TicketPrice
                            }
                        }
                    };
                    order5.TotalPrice = order5.Tickets.Sum(t => t.Price);
                    orders.Add(order5);

                    await context.Orders.AddRangeAsync(orders);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
