using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;

namespace TVSorter
{

    public enum SortAction { Move, Copy, Rename };

    /// <summary>
    /// Class that converts the filename of a TV show to the correct format
    /// </summary>
    class FileHandler
    {
        private const char SeperatorChar = ' ';
        
        private Dictionary<string, Episode> _files;

        //File extensions to look at. Anything else will be ignored
        public static Regex _extensions = new Regex("[.](avi|mkv|mpg|wmv|tbn|nfo)", RegexOptions.IgnoreCase);

        private Dictionary<string, int> _months;

        public FileHandler()
        {
            //A dictionary of filepath and episode objects
            _files = new Dictionary<string, Episode>();
            //The months and their number equivalent
            _months = new Dictionary<string, int>();
            _months.Add("Jan", 1); _months.Add("Feb", 2); _months.Add("Mar", 3);
            _months.Add("Apr", 4); _months.Add("May", 5); _months.Add("Jun", 6);
            _months.Add("Jul", 7); _months.Add("Aug", 8); _months.Add("Sep", 9);
            _months.Add("Oct", 10); _months.Add("Nov", 11); _months.Add("Dec", 12);
            _months.Add("January", 1); _months.Add("February", 2); 
            _months.Add("March", 3); _months.Add("April", 4); 
            _months.Add("June", 6); _months.Add("July", 7); 
            _months.Add("August", 8); _months.Add("September", 9);
            _months.Add("October", 10); _months.Add("November", 11); 
            _months.Add("December", 12);
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
            foreach (string regexp in Settings.FileRegex)
            {
                Match match = Regex.Match(file.Name, regexp, RegexOptions.IgnoreCase);
                //If a match is found then call the appropriate handler for the matching regexp
                if (match.Success)
                {
                    try
                    {
                        return ProcessEpisode(match, file);
                    }
                    catch (Exception e)
                    {
                        Log.Add(e.Message + " - " + regexp);
                    }
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
        /// Gets the necessary info from the file name and return the episode object
        /// </summary>
        /// <param name="match">The regexp match</param>
        /// <param name="file">The file</param>
        /// <returns>The episode object</returns>
        private Episode ProcessEpisode(Match match, FileInfo file)
        {
            //Determine if the match was a season/episode or a date.
            Group season = match.Groups["S"];
            Group episode = match.Groups["E"];
            Group year = match.Groups["Y"];
            Group month = match.Groups["M"];
            Group day = match.Groups["D"];
            if ((!(season.Success && episode.Success)) && 
                (!(year.Success && month.Success && day.Success)))
            {
                throw new Exception("Invalid regular expression.");
            }
            string showName = parseShowName(file.Name, match.Index);
            if (season.Success && episode.Success)
            {
                return new Episode(showName, int.Parse(episode.ToString()),
                    int.Parse(season.ToString()), file);
            }
            else
            {
                DateTime date;
                int monthNum;
                if (int.TryParse(month.ToString(), out monthNum))
                {
                    date = new DateTime(int.Parse(year.ToString()), monthNum, int.Parse(day.ToString()));
                }
                else
                {
                    date = new DateTime(int.Parse(year.ToString()), _months[month.ToString()],
                        int.Parse(day.ToString()));
                }
                return new Episode(showName, date, file);
            }
        }

        /// <summary>
        /// Refreshes the file list
        /// </summary>
        /// <param name="inc">Delegate to the increment method, called after each file is processed</param>
        /// <param name="inputFolder">The input directory</param>
        public void RefreshEpisodes(MethodInvoker inc, string inputFolder)
        {
            Log.Add("Refresh of directory: " + inputFolder);
            DirectoryInfo dir = new DirectoryInfo(inputFolder);
            _files.Clear();
            if (!dir.Exists)
                return;
            //Start the recursive function that processes a directory
            ProcessFiles(dir, inc);
        }

        /// <summary>
        /// A recursive function to process all the files in a given directory to try to 
        /// get all the episode in it.
        /// </summary>
        /// <param name="dir">The directory to process</param>
        /// <param name="inc">Delegate for the increment function</param>
        private void ProcessFiles(DirectoryInfo dir, MethodInvoker inc)
        {
            //Process each file in the directory
            foreach (FileInfo file in dir.GetFiles())
            {
                //Check if it matches one of the valid extensions
                if (!_extensions.IsMatch(file.Extension))
                {
                    //Increment the progress bar and move on if not.
                    if (inc != null)
                        inc();
                    continue;
                }
                try
                {
                    //Attempt to get an episode object
                    Episode episode = GetEpisode(file);
                    if (episode != null)
                    {
                        //Add the file if one is found
                        _files.Add(
                            file.FullName.Replace(Settings.InputDir, ""),
                            episode);
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error getting an episode from file \n" + file.FullName + "\n" + e.Message);
                }
                //Increment the progress bar
                if (inc != null)
                    inc();
            }
            //If the scan should be recursive then process each of the directories in this directory
            if (Settings.RecurseSubDir)
            {
                foreach (DirectoryInfo subdir in dir.GetDirectories())
                {
                    ProcessFiles(subdir, inc);
                }
            }
        }

        public Dictionary<string, Episode> Files
        {
            get { return _files; }
        }

        /// <summary>
        /// Renames and moves/copies the files
        /// </summary>
        /// <param name="inc">Delegate for the increment function</param>
        /// <param name="episodes">The array of episodes to move</param>
        /// <param name="action">The type of sorting to do</param>
        internal void SortEpisodes(MethodInvoker inc, Episode[] episodes, SortAction action)
        {
            //Process each episode
            foreach (Episode ep in episodes)
            {
                string newName = "";
                try
                {
                    //Get the new path
                    newName = Settings.OutputDir + ep.FormatOutputPath();
                    if (action == SortAction.Rename)
                    {
                        newName = newName.Substring(newName.LastIndexOf('\\') + 1);
                        newName = ep.FileInfo.DirectoryName + "\\" + newName;
                    }
                    FileInfo newFile = new FileInfo(newName);
                    //Create the directory if it doesn't exist
                    if (!newFile.Directory.Exists)
                        newFile.Directory.Create();
                    //Move/copy the file and delete the directory it was in if it is now empty
                    if (action == SortAction.Copy)
                    {
                        File.Copy(ep.FileInfo.FullName, newName);
                    }
                    else
                    {
                        File.Move(ep.FileInfo.FullName, newName);
                    }
                    if (Settings.DeleteEmpty)
                    {
                        RecuriveDelete(ep.FileInfo.Directory);
                    }
                    Log.Add(action.ToString() + " file: " + ep.FileInfo.FullName + " -> " + newName);
                }
                catch (Exception e)
                {
                    Log.Add("Failed " + action.ToString() + ": " + ep.FileInfo.FullName + " -> " +
                        newName + " -- " + e.Message);
                }
                finally
                {
                    if (inc != null)
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
            DirectoryInfo inputDir = new DirectoryInfo(Settings.InputDir);
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
    }
}
