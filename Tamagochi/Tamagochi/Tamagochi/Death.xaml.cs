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
    public partial class Death : ContentPage
    {
        public Death()
        {
            Console.WriteLine("DeathLoaded");
            InitializeComponent();
        }
        public Stats stats;

        private void Button_Clicked(object sender, EventArgs e)
        {
            StatHandling.Death(stats);
            StatHandling.CreateInAPI(stats);
            MessagingCenter.Send<string>("Interactions", "Unkidnap");
            Navigation.PopAsync();
        }
    }
}