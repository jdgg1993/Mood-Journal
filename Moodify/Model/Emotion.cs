using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moodify.Model
{
    public class Emotion
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public double anger { get; set; }
        public double contempt { get; set; }
        public double disgust { get; set; }
        public double fear { get; set; }
        public double happiness { get; set; }
        public double neutral { get; set; }
        public double sadness { get; set; }
        public double surprise { get; set; }
        public DateTime createdAt { get; set; }
        public double lat { get; set; }
        public double lon { get; set; }

        public Emotion() { }
    }
}
