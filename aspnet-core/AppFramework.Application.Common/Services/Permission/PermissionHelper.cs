using AppFramework.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AppFramework.Common.Services.Permission
{
    public static class PermissionHelper
    {
        /// <summary>
        /// 创建权限节点目录树
        /// </summary>
        /// <param name="flats"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
        public static ObservableCollection<FlatPermissionModel> CreateTrees(this List<FlatPermissionModel> flats, string parentName)
        {
            var trees = new ObservableCollection<FlatPermissionModel>();

            var nodes = flats.Where(q => q.ParentName == parentName).ToArray();

            foreach (var node in nodes)
            {
                node.Items = CreateTrees(flats, node.Name);
                trees.Add(node);
            }

            return trees;
        }

        /// <summary>
        /// 获取选中的权限节点
        /// </summary>
        /// <param name="nodes"></param>
        /// <param name="GrantedPermissionNames"></param>
        public static void GetSelectedNodes(this ObservableCollection<FlatPermissionModel> nodes,
            ref List<string> GrantedPermissionNames)
        {
            foreach (var flat in nodes)
            {
                if (flat.IsChecked) GrantedPermissionNames.Add(flat.Name);

                GetSelectedNodes(flat.Items, ref GrantedPermissionNames);
            }
        }

        /// <summary>
        /// 更新选中权限节点
        /// </summary>
        /// <param name="GrantedPermissionNames"></param>
        public static void UpdateSelectedNodes(this ObservableCollection<FlatPermissionModel> Flats, List<string> GrantedPermissionNames)
        {
            GrantedPermissionNames.ForEach(item =>
            {
                UpdateSelected(Flats, item);
            });

            void UpdateSelected(ObservableCollection<FlatPermissionModel> nodes, string nodeName)
            {
                foreach (var flat in nodes)
                {
                    if (flat.Name.Equals(nodeName))
                    {
                        flat.IsChecked = true;
                        return;
                    }
                    UpdateSelected(flat.Items, nodeName);
                }
            }
        }
    }
}
