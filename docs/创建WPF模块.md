##  如何创建一个WPF模块？

本文主要讲解如何在框架当中, 新建一个WPF模块，主要分为两个部分:

#### 后端

- 创建表模型 并在 EntityFramework Core中添加表定义
- 创建迁移文件以及更新数据库  add-migration  / update-database
- 添加当前表对应的模块以及功能权限
- 创建接口以及实体模型
- 实现接口以及数据映射模型配置
- 添加对应功能的多语言翻译

### WPF客户端

- 实现接口用于Web请求服务

- 创建视图(View) 以及 视图模型(ViewModel)

- 容器中注入Web服务接口以及 视图(View)

- 添加系统导航的菜单定义

- 添加对应的权限定义 (与后端的定义相同)

  

## 演示

#### 1.创建后端服务。

**第一步**首先在 **AppFramework.Core** 项目中创建名为 Demo文件夹，里面创建一个表名为 **AbpDemo **，如下:

````C#
    [Table("AbpDemos")]
    public class AbpDemo : Entity, IMayHaveTenant
    {
        public int? TenantId { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        public string Address { get; set; }
    }
````

> IMayHaveTenant 为多租户接口

**第二步**在 **AppFramework.EntityFrameworkCore** 项目当中找到 AppFrameworkDbContext 类， 新增 AbpDemo表定义, 如下:

````c#
 public class AppFrameworkDbContext : AbpZeroDbContext<Tenant, Role, User, AppFrameworkDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<AbpDemo> AbpDemos { get; set; }
    
        //.....
    }
````

**第三步**打开 工具 > NuGet包管理器 > 程序包管理器控制台 , 将默认项目设置设置为  EntityFrameworkCore，

依次输入:

````C#
add-migration xxxx
````

> xxxx 是根据你实际输入

````C#
update-database
````

> 生成完成后, 检查表是否已生成至数据库当中。

**第四步**, 打开 AppFramework.Core > Authorization ，找到 AppAuthorizationProvider 、AppPermissions。

在AppPermissions 添加权限的名称常量定义, 如下:

````C#
 		public const string Pages_AbpDemos = "Pages.AbpDemos";
        public const string Pages_AbpDemos_Create = "Pages.AbpDemos.Create";
        public const string Pages_AbpDemos_Edit = "Pages.AbpDemos.Edit";
        public const string Pages_AbpDemos_Delete = "Pages.AbpDemos.Delete";
````

在AppAuthorizationProvider  添加权限的子节点, 如下:

````C#
 var abpDemos = pages.CreateChildPermission(AppPermissions.Pages_AbpDemos, L("AbpDemos"));
            abpDemos.CreateChildPermission(AppPermissions.Pages_AbpDemos_Create, L("CreateNewAbpDemo"));
            abpDemos.CreateChildPermission(AppPermissions.Pages_AbpDemos_Edit, L("EditAbpDemo"));
            abpDemos.CreateChildPermission(AppPermissions.Pages_AbpDemos_Delete, L("DeleteAbpDemo"));
````

**第五步**，在前面几个步骤中, 已经完成了对数据库表以及权限定义的操作, 接下来主要是创建用于Web服务的接口，以及接口实现。在 AppFramework.Application.Shared 项目中, 创建Demo文件夹， 添加 IAbpDemoAppService 接口 以及Dtos文件夹，接口定义如下:

````C#
	public interface IAbpDemoAppService : IApplicationService
    {
        Task<PagedResultDto<AbpDemoDto>> GetAll(GetAllAbpDemoInput input);
    }
````

在Dtos中 定义 AbpDemoDto ,GetAllAbpDemoInput ，如下:

````C#
	public class AbpDemoDto : EntityDto
    {
        public string Name { get; set; }

        public string Sex { get; set; }

        public string Address { get; set; }
    }

	public class GetAllAbpDemoInput : PagedAndSortedResultRequestDto
    {
        public string Filter { get; set; }

        public string NameFilter { get; set; }
    }
````

**第六步**，在 AppFramework.Application 项目中，创建Demo文件夹，添加AbpDemoAppService，如下:

通过构造函数, 注入了AbpDemo表的仓储服务，用于访问数据库。

````C#
    [AbpAuthorize(AppPermissions.Pages_AbpDemos)]
    public class AbpDemoAppService : AppFrameworkAppServiceBase, IAbpDemoAppService
    {
        private readonly IRepository<AbpDemo> _repository;

        public AbpDemoAppService(IRepository<AbpDemo> repository)
        {
            _repository = repository;
        }

        public async Task<PagedResultDto<AbpDemoDto>> GetAll(GetAllAbpDemoInput input)
        {
            var filteredAbpModels = _repository.GetAll()
                       .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Name.Contains(input.Filter) || e.Address.Contains(input.Filter))
                       .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter);

            var pagedAndFilteredAbpVersions = filteredAbpModels
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var totalCount = await filteredAbpModels.CountAsync();
            var dbList = await pagedAndFilteredAbpVersions.ToListAsync();
            var results = ObjectMapper.Map<List<AbpDemoDto>>(dbList);

            return new PagedResultDto<AbpDemoDto>(totalCount, results);
        }
    }
````

> 该类继承于 AppFrameworkAppServiceBase, 包含ABP内部提供的用户、租户等信息

在该类中, 使用了 ObjectMapper 将AbpDemo 转换成AbpDemoDto, 所以我们还需要在 AppFramework.Application 项目当中的  CustomDtoMapper中添加 表的转换映射关系，如下:

````C#
 internal static class CustomDtoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<AbpDemoDto, AbpDemo>().ReverseMap();
            
            //....
        }
    }
````

**第七步**，通过上面的步骤，已经完成了对单个表的访问数据功能, 还有涉及到多语言的设置, 需要在 AppFramework.Core > Localization > AppFramework 当中进行设置， 如下所示:

````C#
    <text name="DemoManager">演示</text>
    <text name="DemoManagerInfo">管理演示信息</text>
    <text name="DemoName">姓名</text>
    <text name="DemoSex">性别</text>
    <text name="DemoAddress">地址</text>
    <text name="CreateNewAbpDemo">创建新数据</text>
    <text name="EditAbpDemo">编辑</text>
    <text name="DeleteAbpDemo">删除</text>
````

> 需要涉及多语言的定义, 都可以在不同的国家语言中进行添加配置，表字段多语言，标题等。

完成后, 启动Web.Host,  通过Swagger 查看接口已经显示在API列表当中，如下:

<img src=".\images\openapi-demo.png" alt="image-20230212123325608" style="zoom:50%;" />

#### 2.创建WPF客户端

