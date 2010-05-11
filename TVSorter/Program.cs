using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net;

namespace TVSorter
{
    static class Program
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole(); 

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());
            }
            else if (args[0] == "-update_all") //Update the database
            {
                AllocConsole();
                Database database = new Database();
                Log.Init();
                //Get all the tv shows
                List<Dictionary<string, object>> shows =
                    database.ExecuteResults("SELECT * FROM shows ORDER BY name");
                List<TVShow> showList = new List<TVShow>();
                foreach (Dictionary<string, object> row in shows)
                {
                    List<Dictionary<string, object>> altName =
                        database.ExecuteResults("Select * From AltNames Where show_id = "
                        + (long)row["id"] + ";");
                    string altNames = "";
                    foreach (Dictionary<string, object> altNameRow in altName)
                        altNames += (string)altNameRow["alt_name"] + ",";
                    if (altNames.EndsWith(","))
                        altNames = altNames.Substring(0, altNames.Length - 1);
                    //Create the TVShow object with the data in the database
                    TVShow tvshow = new TVShow((long)row["id"],
                        (string)row["tvdb_id"], (string)row["name"],
                        (long)row["update_time"],
                        ((long)row["use_default_format"] == 1 ? true : false),
                        (string)row["custom_format"],
                        (string)row["folder_name"],
                        (string)row["banner"],
                        altNames);
                    showList.Add(tvshow);
                }
                try
                {
                    //Update each show
                    foreach (TVShow show in showList)
                    {
                        TVDB.UpdateShow(show, false);
                    }
                }
                catch (WebException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
