using AppFramework.Localization;
using Syncfusion.UI.Xaml.Grid;
using System.Windows;

namespace AppFramework.Extensions
{
    public static class DataGridTranslateExtensions
    {
        public static bool GetTranslate(DependencyObject obj)
        {
            return (bool)obj.GetValue(TranslateProperty);
        }

        public static void SetTranslate(DependencyObject obj, bool value)
        {
            obj.SetValue(TranslateProperty, value);
        }

        public static readonly DependencyProperty TranslateProperty =
            DependencyProperty.RegisterAttached("Translate", typeof(bool),
                typeof(DataGridTranslateExtensions), new PropertyMetadata(false, PropertyChangedCallBack));

        private static void PropertyChangedCallBack(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var sfDataGrid = sender as SfDataGrid;
            sfDataGrid.ItemsSourceChanged += (s, e) =>
            {
                for (int i = 0; i < sfDataGrid.Columns.Count; i++)
                    sfDataGrid.Columns[i].HeaderText = Local.Localize(sfDataGrid.Columns[i].HeaderText);
            };
        }
    }
}