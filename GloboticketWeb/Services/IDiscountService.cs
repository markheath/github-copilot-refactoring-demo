namespace GloboticketWeb.Services;

public interface IDiscountService
{
    bool IsValidDiscountCode(string discountCode);
    decimal ApplyDiscount(decimal price, string discountCode);
}
