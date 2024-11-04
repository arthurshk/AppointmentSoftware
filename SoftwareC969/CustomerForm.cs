using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SoftwareC969
{
    public partial class CustomerForm : Form
    {
        private string connectionString = "Server=localhost;Port=3306;Database=client_schedule;Uid=test;Pwd=test;";
        public CustomerForm()
        {
            InitializeComponent();
            LoadCustomerData();
        }
        private void LoadCustomerData()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT * FROM customer";
                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    dgvCustomers.DataSource = table;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading customer data: " + ex.Message);
                }
            }
        }
        private bool ValidateCustomerData()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text.Trim()) ||
                string.IsNullOrWhiteSpace(txtAddress.Text.Trim()) ||
                string.IsNullOrWhiteSpace(txtPhone.Text.Trim()))
            {
                MessageBox.Show("Name, Address, and Phone fields must not be empty.");
                return false;
            }
            if (!Regex.IsMatch(txtPhone.Text, @"^[\d-]+$"))
            {
                MessageBox.Show("Phone number can only contain digits and dashes.");
                return false;
            }
            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            {
                if (ValidateCustomerData())
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        try
                        {
                            connection.Open();
                            string query = "INSERT INTO customer (customerName, address, phone) VALUES (@name, @address, @phone)";
                            MySqlCommand cmd = new MySqlCommand(query, connection);
                            cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                            cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                            cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());

                            cmd.ExecuteNonQuery();
                            MessageBox.Show("Customer added successfully.");
                            LoadCustomerData(); 
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error adding customer: " + ex.Message);
                        }
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateCustomerData() && dgvCustomers.SelectedRows.Count > 0)
            {
                int customerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["customerId"].Value);

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "UPDATE customer SET customerName=@name, address=@address, phone=@phone WHERE customerId=@customerId";
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                        cmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@customerId", customerId);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Customer updated successfully.");
                        LoadCustomerData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating customer: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a customer to update.");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count > 0)
            {
                int customerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["customerId"].Value);

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        string query = "DELETE FROM customer WHERE customerId=@customerId";
                        MySqlCommand cmd = new MySqlCommand(query, connection);
                        cmd.Parameters.AddWithValue("@customerId", customerId);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Customer deleted successfully.");
                        LoadCustomerData(); 
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting customer: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a customer to delete.");
            }
        }

        private void btnAppointments_Click(object sender, EventArgs e)
        {
            if (dgvCustomers.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a customer to view appointments.");
                return;
            }

            int customerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["customerId"].Value);

            AppointmentForm appointmentForm = new AppointmentForm(customerId);
            appointmentForm.ShowDialog();
        }
    }
}
