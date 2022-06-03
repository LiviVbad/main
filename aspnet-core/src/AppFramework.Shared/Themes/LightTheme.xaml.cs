﻿using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace AppFramework.Shared
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LightTheme
    {
        public LightTheme()
        {
            this.InitializeComponent();
        }
    }
}