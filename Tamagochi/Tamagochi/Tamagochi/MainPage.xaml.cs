using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using System.Timers;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Tamagochi
{
    public partial class MainPage : ContentPage
    {
        readonly Stats stats = new Stats();

        readonly Color normal = Color.FromRgba(0, 0, 0, 75);
        readonly Color red = Color.FromRgba(255, 0, 0, 75);
        readonly Color orange = Color.FromRgba(255, 94, 0, 75);

        Random rnd = new Random((int)(DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalSeconds);


        public MainPage()
        {
            InitializeComponent();
            Initialization();
        }

        #region Init

        private void Initialization()
        {
            //Load Stats
            StatHandling.LoadValuesByPreferences(stats);
            SetupStats();
            SetupTimers();
            SetupMessagingService();
        }

        private string GetRandomPrincess()
        {
            var request = HttpWebRequest.CreateHttp(string.Format("https://tamagotchi.hku.nl/api/Creatures/"));
            request.Method = "GET";
            request.ServerCertificateValidationCallback = delegate { return true; };

            var sr = new StreamReader(request.GetResponse().GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            var jsronArray = JsonConvert.DeserializeObject(result) as JArray;
            for (int i = 0; i < 9; i++)
            {
                int selected = rnd.Next(0, jsronArray.Count);
                var princessObject = jsronArray[selected] as JObject;
                if (princessObject.Value<string>("name") != null)
                {
                    return princessObject.Value<string>("name");
                }
            }
            return "";
        }

        private void SetupMessagingService()
        {
            MessagingCenter.Subscribe<string>("Interactions", "Kidnap", (sender) => {
                Console.WriteLine("Kidnapping Received");
                Device.BeginInvokeOnMainThread(() =>
                {
                    string princess = GetRandomPrincess();
                    if (princess == "") return;
                    stats.princess = princess;
                    PeachText.Text = "Princess " + princess;
                    PeachText.IsVisible = true;
                    Peach.IsVisible = true;
                });
            });
            MessagingCenter.Subscribe<string>("Interactions", "Unkidnap", (sender) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    PeachText.IsVisible = false;
                    Peach.IsVisible = false;
                });
            });
        }

        private Color ColorCase(float value)
        {
            return value switch
            {
                float n when (n >= .8f) => red,
                float n when (n >= .5f) => orange,
                _ => normal,
            };
        }

        private void SetupStats()
        {
            Hunger.ScaleY = stats.hunger;
            Hunger.BackgroundColor = ColorCase(stats.hunger);
            Thirst.ScaleY = stats.thirst;
            Thirst.BackgroundColor = ColorCase(stats.thirst);
            Boring.ScaleY = stats.boringness;
            Boring.BackgroundColor = ColorCase(stats.boringness);
            Lonely.ScaleY = stats.loneliness;
            Lonely.BackgroundColor = ColorCase(stats.loneliness);
            Stimulation.ScaleY = stats.stimulation;
            Stimulation.BackgroundColor = ColorCase(stats.stimulation);
            Sleepiness.ScaleY = stats.sleepiness;
            Sleepiness.BackgroundColor = ColorCase(stats.sleepiness);
            stats.hungerChange += (newValue) => {
                Hunger.ScaleY = newValue;
                Hunger.BackgroundColor = ColorCase(newValue);
            };
            stats.thirstChange += (newValue) => {
                Thirst.ScaleY = newValue;
                Thirst.BackgroundColor = ColorCase(newValue);
            };
            stats.boringnessChange += (newValue) => {
                Boring.ScaleY = newValue;
                Boring.BackgroundColor = ColorCase(newValue);
            };
            stats.lonelinessChange += (newValue) => {
                Lonely.ScaleY = newValue;
                Lonely.BackgroundColor = ColorCase(newValue);
            };
            stats.stimulationChange += (newValue) => {
                Stimulation.ScaleY = newValue;
                Stimulation.BackgroundColor = ColorCase(newValue);
            };
            stats.sleepinessChange += (newValue) => {
                Sleepiness.ScaleY = newValue;
                Sleepiness.BackgroundColor = ColorCase(newValue);
            };

            if (stats.hasCompany)
            {
                PeachText.Text = "Princess " + stats.princess;
                PeachText.IsVisible = true;
                Peach.IsVisible = true;
            }
        }

        private void SetupTimers()
        {
            var valueTimer = new Timer() { Interval = 1000, AutoReset = true };
            var saveTimer = new Timer() { Interval = 20000, AutoReset = true };
            var checkTimer = new Timer() { Interval = 500, AutoReset = true };
            valueTimer.Elapsed += TimerStep;
            saveTimer.Elapsed += SaveStep;
            checkTimer.Elapsed += CheckStep;
            valueTimer.Start();
            saveTimer.Start();
            checkTimer.Start();
        }

        private void TimerStep(object sender, ElapsedEventArgs e)
        {
            stats.UpdateValues(1);
        }

        private void SaveStep(object sender, ElapsedEventArgs e)
        {
            StatHandling.SaveValues(stats);
        }

        private void CheckStep(object sender, ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (stats.hunger >= 1 || stats.thirst >= 1 || stats.boringness >= 1 || stats.loneliness >= 1 || stats.stimulation >= 1 || stats.sleepiness >= 1)
                {

                    if ((bowser.Source as FileImageSource).File != "death.png")
                        bowser.Source = "death.png";
                    DeathNavigation();
                    return;
                }

                if (stats.hunger > .8f || stats.thirst > .8f || stats.boringness > .8f || stats.loneliness > .8f || stats.stimulation > .8f || stats.sleepiness > .8f)
                {
                    if ((bowser.Source as FileImageSource).File != "pain.png")
                        bowser.Source = "pain.png";
                    if (stats.sleeping > 0)
                        stats.sleeping = 0;
                    return;
                }

                if (stats.sleeping > 0)
                {
                    if ((bowser.Source as FileImageSource).File != "sleeping.png")
                        bowser.Source = "sleeping.png";
                    return;
                }

                if (stats.hunger > .5f || stats.thirst > .5f || stats.boringness > .5f || stats.loneliness > .5f || stats.stimulation > .5f || stats.sleepiness > .5f)
                {
                    if ((bowser.Source as FileImageSource).File != "waiting.png")
                        bowser.Source = "waiting.png";
                    return;
                }

                if ((bowser.Source as FileImageSource).File != "bowsey.png")
                    bowser.Source = "bowsey.png";
            });
        }
        #endregion init

        private void OnStatsClick(object sender, EventArgs e)
        {
                if ( Navigation.NavigationStack.Last().GetType() != typeof(StatsOverview))
                {
                    StatsOverview statsView = new StatsOverview();
                    statsView.SetupBinds(stats);
                    Navigation.PushAsync(statsView);
                }
        }

        private void DeathNavigation()
        {
            if (Navigation.NavigationStack.Last().GetType() != typeof(Death))
            {
                var death = new Death{ stats = stats };
                Navigation.PushAsync(death);
            }
        }
    }
}
