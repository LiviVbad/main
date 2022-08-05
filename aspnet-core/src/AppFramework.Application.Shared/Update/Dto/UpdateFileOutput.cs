using System;
using System.Collections.Generic;
using System.Text;

namespace AppFramework.Update.Dto
{
    public class UpdateFileOutput
    {
        public string DownloadURL { get; set; }

        public string ChangelogURL { get; set; }

        public bool IsForced { get; set; }

        public string Version { get; set; }
    }
}
