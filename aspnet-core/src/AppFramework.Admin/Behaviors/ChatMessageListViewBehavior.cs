using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows.Controls;

namespace AppFramework.Behaviors
{
    public class ChatMessageListViewBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            ((ICollectionView)AssociatedObject.Items).CollectionChanged+=ChatMessageListViewBehavior_CollectionChanged;
        }

        private void ChatMessageListViewBehavior_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (AssociatedObject.HasItems)
                AssociatedObject.ScrollIntoView(AssociatedObject.Items[AssociatedObject.Items.Count-1]);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            ((ICollectionView)AssociatedObject.Items).CollectionChanged-=ChatMessageListViewBehavior_CollectionChanged;
        }
    }
}