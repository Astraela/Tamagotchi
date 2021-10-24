using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tamagochi
{
    public partial class InteractionBar : StackLayout
    {
        private float barScale = .5f;
        public float BarScale
        {
            get => barScale; set
            {
                barScale = value;
                OnPropertyChanged();
            }
        }

        private Color bgColor = Color.Green;
        public Color BgColor
        {
            get => bgColor;
            set
            {
                bgColor = value;
                OnPropertyChanged();
            }
        }

        private string actionText = "FEED";
        public string ActionText
        {
            get => actionText;
            set
            {
                actionText = value;
                OnPropertyChanged();
            }
        }

        private Color barColor = Color.FromRgba(.2,.2,.2,0);
        public Color BarColor
        {
            get => barColor;
            set
            {
                barColor = value;
                OnPropertyChanged();
            }
        }

        public InteractionBar()
        {
            BindingContext = this;
            InitializeComponent();
        }
    }
}