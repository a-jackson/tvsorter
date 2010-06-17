using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TVSorter
{
    class Settings
    {
        private const string SettingsDir = "Settings";
        private const string SettingsFile = SettingsDir + "\\settings.cfg";
        private const string RegexpFile = SettingsDir + "\\regexp.cfg";

        public static string OutputDir = "";
        public static string InputDir = "";
        public static bool RecurseSubDir = false;
        public static bool DeleteEmpty = false;
        public static string FileNameFormat = 
            "{SName( )}\\Season {SNum(1)}\\{SName(.)}." +
            "S{SNum(2)}E{ENum(2)}.{EName(.)}{Ext}";

        public static List<string> FileRegex;

        public static void LoadSettings()
        {
            if (!File.Exists(SettingsFile))
            {
                return;
            }
            try
            {
                StreamReader reader = new StreamReader(SettingsFile);
                OutputDir = reader.ReadLine();
                InputDir = reader.ReadLine();
                RecurseSubDir = bool.Parse(reader.ReadLine());
                DeleteEmpty = bool.Parse(reader.ReadLine());
                FileNameFormat = reader.ReadLine();
                reader.Close();
            }
            catch (Exception e)
            {
                Log.Add("Error loading settings, defaults used: " + e.Message);
            }
            finally //Don't allow string to be null
            {
                if (OutputDir == null) OutputDir = "";
                if (InputDir == null) InputDir = "";
                if (FileNameFormat == null) FileNameFormat = "";
            }
        }

        public static void SaveSettings()
        {
            if (!Directory.Exists(SettingsDir))
            {
                Directory.CreateDirectory(SettingsDir);
            }
            try
            {
                StreamWriter writer = new StreamWriter(SettingsFile);
                writer.WriteLine(OutputDir);
                writer.WriteLine(InputDir);
                writer.WriteLine(RecurseSubDir.ToString());
                writer.WriteLine(DeleteEmpty.ToString());
                writer.WriteLine(FileNameFormat);
                writer.Flush();
                writer.Close();
            }
            catch (Exception e)
            {
                Log.Add("Error saving settings: " + e.Message);
            }
        }

        public static void LoadFileRegexp()
        {
            FileRegex = new List<string>();
            if (!File.Exists(RegexpFile))
            {
                LoadDefaultRegexp();
                return;
            }
            try
            {
                StreamReader reader = new StreamReader(RegexpFile);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    FileRegex.Add(line);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Log.Add("Error loading regexp file, using defaults. " + e.Message);
                LoadDefaultRegexp();
            }
        }

        public static void SaveFileRegexp()
        {
            if (!Directory.Exists(SettingsDir))
            {
                Directory.CreateDirectory(SettingsDir);
            }
            try
            {
                StreamWriter writer = new StreamWriter(RegexpFile);
                foreach (string line in FileRegex)
                {
                    writer.WriteLine(line);
                }
                writer.Flush();
                writer.Close();
            }
            catch (Exception e)
            {
                Log.Add("Error saving regexp file. " + e.Message);
            }
        }

        private static void LoadDefaultRegexp()
        {
            FileRegex.Add("s(?<S>[0-9]+)e(?<E>[0-9]+)");
            FileRegex.Add("(?<Y>19\\d\\d|20\\d\\d)[.](?<M>0[1-9]|1[012])[.](?<D>0[1-9]|[12][0-9]|3[01])");
            FileRegex.Add("(?<M>Jan|Feb|Mar|Apr|May|Jun|Jul|Aug|Sep|Oct|Nov|Dec)\\.(?<D>\\d\\d)\\.(?<Y>20\\d\\d)");
            FileRegex.Add("(?<S>[0-9]+)\\s-\\s(?<E>[0-9]+)");
            FileRegex.Add("(?<S>[0-9]+)x(?<E>[0-9]+)");
            FileRegex.Add("(?<S>[0-9][0-9])(?<E>[0-9][0-9])");
            FileRegex.Add("(?<S>[0-9])(?<E>[0-9][0-9])");
            FileRegex.Add("s(?<S>[0-9]+)[.]e(?<E>[0-9]+)");
        }        
    }
}