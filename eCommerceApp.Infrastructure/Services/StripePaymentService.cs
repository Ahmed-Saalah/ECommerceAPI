using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Cart;
using eCommerceApp.Application.Services.Interfaces.Cart;
using eCommerceApp.Domain.Entities;
using Stripe.Checkout;

namespace eCommerceApp.Infrastructure.Services
{
    public class StripePaymentService : IPaymentService
    {
        public async Task<ServiceResponse> Pay(decimal totalAmount, IEnumerable<Product> cartProducts, IEnumerable<ProcessCart> carts)
        {
            try
            {
                var lineItems = new List<SessionLineItemOptions>();
                foreach (var product in cartProducts)
                {
                    var productQuantity = carts.FirstOrDefault(c => c.ProductId == product.Id);
                    lineItems.Add(new SessionLineItemOptions
                    {
                        PriceData = new SessionLineItemPriceDataOptions
                        {
                            Currency = "usd",
                            ProductData = new SessionLineItemPriceDataProductDataOptions
                            {
                                Name = product.Name,
                                Description = product.Description
                            },
                            UnitAmount = (long)(product.Price * 100)
                        },
                        Quantity = productQuantity!.Quantity
                    });
                }

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = ["usd"],
                    LineItems = lineItems,
                    Mode = "payment",
                    SuccessUrl = "http:localhost:5024/payment-success",
                    CancelUrl = "http:localhost:5024/payment-cancel"
                };

                var service = new SessionService();
                Session session = await service.CreateAsync(options);
                return new ServiceResponse(true, session.Url);
            }
            catch (Exception ex)
            {
                return new ServiceResponse(false, ex.Message);
            }
        }
    }
}
