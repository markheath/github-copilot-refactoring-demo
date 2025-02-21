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
}
