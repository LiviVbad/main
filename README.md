## 项目说明

该分支为ABP版本11.2.1, 包含ASP.NET Core、Angular、WPF、Xamarin.Forms项目。

aspnet-core 文件夹中包含三个解决方案。 

- AppFramework.Mobile.sln   Xamarin.Forms移动端项目
- AppFramework.Web.sln     后端项目(WEBAPI)
- AppFramework.Wpf.sln    WPF客户端项目



## WPF项目说明

该项目结构包含如下:

- AppFramework   启动项目
- AppFramework.Admin   核心的业务实现模块
- AppFramework.Application.Client   Web服务器实现类库
- AppFramework.Application.Shared  Web服务接口以及实体模型
- AppFramework.Core.Shared   系统配置实体/常量
- AppFramework.Shared   核心业务的模块共享服务



**注意事项**: 

- 启动项目仅包含部分服务，如: 自动更新、样式资源服务、实体映射服务。

- Admin模块中包含了系统首页、授权、等核心功能, 如果想要为系统添加功能, 可以新建一个独立的模块进行实现，如果需要修改核心的业务模块, 在Admin中进行修改。
- Web服务器的请求地址修改, 在AppFramework.Application.Client类库中的 ApiUrlConfig 类文件中修改。