using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace TVSorter
{
    static class Program
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        public static string VersionNumber = "0.2";

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
            else
            {
                foreach (string arg in args)
                {
                   AllocConsole();
                    Database database = new Database();
                    Log.Init();
                    if (arg == "-update_all") //Update the database
                    {
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
                    else if (arg.StartsWith("-rename_")) //Rename and move/copy/only
                    {
                        //Determine the correct sort action
                        SortAction action;
                        if (arg == "-rename_only")
                        {
                            action = SortAction.Rename;
                        }
                        else if (arg == "-rename_move")
                        {
                            action = SortAction.Move;
                        }
                        else if (arg == "-rename_copy")
                        {
                            action = SortAction.Copy;
                        }
                        else
                        {
                            Log.Add(arg + " not recognised");
                            continue; //Invalid arg, ignore
                        }
                        //Check the directories are valid
                        Properties.Settings settings = Properties.Settings.Default;
                        if (settings.InputDir == "" || !Directory.Exists(settings.InputDir))
                        {
                            Log.Add("Input directory not set");
                            continue;
                        }
                        if ((action != SortAction.Rename) && 
                            (settings.OutputDir == "" || !Directory.Exists(settings.OutputDir)))
                        {
                            Log.Add("Output directory not set");
                        }
                        //Refresh the episodes and the sort them
                        FileHandler fileHandler = new FileHandler();
                        fileHandler.RefreshEpisodes(null, settings.InputDir);
                        Dictionary<string,Episode> episodes = fileHandler.Files;
                        Episode[] episodeArr = new Episode[episodes.Count];
                        episodes.Values.CopyTo(episodeArr,0);
                        fileHandler.SortEpisodes(null, episodeArr, action);
                    }
                }
            }
        }
    }
}
