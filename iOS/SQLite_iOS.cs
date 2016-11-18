using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Text;
using System.IO;
using Moodify.iOS;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace Moodify.iOS
{
    public class SQLite_iOS : ISQLite
    {

        public SQLite_iOS()
        {
        }

        public SQLite.SQLiteConnection GetConnection()
        {
            var sqliteFilename = "TimelineSQLite.db3";
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            string libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.Combine(libraryPath, sqliteFilename);
            
            if (!File.Exists(path))
            {
                File.Copy(sqliteFilename, path);
            }

            var conn = new SQLite.SQLiteConnection(path);
            
            return conn;
        }
    }
}
