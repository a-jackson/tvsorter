// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ShowSelectDialog.Designer.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   Dialog for selecting a show.
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
    /// Dialog for selecting a show.
    /// </summary>
    public partial class ShowSelectDialog
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
        ///   The select button.
        /// </summary>
        private Button selectButton;

        /// <summary>
        ///   The show list.
        /// </summary>
        private ListBox showList;

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
            System.Windows.Forms.FlowLayoutPanel buttonsFlow;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowSelectDialog));
            this.selectButton = new System.Windows.Forms.Button();
            this.closeButton = new System.Windows.Forms.Button();
            this.showList = new System.Windows.Forms.ListBox();
            buttonsFlow = new System.Windows.Forms.FlowLayoutPanel();
            buttonsFlow.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonsFlow
            // 
            buttonsFlow.Controls.Add(this.selectButton);
            buttonsFlow.Controls.Add(this.closeButton);
            buttonsFlow.Dock = System.Windows.Forms.DockStyle.Bottom;
            buttonsFlow.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            buttonsFlow.Location = new System.Drawing.Point(0, 312);
            buttonsFlow.Name = "buttonsFlow";
            buttonsFlow.Size = new System.Drawing.Size(221, 30);
            buttonsFlow.TabIndex = 1;
            // 
            // selectButton
            // 
            this.selectButton.Location = new System.Drawing.Point(143, 3);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(75, 23);
            this.selectButton.TabIndex = 0;
            this.selectButton.Text = "Select";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.SelectButtonClick);
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(62, 3);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.CloseButtonClick);
            // 
            // showList
            // 
            this.showList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.showList.FormattingEnabled = true;
            this.showList.Location = new System.Drawing.Point(0, 0);
            this.showList.Name = "showList";
            this.showList.Size = new System.Drawing.Size(221, 342);
            this.showList.TabIndex = 0;
            // 
            // ShowSelectDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(221, 342);
            this.Controls.Add(buttonsFlow);
            this.Controls.Add(this.showList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "ShowSelectDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select Show";
            this.Load += new System.EventHandler(this.ShowSelectDialogLoad);
            buttonsFlow.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}