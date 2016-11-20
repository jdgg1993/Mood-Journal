using Moodify.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Moodify.Data
{
    public class TimelineDatabase
    {

        static object locker = new object();
        SQLiteConnection database;

        public TimelineDatabase()
        {
            database = DependencyService.Get<ISQLite>().GetConnection();
            database.CreateTable<Emotion>();
        }

        public IEnumerable<Emotion> GetItems()
        {
            lock (locker)
            {
                return (from i in database.Table<Emotion>() select i).ToList();
            }
        }

        public IEnumerable<Emotion> GetItemsNotDone()
        {
            lock (locker)
            {
                return database.Query<Emotion>("SELECT * FROM [Timeline] WHERE [Done] = 0");
            }
        }

        public Emotion GetItem(int id)
        {
            lock (locker)
            {
                return database.Table<Emotion>().FirstOrDefault(x => x.id == id);
            }
        }

        public int SaveItem(Emotion item)
        {
            lock (locker)
            {
                if (item.id != 0)
                {
                    database.Update(item);
                    return item.id;
                }
                else
                {
                    return database.Insert(item);
                }
            }
        }

        public int DeleteItem(int id)
        {
            lock (locker)
            {
                return database.Delete<Emotion>(id);
            }
        }
    }
}
