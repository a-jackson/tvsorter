using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.IO;

namespace TVSorter
{
    class CommandLine
    {
        string[] _args;
        frmCmdLog _log;

        public CommandLine(string[] args, frmCmdLog log)
        {
            _args = args;
            _log = log;
            Thread thread = new Thread(new ThreadStart(run));
            thread.Start();
        }

        public void run()
        {               
            Database database = new Database();
            Settings.LoadSettings();
            Log.Init(_log);
            Log.Add("Program started");
            Log.Add("Input directory: " + Settings.InputDir);
            Log.Add("Output directory: " + Settings.OutputDir);
            Log.Add("Recurse subdirs: " + Settings.RecurseSubDir);
            Log.Add("Delete empty dir: " + Settings.DeleteEmpty);
            Log.Add("Output file format: " + Settings.FileNameFormat);
            foreach (string arg in _args)
            {
                if (arg == "-update_all") //Update the database
                {
                    List<TVShow> showList = TVShow.GetAllShows();
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
                        Log.Add(e.Message);
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
                    if (Settings.InputDir == "" || !Directory.Exists(Settings.InputDir))
                    {
                        Log.Add("Input directory not set");
                        continue;
                    }
                    if ((action != SortAction.Rename) &&
                        (Settings.OutputDir == "" || !Directory.Exists(Settings.OutputDir)))
                    {
                        Log.Add("Output directory not set");
                    }
                    //Refresh the episodes and the sort them
                    FileHandler fileHandler = new FileHandler();
                    fileHandler.RefreshEpisodes(null, Settings.InputDir);
                    Dictionary<string, Episode> episodes = fileHandler.Files;
                    Episode[] episodeArr = new Episode[episodes.Count];
                    episodes.Values.CopyTo(episodeArr, 0);
                    fileHandler.SortEpisodes(null, episodeArr, action);
                }
                else
                {
                    //Probably a log file but might not be valid filename
                    FileInfo log = new FileInfo(arg);
                    Log.SetSaveLocation(log);
                }
            }
            Log.Add("Program completed. Exiting...");
            Log.Save();
            //Sleep briefly to allow reading time
            Thread.Sleep(3000);
            Environment.Exit(0);
        }
    }
}
