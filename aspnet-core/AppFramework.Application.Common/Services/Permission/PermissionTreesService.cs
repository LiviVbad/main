﻿using AppFramework.Authorization.Permissions.Dto;
using AppFramework.Common.Models;
using AppFramework.Common.Services.Permission;
using AutoMapper;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AppFramework.Common
{
    public class PermissionTreesService : BindableBase, IPermissionTreesService
    {
        public PermissionTreesService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        private ObservableCollection<PermissionModel> permissions;

        public ObservableCollection<PermissionModel> Permissions
        {
            get { return permissions; }
            set { permissions = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<object> selectedItems;
        private readonly IMapper mapper;

        public ObservableCollection<object> SelectedItems
        {
            get { return selectedItems; }
            set { selectedItems = value; }
        }

        public void CreatePermissionTrees(List<FlatPermissionDto> permissions, List<string> grantedPermissionNames)
        {
            if (permissions == null)
                throw new NullReferenceException(nameof(permissions));

            if (grantedPermissionNames == null)
                throw new NullReferenceException(nameof(grantedPermissionNames));

            var flats = mapper.Map<List<PermissionModel>>(permissions);

            Permissions = flats.CreateTrees(null);
            SelectedItems = Permissions.GetSelectedItems(grantedPermissionNames);
        }

        public List<string> GetSelectedItems()
        {
            if (SelectedItems == null && SelectedItems.Count == 0) return null;

            return SelectedItems.Select(t => (t as PermissionModel)?.Name).ToList();
        }
    }
}
