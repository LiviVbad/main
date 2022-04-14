using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework
{
    public static class PrismRegionManager
    {
        public static readonly string HomeRegion = "Home";

        /// <summary>
        /// 索引页区域
        /// </summary>
        public static readonly string IndexRegion = "Index";

        /// <summary>
        /// 主区域
        /// </summary>
        public static readonly string MainRegion = "Main";

        /// <summary>
        /// 主页
        /// </summary>
        public static readonly string MainView = "MainView";

        /// <summary>
        /// 索引页
        /// </summary>
        public static readonly string IndexView = "IndexView";

        /// <summary>
        /// 统计面板页
        /// </summary>
        public static readonly string DashboardView = "DashboardView";

        /// <summary>
        /// 登录页
        /// </summary>
        public static readonly string LoginView = "LoginView";
         
        /// <summary>
        /// 组织结构新增
        /// </summary>
        public static readonly string AddOrEditOrganizations = "OrganizationsAddView";

        /// <summary>
        /// 查找组织结构对应用户
        /// </summary>
        public static readonly string FindUser = "UserChooseView";

        /// <summary>
        /// 查找组织结构对应角色
        /// </summary>
        public static readonly string FindRole = "RoleChooseView";

        /// <summary>
        /// 添加用户
        /// </summary>
        public static readonly string UserOrEdit = "UserAddView";
    }
}
