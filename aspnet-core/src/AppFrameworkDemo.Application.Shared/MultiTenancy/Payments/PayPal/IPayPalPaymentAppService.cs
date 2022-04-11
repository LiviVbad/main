using Abp.Application.Services;
using AppFrameworkDemo.MultiTenancy.Payments.PayPal.Dto;
using System.Threading.Tasks;

namespace AppFrameworkDemo.MultiTenancy.Payments.PayPal
{
    public interface IPayPalPaymentAppService : IApplicationService
    {
        Task ConfirmPayment(long paymentId, string paypalOrderId);

        PayPalConfigurationDto GetConfiguration();
    }
}