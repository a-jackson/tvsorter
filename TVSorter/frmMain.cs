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
            Log.Init(lstLog);
            _database = new Database();
            _fileHandler = new FileHandler();
            LoadSettings();
            LoadRegexpSettings();
            UpdateTvShowList();
            grpShowCustomFormat.Enabled = false;
            grpShowCustomNames.Enabled = false;
            grpShowUpdateOptions.Enabled = false;
            tsShowControls.Enabled = false;
            this.Text = "TV Sorter " + Program.VersionNumber;
            Log.Add("Program started");
        }

        /// <summary>
        /// Set the controls to match the settings
        /// </summary>
        private void LoadSettings()
        {
            Settings.LoadSettings();
            this.txtInputFolder.Text = Settings.InputDir;
            this.txtOutputFolder.Text = Settings.OutputDir;
            this.chkRecurse.Checked = Settings.RecurseSubDir;
            this.txtNameFormat.Text = Settings.FileNameFormat;
            this.chkDeleteEmpty.Checked = Settings.DeleteEmpty;
            UpdateExampleOutput();
            UpdateFolderFilter();
        }

        /// <summary>
        /// Sets the settings to match the controls
        /// </summary>
        private void ApplySettings()
        {
            Settings.InputDir = this.txtInputFolder.Text;
            Settings.OutputDir = this.txtOutputFolder.Text;
            Settings.RecurseSubDir = this.chkRecurse.Checked;
            Settings.FileNameFormat = this.txtNameFormat.Text;
            Settings.DeleteEmpty = this.chkDeleteEmpty.Checked;
            UpdateExampleOutput();
            UpdateFolderFilter();
        }

        /// <summary>
        /// Saves the settings
        /// </summary>
        private void SaveSettings()
        {
            ApplySettings();
            Settings.SaveSettings();
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
            if (Settings.InputDir.Length > 0)
            {
                DirectoryInfo dirInfo = new DirectoryInfo(Settings.InputDir);
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
                new frmProgress(Directory.GetDirectories(Settings.OutputDir).Length);
            TVDB.Instance.SetEvents(progress.Increment, progress.Abort, progress.Close);
            SortedDictionary<string, List<TVShow>> shows = TVDB.Instance.SearchNewShows();
            if (progress.ShowDialog() == DialogResult.Abort)
                return;
            TVDB.Instance.ClearEvents(progress.Increment, progress.Abort, progress.Close);
            //Process the results
            foreach (KeyValuePair<string, List<TVShow>> kvPair in shows)
            {
                //If there is just one result, use that
                TVShow show = null;
                if (kvPair.Value.Count == 1)
                {
                    show = kvPair.Value[0];
                }
                else //Else show the possibilities to the user
                {
                    frmShowResults results = new frmShowResults(kvPair.Value, kvPair.Key);
                    results.ShowDialog();
                    TVShow selected = results.GetSelected();
                    if (selected == null)
                    {
                        continue;
                    }
                    show = selected;
                }
                show.FolderName = kvPair.Key;
                show.SaveToDatabase();
            }
            UpdateTvShowList();
        }

        /// <summary>
        /// Updates the list box containing the TV shows.
        /// </summary>
        private void UpdateTvShowList()
        {
            lstTVShows.Items.Clear();
            List<TVShow> shows = TVShow.GetAllShows();
            lstTVShows.Items.AddRange(shows.ToArray());
        }

        /// <summary>
        /// Loads the settings for the selected show. Sets the controls to match the show's object
        /// </summary>
        private void LoadShowSettings()
        {
            tsShowControls.Enabled = true;
            grpShowCustomFormat.Enabled = true;
            grpShowUpdateOptions.Enabled = true;
            grpShowCustomNames.Enabled = true;
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
            if (selectedShow.Locked)
            {
                btnLockShow.Text = "Show Locked";
                btnLockShow.BackColor = Color.Red;
            }
            else
            {
                btnLockShow.Text = "Show Unlocked";
                btnLockShow.BackColor = Color.Green;
            }
            chkDvdOrder.Checked = selectedShow.UseDvdOrder;
        }

        /// <summary>
        /// Saves the selected show's settings into the database
        /// </summary>
        private void SaveShowSettings()
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
            selectedShow.Locked = (btnLockShow.BackColor.Equals(Color.Red));
            selectedShow.UseDvdOrder = chkDvdOrder.Checked;
            selectedShow.SaveToDatabase();
            lstTVShows.Refresh();
        }

        private void UpdateShows(bool force, params TVShow[] shows)
        {
            try
            {
                frmProgress progress = new frmProgress(shows.Length);
                TVDB.Instance.SetEvents(progress.Increment, progress.Abort, progress.Close);
                TVDB.Instance.UpdateShows(force, shows);
                if (progress.ShowDialog() == DialogResult.Abort)
                {
                    MessageBox.Show("An error occured. Unable to update, check the log for details.");
                }
                TVDB.Instance.ClearEvents(progress.Increment, progress.Abort, progress.Close);
                this.lstTVShows_SelectedIndexChanged(this, null);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
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
            if (Settings.RecurseSubDir)
            {
                foreach (DirectoryInfo subdir in dir.GetDirectories())
                {
                    max += GetMaxFiles(subdir);
                }
            }
            return max;
        }

        /// <summary>
        /// Sorts the selected TVShows using the defined sort action
        /// </summary>
        /// <param name="action">The sort action to use</param>
        private void SortEpisodes(SortAction action)
        {
            //Check everything is in order
            if (Settings.InputDir == "" || !Directory.Exists(Settings.InputDir))
            {
                MessageBox.Show("Check your input directory and try again");
                return;
            }
            //Don't need the output folder if only renaming
            if (action != SortAction.Rename)
            {
                if (Settings.OutputDir == "" || !Directory.Exists(Settings.OutputDir))
                {
                    MessageBox.Show("Check your output directory and try again");
                    return;
                }
            }
            if (lstInputFolder.CheckedItems.Count == 0)
            {
                MessageBox.Show("No items checked");
                return;
            }
            frmProgress progress = new frmProgress(lstInputFolder.CheckedItems.Count);
            //Create an array of episode to process
            Episode[] episodes = new Episode[lstInputFolder.CheckedItems.Count];
            for (int i = 0; i < episodes.Length; i++)
            {
                episodes[i] = _files[lstInputFolder.CheckedItems[i].Text];
            }

            _fileHandler.SetEvents(progress.Increment, progress.Abort, progress.Close);
            _fileHandler.SortEpisodes(episodes, action);
            progress.ShowDialog();
            _fileHandler.ClearEvents(progress.Increment, progress.Abort, progress.Close);

            UpdateFolderFilter();
            //Refresh the directory again. - Todo: Only change things that have changed, don't force a full refresh.
            btnRefresh_Click(this, null);
        }

        /// <summary>
        /// Sets all of the shows to either locked or unlocked
        /// </summary>
        /// <param name="shouldLock">If they should be locked or not</param>
        private void SetShowLocks(bool shouldLock)
        {
            foreach (object item in lstTVShows.Items)
            {
                TVShow show = (TVShow)item;
                show.Locked = shouldLock;
                show.SaveToDatabase();
            }
            lstTVShows.Refresh();
        }

        private void LoadRegexpSettings()
        {
            Settings.LoadFileRegexp();
            lstRegexp.Items.Clear();
            foreach (string regexp in Settings.FileRegex)
            {
                lstRegexp.Items.Add(regexp);
            }
        }

        private void ApplyRegexpSettings()
        {
            Settings.FileRegex.Clear();
            foreach (string regexp in lstRegexp.Items)
            {
                Settings.FileRegex.Add(regexp);
            }
        }

        private void SaveRegexpSettings()
        {
            ApplyRegexpSettings();
            Settings.SaveFileRegexp();
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
                    if (txtInputFolder.Text != Settings.InputDir ||
                        txtOutputFolder.Text != Settings.OutputDir ||
                        chkRecurse.Checked != Settings.RecurseSubDir ||
                        txtNameFormat.Text != Settings.FileNameFormat)
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
            if (Settings.InputDir == "" || !Directory.Exists(Settings.InputDir))
            {
                MessageBox.Show("Check your input directory and try again");
                return;
            }
            string inputDir;
            if (cboFolderFilter.SelectedIndex == 0)
            {
                inputDir = Settings.InputDir;
            }
            else
            {
                inputDir = Settings.InputDir + Path.DirectorySeparatorChar + cboFolderFilter.SelectedItem;
            }
            int numFiles = GetMaxFiles(new DirectoryInfo(inputDir));
            if (numFiles == 0)
            {
                lstInputFolder.Items.Clear();
                return;
            }
            frmProgress progress = new frmProgress(numFiles);
            _fileHandler.SetEvents(progress.Increment, progress.Abort, progress.Close);
            new Thread(new ThreadStart(delegate()
            {
                _fileHandler.RefreshEpisodes(inputDir);
            })).Start();
            progress.ShowDialog();
            _fileHandler.ClearEvents(progress.Increment, progress.Abort, progress.Close);
            _files = _fileHandler.Files;
            //Show in list view
            string inputFolder = Settings.InputDir;
            lstInputFolder.Items.Clear();
            foreach (KeyValuePair<string, Episode> episode in _files)
            {
                string path = episode.Key;
                string show = episode.Value.Show.Name;
                string seasonNum = episode.Value.SeasonNum.ToString();
                string episodeNum = episode.Value.EpisodeNum.ToString();
                string name = episode.Value.EpisodeName;
                string outputPath = episode.Value.FormatOutputPath();
                ListViewItem item = new ListViewItem(
                    new string[] { path, show, seasonNum, episodeNum, name, outputPath });
                if (!episode.Value.IsComplete)
                    item.BackColor = Color.Red;
                lstInputFolder.Items.Add(item);
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
            SortEpisodes(SortAction.Move);
        }

        private void btnRenameCopy_Click(object sender, EventArgs e)
        {
            SortEpisodes(SortAction.Copy);
        }

        //Handles the rename only button
        private void btnRenameOnly_Click(object sender, EventArgs e)
        {
            SortEpisodes(SortAction.Rename);
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
                    if (!ep.IsComplete)
                    {
                        item.BackColor = Color.Red;
                    }
                    else
                    {
                        item.BackColor = Color.Transparent;
                    }
                }
            }
        }

        private void btnSort_ButtonClick(object sender, EventArgs e)
        {
            btnSort.ShowDropDown();
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
                DateTime update = TVShow.ConvertFromUnixTimestamp(selected.UpdateTime);
                lblLastUpdate.Text = "Last Update: " + update.ToString();
                lblSelectedShow.Text = selected.Name;
                string image = "Data" + Path.DirectorySeparatorChar + selected.TvdbId;
                if (File.Exists(image))
                {
                    try
                    {
                        picShowPic.Image = Image.FromFile(image);
                    }
                    catch
                    {
                        picShowPic.Image = null;
                        //Delete it so a new one can be downloaded next update
                        File.Delete(image);
                    }
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
                tsShowControls.Enabled = false;
                grpShowCustomFormat.Enabled = false;
                grpShowCustomNames.Enabled = false;
                grpShowUpdateOptions.Enabled = false;
                chkUseDefaultFormat.Checked = false;
                txtShowCustomFormat.Text = "";
                txtShowFolderName.Text = "";
                txtShowExampleFileName.Text = "";
                txtAltNames.Text = "";
                btnLockShow.BackColor = Color.Transparent;
                btnLockShow.Text = "Show Locked";
            }
        }

        //Handles the update selected button
        private void btnUpdateSelected_Click(object sender, EventArgs e)
        {
            if (lstTVShows.SelectedIndex == -1)
                return;
            UpdateShows(false, (TVShow)lstTVShows.SelectedItem);
        }

        //Handles the update all button
        private void btnUpdateAll_Click(object sender, EventArgs e)
        {
            if (lstTVShows.Items.Count == 0)
                return;
            TVShow[] shows = new TVShow[lstTVShows.Items.Count];
            for (int i = 0; i < shows.Length; i++)
                shows[i] = (TVShow)lstTVShows.Items[i];
            UpdateShows(false, shows);
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
            TVShow[] shows = new TVShow[lstTVShows.Items.Count];
            for (int i = 0; i < shows.Length; i++)
                shows[i] = (TVShow)lstTVShows.Items[i];
            UpdateShows(true, shows);
        }

        //Handles the save settings button for TV shows
        private void btnSaveShowSettings_Click(object sender, EventArgs e)
        {
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
                show.SaveToDatabase();
                //Create a new TV show so that it gets the newly added show from the database
                //and therefore changes can be made to it - Issue ID 1.
                show = TVShow.GetTVShow(show.Name);
                if (show != null)
                {
                    lstTVShows.Items.Add(show);
                }
                else
                {
                    Log.Add("Error adding show " + addShow.newShow.Name);
                }
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
                string altNameQuery = "Delete From AltNames Where show_id = " + show.DatabaseId + ";";
                _database.ExecuteQuery(showRemove);
                _database.ExecuteQuery(episodeRemove);
                _database.ExecuteQuery(altNameQuery);
            }
        }

        //Handles the force update selected show button
        private void btnForceUpdateSelected_Click(object sender, EventArgs e)
        {
            if (lstTVShows.SelectedIndex == -1)
                return;
            UpdateShows(true, (TVShow)lstTVShows.SelectedItem);
        }

        //Handles the lock show button
        private void btnLockShow_Click(object sender, EventArgs e)
        {
            TVShow show = (TVShow)lstTVShows.SelectedItem;
            if (show == null)
                return;
            if (btnLockShow.BackColor.Equals(Color.Green))
            {
                btnLockShow.Text = "Show Locked";
                btnLockShow.BackColor = Color.Red;
            }
            else
            {
                btnLockShow.Text = "Show Unlocked";
                btnLockShow.BackColor = Color.Green;
            }
        }

        //Draws the tv show list
        void lstTVShows_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= lstTVShows.Items.Count)
                return;
            TVShow show = (TVShow)lstTVShows.Items[e.Index];
            if (show == null)
                return;

            // Draw the background color depending on 
            // if the item is selected or not.
            Color textColour;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                // The item is selected.
                // We want a highlight color background. This should match the user's theme
                e.Graphics.FillRectangle(new SolidBrush(Color.FromKnownColor(KnownColor.Highlight)), e.Bounds);
                textColour = Color.FromKnownColor(KnownColor.HighlightText);
            }
            else
            {
                // The item is NOT selected.
                // We want a white background color.
                e.Graphics.FillRectangle(new SolidBrush(Color.FromKnownColor(KnownColor.Window)), e.Bounds);
                textColour = Color.FromKnownColor(KnownColor.WindowText);
            }
            if (show.Locked)
            {
                e.Graphics.DrawImage(Properties.Resources._lock,
                    new PointF(5, 2 + e.Bounds.Y + (e.Bounds.Height - 20) / 2));
            }

            SizeF stringSize = e.Graphics.MeasureString(show.Name, e.Font);
            //Draw the text, X is 25 if locked to allow for padlock else 5
            e.Graphics.DrawString(show.Name, e.Font, new SolidBrush(textColour),
                new PointF(show.Locked ? 25 : 5, e.Bounds.Y + (e.Bounds.Height - stringSize.Height) / 2));
        }

        //Handles the Lock all button click event
        private void btnLockAll_Click(object sender, EventArgs e)
        {
            SetShowLocks(true);
        }

        //Handles the Unlock all button click event
        private void btnUnlockAll_Click(object sender, EventArgs e)
        {
            SetShowLocks(false);
        }
        #endregion

        #region Settings Handlers

        #region General Settings
        //Handles the browse button for the input folder
        private void btnBrowseInputFolder_Click(object sender, EventArgs e)
        {
            if (dlgFolderSelect.ShowDialog() == DialogResult.OK)
            {
                txtInputFolder.Text = dlgFolderSelect.SelectedPath;
                if (!txtInputFolder.Text.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    txtInputFolder.Text += Path.DirectorySeparatorChar.ToString();
                }
            }
        }

        //Handles the browse button for the output folder
        private void btnBrowseOutputFolder_Click(object sender, EventArgs e)
        {
            if (dlgFolderSelect.ShowDialog() == DialogResult.OK)
            {
                txtOutputFolder.Text = dlgFolderSelect.SelectedPath;
                if (!txtOutputFolder.Text.EndsWith(Path.DirectorySeparatorChar.ToString()))
                {
                    txtOutputFolder.Text += Path.DirectorySeparatorChar.ToString();
                }
            }
        }

        //Handles the save settings button
        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }

        //Handles the apply settings button
        private void btnApply_Click(object sender, EventArgs e)
        {
            ApplySettings();
        }

        //Handles the revert settings button
        private void btnRevert_Click(object sender, EventArgs e)
        {
            LoadSettings();
        }
        #endregion

        #region Regular Expressions

        //Handles the Add regexp button
        private void btnAddRegexp_Click(object sender, EventArgs e)
        {
            if (txtRegexp.Text.Length > 0)
                lstRegexp.Items.Add(txtRegexp.Text);
        }

        //Handles the apply regexp button
        private void btnApplyRegexp_Click(object sender, EventArgs e)
        {
            ApplyRegexpSettings();
        }

        //Handles the save regexp button
        private void btnSaveRegexp_Click(object sender, EventArgs e)
        {
            SaveRegexpSettings();
        }

        //Handles the revert regexp button
        private void btnRevertRegexp_Click(object sender, EventArgs e)
        {
            LoadRegexpSettings();
        }

        //Handles the moveup regexp button
        private void btnMoveUpRegexp_Click(object sender, EventArgs e)
        {
            if (lstRegexp.SelectedIndex == -1)
            {
                return;
            }
            int index = lstRegexp.SelectedIndex;
            if (index == 0)
                return;
            string regexp = (string)lstRegexp.Items[index];
            lstRegexp.Items.RemoveAt(lstRegexp.SelectedIndex);
            lstRegexp.Items.Insert(index - 1, regexp);
            lstRegexp.SelectedIndex = index - 1;
        }

        //Handles the movedown regexp button
        private void btnMoveDownRegexp_Click(object sender, EventArgs e)
        {
            if (lstRegexp.SelectedIndex == -1)
            {
                return;
            }
            int index = lstRegexp.SelectedIndex;
            if (index == lstRegexp.Items.Count - 1)
                return;
            string regexp = (string)lstRegexp.Items[index];
            lstRegexp.Items.RemoveAt(lstRegexp.SelectedIndex);
            lstRegexp.Items.Insert(index + 1, regexp);
            lstRegexp.SelectedIndex = index + 1;
        }

        //Handles the remove regexp button
        private void btnRemoveRegexp_Click(object sender, EventArgs e)
        {
            if (lstRegexp.SelectedIndex == -1)
            {
                return;
            }
            lstRegexp.Items.RemoveAt(lstRegexp.SelectedIndex);
        }

        #endregion

        private void btnSearchMissingEpisodes_Click(object sender, EventArgs e)
        {
            if (Settings.OutputDir == "" || !Directory.Exists(Settings.InputDir))
            {
                MessageBox.Show("Check your output directory and try again");
                return;
            }
            string outputDir;
            if (cboFolderFilter.SelectedIndex == 0)
            {
                outputDir = Settings.OutputDir;
            }
            else
            {
                outputDir = Settings.OutputDir + Path.DirectorySeparatorChar + cboFolderFilter.SelectedItem;
            }
            tvMissingEps.Nodes.Clear();
            bool recurse = Settings.RecurseSubDir;
            Settings.RecurseSubDir = true;

            int numFiles = GetMaxFiles(new DirectoryInfo(outputDir));
            if (numFiles == 0)
                return;
   
            frmProgress progress = new frmProgress(numFiles);
            _fileHandler.SetEvents(progress.Increment, progress.Abort, progress.Close);
            new Thread(new ThreadStart(delegate()
            {
                _fileHandler.RefreshEpisodes(outputDir);
            })).Start();
            progress.ShowDialog();
            _fileHandler.ClearEvents(progress.Increment, progress.Abort, progress.Close);
            _files = _fileHandler.Files;

            frmProgress progress2 = new frmProgress(3);
            new Thread(new ThreadStart(delegate()
            {
                _database.ExecuteQuery("CREATE TABLE FILES(FILE_ID INTEGER PRIMARY KEY AUTOINCREMENT, EPISODE_ID INTEGER)");
                progress2.Increment();
                //Show in list view
                StringBuilder query = new StringBuilder();
                foreach (KeyValuePair<string, Episode> episode in _files)
                {
                    query.Append("INSERT INTO FILES (EPISODE_ID) VALUES (" + episode.Value.ID + ");");
                }
                progress2.Increment();
                _database.ExecuteQuery("Begin;" + query.ToString() + "Commit;");
                progress2.Increment();
            })).Start();
            progress2.ShowDialog();
            
            long time = TVShow.ConvertToUnixTimestamp(DateTime.Now);
            List<Dictionary<string, object>> missing = _database.ExecuteResults
                ("SELECT Shows.name, Episodes.season_num, Episodes.episode_num, Episodes.episode_name " +
                    "FROM Shows INNER JOIN Episodes On (Shows.id = Episodes.show_id) WHERE Episodes.id NOT " +
                    "IN (SELECT EPISODE_ID FROM FILES) AND Episodes.first_air < " + time +
                    " AND Episodes.first_air != 0" +
                    (chkSkipSeason0.Checked ? " AND Episodes.season_num != 0" : "") +
                    " ORDER BY Shows.name, Episodes.season_num, Episodes.episode_num;");
            TreeNode root = new TreeNode();
            string lastShow= "";
            long lastSeason = -1;
            foreach (Dictionary<string, object> episode in missing)
            {
                string showName = (string)episode["name"];
                long seasonNum = (long)episode["season_num"];
                long episodeNum = (long)episode["episode_num"];
                string episodeName = (string)episode["episode_name"];

                if (showName != lastShow)
                {
                    root.Nodes.Add(showName, showName);
                    lastShow = showName;
                    lastSeason = -1;
                }
                if (lastSeason != seasonNum)
                {
                    root.Nodes[showName].Nodes.Add(seasonNum.ToString(), "Season " + seasonNum);
                    lastSeason = seasonNum;
                }
                root.Nodes[showName].Nodes[seasonNum.ToString()].Nodes.Add(episodeNum.ToString(), episodeNum + " - " + episodeName);
            }
            foreach (TreeNode node in root.Nodes)
            {
                tvMissingEps.Nodes.Add(node);
            }
            _database.ExecuteQuery("DROP TABLE FILES");
            Settings.RecurseSubDir = recurse;
        }
        #endregion
        #endregion
    }
}
