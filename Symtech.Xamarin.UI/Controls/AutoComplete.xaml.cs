using System;
using System.Collections;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Symtech.Xamarin.UI.Extensions;
using System.Windows.Input;

namespace Symtech.Xamarin.UI.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutoComplete : ContentView
    {

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(AutoComplete), string.Empty, BindingMode.TwoWay,
            propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnTextPropertyChanged));
        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(AutoComplete), string.Empty, BindingMode.OneWay, null);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(AutoComplete), default(Color), BindingMode.OneWay, null);
        public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(nameof(ReturnType), typeof(ReturnType), typeof(AutoComplete), ReturnType.Default);
        public static readonly BindableProperty KeyboardProperty = BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(AutoComplete), Keyboard.Default, coerceValue: (o, v) => (Keyboard)v ?? Keyboard.Default);

        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable), typeof(AutoComplete),
            propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnItemsSourcePropertyChanged));
        public static readonly BindableProperty ItemTemplateProperty = BindableProperty.Create(nameof(ItemTemplate), typeof(DataTemplate), typeof(AutoComplete),
            propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnItemTemplatePropertyChanged));

        public static readonly BindableProperty TextChangedCommandProperty = BindableProperty.Create(nameof(TextChangedCommand), typeof(ICommand), typeof(AutoComplete), null);
        public static readonly BindableProperty ItemSelectedCommandProperty = BindableProperty.Create(nameof(ItemSelectedCommand), typeof(ICommand), typeof(AutoComplete), null);

        public new event EventHandler<FocusEventArgs> Focused;
        public new event EventHandler<FocusEventArgs> Unfocused;

        private bool tappingOnSuggestItem = false;

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public ReturnType ReturnType
        {
            get => (ReturnType)GetValue(ReturnTypeProperty);
            set => SetValue(ReturnTypeProperty, value);
        }

        public Keyboard Keyboard
        {
            get { return (Keyboard)GetValue(KeyboardProperty); }
            set { SetValue(KeyboardProperty, value); }
        }

        public IEnumerable ItemsSource
        {
            get => SuggestListView.ItemsSource;
            set => SuggestListView.ItemsSource = value;
        }

        public DataTemplate ItemTemplate
        {
            get => SuggestListView.ItemTemplate;
            set => SuggestListView.ItemTemplate = value;
        }

        public event EventHandler<TextChangedEventArgs> TextChanged;
        public event EventHandler<SelectedItemChangedEventArgs> ItemSelected;

        public ICommand TextChangedCommand
        {
            get { return (ICommand)GetValue(TextChangedCommandProperty); }
            set { SetValue(TextChangedCommandProperty, value); }
        }

        public ICommand ItemSelectedCommand
        {
            get { return (ICommand)GetValue(ItemSelectedCommandProperty); }
            set { SetValue(ItemSelectedCommandProperty, value); }
        }

        public AutoComplete()
        {
            InitializeComponent();
        }

        private void OnClearButtonClicked(object sender, EventArgs e)
        {
            Text = string.Empty;
            DropdownPanel.IsVisible = false;
        }

        private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoComplete autoComplete && newValue is string text)
            {
                autoComplete.TextEntry.Text = text;
            }
        }

        private static void OnItemsSourcePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoComplete autoComplete && newValue is IEnumerable itemsSource)
            {
                autoComplete.SuggestListView.ItemsSource = itemsSource;
                autoComplete.DropdownPanel.IsVisible = itemsSource.IsNotEmpty();
            }
        }

        private static void OnItemTemplatePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is AutoComplete autoComplete && newValue is DataTemplate dataTemplate)
            {
                autoComplete.SuggestListView.ItemTemplate = dataTemplate;
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (tappingOnSuggestItem)
            {
                return; // ignore text change when tapping on suggest item
            }
            TextChangedCommand?.Execute(e);
            TextChanged?.Invoke(this, e);
            ClearButton.IsVisible = !string.IsNullOrEmpty(Text);
        }

        void OnSuggestItemTapped(System.Object sender, ItemTappedEventArgs e)
        {
            tappingOnSuggestItem = true;
            try
            {
                SelectedItemChangedEventArgs sice = new SelectedItemChangedEventArgs(e.Item, e.ItemIndex);
                ItemSelectedCommand?.Execute(sice);
                ItemSelected?.Invoke(this, sice);
                DropdownPanel.IsVisible = false;
            }
            finally
            {
                tappingOnSuggestItem = false;
            }
        }

        private void OnFocused(object sender, FocusEventArgs e)
        {
            Focused?.Invoke(this, e);
        }

        private void OnUnFocused(object sender, FocusEventArgs e)
        {
            DropdownPanel.IsVisible = false;
            Unfocused?.Invoke(this, e);
        }

    }
}
