namespace IpAddressMonitor.WinFormsApp
{
    internal partial class MainForm
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.labelOfInformation = new Label();
            this.SuspendLayout();
            // 
            // labelOfInformation
            // 
            this.labelOfInformation.AutoSize = true;
            this.labelOfInformation.Dock = DockStyle.Fill;
            this.labelOfInformation.Font = new Font("Yu Gothic UI", 12F);
            this.labelOfInformation.Location = new Point(0, 0);
            this.labelOfInformation.Name = "labelOfInformation";
            this.labelOfInformation.Size = new Size(60, 21);
            this.labelOfInformation.TabIndex = 0;
            this.labelOfInformation.Text = "*.*.*.*/*";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.ClientSize = new Size(182, 61);
            this.Controls.Add(this.labelOfInformation);
            this.Icon = (Icon)resources.GetObject("$this.Icon");
            this.Name = "MainForm";
            this.Text = "IPv4";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label labelOfInformation;
    }
}
