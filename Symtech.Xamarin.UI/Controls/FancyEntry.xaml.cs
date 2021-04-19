using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Symtech.Xamarin.UI.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FancyEntry : ContentView
    {

        public event EventHandler Completed;

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create("BorderColor", typeof(Color), typeof(FancyEntry), default(Color), BindingMode.TwoWay, null);
        public static readonly BindableProperty TitleColorProperty = BindableProperty.Create("TitleColor", typeof(Color), typeof(FancyEntry), default(Color), BindingMode.TwoWay, null);
        public static readonly BindableProperty TextColorProperty = BindableProperty.Create("TextColor", typeof(Color), typeof(FancyEntry), default(Color), BindingMode.TwoWay, propertyChanged: new BindableProperty.BindingPropertyChangedDelegate(OnTextPropertyChanged));

        public static readonly BindableProperty TextProperty = BindableProperty.Create("Text", typeof(string), typeof(FancyEntry), string.Empty, BindingMode.TwoWay, null);
        public static readonly BindableProperty TitleProperty = BindableProperty.Create("Title", typeof(string), typeof(FancyEntry), string.Empty, BindingMode.TwoWay, null);
        public static readonly BindableProperty ReturnTypeProperty = BindableProperty.Create(nameof(ReturnType), typeof(ReturnType), typeof(FancyEntry), ReturnType.Default);
        public static readonly BindableProperty IsPasswordProperty = BindableProperty.Create("IsPassword", typeof(bool), typeof(FancyEntry), default(bool));
        public static readonly BindableProperty IsReadOnlyProperty = BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(FancyEntry), default(bool));
        public static readonly BindableProperty KeyboardProperty = BindableProperty.Create("Keyboard", typeof(Keyboard), typeof(FancyEntry), Keyboard.Default, coerceValue: (o, v) => (Keyboard)v ?? Keyboard.Default);

        public FancyEntry()
        {
            InitializeComponent();
            LayoutChanged += (s, e) => {
                Device.BeginInvokeOnMainThread(() => {
                    if (string.IsNullOrEmpty(InputEntry.Text))
                    {
                        PutTitleInPlaceholder();
                    }
                    else
                    {
                        MoveTitleToBorder();
                    }
                });
            };
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

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
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

        private static void OnTextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (bindable is FancyEntry fancyEntry && oldValue != newValue)
            {
                Device.BeginInvokeOnMainThread(() => {
                    if (string.IsNullOrEmpty(fancyEntry.InputEntry.Text))
                    {
                        fancyEntry.PutTitleInPlaceholder();
                    }
                    else
                    {
                        fancyEntry.MoveTitleToBorder();
                    }
                });
            }
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

        private void OnEntryUnfocused(object sender, FocusEventArgs e)
        {
            if (string.IsNullOrEmpty(InputEntry.Text))
            {
                PutTitleInPlaceholder();
            }
        }

        private void OnEntryFocused(object sender, FocusEventArgs e)
        {
            MoveTitleToBorder();
        }

        private void PutTitleInPlaceholder()
        {
            EntryLabel.ScaleXTo(1);
            EntryLabel.ScaleYTo(1);
            EntryLabel.TranslateTo(0, 0);
            BorderEraser.IsVisible = false;
        }

        private void MoveTitleToBorder()
        {
            BorderEraser.IsVisible = true;
            BorderEraser.Margin = new Thickness(EntryLabel.Margin.Left, 0, EntryLabel.Margin.Right, 0);
            BorderEraser.WidthRequest = EntryLabel.Width * 0.8;
            BorderEraser.Color = new Color(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B, 1.0);

            EntryLabel.ScaleYTo(0.8);
            EntryLabel.ScaleXTo(0.8);
            EntryLabel.TranslateTo(0, -(EntryLabel.Height));
        }

        void HandleCompleted(object sender, EventArgs e)
        {
            Completed?.Invoke(this, e);
        }

    }
}
