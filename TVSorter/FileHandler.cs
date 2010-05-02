using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows.Forms;

namespace TVSorter
{
    public delegate Episode NameHandler(Match match, FileInfo file, bool isRootDir);

    /// <summary>
    /// Class that converts the filename of a TV show to the correct format
    /// </summary>
    class FileHandler
    {
        private const char SeperatorChar = ' ';

        private Dictionary<Regex, NameHandler> _regexMethods;

        private Dictionary<string, Episode> _files;

        public static Regex _extensions = new Regex("[.](avi|mkv|mpg|wmv)", RegexOptions.IgnoreCase);

        private Dictionary<string, int> _months;

        public FileHandler()
        {
            _regexMethods = new Dictionary<Regex, NameHandler>();
            _regexMethods.Add(new Regex("s([0-9]+)e([0-9]+)",
                RegexOptions.IgnoreCase),
                new NameHandler(SnEnHandler));
            _regexMethods.Add(new Regex("(19|20)(\\d\\d)[.](0[1-9]|1[012])[.](0[1-9]|[12][0-9]|3[01])"),
                new NameHandler(YmdHandler));
            _regexMethods.Add(new Regex("(Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\\.(\\d\\d)\\.(20\\d\\d)",
                RegexOptions.IgnoreCase), new NameHandler(MMMddyyyHandler));
            _regexMethods.Add(new Regex("([0-9]+)x([0-9]+)",
                RegexOptions.IgnoreCase),
                new NameHandler(SnEnHandler));
            _regexMethods.Add(new Regex("([0-9])([0-9][0-9])"),
                new NameHandler(SnEnHandler));
            _regexMethods.Add(new Regex("([0-9][0-9])([0-9][0-9])"),
                new NameHandler(SnEnHandler));
            _regexMethods.Add(new Regex("s([0-9]+)[.]e([0-9]+)", RegexOptions.IgnoreCase),
                new NameHandler(SnEnHandler));
            _files = new Dictionary<string, Episode>();
            _months = new Dictionary<string, int>();
            _months.Add("Jan", 1); _months.Add("Feb", 2); _months.Add("Mar", 3);
            _months.Add("Apr", 4); _months.Add("May", 5); _months.Add("Jun", 6);
            _months.Add("Jul", 7); _months.Add("Aug", 8); _months.Add("Sep", 9);
            _months.Add("Oct", 10); _months.Add("Nov", 11); _months.Add("Dec", 12);
        }

        private Episode GetEpisode(FileInfo file, bool isRootDir)
        {
            foreach (Regex regex in _regexMethods.Keys)
            {
                Match match = regex.Match(file.Name);
                if (match.Success)
                {
                    return _regexMethods[regex](match, file, isRootDir);
                }
            }
            return null;
        }

        private string parseShowName(string name, int matchPos)
        {
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
        private Episode SnEnHandler(Match match, FileInfo file, bool isRootDir)
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
        private Episode YmdHandler(Match match, FileInfo file, bool isRootDir)
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

        private Episode MMMddyyyHandler(Match match, FileInfo file, bool isRootDir)
        {
            string showName = parseShowName(file.Name, match.Index);
            string month = match.Groups[1].Value;
            string day = match.Groups[2].Value;
            string year = match.Groups[3].Value;
            int monthNum = _months[month];
            return new Episode(showName, new DateTime(int.Parse(year), monthNum, int.Parse(day)), file);
        }

        public void RefreshEpisodes(MethodInvoker inc, string inputFolder)
        {
            Log.Add("Refresh of directory: " + inputFolder);
            DirectoryInfo dir = new DirectoryInfo(inputFolder);
            _files.Clear();
            if (!dir.Exists)
                return;
            ProcessFiles(dir, true, inc);
        }

        private void ProcessFiles(DirectoryInfo dir, bool isRootDir, MethodInvoker inc)
        {
            foreach (FileInfo file in dir.GetFiles())
            {
                if (!_extensions.IsMatch(file.Extension))
                {
                    inc();
                    continue;
                }
                Episode episode = GetEpisode(file, isRootDir);
                if (episode != null)
                {
                    _files.Add(
                        file.FullName.Replace(Properties.Settings.Default.InputDir, ""),
                        episode);
                }
                inc();
            }
            if (Properties.Settings.Default.RecurseSubDir)
            {
                foreach (DirectoryInfo subdir in dir.GetDirectories())
                {
                    ProcessFiles(subdir, false, inc);
                }
            }
        }

        public Dictionary<string, Episode> Files
        {
            get { return _files; }
        }

        internal void RenameMove(MethodInvoker inc, Episode[] episodes)
        {
            foreach (Episode ep in episodes)
            {
                string newName = "";
                try
                {
                    newName = Properties.Settings.Default.OutputDir + ep.FormatOutputPath();
                    FileInfo newFile = new FileInfo(newName);
                    if (!newFile.Directory.Exists)
                        newFile.Directory.Create();
                    Log.Add("Moving file: " + ep.FileInfo.FullName + " to " + newName);
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
                    inc();
                }
            }
        }

        private void RecuriveDelete(DirectoryInfo directory)
        {
            DirectoryInfo inputDir = new DirectoryInfo(Properties.Settings.Default.InputDir);
            string inputDirPath = inputDir.FullName;
            string dirPath = directory.FullName;
            if (!inputDirPath.EndsWith("\\"))
                inputDirPath = inputDirPath + '\\';
            if (!dirPath.EndsWith("\\"))
                dirPath = dirPath + '\\';
            if (inputDirPath == dirPath)
            {
                //Reached the input dir - return
                return;
            }
            if (directory.GetFiles().Length == 0 &&
                directory.GetDirectories().Length == 0)
            {
                directory.Delete();
                RecuriveDelete(directory.Parent);
            }
        }

        internal void RenameOnly(MethodInvoker inc, Episode[] episodes)
        {
            foreach (Episode ep in episodes)
            {
                string newName = ""; ;
                try
                {
                    newName = ep.FormatOutputPath();
                    newName = newName.Substring(newName.LastIndexOf('\\') + 1);
                    newName = ep.FileInfo.DirectoryName + "\\" + newName;
                    Log.Add("Rename file: " + ep.FileInfo.FullName + " to " + newName);
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
