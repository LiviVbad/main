using Abp.Application.Services.Dto;

namespace AppFrameworkDemo.Authorization.Users.Dto
{
    public interface IGetLoginAttemptsInput : ISortedResultRequest
    {
        string Filter { get; set; }
    }
}