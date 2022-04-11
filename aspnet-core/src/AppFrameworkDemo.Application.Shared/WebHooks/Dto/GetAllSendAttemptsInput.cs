using AppFrameworkDemo.Dto;

namespace AppFrameworkDemo.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}