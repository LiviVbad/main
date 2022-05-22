using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class SettingsViewModel : NavigationViewModel
    {
        public DelegateCommand SaveCommand { get; private set; }

        public SettingsViewModel()
        {
            SaveCommand = new DelegateCommand(Save);
        }

        private void Save()
        { 

        }
    }
}
