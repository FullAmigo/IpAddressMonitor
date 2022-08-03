namespace IpAddressMonitor.WinFormsApp
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.labelOfInformation = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelOfInformation
            // 
            this.labelOfInformation.AutoSize = true;
            this.labelOfInformation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelOfInformation.Font = new System.Drawing.Font("Yu Gothic UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.labelOfInformation.Location = new System.Drawing.Point(0, 0);
            this.labelOfInformation.Name = "labelOfInformation";
            this.labelOfInformation.Size = new System.Drawing.Size(60, 21);
            this.labelOfInformation.TabIndex = 0;
            this.labelOfInformation.Text = "*.*.*.*/*";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(182, 61);
            this.Controls.Add(this.labelOfInformation);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "IPv4";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label labelOfInformation;
    }
}
