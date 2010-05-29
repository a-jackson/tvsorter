using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;

namespace TVSorter
{
    public delegate Episode NameHandler(Match match, FileInfo file);

    /// <summary>
    /// Class that converts the filename of a TV show to the correct format
    /// </summary>
    class FileHandler
    {
        private const char SeperatorChar = ' ';

        private Dictionary<Regex, NameHandler> _regexMethods;

        private Dictionary<string, Episode> _files;

        //File extensions to look at. Anything else will be ignored
        public static Regex _extensions = new Regex("[.](avi|mkv|mpg|wmv)", RegexOptions.IgnoreCase);

        private Dictionary<string, int> _months;

        public FileHandler()
        {
            //Add the regular expressions to test as keys with the handler function as a value
            _regexMethods = new Dictionary<Regex, NameHandler>();
            //S01E02
            _regexMethods.Add(new Regex("s([0-9]+)e([0-9]+)",
                RegexOptions.IgnoreCase),
                new NameHandler(SnEnHandler));
            //2010.12.31
            _regexMethods.Add(new Regex("(19|20)(\\d\\d)[.](0[1-9]|1[012])[.](0[1-9]|[12][0-9]|3[01])"),
                new NameHandler(YmdHandler));
            //Dec.31.2010
            _regexMethods.Add(new Regex("(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\\.(\\d\\d)\\.(20\\d\\d)",
                RegexOptions.IgnoreCase), new NameHandler(MMMddyyyHandler));
            //1x02
            _regexMethods.Add(new Regex("([0-9]+)x([0-9]+)",
                RegexOptions.IgnoreCase),
                new NameHandler(SnEnHandler));
            //0102
            _regexMethods.Add(new Regex("([0-9][0-9])([0-9][0-9])"),
                new NameHandler(SnEnHandler));
            //102
            _regexMethods.Add(new Regex("([0-9])([0-9][0-9])"),
                new NameHandler(SnEnHandler));
            //S01.E02
            _regexMethods.Add(new Regex("s([0-9]+)[.]e([0-9]+)", RegexOptions.IgnoreCase),
                new NameHandler(SnEnHandler));
            //A dictionary of filepath and episode objects
            _files = new Dictionary<string, Episode>();
            //The months and their number equivalent
            _months = new Dictionary<string, int>();
            _months.Add("Jan", 1); _months.Add("Feb", 2); _months.Add("Mar", 3);
            _months.Add("Apr", 4); _months.Add("May", 5); _months.Add("Jun", 6);
            _months.Add("Jul", 7); _months.Add("Aug", 8); _months.Add("Sep", 9);
            _months.Add("Oct", 10); _months.Add("Nov", 11); _months.Add("Dec", 12);
        }

        /// <summary>
        /// Returns an episode object for the given file. Hopes that it is able to 
        /// detect which show and episode it and has all the necessary information
        /// </summary>
        /// <param name="file">The file</param>
        /// <param name="isRootDir">If this file is in the root of the input directory</param>
        /// <returns></returns>
        private Episode GetEpisode(FileInfo file)
        {
            //Test each of the regexp
            foreach (Regex regex in _regexMethods.Keys)
            {
                Match match = regex.Match(file.Name);
                //If a match is found then call the appropriate handler for the matching regexp
                if (match.Success)
                {
                    return _regexMethods[regex](match, file);
                }
            }
            //No match, not a TV show.
            return null;
        }

        /// <summary>
        /// Parses the filename to try to determine the shows name and formats it neatly
        /// with spaces between words and each word starting with a capital letter
        /// </summary>
        /// <param name="name">The file name</param>
        /// <param name="matchPos">The index in the string that the match was found</param>
        /// <returns>The parsed episode name</returns>
        private string parseShowName(string name, int matchPos)
        {
            //Assume that the show's name is everything before the season/episode number
            string showName = name.Substring(0, matchPos);
            string[] showNameWords = showName.Split(new char[] { '.', '-', ' ', '_' });
            string finalShowName = "";
            //It is possible that the show name is made up of several 
            //words but they don't have any separating chars
            if (showNameWords.Length == 1 || (showNameWords.Length == 2 && showNameWords[1].Equals("")))
            {
                //String starts with either upper or lowercase letter
                //followed by 1 or more lower case, then anything
                //followed by an uppercase letter
                //e.g wakingTheDead
                Regex regEx = new Regex("^[A-Za-z]([a-z]+).*[A-Z]");
                Match match = regEx.Match(showName);
                if (match.Success)
                {
                    //Separate at the captial letters
                    char[] letters = showName.ToCharArray();
                    //Skip the first letter is intentional
                    for (int i = 1; i < letters.Length; i++)
                    {
                        if (char.IsUpper(letters[i]))
                        {
                            //Insert a . in the string at the new word
                            showName = showName.Insert(i, ".");
                            //Remake the char array and increment i to allow
                            //for the extra char
                            letters = showName.ToCharArray();
                            i++;
                        }
                    }
                    //Split the name again
                    showNameWords = showName.Split(new char[] { '.', '-', ' ', '_' });
                }
            }
            //Loop through each of the word of the name
            //Make them all lower case except for the first char
            //and stitch together into the final show name
            foreach (string nameWord in showNameWords)
            {
                if (nameWord.Length == 0)
                {
                    continue;
                }
                string lowerNameWord = nameWord.ToLower();
                char[] word = lowerNameWord.ToCharArray();
                word[0] = char.ToUpper(word[0]);
                string showWord = new String(word);
                finalShowName += showWord + SeperatorChar;
            }
            return finalShowName.Trim();
        }

        /// <summary>
        /// Handler for names with the episode number in
        /// the format Name.(Season)(Episode) 
        /// Doesn't matter what is inbetween the numbers, the regex handles it.
        /// </summary>
        /// <param name="match">The Regex match</param>
        /// <param name="file">The file that matched</param>
        /// <returns>The TVShow object</returns>
        private Episode SnEnHandler(Match match, FileInfo file)
        {
            string showName = parseShowName(file.Name, match.Index);
            string strSeasonNum = match.Groups[1].ToString();
            string strEpisodeNum = match.Groups[2].ToString();
            return new Episode(showName, int.Parse(strEpisodeNum), int.Parse(strSeasonNum), file);
        }

        /// <summary>
        /// Handles shows that are dates. The regex should have 4 groups.
        /// (yy)(yy)(mm)(dd)
        /// </summary>
        /// <param name="match">The regex match</param>
        /// <param name="file">The file that was matched</param>
        /// <returns>The TVShow object</returns>
        private Episode YmdHandler(Match match, FileInfo file)
        {
            string showName = parseShowName(file.Name, match.Index);
            //want year/month/day numbers
            string yearStr = match.Groups[1].ToString() + match.Groups[2].ToString();
            int year = int.Parse(yearStr);
            int month = int.Parse(match.Groups[3].ToString());
            int day = int.Parse(match.Groups[4].ToString());
            DateTime date = new DateTime(year, month, day);

            return new Episode(showName, date, file);
        }

        /// <summary>
        /// Handler function for dates in the format Dec.31.2010
        /// </summary>
        /// <param name="match">The regexp's match object</param>
        /// <param name="file">The file that was matched</param>
        /// <returns></returns>
        private Episode MMMddyyyHandler(Match match, FileInfo file)
        {
            string showName = parseShowName(file.Name, match.Index);
            string month = match.Groups[1].Value;
            string day = match.Groups[2].Value;
            string year = match.Groups[3].Value;
            int monthNum = _months[month];
            return new Episode(showName, new DateTime(int.Parse(year), monthNum, int.Parse(day)), file);
        }

        /// <summary>
        /// Refreshes the file list
        /// </summary>
        /// <param name="inc">Delegate to the increment method, called after each file is processed</param>
        /// <param name="inputFolder">The input directory</param>
        /// <param name="shows">The TVShow objects</param>
        public void RefreshEpisodes(MethodInvoker inc, string inputFolder, Dictionary<long,TVShow> shows)
        {
            Log.Add("Refresh of directory: " + inputFolder);
            DirectoryInfo dir = new DirectoryInfo(inputFolder);
            _files.Clear();
            if (!dir.Exists)
                return;
            //Start the recursive function that processes a directory
            ProcessFiles(dir, shows, inc);
        }

        /// <summary>
        /// A recursive function to process all the files in a given directory to try to 
        /// get all the episode in it.
        /// </summary>
        /// <param name="dir">The directory to process</param>
        /// <param name="shows">The TVShow objects</param>
        /// <param name="inc">Delegate for the increment function</param>
        private void ProcessFiles(DirectoryInfo dir, Dictionary<long, TVShow> shows, MethodInvoker inc)
        {
            //Process each file in the directory
            foreach (FileInfo file in dir.GetFiles())
            {
                //Check if it matches one of the valid extensions
                if (!_extensions.IsMatch(file.Extension))
                {
                    //Increment the progress bar and move on if not.
                    inc();
                    continue;
                }
                try
                {
                    //Attempt to get an episode object
                    Episode episode = GetEpisode(file);
                    if (episode != null)
                    {
                        //If the show was found in the database then use the object
                        //we already have to get any settings that might be set but not
                        //saved.
                        if (episode.Show != null && episode.Show.DatabaseId != -1)
                        {
                            episode.Show = shows[episode.Show.DatabaseId];
                        }
                        //Set the show to be one from the 
                        //Add the file if one is found
                        _files.Add(
                            file.FullName.Replace(Properties.Settings.Default.InputDir, ""),
                            episode);
                    }
                }
                catch
                {
                    MessageBox.Show("Error getting an episode from file \n" + file.FullName);
                }
                //Increment the progress bar
                inc();
            }
            //If the scan should be recursive then process each of the directories in this directory
            if (Properties.Settings.Default.RecurseSubDir)
            {
                foreach (DirectoryInfo subdir in dir.GetDirectories())
                {
                    ProcessFiles(subdir, shows, inc);
                }
            }
        }

        public Dictionary<string, Episode> Files
        {
            get { return _files; }
        }

        /// <summary>
        /// Renames and moves the files
        /// </summary>
        /// <param name="inc">Delegate for the increment function</param>
        /// <param name="episodes">The array of episodes to move</param>
        internal void RenameMove(MethodInvoker inc, Episode[] episodes)
        {
            //Process each episode
            foreach (Episode ep in episodes)
            {
                string newName = "";
                try
                {
                    //Get the new path
                    newName = Properties.Settings.Default.OutputDir + ep.FormatOutputPath();
                    FileInfo newFile = new FileInfo(newName);
                    //Create the directory if it doesn't exist
                    if (!newFile.Directory.Exists)
                        newFile.Directory.Create();
                    Log.Add("Moving file: " + ep.FileInfo.FullName + " to " + newName);
                    //Move te file and delete the directory it was in if it is now empty
                    File.Move(ep.FileInfo.FullName, newName);
                    if (Properties.Settings.Default.DeleteEmpty)
                    {
                        RecuriveDelete(ep.FileInfo.Directory);
                    }
                    Log.Add("Move complete");
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to move " + ep.FileInfo.FullName + "\n to " +
                        newName + "\n" + e.Message + "\n" + e.StackTrace);
                    Log.Add("Failed to move " + ep.FileInfo.FullName + " to " +
                        newName + " " + e.Message);
                }
                finally
                {
                    inc(); //Inc the progress bar
                }
            }
        }

        /// <summary>
        /// Recursively deletes a directory down to the input directory as long as it is empty.
        /// E.g if directory is c:\tv\some show\season 1 and it is empty it will be deleted.
        /// If this leaves some show empty, that will be deleted. If c:\tv is the input directory
        /// then it will stop here.
        /// </summary>
        /// <param name="directory">The directory to start at</param>
        private void RecuriveDelete(DirectoryInfo directory)
        {
            //Determine if this is the input directory
            DirectoryInfo inputDir = new DirectoryInfo(Properties.Settings.Default.InputDir);
            //Get the paths
            string inputDirPath = inputDir.FullName;
            string dirPath = directory.FullName;
            if (!inputDirPath.EndsWith("\\"))
                inputDirPath = inputDirPath + '\\';
            inputDirPath.ToLower();
            if (!dirPath.EndsWith("\\"))
                dirPath = dirPath + '\\';
            dirPath.ToLower();
            if (inputDirPath == dirPath)
            {
                //Reached the input dir - return
                return;
            }
            //Ensure it is empty and delete
            if (directory.GetFiles().Length == 0 &&
                directory.GetDirectories().Length == 0)
            {
                directory.Delete();
                RecuriveDelete(directory.Parent);
            }
        }

        /// <summary>
        /// Only renames the episodes, doesn't move them
        /// </summary>
        /// <param name="inc">The increment delegate, called after each file</param>
        /// <param name="episodes">The array of episodes to process</param>
        internal void RenameOnly(MethodInvoker inc, Episode[] episodes)
        {
            foreach (Episode ep in episodes)
            {
                string newName = ""; ;
                try
                {
                    //Determine the new name but strip off any part that represents a file path
                    newName = ep.FormatOutputPath();
                    newName = newName.Substring(newName.LastIndexOf('\\') + 1);
                    newName = ep.FileInfo.DirectoryName + "\\" + newName;
                    Log.Add("Rename file: " + ep.FileInfo.FullName + " to " + newName);
                    //Rename the file
                    File.Move(ep.FileInfo.FullName, newName);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Failed to rename " + ep.FileInfo.FullName + "\n to " +
                        newName + "\n" + e.Message + "\n" + e.StackTrace);
                    Log.Add("Failed to rename " + ep.FileInfo.FullName + " to " +
                        newName + " " + e.Message);
                }
                finally
                {
                    inc();
                }
            }
        }
    }
}
