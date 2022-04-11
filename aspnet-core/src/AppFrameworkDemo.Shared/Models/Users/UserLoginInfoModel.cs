﻿using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppFrameworkDemo.Shared.Models
{
    public class UserLoginInfoModel : BindableBase
    {
        private string name;
        private string surname;
        private string userName;
        private string emailAddress;

        public string Name
        {
            get { return name; }
            set { name = value; RaisePropertyChanged(); }
        }

        public string Surname
        {
            get { return surname; }
            set { surname = value; RaisePropertyChanged(); }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; RaisePropertyChanged(); }
        }

        public string EmailAddress
        {
            get { return emailAddress; }
            set { emailAddress = value; RaisePropertyChanged(); }
        }

        public string ProfilePictureId { get; set; }
    }
}
