using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Version.Dtos
{
    public class UpdateFileOutput
    {
        public bool IsNewVersion { get; set; }

        public string DownloadURL { get; set; }

        public string ChangelogURL { get; set; }

        public bool IsForced { get; set; }

        public string Version { get; set; }
    }
}
