using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
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
            "Pink",
            "Orange",
            "Azure"
        };


        private readonly IList<CatBreed> ALL_CAT_BREEDS = new List<CatBreed>()  {
            new CatBreed
            {
                Photo = "https://d17fnq9dkz9hgj.cloudfront.net/breed-uploads/2018/08/american-shorthair-card-large.jpg?bust=1535570019&width=200",
                Name = "American Shorthair"
            },
            new CatBreed
            {
                Photo = "https://d17fnq9dkz9hgj.cloudfront.net/breed-uploads/2018/08/bengal-card-large.jpg?bust=1535569966&width=200",
                Name = "Bengal"
            },
            new CatBreed
            {
                Photo = "https://d17fnq9dkz9hgj.cloudfront.net/breed-uploads/2018/08/british-shorthair-card-large.jpg?bust=1535569902&width=200",
                Name = "British Shorthair"
            },
            new CatBreed
            {
                Photo = "https://d17fnq9dkz9hgj.cloudfront.net/breed-uploads/2018/08/himalayan-card-large.jpg?bust=1535569786&width=200",
                Name = "Himalayan"
            },
            new CatBreed
            {
                Photo = "https://d17fnq9dkz9hgj.cloudfront.net/breed-uploads/2018/08/norwegian-forest-cat-card-large.jpg?bust=1535569662&width=200",
                Name = "Norwegian Forest Cat"
            },
            new CatBreed
            {
                Photo = "https://d17fnq9dkz9hgj.cloudfront.net/breed-uploads/2018/08/singapura-card-large-1.jpg?bust=1540317184&width=200",
                Name = "Singapura"
            },
            new CatBreed
            {
                Photo = "https://d17fnq9dkz9hgj.cloudfront.net/breed-uploads/2018/08/turkish-angora-card-large.jpg?bust=1535569494&width=200",
                Name = "Turkish Angora"
            },
            new CatBreed
            {
                Photo = "https://d17fnq9dkz9hgj.cloudfront.net/breed-uploads/2018/08/turkish-van-card-large.jpg?bust=1535569485&width=200",
                Name = "Turkish Van"
            },
            new CatBreed
            {
                Photo = "https://d17fnq9dkz9hgj.cloudfront.net/breed-uploads/2021/01/PF_TuxedoCat_600x260.jpg?bust=1611699764&width=200",
                Name = "Tuxedo"
            },
        };
        public ObservableCollection<CatBreed> CatBreedList { get; set; }  = new ObservableCollection<CatBreed>();
        private CatBreed _selectedCatBreed;
        public CatBreed SelectedCatBreed
        {
            get => _selectedCatBreed;
            set
            {
                _selectedCatBreed = value;
                NotifyPropertyChanged();
            }
        }
        void OnCatBreedSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            SelectedCatBreed = e.SelectedItem as CatBreed;
        }

        void OnSearchTextChanged(System.Object sender, Xamarin.Forms.TextChangedEventArgs e)
        {
            CatBreedList.Clear();
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                foreach(CatBreed cb in ALL_CAT_BREEDS)
                {
                    CatBreedList.Add(cb);
                }
            }
            else
            {
                var filtered = ALL_CAT_BREEDS.Where(x => x.Name.ToLower().Contains(e.NewTextValue.ToLower()));
                foreach (CatBreed cb in filtered)
                {
                    CatBreedList.Add(cb);
                }
            }
        }


        public MainPage()
        {
            InitializeComponent();
            BindingContext = this;

            EmailEntry.Text = "a@b.cd";

            foreach (CatBreed cb in ALL_CAT_BREEDS)
            {
                CatBreedList.Add(cb);
            }
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

        void OnFocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            Debug.WriteLine("Focus");
        }

        void OnUnfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            Debug.WriteLine("Unfocus");
        }

        private void EmailEntry_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void OnPasswordEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var passwordLength = PasswordEntry.Text.Length;
            PasswordLengthLabel.Text = PasswordEntry.Text.Length > 1 ? $"Your password has {passwordLength} characters" : $"Your password has {passwordLength} character";
        }

        private void OnMobilePhoneEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var mobileLength = MobilePhoneEntry.Text.Length;
            MobileLengthLabel.Text = MobilePhoneEntry.Text.Length > 1 ? $"Your phone number has {mobileLength} characters" : $"Your phone number has {mobileLength} character";
        }
    }

    public class Quote
    {
        public string Words { get; set; }
        public string Author { get; set; }
    }

    public class CatBreed
    {
        public string Photo { get; set; }
        public string Name { get; set; }
    }

}
