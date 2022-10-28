using AppFramework.Shared.Models.Chat;
using AutoUpdaterDotNET;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace AppFramework.Behaviors
{
    public class ChatDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ImageTemplate { get; set; }

        public DataTemplate FileTemplate { get; set; }

        public DataTemplate LinkTemplate { get; set; }

        public DataTemplate TextTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            var chatModel = item as ChatMessageModel;

            if (chatModel!=null)
            {
                switch (chatModel.MessageType)
                {
                    case "image": return ImageTemplate;
                    case "file": return FileTemplate;
                    case "link": return LinkTemplate;
                    case "text": return TextTemplate;
                }
            }

            return base.SelectTemplate(item, container);
        }
    }
}
