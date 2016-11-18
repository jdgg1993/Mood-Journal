using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Moodify.Droid;
using System.IO;

[assembly: Dependency(typeof(SQLite_Android))]
namespace Moodify.Droid
{
    public class SQLite_Android : ISQLite
    {

        public SQLite_Android()
        {
        }

        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "TimelineSQLite.db3";
            string documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, sqliteFilename);

            var conn = new SQLite.SQLiteConnection(path);

            return conn;
        }
    }
}