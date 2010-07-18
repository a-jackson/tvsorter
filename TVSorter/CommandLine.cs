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
            try
            {
                Database database = new Database();
                Settings.LoadSettings();
                Settings.LoadFileRegexp();
                Log.Init(_log);
                Log.Add("Program started on " + DateTime.Now.ToLongDateString());
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
                            TaskWaiter waiter = new TaskWaiter(showList.Count);
                            TVDB.Instance.SetEvents(waiter.Increment, waiter.Abort, waiter.Complete);
                            TVDB.Instance.UpdateShows(false, showList.ToArray());
                            waiter.Wait();
                            TVDB.Instance.ClearEvents(waiter.Increment, waiter.Abort, waiter.Complete);
                            //Wait for the update to finish.
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
                        fileHandler.RefreshEpisodes(Settings.InputDir);
                        //Get the files and copy into an array
                        Dictionary<string, Episode> episodes = fileHandler.Files;
                        Episode[] episodeArr = new Episode[episodes.Count];
                        episodes.Values.CopyTo(episodeArr, 0);
                        //Set up the waiter to wait until the task is complete
                        TaskWaiter waiter = new TaskWaiter(episodes.Count);
                        fileHandler.SetEvents(waiter.Increment, waiter.Abort, waiter.Complete);
                        //Sort the files.
                        fileHandler.SortEpisodes(episodeArr, action);
                        //Wait until complete
                        waiter.Wait();
                        fileHandler.ClearEvents(waiter.Increment, waiter.Abort, waiter.Complete);
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
            }
            finally
            {
                Environment.Exit(0);
            }
        }

        private class TaskWaiter
        {
            private int _num;
            private int _count;
            public TaskWaiter(int num)
            {
                _num = num;
                _count = 0;
            }
            public void Wait()
            {
                lock (this)
                {
                    Monitor.Wait(this);
                }
            }
            public void Increment()
            {
                _count++;
            }
            public void Abort(string err)
            {
            }
            public void Complete()
            {
                lock (this)
                {
                    Monitor.Pulse(this);
                }
            }
        }
    }
}