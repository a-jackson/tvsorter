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
        private readonly IContainer components;

        /// <summary>
        ///   The cancel button.
        /// </summary>
        private Button cancelButton;

        /// <summary>
        ///   The input text.
        /// </summary>
        private TextBox inputText;

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
            this.inputText = new System.Windows.Forms.TextBox();
            this.okButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // inputText
            // 
            this.inputText.Location = new System.Drawing.Point(12, 12);
            this.inputText.Name = "inputText";
            this.inputText.Size = new System.Drawing.Size(260, 20);
            this.inputText.TabIndex = 0;
            // 
            // okButton
            // 
            this.okButton.Location = new System.Drawing.Point(197, 38);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 1;
            this.okButton.Text = "OK";
            this.okButton.UseVisualStyleBackColor = true;
            this.okButton.Click += new System.EventHandler(this.OkButtonClick);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(116, 38);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 2;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.CancelButtonClick);
            // 
            // NumberInputDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 68);
            this.ControlBox = false;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.inputText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "NumberInputDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Number Input";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}