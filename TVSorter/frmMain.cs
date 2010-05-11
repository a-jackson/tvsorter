using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.IO;
using System.Threading;
using System.Data.SQLite;

namespace TVSorter
{
    public partial class frmMain : Form
    {
        Properties.Settings _settings;
        Database _database;
        FileHandler _fileHandler;
        Dictionary<string, Episode> _files;
        private bool _onSettings = false;

        public frmMain()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initialise the form
        /// </summary>
        private void Init()
        {
            _settings = Properties.Settings.Default;
            _database = new Database();
            _fileHandler = new FileHandler();
            LoadSettings();
            UpdateTvShowList();
            grpShowCustomFormat.Enabled = false;
            flShowControls.Enabled = false;
            Log.Init(lstLog);
            Log.Add("Program started");
        }

        /// <summary>
        /// Set the controls to match the settings
        /// </summary>
        private void LoadSettings()
        {
            this.txtInputFolder.Text = _settings.InputDir;
            this.txtOutputFolder.Text = _settings.OutputDir;
            this.chkRecurse.Checked = _settings.RecurseSubDir;
            this.txtNameFormat.Text = _settings.FileNameFormat;
            this.chkDeleteEmpty.Checked = _settings.DeleteEmpty;
            UpdateExampleOutput();
            UpdateFolderFilter();
        }

        /// <summary>
        /// Sets the settings to match the controls
        /// </summary>
        private void ApplySettings()
        {
            _settings.InputDir = this.txtInputFolder.Text;
            _settings.OutputDir = this.txtOutputFolder.Text;
            _settings.RecurseSubDir = this.chkRecurse.Checked;
            _settings.FileNameFormat = this.txtNameFormat.Text;
            _settings.DeleteEmpty = this.chkDeleteEmpty.Checked;
            UpdateExampleOutput();
            UpdateFolderFilter();
        }

        /// <summary>
        /// Saves the settings
        /// </summary>
        private void SaveSettings()
        {
            ApplySettings();
            _settings.Save();
        }

        /// <summary>
        /// Shows the example output format
        /// </summary>
        private void UpdateExampleOutput()
        {
            txtExampleName.Text = Episode.ExampleEpiosde.FormatOutputPath();
        }

        /// <summary>
        /// Updates the combo box to show the available sudirectories
        /// </summary>
        private void UpdateFolderFilter()
        {
            string selected = (string)cboFolderFilter.SelectedItem;
            cboFolderFilter.Items.Clear();
            cboFolderFilter.Items.Add("Subfolder filter (All)");
            if (_settings.InputDir.Length > 0)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(_settings.InputDir);
                if (dirInfo.Exists)
                {
                    foreach (DirectoryInfo subdirInfo in dirInfo.GetDirectories())
                    {
                        cboFolderFilter.Items.Add(subdirInfo.Name);
                    }
                }
            }
            if (selected != null && cboFolderFilter.Items.Contains(selected))
            {
                cboFolderFilter.SelectedIndex = cboFolderFilter.Items.IndexOf(selected);
            }
            else
            {
                cboFolderFilter.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Searches the output directory for folders and assumes them to be show names,
        /// it then attempts to find them on the TVDB and add them to the database.
        /// </summary>
        private void SearchNewShows()
        {
            frmProgress progress =
                new frmProgress(Directory.GetDirectories(_settings.OutputDir).Length);
            SortedDictionary<string, List<TVShow>> shows = new SortedDictionary<string,List<TVShow>>();
            MethodInvoker inc = new MethodInvoker(progress.Increment);
            Thread addShows = new Thread(new ThreadStart(delegate()
            {
                foreach (string dir in Directory.GetDirectories(_settings.OutputDir))
                {
                    //Check if the show has already been added.
                    string name = dir.Substring(dir.LastIndexOf('\\') + 1).Replace("\"","\"\"");
                    if ((long)_database.ExecuteScalar("SELECT COUNT(*) FROM shows WHERE folder_name=\"" 
                        + name + "\"") == 0)
                    {
                        try{                        
                            //Search for the show and add the results
                            List<TVShow> results = TVDB.SearchShow(name);
                            shows.Add(name, results);
                        }
                        catch
                        {
                            MessageBox.Show("The TVDB is down, unable to search for shows.");
                            progress.Abort();
                            return;
                        }
                    }
                    progress.Invoke(inc);
                }
            }));
            addShows.Start();
            if (progress.ShowDialog() == DialogResult.Abort)
                return;
            //Process the results
            foreach (KeyValuePair<string, List<TVShow>> kvPair in shows)
            {
                //If there is just one result, use that
                if (kvPair.Value.Count == 1)
                {
                    _database.ExecuteQuery("Insert Into Shows " +
                        "(tvdb_id, name, update_time, use_default_format, custom_format, folder_name, banner) " +
                        "Values ('" + kvPair.Value[0].TvdbId + "', \"" + kvPair.Value[0].Name.Replace("\"","\"\"") + "\", " +
                         kvPair.Value[0].UpdateTime + ", " 
                         + (kvPair.Value[0].UseDefaultFormat ? 1 : 0) + ", '" +
                          kvPair.Value[0].CustomFormat + "', '"+kvPair.Key+"', '"+kvPair.Value[0].Banner+"');");
                }
                else //Else show the possibilities to the user
                {
                    frmShowResults results = new frmShowResults(kvPair.Value, kvPair.Key);
                    results.ShowDialog();
                    TVShow selected = results.GetSelected();
                    if (selected != null)
                    {
                        _database.ExecuteQuery("Insert Into Shows " +
                            "(tvdb_id, name, update_time, use_default_format, custom_format, folder_name,banner) " +
                            "Values ('" + selected.TvdbId + "', \"" + kvPair.Value[0].Name.Replace("\"","\"\"") + "\", " +
                             selected.UpdateTime + ", "
                             + (selected.UseDefaultFormat ? 1 : 0) + ", '" +
                              selected.CustomFormat + "', '" + kvPair.Key + "','"+selected.Banner+"');");
                    }
                }
            }
            UpdateTvShowList();
        }

        /// <summary>
        /// Updates the list box containing the TV shows.
        /// </summary>
        private void UpdateTvShowList()
        {
            lstTVShows.Items.Clear();
            List<Dictionary<string, object>> shows = _database.ExecuteResults("SELECT * FROM shows ORDER BY name");
            if (shows.Count > 0)
            {
                foreach (Dictionary<string, object> row in shows)
                {
                    List<Dictionary<string, object>> altName = _database.ExecuteResults("Select * From AltNames Where show_id = " 
                        + (long)row["id"] + ";");
                    string altNames = "";
                    foreach (Dictionary<string, object> altNameRow in altName)
                        altNames += (string)altNameRow["alt_name"] + ",";
                    if (altNames.EndsWith(","))
                        altNames = altNames.Substring(0, altNames.Length - 1);
                    TVShow show = new TVShow((long)row["id"],
                        (string)row["tvdb_id"], (string)row["name"],
                        (long)row["update_time"],
                        ((long)row["use_default_format"]==1?true:false),
                        (string)row["custom_format"],
                        (string)row["folder_name"],
                        (string)row["banner"],
                        altNames);
                    lstTVShows.Items.Add(show);
                }
            }
        }

        /// <summary>
        /// Loads the settings for the selected show. Sets the controls to match the show's object
        /// </summary>
        private void LoadShowSettings()
        {
            flShowControls.Enabled = true;
            grpShowCustomFormat.Enabled = true;
            TVShow selectedShow = (TVShow)lstTVShows.SelectedItem;
            chkUseDefaultFormat.Checked = selectedShow.UseDefaultFormat;
            txtShowCustomFormat.Text = selectedShow.CustomFormat;
            txtShowFolderName.Text = selectedShow.FolderName;
            txtAltNames.Text = selectedShow.AltNames;
            if (selectedShow.UseDefaultFormat)
            {
                txtShowExampleFileName.Text = Episode.ExampleEpiosde.FormatOutputPath();
            }
            else
            {
                txtShowExampleFileName.Text = 
                    Episode.ExampleEpiosde.FormatOutputPath(selectedShow.CustomFormat);
            }
        }

        /// <summary>
        /// Applies the settings for the selected show. Sets the show's object to match the controls.
        /// </summary>
        private void ApplyShowSettings()
        {
            TVShow selectedShow = (TVShow)lstTVShows.SelectedItem;
            selectedShow.UseDefaultFormat = chkUseDefaultFormat.Checked;
            selectedShow.CustomFormat = txtShowCustomFormat.Text;
            selectedShow.FolderName = txtShowFolderName.Text;
            selectedShow.AltNames = txtAltNames.Text;
            if (selectedShow.UseDefaultFormat)
            {
                txtShowExampleFileName.Text = Episode.ExampleEpiosde.FormatOutputPath();
            }
            else
            {
                txtShowExampleFileName.Text =
                    Episode.ExampleEpiosde.FormatOutputPath(selectedShow.CustomFormat);
            }
        }

        /// <summary>
        /// Saves the selected show's settings into the database
        /// </summary>
        private void SaveShowSettings()
        {
            TVShow selectedShow = (TVShow)lstTVShows.SelectedItem;
            string query = "Update Shows Set " +
                "use_default_format = "+(selectedShow.UseDefaultFormat?1:0)+
            ", custom_format = \""+selectedShow.CustomFormat.Replace("\"","\"\"") +
            "\", folder_name = \"" + selectedShow.FolderName.Replace("\"", "\"\"") + 
            "\" Where id = "+selectedShow.DatabaseId+";";
            _database.ExecuteQuery(query);
            //Process alternate names - delete existing ones and add everything again
            _database.ExecuteQuery("Delete From AltNames Where show_id = " + selectedShow.DatabaseId + ";");
            string[] altNames = selectedShow.AltNames.Split(',');
            if (altNames.Length > 0)
            {
                string altNameQuery = "";
                foreach (string altName in altNames)
                {
                    if (altName.Length > 0)
                    {
                        altNameQuery += "Insert Into AltNames (show_id, alt_name) Values ("
                            + selectedShow.DatabaseId + ", \""
                            + altName.Replace("\"", "\"\"") + "\");";
                    }
                }
                _database.ExecuteQuery(altNameQuery);
            }
        }

        /// <summary>
        /// Recursive function to determine the number of files in 
        /// a directory and it's subdirectories
        /// </summary>
        /// <param name="dir">The directory to check</param>
        /// <returns>The number of files</returns>
        private int GetMaxFiles(DirectoryInfo dir)
        {
            int max = 0;
            FileInfo[] files = dir.GetFiles();
            max += files.Length;
            //Only check subdirectories if the option is enabled
            if (_settings.RecurseSubDir)
            {
                foreach (DirectoryInfo subdir in dir.GetDirectories())
                {
                    max += GetMaxFiles(subdir);
                }
            }
            return max;
        }

        /// <summary>
        /// Convert a unix timestamp into a datetime object
        /// </summary>
        /// <param name="timestamp">The timestamp to convert</param>
        /// <returns>The DateTime object</returns>
        public static DateTime ConvertFromUnixTimestamp(double timestamp)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return origin.AddSeconds(timestamp);
        }

        /// <summary>
        /// Converts a DateTime object into a unix timestamp
        /// </summary>
        /// <param name="date">The DateTime object to convert</param>
        /// <returns>The timestamp</returns>
        public static long ConvertToUnixTimestamp(DateTime date)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan diff = date - origin;
            return (long)Math.Floor(diff.TotalSeconds);
        }

        #region Event Handlers
        private void frmMain_Load(object sender, EventArgs e)
        {
            Init();
        }

        //Handles the tab changing, prompts the user if they are leaving the 
        //setting tab without saving/applying changes
        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Name == "tpgSettings")
            {
                _onSettings = true;
            }
            else
            {
                if (_onSettings)
                {
                    //Have just left settings - check for changes
                    if (txtInputFolder.Text != _settings.InputDir ||
                        txtOutputFolder.Text != _settings.OutputDir ||
                        chkRecurse.Checked != _settings.RecurseSubDir ||
                        txtNameFormat.Text != _settings.FileNameFormat)
                    {
                        if (MessageBox.Show("You have changed settings but not applied them.\n" +
                            "Do you want to apply them now?", "Changed settings",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        {
                            ApplySettings();
                        }
                    }
                }
                _onSettings = false;
            }
        }

        #region Input Folder Handlers
        //Handles pressing the refresh button
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (_settings.InputDir == "" || !Directory.Exists(_settings.InputDir))
            {
                MessageBox.Show("Check your input directory and try again");
                return;
            }
            string inputDir;
            if (cboFolderFilter.SelectedIndex == 0)
            {
                inputDir = _settings.InputDir;
            }
            else
            {
                inputDir = _settings.InputDir + "\\" + cboFolderFilter.SelectedItem;
            }
            int numFiles = GetMaxFiles(new DirectoryInfo(inputDir));
            if (numFiles == 0)
            {
                lstInputFolder.Items.Clear();
                return;
            }
            frmProgress progress = new frmProgress(numFiles);
            MethodInvoker increment = new MethodInvoker(progress.Increment);
            MethodInvoker inc = new MethodInvoker(delegate() { progress.Invoke(increment); });
            new Thread(new ThreadStart(delegate()
            {
                _fileHandler.RefreshEpisodes(inc, inputDir);
                progress.Close();
            })).Start();
            progress.ShowDialog();
            _files = _fileHandler.Files;
            //Show in list view
            string inputFolder = Properties.Settings.Default.InputDir;
            lstInputFolder.Items.Clear();
            foreach (KeyValuePair<string, Episode> episode in _files)
            {
                string path = episode.Key;
                string show = episode.Value.Show.Name;
                string seasonNum = episode.Value.SeasonNum.ToString();
                string episodeNum = episode.Value.EpisodeNum.ToString();
                string name = episode.Value.EpisodeName;
                string outputPath = episode.Value.FormatOutputPath();
                lstInputFolder.Items.Add(new ListViewItem(
                    new string[] { path, show, seasonNum, episodeNum, name, outputPath }));
            }
        }

        //Handles the deselect all button
        private void btnDeselectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstInputFolder.Items)
            {
                item.Checked = false;
            }
        }

        //Handles the select all button
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstInputFolder.Items)
            {
                item.Checked = true;
            }
        }

        //Handles the rename and move button
        private void btnRenameMove_Click(object sender, EventArgs e)
        {
            //Check everything is in order
            if (_settings.InputDir == "" || !Directory.Exists(_settings.InputDir))
            {
                MessageBox.Show("Check your input directory and try again");
                return;
            }
            if (_settings.OutputDir == "" || !Directory.Exists(_settings.OutputDir))
            {
                MessageBox.Show("Check your output directory and try again");
                return;
            }
            if (lstInputFolder.CheckedItems.Count == 0)
            {
                MessageBox.Show("No items checked");
                return;
            }
            frmProgress progress = new frmProgress(lstInputFolder.CheckedItems.Count);
            MethodInvoker increment = new MethodInvoker(progress.Increment);
            MethodInvoker inc = new MethodInvoker(delegate() { progress.Invoke(increment); });
            //Create an array of episode to process
            Episode[] episodes = new Episode[lstInputFolder.CheckedItems.Count];
            int i = 0;
            foreach (ListViewItem item in lstInputFolder.CheckedItems)
            {
                episodes[i] = _files[lstInputFolder.CheckedItems[i].Text];
                i++;
            }
            new Thread(new ThreadStart(delegate()
            {
                _fileHandler.RenameMove(inc, episodes);
                progress.Close();
            })).Start();
            progress.ShowDialog();
            UpdateFolderFilter();
            //Refresh the directory again. - Todo: Only change things that have changed, don't force a full refresh.
            btnRefresh_Click(sender, e);
        }

        //Handles the rename only button
        private void btnRenameOnly_Click(object sender, EventArgs e)
        {
            if (lstInputFolder.CheckedItems.Count == 0)
            {
                MessageBox.Show("No items checked");
                return;
            }
            frmProgress progress = new frmProgress(lstInputFolder.CheckedItems.Count);
            MethodInvoker increment = new MethodInvoker(progress.Increment);
            MethodInvoker inc = new MethodInvoker(delegate() { progress.Invoke(increment); });
            Episode[] episodes = new Episode[lstInputFolder.CheckedItems.Count];
            int i = 0;
            foreach (ListViewItem item in lstInputFolder.CheckedItems)
            {
                episodes[i] = _files[lstInputFolder.CheckedItems[i].Text];
                i++;
            }
            new Thread(new ThreadStart(delegate()
            {
                _fileHandler.RenameOnly(inc, episodes);
                progress.Close();
            })).Start();
            progress.ShowDialog();
            btnRefresh_Click(sender, e);
        }

        //Handles the set show button
        private void btnSetShow_Click(object sender, EventArgs e)
        {
            if (lstInputFolder.CheckedItems.Count == 0)
            {
                MessageBox.Show("No items checked");
                return;
            }
            frmSelectShow selectShow = new frmSelectShow(lstTVShows.Items);
            if (selectShow.ShowDialog() == DialogResult.OK)
            {
                foreach (ListViewItem item in lstInputFolder.CheckedItems)
                {
                    Episode ep = _files[item.Text];
                    ep.Show = selectShow.SelectedShow;
                    string path = item.Text;
                    string show = ep.Show.Name;
                    string seasonNum = ep.SeasonNum.ToString();
                    string episodeNum = ep.EpisodeNum.ToString();
                    string name = ep.EpisodeName;
                    string outputPath = ep.FormatOutputPath();
                    item.SubItems[1].Text = show;
                    item.SubItems[2].Text = seasonNum;
                    item.SubItems[3].Text = episodeNum;
                    item.SubItems[4].Text = name;
                    item.SubItems[5].Text = outputPath;
                }
            }
        }
        #endregion

        #region TV Show Handlers
        //Handles the search shows button
        private void btnSearchShows_Click(object sender, EventArgs e)
        {
            SearchNewShows();
        }

        //Handles the index of the tv show changing
        private void lstTVShows_SelectedIndexChanged(object sender, EventArgs e)
        {
            TVShow selected = (TVShow)lstTVShows.SelectedItem;
            //Set the controls to match the selected show
            if (selected != null)
            {
                lblTvdbId.Text = "TVDB ID: " + selected.TvdbId;
                DateTime update = ConvertFromUnixTimestamp(selected.UpdateTime);
                lblLastUpdate.Text = "Last Update: " + update.ToString();
                lblSelectedShow.Text = selected.Name;
                string image = Environment
                .GetFolderPath(Environment.SpecialFolder.LocalApplicationData) +
                "\\TVSorter2\\" + selected.Banner.Replace('/', '\\');
                if (File.Exists(image))
                {
                    picShowPic.Image = Image.FromFile(image);
                }
                else
                {
                    picShowPic.Image = null;
                }
                LoadShowSettings();
            }
            else //Reset to blank
            {
                lblSelectedShow.Text = "No Show Selected";
                lblLastUpdate.Text = "Last Update: ";
                lblTvdbId.Text = "TVDB ID: ";
                picShowPic.Image = null;
                flShowControls.Enabled = false;
                grpShowCustomFormat.Enabled = false;
                chkUseDefaultFormat.Checked = false;
                txtShowCustomFormat.Text = "";
                txtShowFolderName.Text = "";
                txtShowExampleFileName.Text = "";
                txtAltNames.Text = "";
            }
        }

        //Handles the update selected button
        private void btnUpdateSelected_Click(object sender, EventArgs e)
        {
            frmProgress progress = new frmProgress(1);
            MethodInvoker inc = new MethodInvoker(progress.Increment);
            TVShow selectedItem = (TVShow)lstTVShows.SelectedItem;
            if (selectedItem == null)
                return;
            new Thread(new ThreadStart(delegate()
            {
                try
                {
                    TVDB.UpdateShow(selectedItem, false);
                }
                catch (Exception ex)
                {
                    progress.Abort();
                    Log.Add("Update failed: " + ex.Message);
                    return;
                }
                progress.Invoke(inc);
                progress.Close();
            })).Start();
            if (progress.ShowDialog() == DialogResult.Abort)
            {
                MessageBox.Show("An error occured. Unable to update, check the log for details.");
            }
            this.lstTVShows_SelectedIndexChanged(this, null);
        }

        //Handles the update all button
        private void btnUpdateAll_Click(object sender, EventArgs e)
        {
            if (lstTVShows.Items.Count == 0)
                return;
            frmProgress progress = new frmProgress(lstTVShows.Items.Count);
            MethodInvoker inc = new MethodInvoker(progress.Increment);
            TVShow[] shows = new TVShow[lstTVShows.Items.Count];
            for (int i = 0; i < shows.Length; i++)
                shows[i] = (TVShow)lstTVShows.Items[i];
            new Thread(new ThreadStart(delegate()
            {
                foreach (TVShow show in shows)
                {
                    try
                    {
                        TVDB.UpdateShow(show, false);
                    }
                    catch (Exception ex)
                    {
                        progress.Abort();
                        Log.Add("Update failed: " + ex.Message);
                        return;
                    }
                    progress.Invoke(inc);
                }
                progress.Close();
            })).Start();
            if (progress.ShowDialog() == DialogResult.Abort)
            {
                MessageBox.Show("An error occured. Unable to update, check the log for details.");
            }
            this.lstTVShows_SelectedIndexChanged(this, null);
        }

        //Handles the force update all button
        private void btnForceUpdateAll_Click(object sender, EventArgs e)
        {
            if (lstTVShows.Items.Count == 0)
                return;
            if (MessageBox.Show("This will refresh the entire library and could take a long time" +
                "\nDo you want to continue?", "Force Update",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }
            frmProgress progress = new frmProgress(lstTVShows.Items.Count);
            MethodInvoker inc = new MethodInvoker(progress.Increment);
            TVShow[] shows = new TVShow[lstTVShows.Items.Count];
            for (int i = 0; i < shows.Length; i++)
                shows[i] = (TVShow)lstTVShows.Items[i];
            new Thread(new ThreadStart(delegate()
            {
                foreach (TVShow show in shows)
                {
                    try
                    {
                        TVDB.UpdateShow(show, true);
                    }
                    catch (Exception ex)
                    {
                        progress.Abort();
                        Log.Add("Update failed: " + ex.Message);
                        return;
                    }
                    progress.Invoke(inc);
                }
                progress.Close();
            })).Start();
            if (progress.ShowDialog() == DialogResult.Abort)
            {
                MessageBox.Show("An error occured. Unable to update, check the log for details.");
            }
            this.lstTVShows_SelectedIndexChanged(this, null);
        }

        //Handles the apply settings button for TV shows.
        private void btnApplyShowSetttings_Click(object sender, EventArgs e)
        {
            ApplyShowSettings();
        }

        //Handles the save settings button for TV shows
        private void btnSaveShowSettings_Click(object sender, EventArgs e)
        {
            ApplyShowSettings();
            SaveShowSettings();
        }

        //Handles the reset settings button for TV shows
        private void btnResetShowSettings_Click(object sender, EventArgs e)
        {
            LoadShowSettings();
        }

        //Handles the checkbox for use default format changing
        private void chkUseDefaultFormat_CheckedChanged(object sender, EventArgs e)
        {
            txtShowCustomFormat.Enabled = !chkUseDefaultFormat.Checked;
        }

        //Handles the add show button
        private void btnAddShow_Click(object sender, EventArgs e)
        {
            frmAddShow addShow = new frmAddShow();
            if (addShow.ShowDialog() == DialogResult.OK)
            {
                TVShow show = addShow.newShow;
                _database.ExecuteQuery("Insert Into Shows " +
                       "(tvdb_id, name, update_time, use_default_format, custom_format, folder_name, banner) " +
                       "Values (\"" + show.TvdbId + "\", \"" + show.Name.Replace("\"","\"\"") + "\", " +
                       show.UpdateTime + ", "
                        + (show.UseDefaultFormat ? 1 : 0) + ", \"" +
                         show.CustomFormat + "\", \"" + show.FolderName + "\", \""
                         + show.Banner + "\");");
                //Create a new TV show so that it gets the newly added show from the database
                //and therefore changes can be made to it - Issue ID 1.
                lstTVShows.Items.Add(new TVShow(show.Name));

            }
        }

        //Handles the remove show button
        private void btnRemoveShow_Click(object sender, EventArgs e)
        {
            TVShow show = (TVShow)lstTVShows.SelectedItem;
            if (show == null)
                return;
            if (MessageBox.Show("Are you sure you want to remove " + show.Name + "?", "Remove Show",
                MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                lstTVShows.Items.Remove(show);
                string showRemove = "Delete From Shows Where id = " + show.DatabaseId + ";";
                string episodeRemove = "Delete From Episodes Where show_id = " + show.DatabaseId + ";";
                _database.ExecuteQuery(showRemove);
                _database.ExecuteQuery(episodeRemove);
            }
        }
        #endregion

        #region Settings Handlers
        //Handles the browse button for the input folder
        private void btnBrowseInputFolder_Click(object sender, EventArgs e)
        {
            if (dlgFolderSelect.ShowDialog() == DialogResult.OK)
            {
                txtInputFolder.Text = dlgFolderSelect.SelectedPath;
                if (!txtInputFolder.Text.EndsWith("\\"))
                {
                    txtInputFolder.Text += "\\";
                }
            }
        }

        //Handles the browse button for the output folder
        private void btnBrowseOutputFolder_Click(object sender, EventArgs e)
        {
            if (dlgFolderSelect.ShowDialog() == DialogResult.OK)
            {
                txtOutputFolder.Text = dlgFolderSelect.SelectedPath;
                if (!txtOutputFolder.Text.EndsWith("\\"))
                {
                    txtOutputFolder.Text += "\\";
                }
            }
        }

        //Handles the save settings button
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        //Handles the reset settings button
        private void btnReset_Click(object sender, EventArgs e)
        {
            LoadSettings();
        }

        //Handles the apply settings button
        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplySettings();
        }

        //Handles the revert settings button
        private void btnRevert_Click(object sender, EventArgs e)
        {
            _settings.Reload();
            LoadSettings();
        }
        #endregion
        #endregion
    }
}
