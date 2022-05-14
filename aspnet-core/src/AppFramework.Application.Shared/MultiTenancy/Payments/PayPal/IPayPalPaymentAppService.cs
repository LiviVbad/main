using Abp.Application.Services;
using AppFramework.MultiTenancy.Payments.PayPal.Dto;
using System.Threading.Tasks;

namespace AppFramework.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}