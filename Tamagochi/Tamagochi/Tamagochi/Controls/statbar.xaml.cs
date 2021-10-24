using System;
using Xamarin.Forms;

namespace Tamagochi
{
    public partial class statbar : StackLayout
    {
        private string image = "meat.png";
        public string Image { get => image; set
            {
                image = value;
                OnPropertyChanged();
            }
        }

        private float barScale = .5f;
        public float BarScale { get => barScale; set
            {
                barScale = value;
                ProgressText = Math.Ceiling(100 * value).ToString();
                OnPropertyChanged();
            } 
        }

        private string text = "HUNGER";
        public string Text { get => text; set
            {
                text = value;
                OnPropertyChanged();
            } 
        }

        private Color bgColor = Color.Green;
        public Color BgColor { get => bgColor; 
            set
            {
                bgColor = value;
                OnPropertyChanged();
            } 
        }

        private string progressText = "0/100";
        public string ProgressText { get => progressText;
            set
            {
                progressText = value + " / 100";
                OnPropertyChanged();
            }
        }

        public statbar()
        {
            BindingContext = this;
            InitializeComponent();
        }
    }
}