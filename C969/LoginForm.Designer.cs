
namespace C969
{
    partial class LoginForm
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
            this.loginButton = new System.Windows.Forms.Button();
            this.usernameTextBox = new System.Windows.Forms.TextBox();
            this.passwordTextBox = new System.Windows.Forms.TextBox();
            this.usernameLabel = new System.Windows.Forms.Label();
            this.passwordLabel = new System.Windows.Forms.Label();
            this.langSelectComboBox = new System.Windows.Forms.ComboBox();
            this.langSelectLabel = new System.Windows.Forms.Label();
            this.englishErrorLabel = new System.Windows.Forms.Label();
            this.otherLangErrorLabel = new System.Windows.Forms.Label();
            this.selectedLanguageLabel = new System.Windows.Forms.Label();
            this.titleLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // loginButton
            // 
            this.loginButton.Location = new System.Drawing.Point(205, 307);
            this.loginButton.Name = "loginButton";
            this.loginButton.Size = new System.Drawing.Size(75, 23);
            this.loginButton.TabIndex = 0;
            this.loginButton.Text = "Login";
            this.loginButton.UseVisualStyleBackColor = true;
            this.loginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // usernameTextBox
            // 
            this.usernameTextBox.Location = new System.Drawing.Point(191, 109);
            this.usernameTextBox.Name = "usernameTextBox";
            this.usernameTextBox.Size = new System.Drawing.Size(196, 20);
            this.usernameTextBox.TabIndex = 1;
            // 
            // passwordTextBox
            // 
            this.passwordTextBox.Location = new System.Drawing.Point(191, 151);
            this.passwordTextBox.Name = "passwordTextBox";
            this.passwordTextBox.Size = new System.Drawing.Size(196, 20);
            this.passwordTextBox.TabIndex = 2;
            // 
            // usernameLabel
            // 
            this.usernameLabel.AutoSize = true;
            this.usernameLabel.Location = new System.Drawing.Point(125, 112);
            this.usernameLabel.Name = "usernameLabel";
            this.usernameLabel.Size = new System.Drawing.Size(60, 13);
            this.usernameLabel.TabIndex = 3;
            this.usernameLabel.Text = "User Name";
            // 
            // passwordLabel
            // 
            this.passwordLabel.AutoSize = true;
            this.passwordLabel.Location = new System.Drawing.Point(125, 154);
            this.passwordLabel.Name = "passwordLabel";
            this.passwordLabel.Size = new System.Drawing.Size(53, 13);
            this.passwordLabel.TabIndex = 4;
            this.passwordLabel.Text = "Password";
            // 
            // langSelectComboBox
            // 
            this.langSelectComboBox.FormattingEnabled = true;
            this.langSelectComboBox.Location = new System.Drawing.Point(12, 49);
            this.langSelectComboBox.Name = "langSelectComboBox";
            this.langSelectComboBox.Size = new System.Drawing.Size(121, 21);
            this.langSelectComboBox.TabIndex = 7;
            // 
            // langSelectLabel
            // 
            this.langSelectLabel.AutoSize = true;
            this.langSelectLabel.Location = new System.Drawing.Point(12, 24);
            this.langSelectLabel.Name = "langSelectLabel";
            this.langSelectLabel.Size = new System.Drawing.Size(106, 13);
            this.langSelectLabel.TabIndex = 8;
            this.langSelectLabel.Text = "Selected Language: ";
            // 
            // englishErrorLabel
            // 
            this.englishErrorLabel.AutoSize = true;
            this.englishErrorLabel.Location = new System.Drawing.Point(125, 196);
            this.englishErrorLabel.Name = "englishErrorLabel";
            this.englishErrorLabel.Size = new System.Drawing.Size(35, 13);
            this.englishErrorLabel.TabIndex = 9;
            this.englishErrorLabel.Text = "label3";
            // 
            // otherLangErrorLabel
            // 
            this.otherLangErrorLabel.AutoSize = true;
            this.otherLangErrorLabel.Location = new System.Drawing.Point(125, 233);
            this.otherLangErrorLabel.Name = "otherLangErrorLabel";
            this.otherLangErrorLabel.Size = new System.Drawing.Size(35, 13);
            this.otherLangErrorLabel.TabIndex = 10;
            this.otherLangErrorLabel.Text = "label4";
            // 
            // selectedLanguageLabel
            // 
            this.selectedLanguageLabel.AutoSize = true;
            this.selectedLanguageLabel.Location = new System.Drawing.Point(114, 24);
            this.selectedLanguageLabel.Name = "selectedLanguageLabel";
            this.selectedLanguageLabel.Size = new System.Drawing.Size(0, 13);
            this.selectedLanguageLabel.TabIndex = 11;
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Location = new System.Drawing.Point(202, 9);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(73, 13);
            this.titleLabel.TabIndex = 12;
            this.titleLabel.Text = "Customer App";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 342);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.selectedLanguageLabel);
            this.Controls.Add(this.otherLangErrorLabel);
            this.Controls.Add(this.englishErrorLabel);
            this.Controls.Add(this.langSelectLabel);
            this.Controls.Add(this.langSelectComboBox);
            this.Controls.Add(this.passwordLabel);
            this.Controls.Add(this.usernameLabel);
            this.Controls.Add(this.passwordTextBox);
            this.Controls.Add(this.usernameTextBox);
            this.Controls.Add(this.loginButton);
            this.Name = "LoginForm";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button loginButton;
        private System.Windows.Forms.TextBox usernameTextBox;
        private System.Windows.Forms.TextBox passwordTextBox;
        private System.Windows.Forms.Label usernameLabel;
        private System.Windows.Forms.Label passwordLabel;
        private System.Windows.Forms.ComboBox langSelectComboBox;
        private System.Windows.Forms.Label langSelectLabel;
        private System.Windows.Forms.Label englishErrorLabel;
        private System.Windows.Forms.Label otherLangErrorLabel;
        private System.Windows.Forms.Label selectedLanguageLabel;
        private System.Windows.Forms.Label titleLabel;
    }
}

