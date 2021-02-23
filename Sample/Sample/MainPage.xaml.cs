using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Sample
{
    public partial class MainPage : ContentPage, INotifyPropertyChanged
    {
        string quote = string.Empty;
        public string Quote {
            get => quote;
            set {
                quote = value;
                NotifyPropertyChanged();
            }
        }

        private IList<Quote> quoteSuggestList = new List<Quote>();
        public IList<Quote> QuoteSuggestList
        {
            get => quoteSuggestList;
            set
            {
                quoteSuggestList = value;
                NotifyPropertyChanged();
            }
        }

        private readonly IList<Quote> ALL_QUOTES = new List<Quote>() {
            new Quote {
                Words = "Any fool can know. The point is to understand.",
                Author = "Albert Einstein",
            },
            new Quote {
                Words = "The fool doth think he is wise, but the wise man knows himself to be a fool.",
                Author = "William Shakespeare",
            },
            new Quote {
                Words = "Knowing yourself is the beginning of all wisdom.",
                Author = "Aristotle",
            },
            new Quote {
                Words = "The only true wisdom is in knowing you know nothing.",
                Author = "Socrates",
            },
            new Quote {
                Words = "Never laugh at live dragons.",
                Author = "J.R.R. Tolkien",
            },
            new Quote {
                Words = "The unexamined life is not worth living.",
                Author = "Albert Einstein",
            },
            new Quote {
                Words = "Turn your wounds into wisdom.",
                Author = "Oprah Winfrey",
            },
            new Quote {
                Words = "Angry people are not always wise.",
                Author = "Jane Austen",
            },
            new Quote {
                Words = "Let no man pull you so low as to hate him.",
                Author = "Martin Luther King",
            },
            new Quote {
                Words = "Yesterday I was clever, so I wanted to change the world. Today I am wise, so I am changing myself.",
                Author = "Rumi",
            },
            new Quote {
                Words = "Count your age by friends, not years. Count your life by smiles, not tears.",
                Author = "John Lennon",
            },
        };

        private string color;
        public string Color
        {
            get => color;
            set
            {
                color = value;
                NotifyPropertyChanged();
            }
        }

        public IList<string> ColorList { get; set; } = new List<string>() {
            "Red",
            "Green",
            "Blue",
            "Purple",
            "Yellow",
            "Pink"
        };

        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;
        }

        private async void OnQuoteTextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.OldTextValue) || string.IsNullOrWhiteSpace(e.NewTextValue) || e.NewTextValue.Length < 2)
            {
                return;
            }
            await Task.Delay(TimeSpan.FromSeconds(1));
            string query = e.NewTextValue.ToLower();
            QuoteSuggestList = ALL_QUOTES.Where(q => q.Words.ToLower().Contains(query) || q.Author.ToLower().Contains(query)).ToList();
        }
        private void OnQuoteItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Quote quote = e.SelectedItem as Quote;
            Quote = $"{quote.Words} - {quote.Author}";
        }


        private void OnColorSelected(object sender, SelectedItemChangedEventArgs e)
        {
            string selectedColor = e.SelectedItem as string;
            Color = selectedColor;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }

    public class Quote
    {
        public string Words { get; set; }
        public string Author { get; set; }
    }
}
