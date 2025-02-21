using GloboticketWeb.Models;
using GloboticketWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GloboticketWeb.Pages;

public class BasketModel : PageModel
{
    private readonly IBasketService _basketService;

    [BindProperty]
    public List<Ticket> BasketItems { get; set; } = new();
    public decimal BasketTotal { get; set; }

    public BasketModel(IBasketService basketService)
    {
        _basketService = basketService;
    }

    public async Task OnGetAsync()
    {
        await LoadBasket();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        // Handle quantity updates
        var sessionId = "1";
        await _basketService.ClearBasket(sessionId);
        foreach (var item in BasketItems)
        {
            await _basketService.AddToBasket(sessionId, item.EventId, item.NumberOfSeats);
        }
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostRemoveAsync(int ticketId)
    {
        await _basketService.RemoveFromBasket("1", ticketId);
        return RedirectToPage();
    }

    public IActionResult OnPostCheckout()
    {
        return RedirectToPage("/Checkout");
    }

    private async Task LoadBasket()
    {
        var sessionId = "1";
        BasketItems = await _basketService.GetBasketItems(sessionId);
        BasketTotal = await _basketService.GetBasketTotal(sessionId);
    }
}
