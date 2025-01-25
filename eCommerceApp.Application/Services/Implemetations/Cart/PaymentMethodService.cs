using AutoMapper;
using eCommerceApp.Application.DTOs.Cart;
using eCommerceApp.Application.Services.Interfaces.Cart;
using eCommerceApp.Domain.Interfaces.Cart;

namespace eCommerceApp.Application.Services.Implemetations.Cart
{
    public class PaymentMethodService(IPaymentMethod paymentMethod
        , IMapper mapper) : IPaymentMethodService
    {
        public async Task<IEnumerable<GetPaymentMethod>> GetPaymentMethods()
        {
            var methods = await paymentMethod.GetPaymentMethods();
            return methods == null ? ([]) : mapper.Map<IEnumerable<GetPaymentMethod>>(methods);
        }
    }
}
