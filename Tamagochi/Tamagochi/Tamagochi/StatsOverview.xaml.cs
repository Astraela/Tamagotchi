using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tamagochi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatsOverview : ContentPage
    {
        public StatsOverview()
        {
            InitializeComponent();
        }
        Stats stats;

        Color normal = Color.FromRgb(0, 255, 0);
        Color red = Color.FromRgb(255, 0, 0);
        Color orange = Color.FromRgb(255, 94, 0);

        Color ColorCase(float value)
        {
            return value switch
            {
                float n when (n >= .8f) => red,
                float n when (n >= .5f) => orange,
                _ => normal,
            };
        }

        public void SetupBinds(Stats stats)
        {
            this.stats = stats;
            Hunger.BarScale = stats.hunger;
            Hunger.BgColor = ColorCase(stats.hunger);
            Thirst.BarScale = stats.thirst;
            Thirst.BgColor = ColorCase(stats.thirst);
            Boring.BarScale = stats.boringness;
            Boring.BgColor = ColorCase(stats.boringness);
            Lonely.BarScale = stats.loneliness;
            Lonely.BgColor = ColorCase(stats.loneliness);
            Stimulation.BarScale = stats.stimulation;
            Stimulation.BgColor = ColorCase(stats.stimulation);
            Sleepiness.BarScale = stats.sleepiness;
            Sleepiness.BgColor = ColorCase(stats.sleepiness);
            stats.hungerChange += (newValue) => {
                Hunger.BarScale = newValue;
                Hunger.BgColor = ColorCase(newValue);
            };
            stats.thirstChange += (newValue) => {
                Thirst.BarScale = newValue;
                Thirst.BgColor = ColorCase(newValue);
            };
            stats.boringnessChange += (newValue) => {
                Boring.BarScale = newValue;
                Boring.BgColor = ColorCase(newValue);
            };
            stats.lonelinessChange += (newValue) => {
                Lonely.BarScale = newValue;
                Lonely.BgColor = ColorCase(newValue);
            };
            stats.stimulationChange += (newValue) => {
                Stimulation.BarScale = newValue;
                Stimulation.BgColor = ColorCase(newValue);
            };
            stats.sleepinessChange += (newValue) => {
                Sleepiness.BarScale = newValue;
                Sleepiness.BgColor = ColorCase(newValue);
            };
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (Navigation.NavigationStack.Last().GetType() != typeof(Interactions))
            {
                Interactions interactions = new Interactions();
                interactions.SetupBinds(stats);
                Navigation.PushAsync(interactions);
            }
        }
    }
}