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
        private string connectionString = ConfigurationManager.ConnectionStrings["ClientScheduleDB"].ConnectionString;
        private int customerId;
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
                    string query = @"SELECT a.appointmentId, c.customerName, a.type, a.start, a.end
                             FROM appointment a
                             JOIN customer c ON a.customerId = c.customerId";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (cmbTimeZone.SelectedItem is TimeZoneInfo selectedTimeZone)
                    {
                        foreach (DataRow row in table.Rows)
                        {
                            DateTime utcStart = (DateTime)row["start"];
                            DateTime utcEnd = (DateTime)row["end"];
                            row["start"] = TimeZoneInfo.ConvertTimeFromUtc(utcStart, selectedTimeZone);
                            row["end"] = TimeZoneInfo.ConvertTimeFromUtc(utcEnd, selectedTimeZone);
                        }
                    }

                    dgvAppointments.DataSource = table;
                    dgvAppointments.Columns["appointmentId"].Visible = false;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading appointment data: " + ex.Message);
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
            DateTime utcStart = TimeZoneInfo.ConvertTimeToUtc(start);
            DateTime utcEnd = TimeZoneInfo.ConvertTimeToUtc(end);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT COUNT(*) FROM appointment 
                             WHERE ((@utcStart < end AND @utcEnd > start) 
                             AND (@appointmentId IS NULL OR appointmentId != @appointmentId))";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@utcStart", utcStart);
                    cmd.Parameters.AddWithValue("@utcEnd", utcEnd);
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
            if (cmbTimeZone.SelectedItem is TimeZoneInfo selectedTimeZone)
            {
                DateTime utcStart = TimeZoneInfo.ConvertTimeToUtc(localStart, selectedTimeZone);
                DateTime utcEnd = TimeZoneInfo.ConvertTimeToUtc(localEnd, selectedTimeZone);

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
            else
            {
                MessageBox.Show("Please select a time zone.");
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

        private void btnClearAll_Click(object sender, EventArgs e)
        {
            try
            {
                var confirmation = MessageBox.Show("Are you sure you want to delete all customer, address, and appointment records?", "Confirm Delete", MessageBoxButtons.YesNo);
                if (confirmation == DialogResult.No) return;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    string deleteAppointmentQuery = "DELETE FROM appointment";
                    MySqlCommand deleteAppointmentCmd = new MySqlCommand(deleteAppointmentQuery, connection);
                    deleteAppointmentCmd.ExecuteNonQuery();
                    string deleteCustomerQuery = "DELETE FROM customer";
                    MySqlCommand deleteCustomerCmd = new MySqlCommand(deleteCustomerQuery, connection);
                    deleteCustomerCmd.ExecuteNonQuery();
                    string deleteAddressQuery = "DELETE FROM address";
                    MySqlCommand deleteAddressCmd = new MySqlCommand(deleteAddressQuery, connection);
                    deleteAddressCmd.ExecuteNonQuery();

                    MessageBox.Show("All customer, address, and appointment records have been deleted.");
                    LoadAppointments(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error clearing customer, address, and appointment records: " + ex.Message);
            }
        }
    }
}
