using Abp.Application.Services.Dto;
using AppFramework.Common.Models;
using AppFramework.Editions.Dto;
using AutoMapper;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AppFramework.Common.Services
{
    public class FeaturesService : BindableBase, IFeaturesService
    {
        private readonly IMapper mapper;

        public FeaturesService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        private ObservableCollection<FlatFeatureModel> features;

        public ObservableCollection<FlatFeatureModel> Features
        {
            get { return features; }
            set { features = value; RaisePropertyChanged(); }
        }

        private ObservableCollection<object> selectedItems;

        public ObservableCollection<object> SelectedItems
        {
            get { return selectedItems; }
            set { selectedItems = value; }
        }

        public void CreateFeatures(List<FlatFeatureDto> features, List<NameValueDto> featureValues)
        {
            if (features == null)
                throw new NullReferenceException(nameof(features));

            if (featureValues == null)
                throw new NullReferenceException(nameof(featureValues));

            var flats = mapper.Map<List<FlatFeatureModel>>(features);

            Features = CreateFeatureTrees(flats, null);
            SelectedItems = GetSelectedItems(Features, featureValues);
        }

        public List<string> GetSelectedItems()
        {
            if (SelectedItems == null && SelectedItems.Count == 0) return null;

            return SelectedItems.Select(t => (t as FlatFeatureModel)?.Name).ToList();
        }

        /// <summary>
        /// 创建功能结点目录树
        /// </summary>
        /// <param name="flats"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
        private ObservableCollection<FlatFeatureModel> CreateFeatureTrees(List<FlatFeatureModel> flatFeatureModels, string parentName)
        {
            var trees = new ObservableCollection<FlatFeatureModel>();
            var nodes = flatFeatureModels.Where(q => q.ParentName == parentName).ToArray();

            foreach (var node in nodes)
            {
                node.Items = CreateFeatureTrees(flatFeatureModels, node.Name);
                trees.Add(node);
            }
            return trees;
        }

        private ObservableCollection<object> GetSelectedItems(ObservableCollection<FlatFeatureModel> features, List<NameValueDto> featureValues)
        {
            var items = new ObservableCollection<object>();
            foreach (var item in featureValues)
            {
                var permItem = GetSelectedItems(features, item);
                if (permItem != null) items.Add(permItem);
            }
            return items;
        }

        FlatFeatureModel GetSelectedItems(ObservableCollection<FlatFeatureModel> flatFeatures, NameValueDto nameValue)
        {
            FlatFeatureModel model = null;

            foreach (var flat in flatFeatures)
            {
                if (flat.Name.Equals(nameValue.Name) && flat.Items.Count == 0)
                {
                    bool isAdd = false;
                    if (bool.TryParse(nameValue.Value, out bool result))
                        isAdd = result;
                    else
                        isAdd = true;

                    if (isAdd)
                    {
                        model = flat;
                        break;
                    }
                }
                model = GetSelectedItems(flat.Items, nameValue);

                if (model != null) break;
            }
            return model;
        }
    }
}
