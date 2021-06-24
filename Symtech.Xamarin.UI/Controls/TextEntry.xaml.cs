using System;
using System.Runtime.CompilerServices;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Symtech.Xamarin.UI.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TextEntry : ContentView
    {
        public event EventHandler Completed;

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create("BorderColor", typeof(Color), typeof(TextEntry), default(Color), BindingMode.TwoWay, null);
        public static readonly BindableProperty TitleColorProperty = BindableProperty.Create("TitleColor", typeof(Color), typeof(TextEntry), default(Color), BindingMode.TwoWay, null);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(TextEntry), default(Color), BindingMode.TwoWay, null);

        public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create("Placeholder", typeof(string), typeof(TextEntry), string.Empty, BindingMode.OneWay, null);
        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(TextEntry), string.Empty, BindingMode.TwoWay, null);
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create("FontSize", typeof(double), typeof(TextEntry), (object)-1.0, BindingMode.OneWay, null);
        public static readonly BindableProperty TitleProperty = BindableProperty.Create("Title", typeof(string), typeof(TextEntry), string.Empty, BindingMode.TwoWay, null);
        public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(nameof(ReturnType), typeof(ReturnType), typeof(TextEntry), ReturnType.Default);
        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create("IsPassword", typeof(bool), typeof(TextEntry), default(bool));
        public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(TextEntry), default(bool));
        public static readonly BindableProperty KeyboardProperty = BindableProperty.Create("Keyboard", typeof(Keyboard), typeof(TextEntry), Keyboard.Default, coerceValue: (o, v) => (Keyboard)v ?? Keyboard.Default);

        public TextEntry()
        {
            InitializeComponent();
        }

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public Color TitleColor
        {
            get => (Color)GetValue(TitleColorProperty);
            set => SetValue(TitleColorProperty, value);
        }

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public string Placeholder
        {
            get => (string)GetValue(PlaceholderProperty);
            set => SetValue(PlaceholderProperty, value);
        }

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public ReturnType ReturnType
        {
            get => (ReturnType)GetValue(ReturnTypeProperty);
            set => SetValue(ReturnTypeProperty, value);
        }

        public bool IsPassword
        {
            get { return (bool)GetValue(IsPasswordProperty); }
            set { SetValue(IsPasswordProperty, value); }
        }

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public Keyboard Keyboard
        {
            get { return (Keyboard)GetValue(KeyboardProperty); }
            set { SetValue(KeyboardProperty, value); }
        }


        public new void Focus()
        {
            if (IsEnabled)
            {
                InputEntry.Focus();
            }
        }


        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(IsEnabled))
            {
                InputEntry.IsEnabled = IsEnabled;
            }
        }

        void HandleCompleted(object sender, EventArgs e)
        {
            Completed?.Invoke(this, e);
        }

        public event EventHandler<TextChangedEventArgs> TextChanged;

        private void OnInputEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            TextChanged?.Invoke(this, new TextChangedEventArgs(e.OldTextValue, e.NewTextValue));
        }

    }
}
