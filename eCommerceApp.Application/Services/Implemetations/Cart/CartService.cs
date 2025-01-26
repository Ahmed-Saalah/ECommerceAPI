using AutoMapper;
using eCommerceApp.Application.DTOs;
using eCommerceApp.Application.DTOs.Cart;
using eCommerceApp.Application.Services.Interfaces.Cart;
using eCommerceApp.Domain.Entities;
using eCommerceApp.Domain.Entities.Cart;
using eCommerceApp.Domain.Interfaces;
using eCommerceApp.Domain.Interfaces.Cart;

namespace eCommerceApp.Application.Services.Implemetations.Cart
{
    public class CartService(ICart cart, IMapper mapper, IGeneric<Product> productInterface, 
        IPaymentMethodService paymentMethodService, IPaymentService paymentService) : ICartService
    {
        public async Task<ServiceResponse> SaveCheckoutHistory(IEnumerable<CreateAchieve> Achieves)
        {
            var mappedData = mapper.Map<IEnumerable<Achieve>>(Achieves);
            var result = await cart.SaveCheckoutHistory(mappedData);
            if (result > 0)
            {
                return new ServiceResponse(true, "Checkout achieved");
            }
            else
            {
                return new ServiceResponse(false, "Error occured while saving");
            }
        }

        public async Task<ServiceResponse> Checkout(Checkout checkout)
        {
            var (products, totalAmount) = await GetCartAmount(checkout.Carts);
            var paymentMethod = await paymentMethodService.GetPaymentMethods();

            return checkout.PaymentMethodId == paymentMethod.FirstOrDefault()!.Id
                ? await paymentService.Pay(totalAmount, products, checkout.Carts)
                : new ServiceResponse(false, "Invalid payment method");
        }

        private async Task<(IEnumerable<Product>, decimal)> GetCartAmount(IEnumerable<ProcessCart> ProcessCarts)
        {
            if (!ProcessCarts.Any())
                return ([], 0);

            var products = await productInterface.GetAllAsync();
            if (!products.Any())
                return ([], 0);

            var cartProducts = ProcessCarts
                .Select(cartItem => products.FirstOrDefault(p => p.Id == cartItem.ProductId))
                .Where(Product => Product != null)
                .ToList();

            var totalAmount = ProcessCarts
                .Where(cartItem => cartProducts.Any(p => p.Id == cartItem.ProductId))
                .Sum(cartItem => cartItem.Quantity * cartProducts.FirstOrDefault(p => p.Id == cartItem.ProductId)!.Price);

            return (cartProducts!, totalAmount);
        }
    }
}
