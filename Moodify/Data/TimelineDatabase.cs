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
            database.CreateTable<Timeline>();
        }

        public IEnumerable<Timeline> GetItems()
        {
            lock (locker)
            {
                return (from i in database.Table<Timeline>() select i).ToList();
            }
        }

        public IEnumerable<Timeline> GetItemsNotDone()
        {
            lock (locker)
            {
                return database.Query<Timeline>("SELECT * FROM [Timeline] WHERE [Done] = 0");
            }
        }

        public Timeline GetItem(int id)
        {
            lock (locker)
            {
                return database.Table<Timeline>().FirstOrDefault(x => x.id == id);
            }
        }

        public int SaveItem(Timeline item)
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
                return database.Delete<Timeline>(id);
            }
        }
    }
}
