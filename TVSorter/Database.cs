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
    /// <summary>
    /// Handles database queries, makes the database if 
    /// one doesn't exist and provides methods for running queries
    /// </summary>
    public class Database
    {
        private static string DatabaseFile = "Data" + Path.DirectorySeparatorChar + "data.db";
        private static string connectionString = "Data Source=" + DatabaseFile + ";Version=3;";
        /// <summary>
        /// The current version of the database, used to detect older databases so they can
        /// be updated to new ones.
        /// </summary>
        private const int DatabaseVersion = 3;

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
                if (version < 3)
                {
                    ExecuteQuery("Alter Table Shows Add Column show_locked Integer Default 0");
                    ExecuteQuery("Alter Table Shows Add Column use_dvd_order Integer Default 0");
                    ExecuteQuery("Update Version Set version = 3 Where id = 1");
                }
            }
        }

        /// <summary>
        /// Creates the initial, verson 1 database file
        /// </summary>
        private void CreateDatabase()
        {
            if (!Directory.Exists("Data"))
            {
                Directory.CreateDirectory("Data");
            }
            SQLiteConnection.CreateFile(DatabaseFile);
            //Create the table for the version number, shows, and episodes
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
        /// <summary>
        /// Executes a query that doesn't return any data, e.g. an Insert, Update, Delete
        /// </summary>
        /// <param name="query">The query to execute</param>
        /// <returns>The number of rows affected by the query</returns>
        public int ExecuteQuery(string query)
        {
            int affected=0;
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection(connectionString))
                {
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

        /// <summary>
        /// Executes a query and returns the first object that is returned. This
        /// is the first field of the first record.
        /// </summary>
        /// <param name="query">The query to execute</param>
        /// <returns>The object that is returned</returns>
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

        /// <summary>
        /// Runs a query that returns lots of data.
        /// </summary>
        /// <param name="query">The query to run</param>
        /// <returns>The data that is returned. The List items are records and the 
        /// dictionary values are the data, the keys are the field names</returns>
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
                        //Read the data
                        while (value.Read())
                        {
                            //Create a new dictionary for the row and fill it
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
