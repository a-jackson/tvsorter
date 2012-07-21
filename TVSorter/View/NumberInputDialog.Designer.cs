// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="NumberInputDialog.Designer.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   A dialog for number entry.
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
    /// A dialog for number entry.
    /// </summary>
    public partial class NumberInputDialog
    {
        #region Constants and Fields

        /// <summary>
        ///   Required designer variable.
        /// </summary>
        private readonly IContainer components = null;

        /// <summary>
        ///   The cancel button.
        /// </summary>
        private Button cancelButton;

        /// <summary>
        ///   The input text.
        /// </summary>
        private TextBox seasonNumber;

        /// <summary>
        ///   The ok button.
        /// </summary>
        private Button okButton;

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
            System.Windows.Forms.TableLayoutPanel table;
            System.Windows.Forms.FlowLayoutPanel flowButtons;
            System.Windows.Forms.Label seasonLabel;
            System.Windows.Forms.Label episodeLabel;
            this.seasonNumber = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.episodeNumber = new System.Windows.Forms.TextBox();
            table = new System.Windows.Forms.TableLayoutPanel();
            flowButtons = new System.Windows.Forms.FlowLayoutPanel();
            seasonLabel = new System.Windows.Forms.Label();
            episodeLabel = new System.Windows.Forms.Label();
            table.SuspendLayout();
            flowButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // seasonNumber
            // 
            this.seasonNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.seasonNumber.Location = new System.Drawing.Point(96, 3);
            this.seasonNumber.Name = "seasonNumber";
            this.seasonNumber.Size = new System.Drawing.Size(114, 20);
            this.seasonNumber.TabIndex = 0;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(135, 3);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(54, 3);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // table
            // 
            table.ColumnCount = 2;
            table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 93F));
            table.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            table.Controls.Add(flowButtons, 0, 2);
            table.Controls.Add(this.episodeNumber, 1, 1);
            table.Controls.Add(this.seasonNumber, 1, 0);
            table.Controls.Add(seasonLabel, 0, 0);
            table.Controls.Add(episodeLabel, 0, 1);
            table.Dock = System.Windows.Forms.DockStyle.Fill;
            table.Location = new System.Drawing.Point(0, 0);
            table.Name = "table";
            table.RowCount = 2;
            table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            table.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            table.Size = new System.Drawing.Size(213, 81);
            table.TabIndex = 3;
            // 
            // flowButtons
            // 
            table.SetColumnSpan(flowButtons, 2);
            flowButtons.Controls.Add(this.okButton);
            flowButtons.Controls.Add(this.cancelButton);
            flowButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            flowButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flowButtons.Location = new System.Drawing.Point(0, 50);
            flowButtons.Margin = new System.Windows.Forms.Padding(0);
            flowButtons.Name = "flowButtons";
            flowButtons.Size = new System.Drawing.Size(213, 56);
            flowButtons.TabIndex = 0;
            // 
            // episodeNumber
            // 
            this.episodeNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.episodeNumber.Location = new System.Drawing.Point(96, 28);
            this.episodeNumber.Name = "episodeNumber";
            this.episodeNumber.Size = new System.Drawing.Size(114, 20);
            this.episodeNumber.TabIndex = 1;
            // 
            // seasonLabel
            // 
            seasonLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            seasonLabel.AutoSize = true;
            seasonLabel.Location = new System.Drawing.Point(6, 6);
            seasonLabel.Name = "seasonLabel";
            seasonLabel.Size = new System.Drawing.Size(84, 13);
            seasonLabel.TabIndex = 2;
            seasonLabel.Text = "Season number:";
            // 
            // episodeLabel
            // 
            episodeLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            episodeLabel.AutoSize = true;
            episodeLabel.Location = new System.Drawing.Point(4, 31);
            episodeLabel.Name = "episodeLabel";
            episodeLabel.Size = new System.Drawing.Size(86, 13);
            episodeLabel.TabIndex = 3;
            episodeLabel.Text = "Episode number:";
            // 
            // NumberInputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(213, 81);
            this.ControlBox = false;
            this.Controls.Add(table);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "NumberInputDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Episode";
            table.ResumeLayout(false);
            table.PerformLayout();
            flowButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TextBox episodeNumber;
    }
}