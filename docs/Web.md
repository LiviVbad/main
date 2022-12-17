## ASP.NET Core WebAPI 项目介绍

下图为ASP.NET Core WebAPI 解决方案的项目列表

![image-20221217185110381](C:\Github\AppFramework\docs\images\aspnetcore.web.sln.png)

解决方案中有 10 个项目：

- **AppFramework.Core.Shared** 项目包含 桌面、移动端和 Web 项目中使用的帮助程序类。
- **AppFramework.Core** 包含域图层类（如[实体](https://aspnetboilerplate.com/Pages/Documents/Entities)和[域服务](https://aspnetboilerplate.com/Pages/Documents/Domain-Services)）。
- **AppFramework.Application.Shared**  项目包含[应用程序服务接口](https://aspnetboilerplate.com/Pages/Documents/Application-Services#DocIApplicationServiceInterface)和 [DTO](https://aspnetboilerplate.com/Pages/Documents/Data-Transfer-Objects)。
- **AppFramework.Application** 项目包含应用程序逻辑（如[应用程序服务](https://aspnetboilerplate.com/Pages/Documents/Application-Services)）。
- **AppFramework.EntityFrameworkCore** 项目包含 DbContext、[存储库](https://aspnetboilerplate.com/Pages/Documents/Repositories)实现、数据库迁移和其他特定于实体框架核心的概念。
- **AppFramework.Web.Host **项目不包含任何与Web相关的文件，如HTML，CSS或JS。相反，它只是将应用程序作为远程 API 提供服务。因此，任何设备都可以将您的应用程序用作 API。有关更多信息，请参阅 [Web.Host 项目](https://docs.aspnetzero.com/en/aspnet-core-angular/latest/Features-Mvc-Core-Web-Host-Project)
- **AppFramework.Web.Core** 项目包含 MVC 和主机项目使用的常见类。
- **AppFramework.Web.Public** 项目是一个单独的 Web 应用程序，可用于为应用程序创建公共网站或登录页。有关详细信息，请参阅[网站](https://docs.aspnetzero.com/en/aspnet-core-angular/latest/Public-Website)。
- **AppFramework.Migrator** 项目是运行数据库**迁移**的控制台应用程序。有关详细信息，请参阅[迁移器控制台应用程序 

