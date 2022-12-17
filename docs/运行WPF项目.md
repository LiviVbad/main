## WPF 项目部署

当您打开服务器端解决方案 **（AppFramework.Wpf.sln）** 使用 **Visual Studio 2022**，您将看到解决方案结构如下：

<img src="C:\Github\AppFramework\docs\images\wpfsln.png" alt="image-20221217211605703" style="zoom:50%;" />

右键单击任意一种 UI 框架，然后选择“**设置为启动项目**”。然后**生成**解决方案。在第一次生成期间可能需要更长的时间，因为将还原所有 **nuget** 包。

### 服务地址配置 

在 **AppFramework.Application.Client** 项目中打开 **ApiUrlConfig** **。并根据需要更改**默认服务器地址：

<img src="C:\Github\AppFramework\docs\images\webapi.url.png" alt="image-20221217211834979" style="zoom:50%;" />**注**: 默认情况下, 服务器地址为 https://localhost:44301/ , 实际生产环境中, 请更改为实际的地址部署。

### 运行WPF项目

确保WebAPI启动的情况下， 运行WPF项目。您将看到如下所示的登录页面：

<img src="C:\Github\AppFramework\docs\images\wpf.login.png" alt="image-20221217212124407" style="zoom:50%;" />

**注意:** 默认的账号密码为 **admin** 、**123qwe**

**登录成功**后,  您可以看到应用程序主页：

![image-20221217212305353](C:\Github\AppFramework\docs\images\wpf.main.png)

## 探索WPF项目

- 启动流程

- 模块系统

- 多UI框架

- MVVM

- 本地化多语言

- 身份授权

- 自动更新

- 即时通讯

  