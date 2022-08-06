using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Common.Models
{
    public class VersionListModel: EntityObject
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string DownloadUrl { get; set; }

        public string ChangelogUrl { get; set; }

        public bool IsEnable { get; set; }

        public bool IsForced { get; set; }
    }
}
