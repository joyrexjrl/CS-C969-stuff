using System;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using C969.Database;

namespace C969
{
    public partial class LoginForm : Form
    {
        string _userNameTest = "test";
        string _passwordTest = "test";
        string _filePath = "../../Resources/";
        RegionInfo _usersRegion = RegionInfo.CurrentRegion;
        bool _isWrongLoginInfo = false;

        public LoginForm()
        {
            InitializeComponent();

            englishErrorLabel.Text = "";
            otherLangErrorLabel.Text = "";

            if (DBConnection.IsConnectionLive)
            {
                langSelectComboBox.Enabled = false;
                langSelectLabel.Enabled = false;
                LanguageErrorDisplayOnline();                
            }
            else
            {
                langSelectComboBox.Enabled = true;
                langSelectLabel.Enabled = true;
                langSelectComboBox.Items.Add("English");
                langSelectComboBox.Items.Add("Spanish");
                langSelectComboBox.Items.Add("French");
                langSelectComboBox.Items.Add("German");
                langSelectComboBox.SelectedIndex = 0;
            }
            
            selectedLanguageLabel.Text = "English";
            langSelectComboBox.SelectedIndexChanged += LangSelectComboBox_SelectedIndexChanged;

            passwordTextBox.UseSystemPasswordChar = true;
            passwordTextBox.PasswordChar = '*';
        }

        void LangSelectComboBox_SelectedIndexChanged(object sender, EventArgs e) => LanguageErrorDisplayOffline();

        void LanguageErrorDisplayOffline()
        {
            string selectedLang = langSelectComboBox.SelectedItem.ToString();
            switch (selectedLang)
            {
                case "English":
                    otherLangErrorLabel.Text = "";
                    selectedLanguageLabel.Text = "English";
                    break;
                case "Spanish":
                    if(_isWrongLoginInfo)otherLangErrorLabel.Text = "El nombre de usuario o la contraseña son incorrectos.";
                    selectedLanguageLabel.Text = "Spanish";
                    break;
                case "French":
                    if (_isWrongLoginInfo) otherLangErrorLabel.Text = "Le nom d'utilisateur ou le mot de passe est incorrect.";
                    selectedLanguageLabel.Text = "French";
                    break;
                case "German":
                    if (_isWrongLoginInfo) otherLangErrorLabel.Text = "Benutzername oder Passwort sind falsch.";
                    selectedLanguageLabel.Text = "German";
                    break;
            }
        }

        void LanguageErrorDisplayOnline()
        {
            if (_usersRegion.Name == "DE")
            {
                titleLabel.Text = "Zeitplan-App";
                usernameLabel.Text = "Nutzername: ";
                passwordLabel.Text = "Passwort: ";
                otherLangErrorLabel.Text = "Benutzername oder Passwort falsch.";
                loginButton.Text = "Anmeldung";
            }
            else if (_usersRegion.Name == "MX")
            {
                titleLabel.Text = "Programar aplicación";
                usernameLabel.Text = "Nombre de usuario:";
                passwordLabel.Text = "Contraseña:";
                otherLangErrorLabel.Text = "Usuario o contraseña invalido.";
                loginButton.Text = "Acceso";
            }
        }

        void LoginButton_Click(object sender, EventArgs e)
        {
            string enteredUsername = usernameTextBox.Text.Trim();
            string enteredPassword = passwordTextBox.Text.Trim();

            if (DBConnection.CurrentMode == ConnectionMode.Offline)
            {
                // Offline mode: hardcoded test credentials
                if (enteredUsername.Equals(_userNameTest, StringComparison.OrdinalIgnoreCase) && enteredPassword == _passwordTest)
                {
                    DBConnection.UserName = _userNameTest;
                    DBConnection.UserId = 1;
                    LogLoginSuccess(isOffline: true);
                    OpenRecordsForm();
                }
                else LogFailedLogin(isOffline: true);
            }
            else if (DBConnection.CurrentMode == ConnectionMode.Online && DBConnection.IsConnectionLive)
            {
                if (DBConnection.LoginUser(enteredUsername, enteredPassword))
                {
                    DBConnection.UserName = enteredUsername;
                    LogLoginSuccess(isOffline: false);
                    OpenRecordsForm();
                }
                else LogFailedLogin(isOffline: false);
            }
            else englishErrorLabel.Text = "No available database connection.";
        }

        void OpenRecordsForm()
        {
            usernameTextBox.Clear();
            passwordTextBox.Clear();
            englishErrorLabel.Text = "";
            otherLangErrorLabel.Text = "";

            RecordsForm recordsForm = new RecordsForm();
            recordsForm.Show();
            Hide();
        }

        void LogLoginSuccess(bool isOffline)
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(_filePath, "userLog.txt"), true))
                {
                    string mode = isOffline ? "[OFFLINE]" : "[ONLINE]";
                    outputFile.WriteLine($"{mode} User {_userNameTest} logged in at {DateTime.Now}");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show($"{err.Message}");
            }
        }

        void LogFailedLogin(bool isOffline)
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter(Path.Combine(_filePath, "userLog.txt"), true))
                {
                    string mode = isOffline ? "[OFFLINE]" : "[ONLINE]";
                    outputFile.WriteLine($"{mode} Failed Login Attempt with {usernameTextBox.Text} at {DateTime.Now}");
                }
            }
            catch (Exception err)
            {
                MessageBox.Show($"{err.Message}");
            }

            if (_usersRegion.Name == "DE") otherLangErrorLabel.Text = "Benutzername oder Passwort stimmen nicht überein.";
            else if (_usersRegion.Name == "MX") otherLangErrorLabel.Text = "nombre de usuario o contraseña no coinciden";
            else otherLangErrorLabel.Text = "Username or Password do not match";

            otherLangErrorLabel.Visible = true;
            usernameTextBox.Text = "";
            passwordTextBox.Text = "";
        }
    }
}
