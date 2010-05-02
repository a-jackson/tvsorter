using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Windows.Forms;

namespace TVSorter
{
    public class Database
    {
        private static string DatabaseFile = Environment
            .GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
            "\\TVSorter2\\data.db";
        private static string connectionString = "Data Source=" + DatabaseFile + ";Version=3;";
        private const int DatabaseVersion = 2;

        public Database()
        {
            //Check there is a database
            if (!File.Exists(DatabaseFile))
            {
                CreateDatabase();
            }
            //Verify the version
            long version = (long)ExecuteScalar("Select version From Version;");
            if (version < DatabaseVersion)
            {
                //Needs update
                if (version < 2)
                {
                    //Update to 1-2
                    ExecuteQuery("Create Table AltNames(id INTEGER PRIMARY KEY AUTOINCREMENT, show_id INTEGER, alt_name TEXT)");
                    ExecuteQuery("Update Version Set version = 2 Where id = 1");
                }
                //Subsequent updates here e.g if (version < 3) should have already been updated to 2
            }
        }

        private void CreateDatabase()
        {
            if (!Directory.Exists(Environment
            .GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
            "\\TVSorter2"))
            {
                Directory.CreateDirectory(Environment
            .GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
            "\\TVSorter2");
            }
            SQLiteConnection.CreateFile(DatabaseFile);

            ExecuteQuery("Create Table Version(id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                        "version INTEGER);");
            ExecuteQuery("Create Table Shows(id INTEGER PRIMARY KEY AUTOINCREMENT, " +
                "tvdb_id TEXT, name TEXT, update_time INTEGER, use_default_format INTEGER, " +
                "custom_format TEXT, folder_name TEXT, banner TEXT);");
            ExecuteQuery("Create Table Episodes" +
                   "(id INTEGER PRIMARY KEY AUTOINCREMENT, show_id INTEGER, tvdb_id TEXT, " +
                   "episode_num INTEGER, season_num INTEGER, first_air INTEGER, episode_name TEXT);");
            //Initialise to version one and then allow the update code above to bring it up to date
            ExecuteQuery("Insert Into Version (version) VALUES ( 1 );");

        }

        public int ExecuteQuery(string query)
        {
            int affected=0;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    //Create the tables
                    using (SQLiteCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = query;
                        comm.CommandType = CommandType.Text;
                        conn.Open();
                        affected = comm.ExecuteNonQuery();
                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(query + "\n" + e.Message + "\n" + e.StackTrace);
            }
            return affected;
        }
        public object ExecuteScalar(string query)
        {
            object value=null;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    using (SQLiteCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = query;
                        comm.CommandType = CommandType.Text;
                        conn.Open();
                        value = comm.ExecuteScalar();
                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(query + "\n" + e.Message + "\n" + e.StackTrace);
            }
            return value;
        }

        public List<Dictionary<string, object>> ExecuteResults(string query)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
                    using (SQLiteCommand comm = conn.CreateCommand())
                    {
                        comm.CommandText = query;
                        comm.CommandType = CommandType.Text;
                        conn.Open();
                        SQLiteDataReader value = comm.ExecuteReader();
                        while (value.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            for (int i = 0; i < value.FieldCount; i++)
                            {
                                row.Add(value.GetName(i), value.GetValue(i));
                            }
                            results.Add(row);
                        }
                        conn.Close();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(query + "\n" + e.Message + "\n" + e.StackTrace);
            }
            return results;
        }
    }
}
