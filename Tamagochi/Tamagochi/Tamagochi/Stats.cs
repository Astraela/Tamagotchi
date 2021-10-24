using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Essentials;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Plugin.Connectivity;
using System.Diagnostics;

namespace Tamagochi
{
    public class Stats
    {
        private int _id;
        private string _name;
        private float _hunger;
        private float _thirst;
        private float _boringness;
        private float _loneliness;
        private float _stimulation;
        private float _sleepiness;
        private float _sleeping;

        public delegate void statChange(float newValue);
        public statChange hungerChange;
        public statChange thirstChange;
        public statChange boringnessChange;
        public statChange lonelinessChange;
        public statChange stimulationChange;
        public statChange sleepinessChange;
        public statChange sleepingChange;

        public int id            { get => _id;          set => _id = value; }
        public string name       { get => _name;        set => _name = value; }
        public float hunger      { get => _hunger;      set { _hunger = Math.Min(Math.Max(value, 0), 1);        hungerChange?.Invoke(_hunger); } }
        public float thirst      { get => _thirst;      set { _thirst = Math.Min(Math.Max(value, 0), 1);        thirstChange?.Invoke(_thirst); } }
        public float boringness  { get => _boringness;  set { _boringness = Math.Min(Math.Max(value, 0), 1);    boringnessChange?.Invoke(_boringness); } }
        public float loneliness  { get => _loneliness;  set { _loneliness = Math.Min(Math.Max(value, 0), 1);    lonelinessChange?.Invoke(_loneliness); } }
        public float stimulation { get => _stimulation; set { _stimulation = Math.Min(Math.Max(value, 0), 1);   stimulationChange?.Invoke(_stimulation); } }
        public float sleepiness  { get => _sleepiness;  set { _sleepiness = Math.Min(Math.Max(value, 0), 1);    sleepinessChange?.Invoke(_sleepiness); } }
        public float sleeping    { get => _sleeping;    set { _sleeping = value;                                sleepingChange?.Invoke(_sleeping); } }
        public bool hasCompany = false;
        public string princess = "";

        //Modifications in Change Per Minute
        private const float hungerModifier = 1f;
        private const float thirstModifier = .013f;
        private const float boringnessModifier = .002f;
        private float lonelinessModifier { get { return hasCompany ? -.01f : .0015f;} }
        private float stimulationModifier { get { return hasCompany ? .05f : 0; } }
        private const float sleepinessModifier = .007f;
        private const float sleepingModifier = -60f;

        public Stats(){}
        public Stats(int id, string name, float hunger, float thirst, float boringness, float loneliness, float stimulation, float sleepiness) => (this.id,this.name,this.hunger,this.thirst,this.boringness,this.loneliness,this.stimulation,this.sleepiness) = (id,name,hunger,thirst,boringness,loneliness,stimulation,sleepiness);
    
        public void UpdateValues(float ElapsedSeconds)
        {
            if(sleeping >= 0)
            {
                hunger += hungerModifier * (ElapsedSeconds / 60) * .5f;
                thirst += thirstModifier * (ElapsedSeconds / 60) * .5f;
                boringness += boringnessModifier * (ElapsedSeconds / 60) * .5f;
                loneliness += lonelinessModifier * (ElapsedSeconds / 60) * 1.5f;
                stimulation += stimulationModifier * (ElapsedSeconds / 60) * .5f;
                sleepiness -= sleepinessModifier * (ElapsedSeconds / 60) * 3f;
            }
            else
            {
                hunger += hungerModifier * (ElapsedSeconds / 60);
                thirst += thirstModifier * (ElapsedSeconds / 60);
                boringness += boringnessModifier * (ElapsedSeconds / 60);
                loneliness += lonelinessModifier * (ElapsedSeconds / 60);
                stimulation += stimulationModifier * (ElapsedSeconds / 60);
                sleepiness += sleepinessModifier * (ElapsedSeconds / 60);
            }
            sleeping += sleepingModifier * (ElapsedSeconds / 60);
        }
    }


}
