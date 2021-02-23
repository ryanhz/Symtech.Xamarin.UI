using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections;
using Xamarin.Forms;

namespace Symtech.Xamarin.UI.Controls
{
    public partial class FancyPicker : ContentView
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(FancyPicker),
            propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnItemsSourceChanged));

        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(FancyPicker),
            propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnItemTemplateChanged));

        public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(object), typeof(FancyPicker),
            defaultValue: null, defaultBindingMode: BindingMode.TwoWay, propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnSelectedItemChanged));

        private readonly PopupPage popupPage;
        private readonly ListView listView;
        private object selectedItem;

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
            get => listView.ItemTemplate;
            set => listView.ItemTemplate = value;
        }

        public object SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                this.SetValue(SelectedItemProperty, value);
            }
        }

        public IEnumerable ItemsSource
        {
            get => listView.ItemsSource;
            set => listView.ItemsSource = value;
        }

        public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

        public FancyPicker()
        {
            InitializeComponent();
            contentControl.BindingContext = null;

            listView = new ListView
            {
                MinimumHeightRequest = 300,
                HeightRequest = 360,
                SelectionMode = ListViewSelectionMode.None,
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            listView.ItemTapped += OnDropdownItemTapped;
            ItemSelected += OnDropdownItemSelected;

            var stackLayout = new StackLayout
            {
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Margin = new Thickness(6),
                BackgroundColor = Color.White,
            };
            stackLayout.Children.Add(listView);

            popupPage = new PopupPage
            {
                Content = stackLayout,
            };

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
            if (bindable is FancyPicker fancyPicker && newValue is IEnumerable itemsSource)
            {
                fancyPicker.listView.ItemsSource = itemsSource;
            }
        }

        private static void OnItemTemplateChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FancyPicker fancyPicker && newValue is DataTemplate dataTemplate)
            {
                fancyPicker.listView.ItemTemplate = dataTemplate;
            }
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            EventHandler<SelectedItemChangedEventArgs> itemSelected = ((FancyPicker)bindable).ItemSelected;
            if (itemSelected == null)
                return;
            itemSelected((object)bindable, new SelectedItemChangedEventArgs(newValue, ((FancyPicker)bindable).listView.TemplatedItems.GetGlobalIndexOfItem(newValue)));
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
            if (PopupNavigation.Instance.PopupStack.Count > 0)
            {
                PopupNavigation.Instance.PopAllAsync();
            }
            else
            {
                PopupNavigation.Instance.PushAsync(popupPage);
            }
        }
    }
}
