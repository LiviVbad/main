using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using AppFrameworkDemo.MultiTenancy.Accounting.Dto;

namespace AppFrameworkDemo.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
