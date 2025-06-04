
namespace C969
{
    partial class ViewLogs
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
            this.logsBackButton = new System.Windows.Forms.Button();
            this.viewLogsTextbox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // logsBackButton
            // 
            this.logsBackButton.Location = new System.Drawing.Point(435, 685);
            this.logsBackButton.Name = "logsBackButton";
            this.logsBackButton.Size = new System.Drawing.Size(75, 23);
            this.logsBackButton.TabIndex = 0;
            this.logsBackButton.Text = "Back";
            this.logsBackButton.UseVisualStyleBackColor = true;
            this.logsBackButton.Click += new System.EventHandler(this.logsBackButton_Click);
            // 
            // viewLogsTextbox
            // 
            this.viewLogsTextbox.Location = new System.Drawing.Point(12, 12);
            this.viewLogsTextbox.Multiline = true;
            this.viewLogsTextbox.Name = "viewLogsTextbox";
            this.viewLogsTextbox.ReadOnly = true;
            this.viewLogsTextbox.Size = new System.Drawing.Size(498, 667);
            this.viewLogsTextbox.TabIndex = 1;
            // 
            // ViewLogs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(522, 720);
            this.Controls.Add(this.viewLogsTextbox);
            this.Controls.Add(this.logsBackButton);
            this.Name = "ViewLogs";
            this.Text = "ViewLogs";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button logsBackButton;
        private System.Windows.Forms.TextBox viewLogsTextbox;
    }
}