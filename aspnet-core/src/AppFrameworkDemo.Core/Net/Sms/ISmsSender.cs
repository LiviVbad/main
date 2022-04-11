using System.Threading.Tasks;

namespace AppFrameworkDemo.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}