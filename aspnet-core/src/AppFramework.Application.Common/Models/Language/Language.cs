using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Common.Models
{
    public struct LanguageStruct
    {
        public LanguageStruct(string name, string displayName)
        {
            Name = name;
            DisplayName = displayName;
        }

        public string Name { get; }

        public string DisplayName { get; }
    }
}
