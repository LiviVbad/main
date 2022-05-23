using AppFramework.Common;
using AppFramework.DynamicEntityProperties;
using AppFramework.DynamicEntityProperties.Dto;
using Prism.Commands;
using Prism.Services.Dialogs;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using AppFramework.ApiClient;
using AppFramework.Services;

namespace AppFramework.ViewModels
{
    public class DynamicEntityDetailsViewModel : HostDialogViewModel
    {
        private readonly IDynamicEntityPropertyAppService appService;
        private readonly IApplicationContext context;
        private readonly IDataPagerService dataPager;
        private readonly IHostDialogService dialog;
        private int Id;
        private string EntityFullName;
        private bool isAdd;

        public bool IsAdd
        {
            get { return isAdd; }
            set { isAdd = value; RaisePropertyChanged(); }
        } 

        public DelegateCommand ShowAddCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand AddValueCommand { get; private set; }

        public DelegateCommand<DynamicEntityPropertyDto> DeleteCommand { get; private set; }

        public DynamicEntityDetailsViewModel(
            IDynamicEntityPropertyAppService appService,
            IApplicationContext context,
            IDataPagerService dataPager,
            IHostDialogService dialog)
        {
            this.appService = appService;
            this.context = context;
            this.dataPager = dataPager;
            this.dialog = dialog;
            RefreshCommand = new DelegateCommand(Refresh);
            ShowAddCommand = new DelegateCommand(() => IsAdd = true);
            AddValueCommand = new DelegateCommand(AddValue);
            DeleteCommand = new DelegateCommand<DynamicEntityPropertyDto>(Delete);
        }

        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="obj"></param>
        private async void Delete(DynamicEntityPropertyDto obj)
        {
            if (await dialog.Question("", AppCommonConsts.DialogIdentifier))
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() =>
                            appService.Delete(obj.Id),
                            GetAllPropertiesOfAnEntity);
                });
            }
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        private async void Refresh()
        {
            await GetAllPropertiesOfAnEntity();
        }

        /// <summary>
        /// 添加值
        /// </summary>
        private async void AddValue()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() =>
                        appService.Add(new DynamicEntityPropertyDto()
                        {
                            DynamicPropertyId = Id,
                            TenantId = context.CurrentTenant?.TenantId,
                            DynamicPropertyName = "",
                            EntityFullName = ""
                        }), GetAllPropertiesOfAnEntity);
            });
        }
         
        public async Task GetAllPropertiesOfAnEntity()
        {
            await SetBusyAsync(async () =>
            {
                await WebRequest.Execute(() =>
                        appService.GetAllPropertiesOfAnEntity(new DynamicEntityPropertyGetAllInput()
                        {
                            EntityFullName = EntityFullName,
                        })
                        , async result =>
                        {
                            dataPager.SetList(new PagedResultDto<DynamicEntityPropertyDto>()
                            {
                                Items = result.Items
                            });
                            await Task.CompletedTask;
                        });
            });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Id"))
            {
                Id = parameters.GetValue<int>("Id");
                EntityFullName = parameters.GetValue<string>("Name");

                await GetAllPropertiesOfAnEntity();
            }
        }
    }
}
