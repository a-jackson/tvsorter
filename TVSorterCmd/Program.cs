// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Program.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   TVSorterCmd's program.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter
{
    #region Using Directives

    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    using TVSorter.DAL;

    #endregion

    /// <summary>
    /// TVSorterCmd's program.
    /// </summary>
    internal class Program
    {
        #region Public Methods and Operators

        /// <summary>
        /// The program's entry point.
        /// </summary>
        /// <param name="args">
        /// The command line arguments. 
        /// </param>
        public static void Main(string[] args)
        {
            var storage = Factory.StorageProvider;
            var scanner = Factory.Scanner;
            var fileManager = Factory.FileManager;
            var data = Factory.DataProvider;

            data.LogMessage += LogMessageReceived;
            scanner.LogMessage += LogMessageReceived;
            fileManager.LogMessage += LogMessageReceived;

            var shows = storage.LoadTvShows();

            foreach (var arg in args)
            {
                switch (arg.ToLower())
                {
                    case "-update_all":
                        data.UpdateShows(shows.Where(x => !x.Locked).ToList());
                        break;
                    case "-copy":
                    case "-move":
                        var results = scanner.Refresh(
                            Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture));
                        if (arg.Equals("-copy"))
                        {
                            fileManager.CopyFile(results);
                        }
                        else
                        {
                            fileManager.MoveFile(results);
                        }

                        break;
                    default:
                        Console.WriteLine("Unrecognised switch: {0}", arg);
                        return;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Handles a log message being received.
        /// </summary>
        /// <param name="sender">
        /// The sender of the event. 
        /// </param>
        /// <param name="e">
        /// The arguments of the event. 
        /// </param>
        private static void LogMessageReceived(object sender, LogMessageEventArgs e)
        {
            Console.WriteLine(e.ToString());
        }

        #endregion
    }
}