using AppFramework.ApiClient;
using AppFramework.ApiClient.Models;
using AppFramework.Sessions.Dto; 
using AutoMapper;
using System.Threading.Tasks;
using AppFramework.Common.Services.Storage;
using AppFramework.Common.Models;

namespace AppFramework.Services.Account
{
    public class AccountStorageService : IAccountStorageService
    {
        private readonly IDataStorageService _dataStorageManager;
        private readonly IMapper mapper;

        public AccountStorageService(
            IDataStorageService dataStorageManager,
            IMapper mapper)
        {
            _dataStorageManager = dataStorageManager;
            this.mapper = mapper;
        }

        public async Task StoreAccessTokenAsync(string newAccessToken)
        {
            var authenticateResult = _dataStorageManager.GetValue<AuthenticateResultPersistanceModel>(DataStorageKey.CurrentSession_TokenInfo);

            authenticateResult.AccessToken = newAccessToken;

            await _dataStorageManager.SetValueAsync(DataStorageKey.CurrentSession_TokenInfo, authenticateResult);
        }

        public AbpAuthenticateResultModel RetrieveAuthenticateResult()
        {
            return mapper.Map<AbpAuthenticateResultModel>(
                _dataStorageManager.GetValue<AuthenticateResultPersistanceModel>(
                    DataStorageKey.CurrentSession_TokenInfo));
        }

        public async Task StoreAuthenticateResultAsync(AbpAuthenticateResultModel authenticateResultModel)
        {
            await _dataStorageManager.SetValueAsync(
                DataStorageKey.CurrentSession_TokenInfo,
                mapper.Map<AuthenticateResultPersistanceModel>(authenticateResultModel)
            );
        }

        public TenantInformation RetrieveTenantInfo()
        {
            return mapper.Map<TenantInformation>(
                _dataStorageManager.GetValue<TenantInformationPersistanceModel>(
                    DataStorageKey.CurrentSession_TenantInfo
                )
            );
        }

        public async Task StoreTenantInfoAsync(TenantInformation tenantInfo)
        {
            await _dataStorageManager.SetValueAsync(
                DataStorageKey.CurrentSession_TenantInfo,
                mapper.Map<TenantInformationPersistanceModel>(tenantInfo)
            );
        }

        public GetCurrentLoginInformationsOutput RetrieveLoginInfo()
        {
            return mapper.Map<GetCurrentLoginInformationsOutput>(
                _dataStorageManager.GetValue<CurrentLoginInformationPersistanceModel>(
                    DataStorageKey.CurrentSession_LoginInfo
                )
            );
        }

        public async Task StoreLoginInformationAsync(GetCurrentLoginInformationsOutput loginInfo)
        {
            var value = mapper
                .Map<CurrentLoginInformationPersistanceModel>(loginInfo);

            await _dataStorageManager.SetValueAsync(
                DataStorageKey.CurrentSession_LoginInfo, value);
        }

        public void ClearSessionPersistance()
        {
            _dataStorageManager.RemoveIfExists(DataStorageKey.CurrentSession_TokenInfo);
            _dataStorageManager.RemoveIfExists(DataStorageKey.CurrentSession_TenantInfo);
            _dataStorageManager.RemoveIfExists(DataStorageKey.CurrentSession_LoginInfo);
        }
    }
}