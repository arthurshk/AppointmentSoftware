using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace SoftwareC969
{
    public partial class Form1 : Form
    {
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


        private void btnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (VerifyCredentials(username, password))
            {
                CheckUpcomingAppointments();
                MessageBox.Show(messages["welcome"]);
                CustomerForm customerForm = new CustomerForm();
                customerForm.Show();
                this.Close();
            }
            else
            {
                lblErrorMessage.Text = messages["login_error"];
                lblErrorMessage.Visible = true;
            }
        }

        private bool VerifyCredentials(string username, string password)
        {
            string connectionString = "Server=localhost;Port=3306;Database=client_schedule;Uid=test;Pwd=test;";

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
            string connectionString = "Server=localhost;Port=3306;Database=client_schedule;Uid=test;Pwd=test;";

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
            CultureInfo culture = CultureInfo.CurrentCulture;
            userLanguage = culture.TwoLetterISOLanguageName;

            lblLocation.Text = $"Location detected: {culture.DisplayName}";
        }

        private void LoadMessages()
        {
            messages = new Dictionary<string, string>();

            if (userLanguage == "es") 
            {
                messages["login_error"] = "El nombre de usuario y la contraseña no coinciden.";
                messages["welcome"] = "Bienvenido!";
            }
            else 
            {
                messages["login_error"] = "The username and password do not match.";
                messages["welcome"] = "Welcome!";
            }
        }

    }
}
