using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace SoftwareC969
{
    public partial class Form1 : Form
    {
        private readonly string logFilePath = "Login_History.txt";
        private string userLanguage;
        private Dictionary<string, string> messages;
        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lblErrorMessage.Visible = false;
            DetermineUserLocation();
            LoadMessages();
        }
        private void LogLoginAttempt(string username)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); 
            string logEntry = $"{timestamp} - {username} logged in";

            try
            {
                System.IO.File.AppendAllText(logFilePath, logEntry + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error logging the login attempt: " + ex.Message);
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (VerifyCredentials(username, password))
            {
                LogLoginAttempt(username);
                CheckUpcomingAppointments();
               MessageBox.Show(messages["welcome"]);
                CustomerForm customerForm = new CustomerForm();
                customerForm.Show();
                this.Hide();
            }
            else
            {
                lblErrorMessage.Text = messages["login_error"];
                lblErrorMessage.Visible = true;
            }
        }

        private bool VerifyCredentials(string username, string password)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ClientScheduleDB"].ConnectionString;
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT COUNT(1) FROM user WHERE userName = @username AND password = @password";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@username", username);
                    cmd.Parameters.AddWithValue("@password", password);

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result == 1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error connecting to database: " + ex.Message);
                    return false;
                }
            }
        }
        private void CheckUpcomingAppointments()
        {
         string connectionString = ConfigurationManager.ConnectionStrings["ClientScheduleDB"].ConnectionString;
        DateTime currentTimeUtc = DateTime.UtcNow;
            DateTime alertThresholdTime = currentTimeUtc.AddMinutes(15);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT * FROM appointment 
                             WHERE start >= @currentTime AND start <= @alertThresholdTime";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@currentTime", currentTimeUtc);
                    cmd.Parameters.AddWithValue("@alertThresholdTime", alertThresholdTime);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        StringBuilder alertMessage = new StringBuilder("You have an appointment within the next 15 minutes:\n");

                        while (reader.Read())
                        {
                            DateTime appointmentTimeUtc = reader.GetDateTime("start");
                            string type = reader["type"].ToString();
                            DateTime appointmentTimeLocal = TimeZoneInfo.ConvertTimeFromUtc(appointmentTimeUtc, TimeZoneInfo.Local);

                            alertMessage.AppendLine($"- {type} at {appointmentTimeLocal}");
                        }

                        MessageBox.Show(alertMessage.ToString(), "Upcoming Appointment Alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking for upcoming appointments: " + ex.Message);
                }
            }
        }
        private void DetermineUserLocation()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            RegionInfo regionInfo = new RegionInfo(currentCulture.Name);

            lblLocation.Text = $"Location detected: {regionInfo.EnglishName}";
        }

        private void LoadMessages()
        {
            messages = new Dictionary<string, string>();

            if (userLanguage == "es")
            {
                messages["login_error"] = "El nombre de usuario y la contraseña no coinciden.";
                messages["welcome"] = "¡Bienvenido!";
            }
            else
            {
                messages["login_error"] = "The username and password do not match.";
                messages["welcome"] = "Welcome!";
            }
        }

        private void cmbLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            userLanguage = cmbLanguage.SelectedIndex == 1 ? "es" : "en";
            LoadMessages(); 
        }
    }
}
