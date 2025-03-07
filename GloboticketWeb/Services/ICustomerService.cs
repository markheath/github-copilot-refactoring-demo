using GloboticketWeb.Models;
namespace GloboticketWeb.Services;

public interface ICustomerService
{
    Task<Customer> CreateCustomerAsync(string email, string firstName, string lastName);
    Task<List<string>> GetCustomerFavoriteArtists(string email);
}