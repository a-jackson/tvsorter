﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Program.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   TVSorterCmd's program.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace TVSorter
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Linq;

    using TVSorter.Model;

    #endregion

    /// <summary>
    /// TVSorterCmd's program.
    /// </summary>
    internal static class Program
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
            Logger.LogMessage += (sender, e) => Console.WriteLine(e.LogMessage);
            Console.WriteLine("TVSorter Version {0}", Version.CurrentVersion);

            foreach (string arg in args)
            {
                switch (arg.ToLower())
                {
                    case "-update_all":
                        List<TvShow> shows = TvShow.GetTvShows().Where(x => !x.Locked).ToList();
                        TvShow.UpdateShows(shows);
                        break;
                    case "-copy":
                    case "-move":
                        var fileSearch = new FileSearch();
                        fileSearch.Search(null);
                        foreach (FileResult result in fileSearch.Results)
                        {
                            result.Checked = true;
                        }

                        if (arg.Equals("-copy"))
                        {
                            fileSearch.Copy();
                        }
                        else
                        {
                            fileSearch.Move();
                        }

                        break;
                    default:
                        Console.WriteLine("Unrecognised switch: {0}", arg);
                        return;
                }
            }
        }

        #endregion
    }
}