using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace SoftwareC969
{
    public partial class CalendarForm : Form
    {
        private int customerId;
        private string connectionString = ConfigurationManager.ConnectionStrings["ClientScheduleDB"].ConnectionString;
        public CalendarForm(int customerId)
        {
            InitializeComponent();
            this.customerId = customerId;
            monthCalendar.DateChanged += monthCalendar_DateChanged;
            LoadAppointmentsForDate(DateTime.Now); 
        }

        private void monthCalendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            DateTime selectedDate = monthCalendar.SelectionStart;
            LoadAppointmentsForDate(selectedDate);
        }
        private void LoadAppointmentsForDate(DateTime date)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = @"SELECT appointmentId, customerId, type, start, end 
                             FROM appointment 
                             WHERE customerId = @customerId 
                             AND DATE(start) = @date";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@customerId", customerId);
                    cmd.Parameters.AddWithValue("@date", date.Date);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    TimeZoneInfo userTimeZone = TimeZoneInfo.Local; 

                    foreach (DataRow row in table.Rows)
                    {
                        DateTime utcStart = (DateTime)row["start"];
                        DateTime utcEnd = (DateTime)row["end"];
                        DateTime localStart = TimeZoneInfo.ConvertTimeFromUtc(utcStart, userTimeZone);
                        DateTime localEnd = TimeZoneInfo.ConvertTimeFromUtc(utcEnd, userTimeZone);
                        row["start"] = localStart;
                        row["end"] = localEnd;
                    }

                    dgvDailyAppointments.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading appointments for the selected date: " + ex.Message);
                }
            }
        }

        private void btnViewReports3_Click(object sender, EventArgs e)
        {
            ReportsForm reportsForm = new ReportsForm();
            reportsForm.ShowDialog();
        }
    }
}
