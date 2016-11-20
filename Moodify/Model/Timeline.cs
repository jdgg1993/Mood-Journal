using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moodify.Model
{
    public class Timeline
    {

        string id;
        double anger;
        double contempt;
        double disgust;
        double fear;
        double happiness;
        double neutral;
        double sadness;
        double surprise;
        DateTime createdAt;
        double lat;
        double lon;

        [JsonProperty(PropertyName = "id")]
        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        [JsonProperty(PropertyName = "anger")]
        public double Anger
        {
            get { return anger; }
            set { anger = value; }
        }

        [JsonProperty(PropertyName = "contempt")]
        public double Contempt
        {
            get { return contempt; }
            set { contempt = value; }
        }


        [JsonProperty(PropertyName = "disgust")]
        public double Disgust
        {
            get { return disgust; }
            set { disgust = value; }
        }


        [JsonProperty(PropertyName = "fear")]
        public double Fear
        {
            get { return fear; }
            set { fear = value; }
        }


        [JsonProperty(PropertyName = "happiness")]
        public double Happiness
        {
            get { return happiness; }
            set { happiness = value; }
        }


        [JsonProperty(PropertyName = "neutral")]
        public double Neutral
        {
            get { return neutral; }
            set { neutral = value; }
        }


        [JsonProperty(PropertyName = "sadness")]
        public double Sadness
        {
            get { return sadness; }
            set { sadness = value; }
        }


        [JsonProperty(PropertyName = "surprise")]
        public double Surprise
        {
            get { return surprise; }
            set { surprise = value; }
        }


        [JsonProperty(PropertyName = "createdAt")]
        public DateTime CreatedAt
        {
            get { return createdAt; }
            set { createdAt = value; }
        }

        [JsonProperty(PropertyName = "lat")]
        public double Lat
        {
            get { return lat; }
            set { lat = value; }
        }

        [JsonProperty(PropertyName = "lon")]
        public double Lon
        {
            get { return lon; }
            set { lon = value; }
        }

        [Version]
        public string Version { get; set; }
    }
}
