using MySql.Data.MySqlClient;
using SoftwareC969.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace SoftwareC969
{
    public partial class ReportsForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ClientScheduleDB"].ConnectionString;
        public ReportsForm()
        {
            InitializeComponent();
        }

        private void btnGenerateReports_Click(object sender, EventArgs e)
        {
            GenerateReport1();
            GenerateReport2();
            GenerateReport3();
        }
        private void GenerateReport1()
        {
            List<Appointment> appointments = GetAppointments();
            var reportData = appointments
                .GroupBy(a => new { Month = a.Start.Month, Type = a.Type })
                .Select(g => new
                {
                    Month = g.Key.Month,
                    Type = g.Key.Type,
                    Count = g.Count()
                })
                .OrderBy(r => r.Month)
                .ToList();

            dgvReport1.DataSource = reportData;
        }
        private void GenerateReport2()
        {
            List<Appointment> appointments = GetAppointments();
            List<Customer> customers = GetCustomers();

            var reportData = new List<dynamic>();

            foreach (var customer in customers)
            {
                reportData.Add(new
                {
                    CustomerName = customer.Name,
                    Type = "Customer Schedule",
                    Start = "",
                    End = ""
                });

                var customerAppointments = appointments
                    .Where(a => a.CustomerId == customer.CustomerId)
                    .OrderBy(a => a.Start)
                    .Select(a => new
                    {
                        CustomerName = "", 
                        Type = a.Type,
                        Start = a.Start.ToString("g"),
                        End = a.End.ToString("g")
                    })
                    .ToList();

                reportData.AddRange(customerAppointments);

                reportData.Add(new
                {
                    CustomerName = "",
                    Type = "",
                    Start = "",
                    End = ""
                });
            }

            dgvReport2.DataSource = reportData;
        }
        private void GenerateReport3()
        {
            List<Appointment> appointments = GetAppointments();
            List<Customer> customers = GetCustomers();

            var reportData = customers
                .Select(customer => new
                {
                    CustomerName = customer.Name,
                    AppointmentCount = appointments.Count(a => a.CustomerId == customer.CustomerId)
                })
                .OrderByDescending(r => r.AppointmentCount)
                .ToList();

            dgvReport3.DataSource = reportData;
        }
        private List<Appointment> GetAppointments()
        {
            List<Appointment> appointments = new List<Appointment>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT appointmentId, customerId, userId, type, start, end FROM appointment";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        appointments.Add(new Appointment
                        {
                            AppointmentId = reader["appointmentId"] != DBNull.Value ? reader.GetInt32("appointmentId") : 0,
                            CustomerId = reader["customerId"] != DBNull.Value ? reader.GetInt32("customerId") : 0,
                            UserId = reader["userId"] != DBNull.Value ? reader.GetInt32("userId") : 0,
                            Type = reader["type"] != DBNull.Value ? reader.GetString("type") : "Unknown",
                            Start = reader["start"] != DBNull.Value ? reader.GetDateTime("start") : DateTime.MinValue,
                            End = reader["end"] != DBNull.Value ? reader.GetDateTime("end") : DateTime.MinValue
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving appointments: " + ex.Message);
                }
            }

            return appointments;
        }
        private List<User> GetUsers()
        {
            List<User> users = new List<User>();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT userId, userName FROM user";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            UserId = reader["userId"] != DBNull.Value ? reader.GetInt32("userId") : 0,
                            Name = reader["userName"] != DBNull.Value ? reader.GetString("userName") : "Unknown"
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving users: " + ex.Message);
                }
            }

            return users;
        }


        private List<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();

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
                        customers.Add(new Customer
                        {
                            CustomerId = reader["customerId"] != DBNull.Value ? reader.GetInt32("customerId") : 0,
                            Name = reader["customerName"] != DBNull.Value ? reader.GetString("customerName") : "Unknown"
                        });
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error retrieving customers: " + ex.Message);
                }
            }

            return customers;
        }
    }

}
