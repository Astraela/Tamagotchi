using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Essentials;

namespace Tamagochi
{
    static public class StatHandling
    {
        static public void Death(Stats stats)
        {
            string uri = "https://tamagotchi.hku.nl/api/Creatures/" + stats.id;
            var request = HttpWebRequest.CreateHttp(uri);
            request.Method = "DELETE";
            request.ServerCertificateValidationCallback = delegate { return true; };
            request.GetResponse();
            stats.id = -1;
            stats.name = "Bowser";
            stats.hunger = 0;
            stats.thirst = 0;
            stats.boringness = 0;
            stats.loneliness = 0;
            stats.stimulation = 0;
            stats.sleepiness = 0;
            stats.sleeping = 0;
            stats.hasCompany = false;
            stats.princess = "";
        }

        static public void SaveValues(Stats stats)
        {
            Preferences.Set("Id", stats.id);
            Preferences.Set("Name", stats.name);
            Preferences.Set("Hunger", stats.hunger);
            Preferences.Set("Thirst", stats.thirst);
            Preferences.Set("Boringness", stats.boringness);
            Preferences.Set("Loneliness", stats.loneliness);
            Preferences.Set("Stimulation", stats.stimulation);
            Preferences.Set("Sleepiness", stats.sleepiness);
            Preferences.Set("Sleeping", stats.sleeping);
            Preferences.Set("HasCompany", stats.hasCompany);
            Preferences.Set("Princess", stats.princess);
            SaveValuesByApi(stats);
            Console.WriteLine("Saved");
        }

        static public void SaveValuesByApi(Stats stats)
        {
            Console.WriteLine("Saving By Api...");
            if (!CrossConnectivity.Current.IsConnected) return;

            var values = "{" + string.Format("\"id\":" + stats.id + ",\"name\":{0},\"userName\":{1},\"hunger\":{2},\"thirst\":{3},\"boredom\":{4},\"loneliness\":{5},\"stimulated\":{6},\"tired\":{7}",
                                    "\"" + stats.name + "\"", "\"Amy\"", stats.hunger, stats.thirst, stats.boringness, stats.loneliness, stats.stimulation, stats.sleepiness) + "}";
            Console.WriteLine(values);
            var bytes = Encoding.ASCII.GetBytes(values);
            var request = HttpWebRequest.CreateHttp(@"https://tamagotchi.hku.nl/api/Creatures/" + stats.id);
            request.Method = "PUT";
            request.ContentType = "application/json";
            request.ContentLength = bytes.Length;
            request.ServerCertificateValidationCallback = delegate { return true; };
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();

            Console.WriteLine("Saved By Api: " + response);
        }

        static public void LoadValuesByPreferences(Stats stats)
        {
            Console.WriteLine("Loading...");
            stats.id = Preferences.Get("Id", -1);
            stats.name = Preferences.Get("Name", "Bowser");
            stats.hunger = Preferences.Get("Hunger", 0f);
            stats.thirst = Preferences.Get("Thirst", 0f);
            stats.boringness = Preferences.Get("Boringness", 0f);
            stats.loneliness = Preferences.Get("Loneliness", 0f);
            stats.stimulation = Preferences.Get("Stimulation", 0f);
            stats.sleepiness = Preferences.Get("Sleepiness", 0f);
            stats.sleeping = Preferences.Get("Sleeping", 0f);
            stats.hasCompany = Preferences.Get("HasCompany", false);
            stats.princess = Preferences.Get("Princess", "");
            if (stats.id == -1)
            {
                CreateInAPI(stats);
            }
            else
            {
                LoadValuesByAPI(stats);
            }
            Console.WriteLine("Loaded");
        }

        static public void CreateInAPI(Stats stats)
        {
            Console.WriteLine("Creating...");
            if (!CrossConnectivity.Current.IsConnected) return;

            var values = "{" + string.Format("\"id\":" + 0 + ",\"name\":{0},\"userName\":{1},\"hunger\":{2},\"thirst\":{3},\"boredom\":{4},\"loneliness\":{5},\"stimulated\":{6},\"tired\":{7}",
                                    "\"" + stats.name + "\"", "\"Amy\"", 0, 0, 0, 0, 0, 0) + "}";

            var bytes = Encoding.ASCII.GetBytes(values);
            var request = HttpWebRequest.CreateHttp(string.Format("https://tamagotchi.hku.nl/api/Creatures/"));
            request.Method = "POST";
            request.ContentType = "application/json";
            request.ContentLength = bytes.Length;
            request.ServerCertificateValidationCallback = delegate { return true; };
            using (var requestStream = request.GetRequestStream())
            {
                requestStream.Write(bytes, 0, bytes.Length);
            }
            var sr = new StreamReader(request.GetResponse().GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            var jsonObject = JsonConvert.DeserializeObject(result) as JObject;

            stats.id = jsonObject.Value<int>("id");

            Console.WriteLine("Created id: " + stats.id);
            SaveValues(stats);
        }

        static public void LoadValuesByAPI(Stats stats)
        {
            Console.WriteLine("Loading By Api...");
            if (!CrossConnectivity.Current.IsConnected) return;

            var request = HttpWebRequest.CreateHttp(string.Format("https://tamagotchi.hku.nl/api/Creatures/" + stats.id));
            request.Method = "GET";
            request.ServerCertificateValidationCallback = delegate { return true; };

            var sr = new StreamReader(request.GetResponse().GetResponseStream(), System.Text.Encoding.UTF8);
            string result = sr.ReadToEnd();
            sr.Close();
            var jsonObject = JsonConvert.DeserializeObject(result) as JObject;
            Console.WriteLine(result);
            Console.WriteLine(stats.id);

            stats.name = jsonObject.Value<string>("name");
            stats.hunger = jsonObject.Value<float>("hunger");
            stats.thirst = jsonObject.Value<float>("thirst");
            stats.boringness = jsonObject.Value<float>("boringness");
            stats.loneliness = jsonObject.Value<float>("loneliness");
            stats.stimulation = jsonObject.Value<float>("stimulation");
            stats.sleepiness = jsonObject.Value<float>("sleepiness");

            Console.WriteLine("Loaded...");
        }
    }
}
