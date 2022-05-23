﻿using AppFramework.Common;
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
    /// <summary>
    /// 编辑值视图模型
    /// </summary>
    public class DynamicEditValuesViewModel : HostDialogViewModel
    {
        private readonly IDynamicPropertyValueAppService appService;
        private readonly IApplicationContext context;
        public IDataPagerService dataPager { get; private set; }
        private readonly IHostDialogService dialog;
        private int Id;
        private bool isAdd;

        public bool IsAdd
        {
            get { return isAdd; }
            set { isAdd = value; RaisePropertyChanged(); }
        }

        private string inputValue;

        public string InputValue
        {
            get { return inputValue; }
            set { inputValue = value; RaisePropertyChanged(); }
        }

        public DelegateCommand ShowAddCommand { get; private set; }
        public DelegateCommand RefreshCommand { get; private set; }
        public DelegateCommand AddValueCommand { get; private set; }

        public DelegateCommand<DynamicPropertyValueDto> EditCommand { get; private set; }
        public DelegateCommand<DynamicPropertyValueDto> DeleteCommand { get; private set; }

        public DynamicEditValuesViewModel(
            IDynamicPropertyValueAppService appService,
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

            EditCommand = new DelegateCommand<DynamicPropertyValueDto>(Edit);
            DeleteCommand = new DelegateCommand<DynamicPropertyValueDto>(Delete);
        }

        /// <summary>
        /// 删除值
        /// </summary>
        /// <param name="obj"></param>
        private async void Delete(DynamicPropertyValueDto obj)
        {
            if (await dialog.Question(Local.Localize("DeleteDynamicPropertyValueMessage"), AppCommonConsts.DialogIdentifier))
            {
                await SetBusyAsync(async () =>
                {
                    await WebRequest.Execute(() =>
                            appService.Delete(obj.Id),
                            GetAllValuesOfDynamicProperty);
                });
            }
        }

        /// <summary>
        /// 编辑值
        /// </summary>
        /// <param name="obj"></param>
        private void Edit(DynamicPropertyValueDto obj)
        {
            InputValue = obj.Value;
        }

        /// <summary>
        /// 刷新列表
        /// </summary>
        private async void Refresh()
        {
            await GetAllValuesOfDynamicProperty();
        }

        /// <summary>
        /// 添加值
        /// </summary>
        private async void AddValue()
        {
            if (string.IsNullOrWhiteSpace(InputValue)) return;

            await SetBusyAsync(async () =>
             {
                 await WebRequest.Execute(() =>
                         appService.Add(new DynamicPropertyValueDto()
                         {
                             DynamicPropertyId = Id,
                             TenantId = context.CurrentTenant?.TenantId,
                             Value = InputValue,
                         }), GetAllValuesOfDynamicProperty);
             });
        }

        /// <summary>
        /// 获取动态属性的所有值
        /// </summary>
        /// <returns></returns>
        public async Task GetAllValuesOfDynamicProperty()
        {
            IsAdd = false;

            await SetBusyAsync(async () =>
              {
                  await WebRequest.Execute(() =>
                          appService.GetAllValuesOfDynamicProperty(new EntityDto(Id))
                          , async result =>
                          {
                              dataPager.SetList(new PagedResultDto<DynamicPropertyValueDto>()
                              {
                                  Items = result.Items
                              });
                              await Task.CompletedTask;
                          });
              });
        }

        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            if (parameters.ContainsKey("Value"))
            {
                Id = parameters.GetValue<DynamicPropertyDto>("Value").Id;

                await SetBusyAsync(GetAllValuesOfDynamicProperty);
            }
        }
    }
}
