using Abp.Application.Services;
using AppFrameworkDemo.MultiTenancy.Payments.Dto;
using AppFrameworkDemo.MultiTenancy.Payments.Stripe.Dto;
using System.Threading.Tasks;

namespace AppFrameworkDemo.MultiTenancy.Payments.Stripe
{
    public interface IStripePaymentAppService : IApplicationService
    {
        Task ConfirmPayment(StripeConfirmPaymentInput input);

        StripeConfigurationDto GetConfiguration();

        Task<SubscriptionPaymentDto> GetPaymentAsync(StripeGetPaymentInput input);

        Task<string> CreatePaymentSession(StripeCreatePaymentSessionInput input);
    }
}