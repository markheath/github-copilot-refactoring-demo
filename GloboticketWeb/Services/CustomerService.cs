using System.ComponentModel;
using GloboticketWeb.Models;
using Microsoft.EntityFrameworkCore;
namespace GloboticketWeb.Services;

public class CustomerService : ICustomerService
{
    private readonly GloboticketDbContext _context;

    public CustomerService(GloboticketDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> GetCustomerAsync(string email)
    {
        var customer = await _context.Customers.Where(c => c.Email == email).FirstOrDefaultAsync();
        if (customer == null)
        {
            throw new ArgumentException($"Customer with email {email} not found");
        }
        return customer;
    }

    public async Task<Customer> CreateCustomerAsync(string email, string firstName, string lastName)
    {
        var customer = new Customer
        {
            Email = email,
            FirstName = firstName,
            LastName = lastName,
        };

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return customer;
    }

    public async Task<List<string>> GetCustomerFavoriteArtists(string email)
    {
        var artists = new HashSet<string>();
        var orders =  await _context.Orders
            .Where(o => o.CustomerDetails.Email == email)
            .Include(o => o.Tickets)
            .ThenInclude(t => t.Event)
            .ToListAsync();

        foreach(var order in orders)
        {
            foreach(var ticket in order.Tickets)
            {
                artists.Add(ticket.Event.Artist);
            }
        }
        return artists.ToList();
    }

    // get customer orders
    public async Task<List<Order>> GetCustomerOrdersAsync(string email)
    {
        return await _context.Orders
            .Include(o => o.Tickets)
            .Where(o => o.CustomerDetails.Email == email)
            .ToListAsync();
    }

    public static decimal CalculateOrderTotal(List<Order> orders)
    {
        return orders
            .Where(order => order.Status == OrderStatus.Paid)
            .Sum(order => order.TotalPrice);
    }
}
