using AppFramework.Shared.Models;
using System;

namespace AppFramework.Models
{
    public class RoleListModel : EntityObject
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public bool IsStatic { get; set; }

        public bool IsDefault { get; set; }

        public DateTime CreationTime { get; set; }
    }
}