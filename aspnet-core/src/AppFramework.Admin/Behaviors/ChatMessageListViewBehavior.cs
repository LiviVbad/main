using Microsoft.Xaml.Behaviors;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace AppFramework.Behaviors
{
    public class ChatMessageListBoxGroupBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            ((ICollectionView)AssociatedObject.Items).GroupDescriptions.Add(new PropertyGroupDescription("CreationTime"));
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            ((ICollectionView)AssociatedObject.Items).GroupDescriptions.Remove(new PropertyGroupDescription("CreationTime"));
        }
    }


    public class ChatMessageListBoxBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            ((ICollectionView)AssociatedObject.Items).CollectionChanged += ChatMessageListViewBehavior_CollectionChanged;
        }

        private void ChatMessageListViewBehavior_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (AssociatedObject.HasItems)
                AssociatedObject.ScrollIntoView(e.NewItems[e.NewItems.Count-1]);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            ((ICollectionView)AssociatedObject.Items).CollectionChanged -= ChatMessageListViewBehavior_CollectionChanged;
        }
    }
}