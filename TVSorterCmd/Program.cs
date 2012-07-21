// --------------------------------------------------------------------------------------------------------------------
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
            Logger.LogMessage += (sender, e) => Console.WriteLine(e.LogMessage);

            foreach (string arg in args)
            {
                switch (arg.ToLower())
                {
                    case "-update_all":
                        IEnumerable<TvShow> shows = TvShow.GetTvShows();
                        foreach (TvShow show in shows.Where(x => !x.Locked))
                        {
                            show.Update();
                        }

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