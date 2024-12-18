﻿using MySql.Data.MySqlClient;
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
                    string query = @"SELECT c.customerName, a.type, a.start, a.end
                             FROM appointment a
                             JOIN customer c ON a.customerId = c.customerId
                             WHERE DATE(a.start) = @date";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@date", date.Date);

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    if (table.Rows.Count == 0)
                    {
                        MessageBox.Show("No appointments found for this date.");
                    }

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
                    dgvDailyAppointments.Columns["customerName"].HeaderText = "Customer Name";
                    dgvDailyAppointments.Columns["type"].HeaderText = "Type";
                    dgvDailyAppointments.Columns["start"].HeaderText = "Start Time";
                    dgvDailyAppointments.Columns["end"].HeaderText = "End Time";
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
