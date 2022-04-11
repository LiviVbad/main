using System.Threading.Tasks;

namespace AppFrameworkDemo.Shared.Services.Storage
{
    /// <summary>
    /// 数据存储服务
    /// 说明: 用于保存或设置额及移除存储数据
    /// </summary>
    public interface IDataStorageService
    {
        bool ContainsKey(string key);

        T GetValue<T>(string key, T defaultValue = default(T), bool shouldDecrpyt = false);

        Task SetValueAsync<T>(string key, T value, bool shouldEncrypt = false);

        void RemoveIfExists(string key);
    }
}