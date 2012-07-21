// --------------------------------------------------------------------------------------------------------------------
// <copyright company="TVSorter" file="ListDialog.Designer.cs">
//   2012 - Andrew Jackson
// </copyright>
// <summary>
//   The list dialog.
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
    /// The list dialog.
    /// </summary>
    public partial class ListDialog
    {
        #region Constants and Fields

        /// <summary>
        ///   Required designer variable.
        /// </summary>
        private readonly IContainer components = null;

        /// <summary>
        ///   The add button.
        /// </summary>
        private Button addButton;

        /// <summary>
        ///   The close button.
        /// </summary>
        private Button closeButton;

        /// <summary>
        ///   The list.
        /// </summary>
        private ListBox list;

        /// <summary>
        ///   The remove button.
        /// </summary>
        private Button removeButton;

        /// <summary>
        ///   The save button.
        /// </summary>
        private Button saveButton;

        /// <summary>
        ///   The text.
        /// </summary>
        private TextBox text;

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
            System.Windows.Forms.TableLayoutPanel tableMain;
            System.Windows.Forms.FlowLayoutPanel flowBottomButtons;
            this.closeButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.list = new System.Windows.Forms.ListBox();
            this.text = new System.Windows.Forms.TextBox();
            this.addButton = new System.Windows.Forms.Button();
            this.removeButton = new System.Windows.Forms.Button();
            tableMain = new System.Windows.Forms.TableLayoutPanel();
            flowBottomButtons = new System.Windows.Forms.FlowLayoutPanel();
            tableMain.SuspendLayout();
            flowBottomButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableMain
            // 
            tableMain.ColumnCount = 2;
            tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 118F));
            tableMain.Controls.Add(flowBottomButtons, 0, 2);
            tableMain.Controls.Add(this.list, 0, 1);
            tableMain.Controls.Add(this.text, 0, 0);
            tableMain.Controls.Add(this.addButton, 1, 0);
            tableMain.Controls.Add(this.removeButton, 1, 1);
            tableMain.Dock = System.Windows.Forms.DockStyle.Fill;
            tableMain.Location = new System.Drawing.Point(0, 0);
            tableMain.Name = "tableMain";
            tableMain.RowCount = 3;
            tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            tableMain.Size = new System.Drawing.Size(306, 237);
            tableMain.TabIndex = 0;
            // 
            // flowBottomButtons
            // 
            tableMain.SetColumnSpan(flowBottomButtons, 2);
            flowBottomButtons.Controls.Add(this.closeButton);
            flowBottomButtons.Controls.Add(this.saveButton);
            flowBottomButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            flowBottomButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            flowBottomButtons.Location = new System.Drawing.Point(0, 207);
            flowBottomButtons.Margin = new System.Windows.Forms.Padding(0);
            flowBottomButtons.Name = "flowBottomButtons";
            flowBottomButtons.Size = new System.Drawing.Size(306, 30);
            flowBottomButtons.TabIndex = 0;
            // 
            // closeButton
            // 
            this.closeButton.Location = new System.Drawing.Point(228, 3);
            this.closeButton.Name = "closeButton";
            this.closeButton.Size = new System.Drawing.Size(75, 23);
            this.closeButton.TabIndex = 1;
            this.closeButton.Text = "Close";
            this.closeButton.UseVisualStyleBackColor = true;
            this.closeButton.Click += new System.EventHandler(this.CloseButtonClick);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(147, 3);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 0;
            this.saveButton.Text = "OK";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.SaveButtonClick);
            // 
            // list
            // 
            this.list.Dock = System.Windows.Forms.DockStyle.Fill;
            this.list.FormattingEnabled = true;
            this.list.Location = new System.Drawing.Point(3, 31);
            this.list.Name = "list";
            this.list.Size = new System.Drawing.Size(182, 173);
            this.list.TabIndex = 1;
            // 
            // text
            // 
            this.text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.text.Location = new System.Drawing.Point(3, 3);
            this.text.Name = "text";
            this.text.Size = new System.Drawing.Size(182, 20);
            this.text.TabIndex = 2;
            // 
            // addButton
            // 
            this.addButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.addButton.Location = new System.Drawing.Point(191, 3);
            this.addButton.Name = "addButton";
            this.addButton.Size = new System.Drawing.Size(112, 22);
            this.addButton.TabIndex = 3;
            this.addButton.Text = "Add";
            this.addButton.UseVisualStyleBackColor = true;
            this.addButton.Click += new System.EventHandler(this.AddButtonClick);
            // 
            // removeButton
            // 
            this.removeButton.Dock = System.Windows.Forms.DockStyle.Top;
            this.removeButton.Location = new System.Drawing.Point(191, 31);
            this.removeButton.Name = "removeButton";
            this.removeButton.Size = new System.Drawing.Size(112, 23);
            this.removeButton.TabIndex = 4;
            this.removeButton.Text = "Remove";
            this.removeButton.UseVisualStyleBackColor = true;
            this.removeButton.Click += new System.EventHandler(this.RemoveButtonClick);
            // 
            // ListDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(306, 237);
            this.ControlBox = false;
            this.Controls.Add(tableMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "ListDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ListDialog";
            this.Load += new System.EventHandler(this.ListDialogLoad);
            tableMain.ResumeLayout(false);
            tableMain.PerformLayout();
            flowBottomButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
    }
}