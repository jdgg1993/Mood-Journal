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
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public double Anger { get; set; }
        public double Contempt { get; set; }
        public double Disgust { get; set; }
        public double Fear { get; set; }
        public double Happiness { get; set; }
        public double Neutral { get; set; }
        public double Sadness { get; set; }
        public double Surprise { get; set; }
        public DateTime createdAt { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }

        public Timeline() { }
    }
}
