using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.ViewModels
{
    public class MessageViewModel : DialogViewModel
    {
        private string title;

        public string Title
        {
            get { return title; }
            set { title = value; RaisePropertyChanged(); }
        }

        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; RaisePropertyChanged(); }
        }
    }
}
