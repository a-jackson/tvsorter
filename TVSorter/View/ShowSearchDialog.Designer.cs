// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ShowSearchDialog.Designer.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Dialog for searching for a show.
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
    /// Dialog for searching for a show.
    /// </summary>
    public partial class ShowSearchDialog
    {
        #region Constants and Fields

        /// <summary>
        ///   Required designer variable.
        /// </summary>
        private readonly IContainer components = null;

        /// <summary>
        ///   The close button.
        /// </summary>
        private Button closeButton;

        /// <summary>
        ///   The id column.
        /// </summary>
        private ColumnHeader idColumn;

        /// <summary>
        ///   The list results.
        /// </summary>
        private ListView listResults;

        /// <summary>
        ///   The name column.
        /// </summary>
        private ColumnHeader nameColumn;

        /// <summary>
        ///   The name text.
        /// </summary>
        private TextBox nameText;

        /// <summary>
        ///   The search button.
        /// </summary>
        private Button searchButton;

        /// <summary>
        ///   The select button.
        /// </summary>
        private Button selectButton;

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
            System.Windows.Forms.Label nameLabel;
            this.listResults = new System.Windows.Forms.ListView();
            this.nameColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.idColumn = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nameText = new System.Windows.Forms.TextBox();
            this.searchButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.selectButton = new System.Windows.Forms.Button();
            mainTable = new System.Windows.Forms.TableLayoutPanel();
            nameLabel = new System.Windows.Forms.Label();
            mainTable.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTable
            // 
            mainTable.ColumnCount = 5;
            mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            mainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            mainTable.Controls.Add(this.listResults, 0, 0);
            mainTable.Controls.Add(nameLabel, 0, 1);
            mainTable.Controls.Add(this.nameText, 1, 1);
            mainTable.Controls.Add(this.searchButton, 2, 1);
            mainTable.Controls.Add(this.closeButton, 4, 1);
            mainTable.Controls.Add(this.selectButton, 3, 1);
            mainTable.Dock = System.Windows.Forms.DockStyle.Fill;
            mainTable.Location = new System.Drawing.Point(0, 0);
            mainTable.Name = "mainTable";
            mainTable.RowCount = 2;
            mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 89.31298F));
            mainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.68702F));
            mainTable.Size = new System.Drawing.Size(458, 237);
            mainTable.TabIndex = 1;
            // 
            // listResults
            // 
            this.listResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumn,
            this.idColumn});
            mainTable.SetColumnSpan(this.listResults, 5);
            this.listResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listResults.FullRowSelect = true;
            this.listResults.Location = new System.Drawing.Point(3, 3);
            this.listResults.MultiSelect = false;
            this.listResults.Name = "listResults";
            this.listResults.Size = new System.Drawing.Size(452, 205);
            this.listResults.TabIndex = 0;
            this.listResults.UseCompatibleStateImageBehavior = false;
            this.listResults.View = System.Windows.Forms.View.Details;
            this.listResults.DoubleClick += new System.EventHandler(this.ListResultsDoubleClick);
            // 
            // nameColumn
            // 
            this.nameColumn.Text = "Show Name";
            this.nameColumn.Width = 338;
            // 
            // idColumn
            // 
            this.idColumn.Text = "TVDB ID";
            this.idColumn.Width = 105;
            // 
            // nameLabel
            // 
            nameLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            nameLabel.AutoSize = true;
            nameLabel.Location = new System.Drawing.Point(3, 217);
            nameLabel.Name = "nameLabel";
            nameLabel.Size = new System.Drawing.Size(38, 13);
            nameLabel.TabIndex = 1;
            nameLabel.Text = "Name:";
            // 
            // nameText
            // 
            this.nameText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nameText.Location = new System.Drawing.Point(47, 214);
            this.nameText.Name = "nameText";
            this.nameText.Size = new System.Drawing.Size(165, 20);
            this.nameText.TabIndex = 2;
            // 
            // searchButton
            // 
            this.searchButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.searchButton.Location = new System.Drawing.Point(218, 214);
            this.searchButton.Name = "searchButton";
            this.searchButton.Size = new System.Drawing.Size(75, 20);
            this.searchButton.TabIndex = 3;
            this.searchButton.Text = "Search";
            this.searchButton.UseVisualStyleBackColor = true;
            this.searchButton.Click += new System.EventHandler(this.SearchButtonClick);
            // 
            // closeButton
            // 
            this.closeButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.closeButton.Location = new System.Drawing.Point(380, 214);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 20);
            this.closeButton.TabIndex = 4;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.CloseButtonClick);
            // 
            // selectButton
            // 
            this.selectButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectButton.Location = new System.Drawing.Point(299, 214);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(75, 20);
            this.selectButton.TabIndex = 5;
            this.selectButton.Text = "Select";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.SelectButtonClick);
            // 
            // ShowSearchDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(458, 237);
            this.ControlBox = false;
            this.Controls.Add(mainTable);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "ShowSearchDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Show Search";
            this.Load += new System.EventHandler(this.ShowSearchDialogLoad);
            mainTable.ResumeLayout(false);
            mainTable.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
    }
}