using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Symtech.Xamarin.UI.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FancyDropdown : ContentView
    {
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FancyDropdown), default(Color), BindingMode.OneWay, null);

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(FancyDropdown),
            propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnItemsSourceChanged));

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(FancyDropdown),
            propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnItemTemplateChanged));

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(FancyDropdown),
            defaultValue: null, defaultBindingMode: BindingMode.TwoWay, propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnSelectedItemChanged));

        public static readonly BindableProperty ItemSelectedCommandProperty = BindableProperty.Create(nameof(ItemSelectedCommand), typeof(ICommand), typeof(FancyDropdown), null);

        public static readonly BindableProperty IsTopBarVisibleProperty = BindableProperty.Create(nameof(IsTopBarVisible), typeof(bool), typeof(FancyDropdown), true, BindingMode.TwoWay, 
            propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnTopBarVisibilityChanged));
        public static readonly BindableProperty IsBottomBarVisibleProperty = BindableProperty.Create(nameof(IsBottomBarVisible), typeof(bool), typeof(FancyDropdown), false, BindingMode.TwoWay, 
            propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnBottomBarVisibilityChanged));

        public bool IsTopBarVisible
        {
            get => (bool)GetValue(IsTopBarVisibleProperty);
            set => SetValue(IsTopBarVisibleProperty, value);
        }
        public bool IsBottomBarVisible
        {
            get => (bool) GetValue(IsBottomBarVisibleProperty);
            set => SetValue(IsBottomBarVisibleProperty, value);
        }

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public DataTemplate ContentTemplate
        {
            get => contentControl.ContentTemplate;
            set => contentControl.ContentTemplate = value;
        }

        public DataTemplate PlaceholderTemplate
        {
            get => placeholderControl.ContentTemplate;
            set => placeholderControl.ContentTemplate = value;
        }

        public DataTemplate ItemTemplate
        {
            get => DropdownListView.ItemTemplate;
            set => DropdownListView.ItemTemplate = value;
        }

        public object SelectedItem
        {
            get => GetValue(SelectedItemProperty);
            set
            {
                SetValue(SelectedItemProperty, value);
            }
        }

        public IEnumerable ItemsSource
        {
            get => DropdownListView.ItemsSource;
            set => DropdownListView.ItemsSource = value;
        }

        public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }

        public FancyDropdown()
        {
            InitializeComponent();
            contentControl.BindingContext = null;

            DropdownHeader.IsVisible = IsTopBarVisible;
            DropdownPanel.IsVisible = IsBottomBarVisible;
            ItemSelected += OnDropdownItemSelected;
        }

        private void OnDropdownEntryTapped(object sender, EventArgs e)
        {
            ToggleDropdownList();
        }

        private void OnDropdownButtonClicked(object sender, EventArgs e)
        {
            ToggleDropdownList();
        }

        private void OnDropdownItemTapped(object sender, ItemTappedEventArgs e)
        {
            SelectedItem = e.Item;
            ToggleDropdownList();
        }

        private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FancyDropdown fancyDropdown && newValue is IEnumerable itemsSource)
            {
                fancyDropdown.DropdownListView.ItemsSource = itemsSource;
            }
        }

        private static void OnTopBarVisibilityChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FancyDropdown fancyDropdown && newValue is bool isTopBarVisible)
            {
                fancyDropdown.DropdownHeader.IsVisible = isTopBarVisible;
            }
        }
        private static void OnBottomBarVisibilityChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FancyDropdown fancyDropdown && newValue is bool isBottomBarVisible)
            {
                fancyDropdown.DropdownPanel.IsVisible = isBottomBarVisible;
            }
        }

        private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FancyDropdown fancyDropdown && newValue is DataTemplate dataTemplate)
            {
                fancyDropdown.DropdownListView.ItemTemplate = dataTemplate;
            }
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var e = new SelectedItemChangedEventArgs(newValue, ((FancyDropdown)bindable).DropdownListView.TemplatedItems.GetGlobalIndexOfItem(newValue));
            EventHandler<SelectedItemChangedEventArgs> itemSelected = ((FancyDropdown)bindable).ItemSelected;
            itemSelected?.Invoke((object)bindable, e);

            ICommand itemSelectedCommand = ((FancyDropdown)bindable).ItemSelectedCommand;
            itemSelectedCommand?.Execute(e);
        }

        private void OnDropdownItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var value = e.SelectedItem;
            if (value == null)
            {
                contentControl.IsVisible = false;
                placeholderControl.IsVisible = true;
            }
            else
            {
                contentControl.BindingContext = value;
                contentControl.IsVisible = true;
                placeholderControl.IsVisible = false;
            }
        }

        private void ToggleDropdownList()
        {
            if (DropdownPanel.IsVisible)
            {
                DropdownPanel.IsVisible = false;
            }
            else
            {
                DropdownPanel.IsVisible = true;
            }
        }

    }
}
