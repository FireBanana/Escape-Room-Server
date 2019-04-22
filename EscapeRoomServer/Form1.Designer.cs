namespace EscapeRoomServer
{
    partial class Form1
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.IpAddressText = new System.Windows.Forms.Label();
            this.ClientListBox = new System.Windows.Forms.ListBox();
            this.MainBackgroundWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP ADDRESS:";
            // 
            // IpAddressText
            // 
            this.IpAddressText.AutoSize = true;
            this.IpAddressText.Location = new System.Drawing.Point(112, 9);
            this.IpAddressText.Name = "IpAddressText";
            this.IpAddressText.Size = new System.Drawing.Size(40, 17);
            this.IpAddressText.TabIndex = 1;
            this.IpAddressText.Text = "none";
            // 
            // ClientListBox
            // 
            this.ClientListBox.FormattingEnabled = true;
            this.ClientListBox.ItemHeight = 16;
            this.ClientListBox.Location = new System.Drawing.Point(994, 9);
            this.ClientListBox.Name = "ClientListBox";
            this.ClientListBox.Size = new System.Drawing.Size(360, 324);
            this.ClientListBox.TabIndex = 2;
            // 
            // MainBackgroundWorker
            // 
            this.MainBackgroundWorker.WorkerReportsProgress = true;
            this.MainBackgroundWorker.WorkerSupportsCancellation = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.ClientListBox);
            this.Controls.Add(this.IpAddressText);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label IpAddressText;
        private System.Windows.Forms.ListBox ClientListBox;
        private System.ComponentModel.BackgroundWorker MainBackgroundWorker;
    }
}

