using System;
using System.Collections;
using System.Windows.Input;
using Xamarin.Forms;

namespace Symtech.Xamarin.UI.Controls
{
    public partial class FuzzyDropdown : ContentView
    {
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(FuzzyDropdown), default(Color), BindingMode.OneWay, null);

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(FuzzyDropdown),
            propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnItemsSourceChanged));

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(FuzzyDropdown),
            propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnItemTemplateChanged));

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(FuzzyDropdown),
            defaultValue: null, defaultBindingMode: BindingMode.TwoWay, propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnSelectedItemChanged));

        public static readonly BindableProperty ItemSelectedCommandProperty = BindableProperty.Create(nameof(ItemSelectedCommand), typeof(ICommand), typeof(FuzzyDropdown), null);

        public static readonly BindableProperty SearchTextChangedCommandProperty = BindableProperty.Create(nameof(SearchTextChangedCommand), typeof(ICommand), typeof(FuzzyDropdown), null);

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
        public event EventHandler<TextChangedEventArgs> SearchTextChanged;

        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }

        public ICommand SearchTextChangedCommand
        {
            get { return (ICommand)GetValue(SearchTextChangedCommandProperty); }
            set { SetValue(SearchTextChangedCommandProperty, value); }
        }

        public FuzzyDropdown()
        {
            InitializeComponent();
            contentControl.BindingContext = null;

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
            if (bindable is FuzzyDropdown fuzzyDropdown && newValue is IEnumerable itemsSource)
            {
                fuzzyDropdown.DropdownListView.ItemsSource = itemsSource;
            }
        }

        private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FuzzyDropdown fuzzyDropdown && newValue is DataTemplate dataTemplate)
            {
                fuzzyDropdown.DropdownListView.ItemTemplate = dataTemplate;
            }
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var e = new SelectedItemChangedEventArgs(newValue, ((FuzzyDropdown)bindable).DropdownListView.TemplatedItems.GetGlobalIndexOfItem(newValue));
            EventHandler<SelectedItemChangedEventArgs> itemSelected = ((FuzzyDropdown)bindable).ItemSelected;
            itemSelected?.Invoke((object)bindable, e);

            ICommand itemSelectedCommand = ((FuzzyDropdown)bindable).ItemSelectedCommand;
            itemSelectedCommand?.Execute(e);
        }

        void OnSearchTextChanged(object sender, TextChangedEventArgs e)
        {
            SearchTextChanged?.Invoke(sender, e);
            SearchTextChangedCommand?.Execute(e);
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
            SearchTextEntry.Text = string.Empty;
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
