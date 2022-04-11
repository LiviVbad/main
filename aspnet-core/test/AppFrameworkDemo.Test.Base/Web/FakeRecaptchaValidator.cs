using System.Threading.Tasks;
using AppFrameworkDemo.Security.Recaptcha;

namespace AppFrameworkDemo.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
