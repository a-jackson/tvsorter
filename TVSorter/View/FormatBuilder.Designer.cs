namespace TVSorter.View
{
    partial class FormatBuilder
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }


        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TableLayoutPanel tableMain;
            System.Windows.Forms.FlowLayoutPanel flowButtons;
            System.Windows.Forms.Label formatLabel;
            System.Windows.Forms.Label exampleLabel;
            System.Windows.Forms.Panel panel;
            System.Windows.Forms.GroupBox groupNumbers;
            System.Windows.Forms.GroupBox otherGroup;
            System.Windows.Forms.GroupBox namesGroup;
            System.Windows.Forms.GroupBox dateGroup;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormatBuilder));
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.textExample = new System.Windows.Forms.TextBox();
            this.textFormat = new System.Windows.Forms.TextBox();
            this.digitalSelector = new System.Windows.Forms.NumericUpDown();
            this.episodeNumberButton = new System.Windows.Forms.Button();
            this.seasonNumberButton = new System.Windows.Forms.Button();
            this.directoryButton = new System.Windows.Forms.Button();
            this.fileExtensionButton = new System.Windows.Forms.Button();
            this.folderNameButton = new System.Windows.Forms.Button();
            this.wordSeparator = new System.Windows.Forms.ComboBox();
            this.episodeNameButton = new System.Windows.Forms.Button();
            this.showNameButton = new System.Windows.Forms.Button();
            this.dateFormat = new System.Windows.Forms.TextBox();
            this.dateExample = new System.Windows.Forms.TextBox();
            this.dateButton = new System.Windows.Forms.Button();
            tableMain = new System.Windows.Forms.TableLayoutPanel();
            flowButtons = new System.Windows.Forms.FlowLayoutPanel();
            formatLabel = new System.Windows.Forms.Label();
            exampleLabel = new System.Windows.Forms.Label();
            panel = new System.Windows.Forms.Panel();
            groupNumbers = new System.Windows.Forms.GroupBox();
            otherGroup = new System.Windows.Forms.GroupBox();
            namesGroup = new System.Windows.Forms.GroupBox();
            dateGroup = new System.Windows.Forms.GroupBox();
            tableMain.SuspendLayout();
            flowButtons.SuspendLayout();
            panel.SuspendLayout();
            groupNumbers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.digitalSelector)).BeginInit();
            otherGroup.SuspendLayout();
            namesGroup.SuspendLayout();
            dateGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableMain
            // 
            tableMain.ColumnCount = 2;
            tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 104F));
            tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            tableMain.Controls.Add(flowButtons, 0, 3);
            tableMain.Controls.Add(this.textExample, 1, 2);
            tableMain.Controls.Add(this.textFormat, 1, 1);
            tableMain.Controls.Add(formatLabel, 0, 1);
            tableMain.Controls.Add(exampleLabel, 0, 2);
            tableMain.Controls.Add(panel, 0, 0);
            tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableMain.Location = new System.Drawing.Point(0, 0);
            tableMain.Name = "tableMain";
            tableMain.RowCount = 4;
            tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableMain.Size = new System.Drawing.Size(371, 221);
            tableMain.TabIndex = 0;
            // 
            // flowButtons
            // 
            tableMain.SetColumnSpan(flowButtons, 2);
            flowButtons.Controls.Add(this.okButton);
            flowButtons.Controls.Add(this.cancelButton);
            flowButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flowButtons.Location = new System.Drawing.Point(0, 191);
            flowButtons.Margin = new System.Windows.Forms.Padding(0);
            flowButtons.Name = "flowButtons";
            flowButtons.Size = new System.Drawing.Size(371, 30);
            flowButtons.TabIndex = 0;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(293, 3);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 0;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(212, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // textExample
            // 
            this.textExample.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textExample.Location = new System.Drawing.Point(107, 169);
            this.textExample.Name = "textExample";
            this.textExample.ReadOnly = true;
            this.textExample.Size = new System.Drawing.Size(261, 20);
            this.textExample.TabIndex = 1;
            // 
            // textFormat
            // 
            this.textFormat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textFormat.Location = new System.Drawing.Point(107, 144);
            this.textFormat.Name = "textFormat";
            this.textFormat.Size = new System.Drawing.Size(261, 20);
            this.textFormat.TabIndex = 2;
            this.textFormat.TextChanged += new System.EventHandler(this.TextFormatTextChanged);
            // 
            // formatLabel
            // 
            formatLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            formatLabel.AutoSize = true;
            formatLabel.Location = new System.Drawing.Point(29, 147);
            formatLabel.Name = "formatLabel";
            formatLabel.Size = new System.Drawing.Size(72, 13);
            formatLabel.TabIndex = 3;
            formatLabel.Text = "Format String:";
            // 
            // exampleLabel
            // 
            exampleLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            exampleLabel.AutoSize = true;
            exampleLabel.Location = new System.Drawing.Point(16, 172);
            exampleLabel.Name = "exampleLabel";
            exampleLabel.Size = new System.Drawing.Size(85, 13);
            exampleLabel.TabIndex = 4;
            exampleLabel.Text = "Example Output:";
            // 
            // panel
            // 
            tableMain.SetColumnSpan(panel, 2);
            panel.Controls.Add(groupNumbers);
            panel.Controls.Add(otherGroup);
            panel.Controls.Add(namesGroup);
            panel.Controls.Add(dateGroup);
            panel.Dock = System.Windows.Forms.DockStyle.Fill;
            panel.Location = new System.Drawing.Point(3, 3);
            panel.Name = "panel";
            panel.Size = new System.Drawing.Size(365, 135);
            panel.TabIndex = 10;
            // 
            // groupNumbers
            // 
            groupNumbers.AutoSize = true;
            groupNumbers.Controls.Add(this.digitalSelector);
            groupNumbers.Controls.Add(this.episodeNumberButton);
            groupNumbers.Controls.Add(this.seasonNumberButton);
            groupNumbers.Location = new System.Drawing.Point(3, 9);
            groupNumbers.Name = "groupNumbers";
            groupNumbers.Size = new System.Drawing.Size(82, 119);
            groupNumbers.TabIndex = 6;
            groupNumbers.TabStop = false;
            groupNumbers.Text = "Numbers";
            // 
            // digitalSelector
            // 
            this.digitalSelector.Location = new System.Drawing.Point(6, 77);
            this.digitalSelector.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.digitalSelector.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.digitalSelector.Name = "digitalSelector";
            this.digitalSelector.Size = new System.Drawing.Size(65, 20);
            this.digitalSelector.TabIndex = 2;
            this.digitalSelector.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // episodeNumberButton
            // 
            this.episodeNumberButton.Location = new System.Drawing.Point(6, 48);
            this.episodeNumberButton.Name = "episodeNumberButton";
            this.episodeNumberButton.Size = new System.Drawing.Size(65, 23);
            this.episodeNumberButton.TabIndex = 1;
            this.episodeNumberButton.Text = "Episode";
            this.episodeNumberButton.UseVisualStyleBackColor = true;
            this.episodeNumberButton.Click += new System.EventHandler(this.EpisodeNumberButtonClick);
            // 
            // seasonNumberButton
            // 
            this.seasonNumberButton.Location = new System.Drawing.Point(6, 19);
            this.seasonNumberButton.Name = "seasonNumberButton";
            this.seasonNumberButton.Size = new System.Drawing.Size(65, 23);
            this.seasonNumberButton.TabIndex = 0;
            this.seasonNumberButton.Text = "Season";
            this.seasonNumberButton.UseVisualStyleBackColor = true;
            this.seasonNumberButton.Click += new System.EventHandler(this.SeasonNumberButtonClick);
            // 
            // otherGroup
            // 
            otherGroup.AutoSize = true;
            otherGroup.Controls.Add(this.directoryButton);
            otherGroup.Controls.Add(this.fileExtensionButton);
            otherGroup.Controls.Add(this.folderNameButton);
            otherGroup.Location = new System.Drawing.Point(257, 9);
            otherGroup.Name = "otherGroup";
            otherGroup.Size = new System.Drawing.Size(96, 119);
            otherGroup.TabIndex = 9;
            otherGroup.TabStop = false;
            otherGroup.Text = "Other";
            // 
            // directoryButton
            // 
            this.directoryButton.Location = new System.Drawing.Point(6, 77);
            this.directoryButton.Name = "directoryButton";
            this.directoryButton.Size = new System.Drawing.Size(84, 23);
            this.directoryButton.TabIndex = 12;
            this.directoryButton.Text = "Directory";
            this.directoryButton.UseVisualStyleBackColor = true;
            this.directoryButton.Click += new System.EventHandler(this.DirectoryButtonClick);
            // 
            // fileExtensionButton
            // 
            this.fileExtensionButton.Location = new System.Drawing.Point(6, 48);
            this.fileExtensionButton.Name = "fileExtensionButton";
            this.fileExtensionButton.Size = new System.Drawing.Size(84, 23);
            this.fileExtensionButton.TabIndex = 11;
            this.fileExtensionButton.Text = "File Extension";
            this.fileExtensionButton.UseVisualStyleBackColor = true;
            this.fileExtensionButton.Click += new System.EventHandler(this.FileExtensionButtonClick);
            // 
            // folderNameButton
            // 
            this.folderNameButton.Location = new System.Drawing.Point(6, 19);
            this.folderNameButton.Name = "folderNameButton";
            this.folderNameButton.Size = new System.Drawing.Size(84, 23);
            this.folderNameButton.TabIndex = 10;
            this.folderNameButton.Text = "Folder Name";
            this.folderNameButton.UseVisualStyleBackColor = true;
            this.folderNameButton.Click += new System.EventHandler(this.FolderNameButtonClick);
            // 
            // namesGroup
            // 
            namesGroup.AutoSize = true;
            namesGroup.Controls.Add(this.wordSeparator);
            namesGroup.Controls.Add(this.episodeNameButton);
            namesGroup.Controls.Add(this.showNameButton);
            namesGroup.Location = new System.Drawing.Point(91, 9);
            namesGroup.Name = "namesGroup";
            namesGroup.Size = new System.Drawing.Size(77, 119);
            namesGroup.TabIndex = 7;
            namesGroup.TabStop = false;
            namesGroup.Text = "Names";
            // 
            // wordSeparator
            // 
            this.wordSeparator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.wordSeparator.FormattingEnabled = true;
            this.wordSeparator.Items.AddRange(new object[] {
            "[space]",
            ".",
            "-",
            "_"});
            this.wordSeparator.Location = new System.Drawing.Point(6, 76);
            this.wordSeparator.Name = "wordSeparator";
            this.wordSeparator.Size = new System.Drawing.Size(65, 21);
            this.wordSeparator.TabIndex = 2;
            // 
            // episodeNameButton
            // 
            this.episodeNameButton.Location = new System.Drawing.Point(6, 48);
            this.episodeNameButton.Name = "episodeNameButton";
            this.episodeNameButton.Size = new System.Drawing.Size(65, 23);
            this.episodeNameButton.TabIndex = 1;
            this.episodeNameButton.Text = "Episode";
            this.episodeNameButton.UseVisualStyleBackColor = true;
            this.episodeNameButton.Click += new System.EventHandler(this.EpisodeNameButtonClick);
            // 
            // showNameButton
            // 
            this.showNameButton.Location = new System.Drawing.Point(6, 19);
            this.showNameButton.Name = "showNameButton";
            this.showNameButton.Size = new System.Drawing.Size(65, 23);
            this.showNameButton.TabIndex = 0;
            this.showNameButton.Text = "Show";
            this.showNameButton.UseVisualStyleBackColor = true;
            this.showNameButton.Click += new System.EventHandler(this.ShowNameButtonClick);
            // 
            // dateGroup
            // 
            dateGroup.AutoSize = true;
            dateGroup.Controls.Add(this.dateFormat);
            dateGroup.Controls.Add(this.dateExample);
            dateGroup.Controls.Add(this.dateButton);
            dateGroup.Location = new System.Drawing.Point(174, 9);
            dateGroup.Name = "dateGroup";
            dateGroup.Size = new System.Drawing.Size(77, 119);
            dateGroup.TabIndex = 8;
            dateGroup.TabStop = false;
            dateGroup.Text = "Date";
            // 
            // dateFormat
            // 
            this.dateFormat.Location = new System.Drawing.Point(6, 51);
            this.dateFormat.Name = "dateFormat";
            this.dateFormat.Size = new System.Drawing.Size(65, 20);
            this.dateFormat.TabIndex = 1;
            this.dateFormat.TextChanged += new System.EventHandler(this.DateFormatTextChanged);
            // 
            // dateExample
            // 
            this.dateExample.Location = new System.Drawing.Point(6, 77);
            this.dateExample.Name = "dateExample";
            this.dateExample.ReadOnly = true;
            this.dateExample.Size = new System.Drawing.Size(65, 20);
            this.dateExample.TabIndex = 2;
            // 
            // dateButton
            // 
            this.dateButton.Location = new System.Drawing.Point(6, 19);
            this.dateButton.Name = "dateButton";
            this.dateButton.Size = new System.Drawing.Size(65, 23);
            this.dateButton.TabIndex = 0;
            this.dateButton.Text = "Date";
            this.dateButton.UseVisualStyleBackColor = true;
            this.dateButton.Click += new System.EventHandler(this.DateButtonClick);
            // 
            // FormatBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 221);
            this.Controls.Add(tableMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(387, 259);
            this.Name = "FormatBuilder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Format Builder";
            this.Load += new System.EventHandler(this.FormatBuilderLoad);
            tableMain.ResumeLayout(false);
            tableMain.PerformLayout();
            flowButtons.ResumeLayout(false);
            panel.ResumeLayout(false);
            panel.PerformLayout();
            groupNumbers.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.digitalSelector)).EndInit();
            otherGroup.ResumeLayout(false);
            namesGroup.ResumeLayout(false);
            dateGroup.ResumeLayout(false);
            dateGroup.PerformLayout();
            this.ResumeLayout(false);

        }


        private System.Windows.Forms.TextBox textExample;
        private System.Windows.Forms.TextBox textFormat;
        private System.Windows.Forms.Button okButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox dateExample;
        private System.Windows.Forms.Button dateButton;
        private System.Windows.Forms.ComboBox wordSeparator;
        private System.Windows.Forms.Button episodeNameButton;
        private System.Windows.Forms.Button showNameButton;
        private System.Windows.Forms.NumericUpDown digitalSelector;
        private System.Windows.Forms.Button episodeNumberButton;
        private System.Windows.Forms.Button seasonNumberButton;
        private System.Windows.Forms.TextBox dateFormat;
        private System.Windows.Forms.Button directoryButton;
        private System.Windows.Forms.Button fileExtensionButton;
        private System.Windows.Forms.Button folderNameButton;
    }
}
