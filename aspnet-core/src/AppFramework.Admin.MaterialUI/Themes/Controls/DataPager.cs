using Prism.Commands;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;

namespace AppFramework.Admin.MaterialUI.Themes.Controls
{
    internal class DataPager : Control, INotifyPropertyChanged
    {
        public DataPager()
        {
            ButtonCollections=new ObservableCollection<string>();
            ButtonIndexCommand =new DelegateCommand<string>(ButtonIndexClick);
        }

        /// <summary>
        /// 显示按钮数量
        /// </summary>
        public int NumericButtonCount
        {
            get { return (int)GetValue(NumericButtonCountProperty); }
            set { SetValue(NumericButtonCountProperty, value); }
        }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount
        {
            get { return (int)GetValue(PageCountProperty); }
            set { SetValue(PageCountProperty, value); }
        }

        /// <summary>
        /// 每页大小
        /// </summary>
        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        /// <summary>
        /// 当前页索引
        /// </summary>
        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }


        private ObservableCollection<string> buttonCollections;

        public ObservableCollection<string> ButtonCollections
        {
            get { return buttonCollections; }
            set
            {
                buttonCollections = value;
                OnPropertyChanged();
            }
        }

        public DelegateCommand<string> ButtonIndexCommand
        {
            get { return (DelegateCommand<string>)GetValue(ButtonIndexCommandProperty); }
            set { SetValue(ButtonIndexCommandProperty, value); }
        }

        public static readonly DependencyProperty ButtonIndexCommandProperty =
            DependencyProperty.Register("ButtonIndexCommand", typeof(DelegateCommand<string>), typeof(DataPager));

        public static readonly DependencyProperty NumericButtonCountProperty =
            DependencyProperty.Register("NumericButtonCount", typeof(int), typeof(DataPager), new PropertyMetadata(NumericButtonCountChangedCallback));

        public static readonly DependencyProperty PageCountProperty =
            DependencyProperty.Register("PageCount", typeof(int), typeof(DataPager), new PropertyMetadata(PageCountChangedCallback));

        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(DataPager), new PropertyMetadata(0));

        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), typeof(DataPager), new PropertyMetadata(0));

        public event PropertyChangedEventHandler? PropertyChanged;

        public override void OnApplyTemplate()
        {
            var itemsControl = GetTemplateChild("ItemsControl") as ItemsControl;

            if (itemsControl!=null)
            {
                itemsControl.ItemsSource=ButtonCollections;
            }

            GetTemplateButtonByName("HomePage").Click+=HomePage_Click;
            GetTemplateButtonByName("PreviousPage").Click+=PreviousPage_Click;
            GetTemplateButtonByName("NextPage").Click+=NextPage_Click;
            GetTemplateButtonByName("EndPage").Click+=EndPage;

            base.OnApplyTemplate();
        }

        void ButtonIndexClick(string Index)
        {
            System.Diagnostics.Debug.WriteLine($"ButtonIndexClick Click! , Index:{Index}");
        }

        void HomePage_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("HomePage Click!");
        }

        void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("PreviousPage Click!");
        }

        void NextPage_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("NextPage Click!");
        }

        void EndPage(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("EndPage Click!");
        }

        private Button GetTemplateButtonByName(string Name)
        {
            return GetTemplateChild(Name) as Button;
        }

        private static void NumericButtonCountChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataPager = (DataPager)d;

            dataPager.ButtonCollections.Clear();

            if (dataPager.PageCount==0)
            {
                dataPager.ButtonCollections.Add("1");
                return;
            }

            if (int.TryParse(e.NewValue.ToString(), out int buttonCount))
            {
                for (int i = 1; i < buttonCount+1; i++)
                    dataPager.ButtonCollections.Add(i.ToString());
            }
        }

        private static void PageCountChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var dataPager = (DataPager)d;

            dataPager.ButtonCollections.Clear();

            if (dataPager.PageCount==0)
            {
                dataPager.ButtonCollections.Add("1");
                return;
            }

            if (int.TryParse(e.NewValue.ToString(), out int pageCount))
            {
                int Count = 0;
                if (dataPager.NumericButtonCount> pageCount)
                    Count=pageCount;
                else
                    Count=dataPager.NumericButtonCount;

                for (int i = 1; i < Count+1; i++)
                    dataPager.ButtonCollections.Add(i.ToString());
            }
        }

        void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
