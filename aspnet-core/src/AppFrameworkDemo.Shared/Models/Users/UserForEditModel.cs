using AppFrameworkDemo.Authorization.Users.Dto;
using AppFrameworkDemo.Organizations.Dto;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace AppFrameworkDemo.Shared.Models
{
    public class UserForEditModel : BindableBase
    {
        public Guid? ProfilePictureId { get; set; }

        public UserEditModel User { get; set; }

        public UserRoleDto[] Roles { get; set; }

        public List<OrganizationUnitDto> AllOrganizationUnits { get; set; }

        public List<string> MemberedOrganizationUnits { get; set; }

        private ImageSource _photo;
        private List<OrganizationUnitModel> _organizationUnits;

        public string FullName => User == null ? string.Empty : User.Name + " " + User.Surname;

        public DateTime CreationTime { get; set; }

        public bool IsEmailConfirmed { get; set; }

        public ImageSource Photo
        {
            get => _photo;
            set
            {
                _photo = value;
                RaisePropertyChanged();
            }
        }

        public List<OrganizationUnitModel> OrganizationUnits
        {
            get => _organizationUnits;
            set
            {
                _organizationUnits = value?.OrderBy(o => o.Code).ToList();
                SetAsAssignedForMemberedOrganizationUnits();
                RaisePropertyChanged();
            }
        }

        private void SetAsAssignedForMemberedOrganizationUnits()
        {
            if (_organizationUnits != null)
            {
                MemberedOrganizationUnits?.ForEach(memberedOrgUnitCode =>
                {
                    _organizationUnits
                        .Single(o => o.Code == memberedOrgUnitCode)
                        .IsAssigned = true;
                });
            }
        }
    }
}