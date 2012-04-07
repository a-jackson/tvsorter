// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="Settings.Designer.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The settings tab.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------

namespace TVSorter.View
{
    #region Using Directives

    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;

    #endregion

    /// <summary>
    /// The settings tab.
    /// </summary>
    public partial class Settings
    {
        #region Constants and Fields

        /// <summary>
        ///   Required designer variable.
        /// </summary>
        private readonly IContainer components;

        /// <summary>
        ///   The add destination button.
        /// </summary>
        private Button addDestinationButton;

        /// <summary>
        ///   The delete empty check.
        /// </summary>
        private CheckBox deleteEmptyCheck;

        /// <summary>
        ///   The destination list.
        /// </summary>
        private ListBox destinationList;

        /// <summary>
        ///   The file extensions button.
        /// </summary>
        private Button fileExtensionsButton;

        /// <summary>
        ///   The folder dialog.
        /// </summary>
        private FolderBrowserDialog folderDialog;

        /// <summary>
        ///   The format builder button.
        /// </summary>
        private Button formatBuilderButton;

        /// <summary>
        ///   The format text.
        /// </summary>
        private TextBox formatText;

        /// <summary>
        ///   The group directories.
        /// </summary>
        private GroupBox groupDirectories;

        /// <summary>
        ///   The recurse subdirectories check.
        /// </summary>
        private CheckBox recurseSubdirectoriesCheck;

        /// <summary>
        ///   The reg ex button.
        /// </summary>
        private Button regExButton;

        /// <summary>
        ///   The remove destination button.
        /// </summary>
        private Button removeDestinationButton;

        /// <summary>
        ///   The rename if exists check.
        /// </summary>
        private CheckBox renameIfExistsCheck;

        /// <summary>
        ///   The revert button.
        /// </summary>
        private Button revertButton;

        /// <summary>
        ///   The save button.
        /// </summary>
        private Button saveButton;

        /// <summary>
        ///   The source browse.
        /// </summary>
        private Button sourceBrowse;

        /// <summary>
        ///   The source text.
        /// </summary>
        private TextBox sourceText;

        #endregion

        #region Methods

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">
        /// true if managed resources should be disposed; otherwise, false.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Required method for Designer support - do not modify 
        ///   the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.TableLayoutPanel mainTable;
            System.Windows.Forms.TableLayoutPanel tableDirectories;
            System.Windows.Forms.Label sourceLabel;
            System.Windows.Forms.Label destinationListLabel;
            System.Windows.Forms.FlowLayoutPanel destinationButtonsFlow;
            System.Windows.Forms.Label destinationDescLabel;
            System.Windows.Forms.GroupBox sortOptionsGroup;
            System.Windows.Forms.FlowLayoutPanel sortOptionsFlow;
            System.Windows.Forms.GroupBox searchOptionsGroup;
            System.Windows.Forms.FlowLayoutPanel searchOptionsFlow;
            System.Windows.Forms.GroupBox formatOptionsGroup;
            System.Windows.Forms.TableLayoutPanel formatTable;
            System.Windows.Forms.Label formatLabel;
            System.Windows.Forms.FlowLayoutPanel flowBottomButtons;
            this.groupDirectories = new System.Windows.Forms.GroupBox();
            this.sourceText = new System.Windows.Forms.TextBox();
            this.sourceBrowse = new System.Windows.Forms.Button();
            this.destinationList = new System.Windows.Forms.ListBox();
            this.addDestinationButton = new System.Windows.Forms.Button();
            this.removeDestinationButton = new System.Windows.Forms.Button();
            this.recurseSubdirectoriesCheck = new System.Windows.Forms.CheckBox();
            this.deleteEmptyCheck = new System.Windows.Forms.CheckBox();
            this.renameIfExistsCheck = new System.Windows.Forms.CheckBox();
            this.regExButton = new System.Windows.Forms.Button();
            this.fileExtensionsButton = new System.Windows.Forms.Button();
            this.formatText = new System.Windows.Forms.TextBox();
            this.formatBuilderButton = new System.Windows.Forms.Button();
            this.revertButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.folderDialog = new System.Windows.Forms.FolderBrowserDialog();
            mainTable = new System.Windows.Forms.TableLayoutPanel();
            tableDirectories = new System.Windows.Forms.TableLayoutPanel();
            sourceLabel = new System.Windows.Forms.Label();
            destinationListLabel = new System.Windows.Forms.Label();
            destinationButtonsFlow = new System.Windows.Forms.FlowLayoutPanel();
            destinationDescLabel = new System.Windows.Forms.Label();
            sortOptionsGroup = new System.Windows.Forms.GroupBox();
            sortOptionsFlow = new System.Windows.Forms.FlowLayoutPanel();
            searchOptionsGroup = new System.Windows.Forms.GroupBox();
            searchOptionsFlow = new System.Windows.Forms.FlowLayoutPanel();
            formatOptionsGroup = new System.Windows.Forms.GroupBox();
            formatTable = new System.Windows.Forms.TableLayoutPanel();
            formatLabel = new System.Windows.Forms.Label();
            flowBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            mainTable.SuspendLayout();
            this.groupDirectories.SuspendLayout();
            tableDirectories.SuspendLayout();
            destinationButtonsFlow.SuspendLayout();
            sortOptionsGroup.SuspendLayout();
            sortOptionsFlow.SuspendLayout();
            searchOptionsGroup.SuspendLayout();
            searchOptionsFlow.SuspendLayout();
            formatOptionsGroup.SuspendLayout();
            formatTable.SuspendLayout();
            flowBottomButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTable
            // 
            mainTable.ColumnCount = 1;
            mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            mainTable.Controls.Add(this.groupDirectories, 0, 0);
            mainTable.Controls.Add(sortOptionsGroup, 0, 2);
            mainTable.Controls.Add(searchOptionsGroup, 0, 3);
            mainTable.Controls.Add(formatOptionsGroup, 0, 1);
            mainTable.Controls.Add(flowBottomButtons, 0, 4);
            mainTable.Dock = System.Windows.Forms.DockStyle.Fill;
            mainTable.Location = new System.Drawing.Point(0, 0);
            mainTable.Name = "mainTable";
            mainTable.RowCount = 5;
            mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 54F));
            mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 95F));
            mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            mainTable.Size = new System.Drawing.Size(637, 450);
            mainTable.TabIndex = 0;
            // 
            // groupDirectories
            // 
            this.groupDirectories.Controls.Add(tableDirectories);
            this.groupDirectories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupDirectories.Location = new System.Drawing.Point(3, 3);
            this.groupDirectories.Name = "groupDirectories";
            this.groupDirectories.Size = new System.Drawing.Size(631, 199);
            this.groupDirectories.TabIndex = 0;
            this.groupDirectories.TabStop = false;
            this.groupDirectories.Text = "Directory Settings";
            // 
            // tableDirectories
            // 
            tableDirectories.ColumnCount = 3;
            tableDirectories.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            tableDirectories.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableDirectories.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            tableDirectories.Controls.Add(sourceLabel, 0, 0);
            tableDirectories.Controls.Add(this.sourceText, 1, 0);
            tableDirectories.Controls.Add(this.sourceBrowse, 2, 0);
            tableDirectories.Controls.Add(destinationListLabel, 0, 2);
            tableDirectories.Controls.Add(this.destinationList, 1, 2);
            tableDirectories.Controls.Add(destinationButtonsFlow, 2, 2);
            tableDirectories.Controls.Add(destinationDescLabel, 1, 1);
            tableDirectories.Dock = System.Windows.Forms.DockStyle.Fill;
            tableDirectories.Location = new System.Drawing.Point(3, 16);
            tableDirectories.Name = "tableDirectories";
            tableDirectories.RowCount = 3;
            tableDirectories.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            tableDirectories.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 34F));
            tableDirectories.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableDirectories.Size = new System.Drawing.Size(625, 180);
            tableDirectories.TabIndex = 0;
            // 
            // sourceLabel
            // 
            sourceLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            sourceLabel.AutoSize = true;
            sourceLabel.Location = new System.Drawing.Point(34, 6);
            sourceLabel.Name = "sourceLabel";
            sourceLabel.Size = new System.Drawing.Size(89, 13);
            sourceLabel.TabIndex = 0;
            sourceLabel.Text = "Source Directory:";
            // 
            // sourceText
            // 
            this.sourceText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceText.Location = new System.Drawing.Point(129, 3);
            this.sourceText.Name = "sourceText";
            this.sourceText.ReadOnly = true;
            this.sourceText.Size = new System.Drawing.Size(388, 20);
            this.sourceText.TabIndex = 1;
            // 
            // sourceBrowse
            // 
            this.sourceBrowse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sourceBrowse.Location = new System.Drawing.Point(523, 3);
            this.sourceBrowse.Name = "sourceBrowse";
            this.sourceBrowse.Size = new System.Drawing.Size(99, 20);
            this.sourceBrowse.TabIndex = 2;
            this.sourceBrowse.Text = "Browse";
            this.sourceBrowse.UseVisualStyleBackColor = true;
            this.sourceBrowse.Click += new System.EventHandler(this.SourceBrowseClick);
            // 
            // destinationListLabel
            // 
            destinationListLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            destinationListLabel.AutoSize = true;
            destinationListLabel.Location = new System.Drawing.Point(7, 113);
            destinationListLabel.Name = "destinationListLabel";
            destinationListLabel.Size = new System.Drawing.Size(116, 13);
            destinationListLabel.TabIndex = 5;
            destinationListLabel.Text = "Destination Directories:";
            // 
            // destinationList
            // 
            this.destinationList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.destinationList.FormattingEnabled = true;
            this.destinationList.Location = new System.Drawing.Point(129, 63);
            this.destinationList.Name = "destinationList";
            this.destinationList.Size = new System.Drawing.Size(388, 114);
            this.destinationList.TabIndex = 6;
            // 
            // destinationButtonsFlow
            // 
            destinationButtonsFlow.Controls.Add(this.addDestinationButton);
            destinationButtonsFlow.Controls.Add(this.removeDestinationButton);
            destinationButtonsFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            destinationButtonsFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            destinationButtonsFlow.Location = new System.Drawing.Point(520, 60);
            destinationButtonsFlow.Margin = new System.Windows.Forms.Padding(0);
            destinationButtonsFlow.Name = "destinationButtonsFlow";
            destinationButtonsFlow.Size = new System.Drawing.Size(105, 120);
            destinationButtonsFlow.TabIndex = 7;
            // 
            // addDestinationButton
            // 
            this.addDestinationButton.Location = new System.Drawing.Point(3, 3);
            this.addDestinationButton.Name = "addDestinationButton";
            this.addDestinationButton.Size = new System.Drawing.Size(99, 23);
            this.addDestinationButton.TabIndex = 0;
            this.addDestinationButton.Text = "Add";
            this.addDestinationButton.UseVisualStyleBackColor = true;
            this.addDestinationButton.Click += new System.EventHandler(this.AddDestinationButtonClick);
            // 
            // removeDestinationButton
            // 
            this.removeDestinationButton.Location = new System.Drawing.Point(3, 32);
            this.removeDestinationButton.Name = "removeDestinationButton";
            this.removeDestinationButton.Size = new System.Drawing.Size(99, 23);
            this.removeDestinationButton.TabIndex = 1;
            this.removeDestinationButton.Text = "Remove";
            this.removeDestinationButton.UseVisualStyleBackColor = true;
            this.removeDestinationButton.Click += new System.EventHandler(this.RemoveDestinationButtonClick);
            // 
            // destinationDescLabel
            // 
            destinationDescLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            destinationDescLabel.AutoSize = true;
            destinationDescLabel.Location = new System.Drawing.Point(129, 30);
            destinationDescLabel.Name = "destinationDescLabel";
            destinationDescLabel.Size = new System.Drawing.Size(351, 26);
            destinationDescLabel.TabIndex = 8;
            destinationDescLabel.Text = "List the directories to sort files into. Most features will use all 3 directories" +
    ". \r\nThe one selected will be used as the default directory to move files to.";
            // 
            // sortOptionsGroup
            // 
            sortOptionsGroup.Controls.Add(sortOptionsFlow);
            sortOptionsGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            sortOptionsGroup.Location = new System.Drawing.Point(3, 262);
            sortOptionsGroup.Name = "sortOptionsGroup";
            sortOptionsGroup.Size = new System.Drawing.Size(631, 89);
            sortOptionsGroup.TabIndex = 1;
            sortOptionsGroup.TabStop = false;
            sortOptionsGroup.Text = "Sort Options";
            // 
            // sortOptionsFlow
            // 
            sortOptionsFlow.Controls.Add(this.recurseSubdirectoriesCheck);
            sortOptionsFlow.Controls.Add(this.deleteEmptyCheck);
            sortOptionsFlow.Controls.Add(this.renameIfExistsCheck);
            sortOptionsFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            sortOptionsFlow.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            sortOptionsFlow.Location = new System.Drawing.Point(3, 16);
            sortOptionsFlow.Name = "sortOptionsFlow";
            sortOptionsFlow.Size = new System.Drawing.Size(625, 70);
            sortOptionsFlow.TabIndex = 0;
            // 
            // recurseSubdirectoriesCheck
            // 
            this.recurseSubdirectoriesCheck.AutoSize = true;
            this.recurseSubdirectoriesCheck.Location = new System.Drawing.Point(3, 3);
            this.recurseSubdirectoriesCheck.Name = "recurseSubdirectoriesCheck";
            this.recurseSubdirectoriesCheck.Size = new System.Drawing.Size(136, 17);
            this.recurseSubdirectoriesCheck.TabIndex = 0;
            this.recurseSubdirectoriesCheck.Text = "Recurse Subdirectories";
            this.recurseSubdirectoriesCheck.UseVisualStyleBackColor = true;
            // 
            // deleteEmptyCheck
            // 
            this.deleteEmptyCheck.AutoSize = true;
            this.deleteEmptyCheck.Location = new System.Drawing.Point(3, 26);
            this.deleteEmptyCheck.Name = "deleteEmptyCheck";
            this.deleteEmptyCheck.Size = new System.Drawing.Size(159, 17);
            this.deleteEmptyCheck.TabIndex = 1;
            this.deleteEmptyCheck.Text = "Delete Empty Subdirectories";
            this.deleteEmptyCheck.UseVisualStyleBackColor = true;
            // 
            // renameIfExistsCheck
            // 
            this.renameIfExistsCheck.AutoSize = true;
            this.renameIfExistsCheck.Location = new System.Drawing.Point(3, 49);
            this.renameIfExistsCheck.Name = "renameIfExistsCheck";
            this.renameIfExistsCheck.Size = new System.Drawing.Size(213, 17);
            this.renameIfExistsCheck.TabIndex = 2;
            this.renameIfExistsCheck.Text = "Rename if Episode Exists at Destination";
            this.renameIfExistsCheck.UseVisualStyleBackColor = true;
            // 
            // searchOptionsGroup
            // 
            searchOptionsGroup.Controls.Add(searchOptionsFlow);
            searchOptionsGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            searchOptionsGroup.Location = new System.Drawing.Point(3, 357);
            searchOptionsGroup.Name = "searchOptionsGroup";
            searchOptionsGroup.Size = new System.Drawing.Size(631, 50);
            searchOptionsGroup.TabIndex = 2;
            searchOptionsGroup.TabStop = false;
            searchOptionsGroup.Text = "Search Options";
            // 
            // searchOptionsFlow
            // 
            searchOptionsFlow.Controls.Add(this.regExButton);
            searchOptionsFlow.Controls.Add(this.fileExtensionsButton);
            searchOptionsFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            searchOptionsFlow.Location = new System.Drawing.Point(3, 16);
            searchOptionsFlow.Name = "searchOptionsFlow";
            searchOptionsFlow.Size = new System.Drawing.Size(625, 31);
            searchOptionsFlow.TabIndex = 0;
            // 
            // regExButton
            // 
            this.regExButton.Location = new System.Drawing.Point(3, 3);
            this.regExButton.Name = "regExButton";
            this.regExButton.Size = new System.Drawing.Size(136, 23);
            this.regExButton.TabIndex = 0;
            this.regExButton.Text = "Edit Regular Expressions";
            this.regExButton.UseVisualStyleBackColor = true;
            this.regExButton.Click += new System.EventHandler(this.RegExButtonClick);
            // 
            // fileExtensionsButton
            // 
            this.fileExtensionsButton.Location = new System.Drawing.Point(145, 3);
            this.fileExtensionsButton.Name = "fileExtensionsButton";
            this.fileExtensionsButton.Size = new System.Drawing.Size(136, 23);
            this.fileExtensionsButton.TabIndex = 1;
            this.fileExtensionsButton.Text = "Edit File Extensions";
            this.fileExtensionsButton.UseVisualStyleBackColor = true;
            this.fileExtensionsButton.Click += new System.EventHandler(this.FileExtensionsButtonClick);
            // 
            // formatOptionsGroup
            // 
            formatOptionsGroup.Controls.Add(formatTable);
            formatOptionsGroup.Dock = System.Windows.Forms.DockStyle.Fill;
            formatOptionsGroup.Location = new System.Drawing.Point(3, 208);
            formatOptionsGroup.Name = "formatOptionsGroup";
            formatOptionsGroup.Size = new System.Drawing.Size(631, 48);
            formatOptionsGroup.TabIndex = 3;
            formatOptionsGroup.TabStop = false;
            formatOptionsGroup.Text = "Format Options";
            // 
            // formatTable
            // 
            formatTable.ColumnCount = 3;
            formatTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 126F));
            formatTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            formatTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            formatTable.Controls.Add(formatLabel, 0, 0);
            formatTable.Controls.Add(this.formatText, 1, 0);
            formatTable.Controls.Add(this.formatBuilderButton, 2, 0);
            formatTable.Dock = System.Windows.Forms.DockStyle.Fill;
            formatTable.Location = new System.Drawing.Point(3, 16);
            formatTable.Name = "formatTable";
            formatTable.RowCount = 1;
            formatTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            formatTable.Size = new System.Drawing.Size(625, 29);
            formatTable.TabIndex = 0;
            // 
            // formatLabel
            // 
            formatLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            formatLabel.AutoSize = true;
            formatLabel.Location = new System.Drawing.Point(52, 8);
            formatLabel.Name = "formatLabel";
            formatLabel.Size = new System.Drawing.Size(71, 13);
            formatLabel.TabIndex = 0;
            formatLabel.Text = "Format Label:";
            // 
            // formatText
            // 
            this.formatText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.formatText.Location = new System.Drawing.Point(129, 3);
            this.formatText.Name = "formatText";
            this.formatText.Size = new System.Drawing.Size(388, 20);
            this.formatText.TabIndex = 1;
            // 
            // formatBuilderButton
            // 
            this.formatBuilderButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.formatBuilderButton.Location = new System.Drawing.Point(523, 3);
            this.formatBuilderButton.Name = "formatBuilderButton";
            this.formatBuilderButton.Size = new System.Drawing.Size(99, 23);
            this.formatBuilderButton.TabIndex = 2;
            this.formatBuilderButton.Text = "Format Builder";
            this.formatBuilderButton.UseVisualStyleBackColor = true;
            this.formatBuilderButton.Click += new System.EventHandler(this.FormatBuilderButtonClick);
            // 
            // flowBottomButtons
            // 
            flowBottomButtons.Controls.Add(this.revertButton);
            flowBottomButtons.Controls.Add(this.saveButton);
            flowBottomButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            flowBottomButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flowBottomButtons.Location = new System.Drawing.Point(3, 413);
            flowBottomButtons.Name = "flowBottomButtons";
            flowBottomButtons.Size = new System.Drawing.Size(631, 34);
            flowBottomButtons.TabIndex = 4;
            // 
            // revertButton
            // 
            this.revertButton.Location = new System.Drawing.Point(553, 3);
            this.revertButton.Name = "revertButton";
            this.revertButton.Size = new System.Drawing.Size(75, 23);
            this.revertButton.TabIndex = 0;
            this.revertButton.Text = "Revert";
            this.revertButton.UseVisualStyleBackColor = true;
            this.revertButton.Click += new System.EventHandler(this.RevertButtonClick);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(472, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 1;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButtonClick);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(mainTable);
            this.Name = "Settings";
            this.Size = new System.Drawing.Size(637, 450);
            this.Load += new System.EventHandler(this.SettingsLoad);
            mainTable.ResumeLayout(false);
            this.groupDirectories.ResumeLayout(false);
            tableDirectories.ResumeLayout(false);
            tableDirectories.PerformLayout();
            destinationButtonsFlow.ResumeLayout(false);
            sortOptionsGroup.ResumeLayout(false);
            sortOptionsFlow.ResumeLayout(false);
            sortOptionsFlow.PerformLayout();
            searchOptionsGroup.ResumeLayout(false);
            searchOptionsFlow.ResumeLayout(false);
            formatOptionsGroup.ResumeLayout(false);
            formatTable.ResumeLayout(false);
            formatTable.PerformLayout();
            flowBottomButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}