using System.Threading.Tasks;

namespace AppFrameworkDemo.Shared.Services.Storage
{
    /// <summary>
    /// ���ݴ洢����
    /// ˵��: ���ڱ�������ö�Ƴ��洢����
    /// </summary>
    public interface IDataStorageService
    {
        bool ContainsKey(string key);

        T GetValue<T>(string key, T defaultValue = default(T), bool shouldDecrpyt = false);

        Task SetValueAsync<T>(string key, T value, bool shouldEncrypt = false);

        void RemoveIfExists(string key);
    }
}