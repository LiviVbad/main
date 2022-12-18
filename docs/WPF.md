## WPF 项目介绍

下图为 WPF 解决方案的项目列表

![image-20221217190738263](C:\Users\zhouh\AppData\Roaming\Typora\typora-user-images\image-20221217190738263.png)

解决方案中有 8 个项目：

- **AppFramework.Admin**  包含核心功能业务逻辑
- **AppFramework.Admin.HandyUI ** HandyControl UI的启动项目
- **AppFramework.Admin.MaterialUI ** MaterialDesign UI的启动项目
- **AppFramework.Admin.SyncUI**  Syncfusion UI 的启动项目
- **AppFramework.Application.Client**  访问Web服务核心类库
- **AppFramework.Application.Shared**  包含[应用程序服务接口](https://aspnetboilerplate.com/Pages/Documents/Application-Services#DocIApplicationServiceInterface)和 [DTO](https://aspnetboilerplate.com/Pages/Documents/Data-Transfer-Objects)。
- **AppFramework.Core.Shared** 项目包含 桌面、移动端和 Web 项目中使用的帮助程序类。
- **AppFramework.Shared**  包含WPF客户端的公共层



引用关系图如下所示:

<img src="C:\Github\AppFramework\docs\images\refurl.png" alt="image-20221218121609876" style="zoom:50%;" />



### AppFramework.Admdin 

该项目为核心的业务模块，项目结构如下图所示;

<img src="C:\Github\AppFramework\docs\images\appframework.admin.png" alt="image-20221218111905101" style="zoom:50%;" />

- **Behaviors**   包含项目中使用的部分Behavior类
- **Extensions**  包含项目中使用的扩展类库
- **Models**  包含用于业务模块中数据绑定的模型 (Model)
- **Services**  包含核心的服务模块 (授权、账户信息、导航、通知、权限服务)
- **Validations**  包含项目实体模型验证器
- **ViewModels**  包含项目中所有模块功能的视图模型 (ViewModel)



### AppFramework.Admin.HandyUI

该项目是基于HandyControl 为UI框架的启动项，项目结构如下图所示:

<img src="C:\Github\AppFramework\docs\images\handyUI.png" alt="image-20221218112605526" style="zoom:50%;" />

- **Assets**  包含项目中使用的字体库、图像资源
- **Converters**  包含视图层使用的转换器
- **Extensions** 包含视图层使用的扩展类库
- **Localization** 本地化的资源类库
- **Services**   项目中使用的服务
- **Themes**  基于HandyControl UI 的样式资源
- **Views**  基于HandyControl UI 的视图定义



### AppFramework.Admin.MaterialUI

该项目是基于MaterialDesign 为UI框架的启动项，项目结构如下图所示:

<img src="C:\Github\AppFramework\docs\images\materialUI.png" alt="image-20221218113543340" style="zoom:50%;" />

- **Assets**  包含项目中使用的字体库、图像资源
- **Converters**  包含视图层使用的转换器
- **Extensions** 包含视图层使用的扩展类库
- **Localization** 本地化的资源类库
- **Services**   项目中使用的服务
- **Themes**  基于MaterialDesign UI 的样式资源
- **Views**  基于MaterialDesign UI 的视图定义



### AppFramework.Admin.SyncUI

该项目是基于Syncfusion 为UI框架的启动项，项目结构如下图所示:

<img src="C:\Github\AppFramework\docs\images\syncfusionUI.png" alt="image-20221218113636159" style="zoom:50%;" />

- **Assets**  包含项目中使用的字体库、图像资源
- **Converters**  包含视图层使用的转换器
- **Extensions** 包含视图层使用的扩展类库
- **Localization** 本地化的资源类库
- **Services**   项目中使用的服务
- **Themes**  基于Syncfusion UI 的样式资源
- **Views**  基于Syncfusion UI 的视图定义



### AppFramework.Shared

该项目为 WPF 客户端的公共类库，其中包含了系统常量、常用的转换器、扩展方法、公共模型、公共服务、公共的资源样式以及基础的ViewModel 基类定义。

<img src="C:\Github\AppFramework\docs\images\appshared.png" alt="image-20221218113854343" style="zoom:50%;" />

- **Consts**  公共常量定义
- **Converters**  常用的视图转换器集合
- **Extensions**  各种扩展帮助类
- **Models** 公共模型类
- **Services**  公共层服务接口定义 (分页、多语言、本地存储、聊天)
- **Validations**  验证器接口定义
- **ViewModels** 视图模型的基础实现定义 

