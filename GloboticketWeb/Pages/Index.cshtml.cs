using GloboticketWeb.Models;
using GloboticketWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GloboticketWeb.Pages;

public class IndexModel : PageModel
{
    private readonly IEventService _eventService;
    private readonly IBasketService _basketService;

    public List<Event> Events { get; set; }
    [BindProperty]
    public Dictionary<int, int> TicketQuantities { get; set; } = new();

    public IndexModel(IEventService eventService, IBasketService basketService)
    {
        _eventService = eventService;
        _basketService = basketService;
    }

    public async Task OnGetAsync()
    {
        Events = await _eventService.GetTopEventsAsync();
    }

    public async Task<IActionResult> OnPostAddToBasketAsync()
    {
        foreach (var (eventId, quantity) in TicketQuantities)
        {
            if (quantity > 0)
            {
                var sessionId = "1";
                await _basketService.AddToBasket(sessionId, eventId, quantity);
            }
        }
        return RedirectToPage("/Basket");
    }
}