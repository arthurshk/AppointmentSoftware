using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SoftwareC969
{
    public partial class AppointmentForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ClientScheduleDB"].ConnectionString; private int customerId;
        public AppointmentForm(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            PopulateTimeZoneComboBox();
            LoadCustomers();
            LoadAppointments();
        }
        private void LoadCustomers()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT customerId, customerName FROM customer";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        cmbCustomer.Items.Add(new { Text = reader["customerName"].ToString(), Value = reader["customerId"] });
                    }
                    cmbCustomer.DisplayMember = "Text";
                    cmbCustomer.ValueMember = "Value";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading customers: " + ex.Message);
                }
            }
        }
        private void LoadAppointments()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM appointment";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dgvAppointments.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading appointments: " + ex.Message);
                }
            }
        }

        private bool IsWithinBusinessHours(DateTime start, DateTime end)
        {
            TimeZoneInfo est = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime startEST = TimeZoneInfo.ConvertTime(start, est);
            DateTime endEST = TimeZoneInfo.ConvertTime(end, est);

            bool isBusinessHours = startEST.TimeOfDay >= TimeSpan.FromHours(9) &&
                                   endEST.TimeOfDay <= TimeSpan.FromHours(17) &&
                                   startEST.DayOfWeek != DayOfWeek.Saturday &&
                                   startEST.DayOfWeek != DayOfWeek.Sunday;

            return isBusinessHours;
        }
        private bool IsOverlappingAppointment(DateTime start, DateTime end, int? appointmentId = null)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT COUNT(*) FROM appointment 
                             WHERE ((@start < end AND @end > start) AND (@appointmentId IS NULL OR appointmentId != @appointmentId))";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@start", start);
                    cmd.Parameters.AddWithValue("@end", end);
                    cmd.Parameters.AddWithValue("@appointmentId", appointmentId.HasValue ? (object)appointmentId.Value : DBNull.Value);

                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error checking for overlapping appointments: " + ex.Message);
                    return false;
                }
            }
        }


        private void dgvAppointments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DateTime localStart = dtpStart.Value;
            DateTime localEnd = dtpEnd.Value;
            var customer = cmbCustomer.SelectedItem as dynamic;

            if (customer == null)
            {
                MessageBox.Show("Please select a customer.");
                return;
            }

            int customerId = customer.Value;
            string type = txtType.Text.Trim();
            if (string.IsNullOrEmpty(type) || !IsWithinBusinessHours(localStart, localEnd))
            {
                MessageBox.Show("Appointment must be within business hours (9 AM - 5 PM EST, Mon-Fri) and type must not be empty.");
                return;
            }

            if (IsOverlappingAppointment(localStart, localEnd))
            {
                MessageBox.Show("This appointment overlaps with an existing appointment.");
                return;
            }

            SaveAppointment(localStart, localEnd, customerId, type);
        }
        private void SaveAppointment(DateTime localStart, DateTime localEnd, int customerId, string type)
        {
            DateTime utcStart = TimeZoneInfo.ConvertTimeToUtc(localStart);
            DateTime utcEnd = TimeZoneInfo.ConvertTimeToUtc(localEnd);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO appointment (customerId, type, start, end) VALUES (@customerId, @type, @start, @end)";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@start", utcStart);
                    cmd.Parameters.AddWithValue("@end", utcEnd);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Appointment added successfully.");
                    LoadAppointments();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding appointment: " + ex.Message);
                }
            }
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment to update.");
                return;
            }

            int appointmentId = Convert.ToInt32(dgvAppointments.SelectedRows[0].Cells["appointmentId"].Value);
            DateTime start = dtpStart.Value;
            DateTime end = dtpEnd.Value;
            var customer = cmbCustomer.SelectedItem as dynamic;
            if (customer == null)
            {
                MessageBox.Show("Please select a customer.");
                return;
            }

            int customerId = customer.Value;
            string type = txtType.Text.Trim();

            if (string.IsNullOrEmpty(type) || !IsWithinBusinessHours(start, end))
            {
                MessageBox.Show("Appointment must be within business hours (9 AM - 5 PM EST, Mon-Fri) and type must not be empty.");
                return;
            }

            if (IsOverlappingAppointment(start, end, appointmentId))
            {
                MessageBox.Show("This appointment overlaps with an existing appointment.");
                return;
            }

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "UPDATE appointment SET customerId=@customerId, type=@type, start=@start, end=@end WHERE appointmentId=@appointmentId";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Parameters.AddWithValue("@type", type);
                    cmd.Parameters.AddWithValue("@start", start);
                    cmd.Parameters.AddWithValue("@end", end);
                    cmd.Parameters.AddWithValue("@appointmentId", appointmentId);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Appointment updated successfully.");
                    LoadAppointments();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating appointment: " + ex.Message);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvAppointments.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select an appointment to delete.");
                return;
            }

            int appointmentId = Convert.ToInt32(dgvAppointments.SelectedRows[0].Cells["appointmentId"].Value);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "DELETE FROM appointment WHERE appointmentId=@appointmentId";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@appointmentId", appointmentId);

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Appointment deleted successfully.");
                    LoadAppointments();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting appointment: " + ex.Message);
                }
            }
        }
        private void btnViewCalendar_Click(object sender, EventArgs e)
        {
            CalendarForm calendarForm = new CalendarForm(customerId);
            calendarForm.ShowDialog();
        }
        private void PopulateTimeZoneComboBox()
        {
            foreach (TimeZoneInfo timeZone in TimeZoneInfo.GetSystemTimeZones())
            {
                cmbTimeZone.Items.Add(timeZone);
            }
            cmbTimeZone.DisplayMember = "DisplayName";
        }

        private void btnViewReports2_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm();
            reportsForm.ShowDialog(); 
        }
    }
}
