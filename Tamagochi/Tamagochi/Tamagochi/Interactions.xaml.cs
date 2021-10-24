using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Tamagochi
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Interactions : ContentPage
    {
        public Interactions()
        {
            InitializeComponent();
        }
        public Stats stats;

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

            if(stats.sleeping > 0)
            {
                Sleepiness.ActionText = "Wake up";
            }
            if (stats.hasCompany)
            {
                Lonely.ActionText = "Kidnap a different princess";
            }
        }

        void Feed(object sender, EventArgs e)
        {
            Change(Hunger);
            stats.hunger -= .1f;
            if (stats.sleeping > 0) stats.sleeping = 0;
        }
        void Water(object sender, EventArgs e)
        {
            Change(Thirst);
            stats.thirst -= .1f;
            if (stats.sleeping > 0) stats.sleeping = 0;
        }
        void Play(object sender, EventArgs e)
        {
            Change(Boring);
            stats.boringness -= .1f;
            if (stats.sleeping > 0) stats.sleeping = 0;
        }
        void Kidnap(object sender, EventArgs e)
        {
            Change(Lonely);
            stats.hasCompany = true;
            MessagingCenter.Send<string>("Interactions", "Kidnap");
            Navigation.PopAsync();
            Navigation.PopAsync();
        }
        void Unkidnap(object sender, EventArgs e)
        {
            Change(Stimulation);
            stats.hasCompany = false;
            MessagingCenter.Send<string>("Interactions", "Unkidnap");
            Navigation.PopAsync();
            Navigation.PopAsync();
        }
        void Sleepy(object sender, EventArgs e)
        {
            Change(Sleepiness);
            if (stats.sleeping > 0)
            {
                stats.sleeping = 0;
                Sleepiness.ActionText = "Take bowsey to bed";
                return;
            }
            stats.sleeping = 3600*8; //8 hours
            Sleepiness.ActionText = "Wake up";
        }

        private async void Change(InteractionBar bar)
        {
            bar.BarColor = Color.FromRgba(.2, .2, .2, .2);
            await Task.Delay(200);
            bar.BarColor = Color.FromRgba(.2, .2, .2, 0);
        }
    }
}