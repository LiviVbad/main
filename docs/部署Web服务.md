##  Web服务部署指南

**ASP.NET Core** 运行环境要求：

- Visual Studio 2022  / .NET 6.0
- SqlServer  2012 以上 / MySql

**Angular** 项目要求:

- [Visual Studio 2017 (v15.9.0+)](https://www.visualstudio.com/)  
- [Node.js 6.9+ with NPM 3.10+](https://nodejs.org/en/download/)
- [Yarn](https://yarnpkg.com/)

## ASP.NET Core 应用程序部署

当您打开服务器端解决方案 **（AppFramework.Web.sln）** 使用 **Visual Studio 2022**，您将看到解决方案结构如下：

<img src="C:\Users\zhouh\AppData\Roaming\Typora\typora-user-images\image-20221217211345054.png" alt="image-20221217211345054" style="zoom:50%;" />

右键单击 **.Web.Host**项目，然后选择“**设置为启动项目**”。然后**生成**解决方案。在第一次生成期间可能需要更长的时间，因为将还原所有 **nuget** 包。

### 数据库配置 

在 **Web.Host** 项目中打开 **appsettings.json** **。并根据需要更改**默认数据库连接字符串：

````c#
"ConnectionStrings": { 
    "Default": "Server=localhost; Database=AppFrameworkDb; Trusted_Connection=True;"
  }
````

### 迁移数据库

在 Visual Studio 中打开**程序包管理器控制台**，设置 **EntityFrameworkCore** 作为**默认项目**，并运行 **Update-Database** 命令，如下所示：

![image-20221217210301724](C:\Github\AppFramework\docs\images\update.database.png)

此命令将创建数据库。运行Web.Host 项目时将插入初始数据。可以打开 SQL Server Management Studio 以检查是否已创建数据库： 

![image-20221217210522535](C:\Github\AppFramework\docs\images\mssql.png)

### 运行WebAPI项目

完成配置后，即可运行应用程序。服务器端应用程序仅包含 API。当您启动应用程序时，您将看到如下所示的登录页面：

<img src="C:\Github\AppFramework\docs\images\webapi.login.png" alt="image-20221217210732691" style="zoom: 33%;" />

如果启用了多租户，你将看到当前租户和更改链接。如果是这样，请单击“**更改**”并输入**默认值**作为租户名称。如果将其留空，则以主机管理员用户身份登录。然后输入 **admin** 作为用户名，**输入 123qwe** 作为密码（请记住在生产中将其更改为更安全的密码！)

当您导航 **Swagger UI** 时，您将看到以下页面：

<img src="C:\Github\AppFramework\docs\images\openapi.png" alt="image-20221217211017644" style="zoom:50%;" />

## Angular 项目部署

### 还原包

导航到 **angular** 文件夹，打开命令行并运行以下命令以还原包：

````
yarn
````

然后，运行以下命令以创建动态分发包*（仅在首次下载项目或更新动态分发包时需要执行此操作）：*

````
npm run create-dynamic-bundles
````

### 运行应用程序

在命令行中运行以下命令：

````
npm start
````

应用程序编译完成后，您可以在浏览器中浏览 [http://localhost:4200](http://localhost:4200/)。ASP.NET Zero还启用了**HMR**（热模块更换）。您可以使用以下命令（而不是 NPM 启动）在开发时启用 HMR：

````
npm run hmr
````

### 登录

都准备好了！现在，您可以登录到应用程序。
