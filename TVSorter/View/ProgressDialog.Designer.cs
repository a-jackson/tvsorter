// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ProgressDialog.Designer.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The dialog showing the progress bar.
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
    /// The dialog showing the progress bar.
    /// </summary>
    public partial class ProgressDialog
    {
        #region Constants and Fields

        /// <summary>
        ///   Required designer variable.
        /// </summary>
        private readonly IContainer components;

        /// <summary>
        ///   The task progress.
        /// </summary>
        private ProgressBar taskProgress;

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
            this.taskProgress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // taskProgress
            // 
            this.taskProgress.Dock = System.Windows.Forms.DockStyle.Fill;
            this.taskProgress.Location = new System.Drawing.Point(0, 0);
            this.taskProgress.Name = "taskProgress";
            this.taskProgress.Size = new System.Drawing.Size(160, 27);
            this.taskProgress.TabIndex = 0;
            // 
            // ProgressDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(160, 27);
            this.ControlBox = false;
            this.Controls.Add(this.taskProgress);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ProgressDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Task";
            this.Load += new System.EventHandler(this.ProgressDialogLoad);
            this.ResumeLayout(false);

        }

        #endregion
    }
}