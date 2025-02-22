using GloboticketWeb.Models;

namespace GloboticketWeb.Services.Tests;

[TestFixture]
public class CustomerServiceTest
{
    [Test]
    public void CalculateOrderTotal_WithNoOrders_ReturnsZero()
    {
        // Arrange
        var orders = new List<Order>();

        // Act
        var total = CustomerService.CalculateOrderTotal(orders);

        // Assert
        Assert.That(total, Is.EqualTo(0m));
    }

    [Test]
    public void CalculateOrderTotal_WithNoPaidOrders_ReturnsZero()
    {
        // Arrange
        var orders = new List<Order>
        {
            CreateOrder(OrderStatus.Pending, 100m),
            CreateOrder(OrderStatus.Cancelled, 200m)
        };

        // Act
        var total = CustomerService.CalculateOrderTotal(orders);

        // Assert
        Assert.That(total, Is.EqualTo(0m));
    }

    private Order CreateOrder(OrderStatus orderStatus, decimal value)
    {
        var customer = new Customer() { Email = "customer@example.com", FirstName = "Alice", LastName = "Doe" };   
        var paymentInfo = new Payment() { PaymentMethod = "CreditCard", };
        return new Order { 
            Status = orderStatus, 
            TotalPrice = value, 
            CustomerDetails = customer, 
            PaymentInfo = paymentInfo };
    }

    [Test]
    public void CalculateOrderTotal_WithMixedOrders_ReturnsSumOfPaidOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            CreateOrder(OrderStatus.Paid, 100m),
            CreateOrder(OrderStatus.Pending, 50m),
            CreateOrder(OrderStatus.Paid, 200m),
        };

        // Act
        var total = CustomerService.CalculateOrderTotal(orders);

        // Assert
        Assert.That(total, Is.EqualTo(300m));
    }
}