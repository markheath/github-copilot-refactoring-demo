using Azure.Storage.Blobs;
using GloboticketWeb.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AzureStorage");

// Add services to the container
builder.Services.AddRazorPages();
builder.Services.AddDbContext<GloboticketDbContext>();
//builder.Services.AddDbContext<GloboticketDbContext>(options =>
//    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSession();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<IBasketService, BasketService>();
// Add this line to your service registration
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddHttpClient<IPaymentProcessor, PaymentProcessor>();
builder.Services.AddSingleton(x => new BlobServiceClient(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

await EventDataSeeder.SeedEvents(app.Services);
await EventDataSeeder.SeedCustomersAndOrders(app.Services);

// test GetCustomerFavoriteArtists
using (var scope = app.Services.CreateScope())
{
    var customerService = scope.ServiceProvider.GetRequiredService<ICustomerService>();
    var favoriteArtists = await customerService.GetCustomerFavoriteArtists("priya.patel@example.com");
    Console.WriteLine($"Favorite artists for customer: {string.Join(", ", favoriteArtists)}");
}

app.Run();
