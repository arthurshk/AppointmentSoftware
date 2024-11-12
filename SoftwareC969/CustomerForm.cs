using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SoftwareC969
{
    public partial class CustomerForm : Form
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["ClientScheduleDB"].ConnectionString; public CustomerForm()
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
                    string query = @"SELECT c.customerId, c.customerName, a.addressId, a.address, a.phone, c.active 
                             FROM customer c
                             JOIN address a ON c.addressId = a.addressId";

                    MySqlCommand cmd = new MySqlCommand(query, connection);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);

                    dgvCustomers.DataSource = table;
                    dgvCustomers.Columns["addressId"].Visible = false; 
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
                MessageBox.Show("Name, Address, and Phone fields cannot be empty.");
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
            if (ValidateCustomerData())
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();
                        MySqlCommand getCityIdCmd = new MySqlCommand("SELECT cityId FROM city LIMIT 1", connection);
                        int cityId = Convert.ToInt32(getCityIdCmd.ExecuteScalar());
                        string addressQuery = "INSERT INTO address (address, address2, phone, cityId, postalCode, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                                              "VALUES (@address, @address2, @phone, @cityId, @postalCode, NOW(), 'Admin', NOW(), 'Admin')";
                        MySqlCommand addressCmd = new MySqlCommand(addressQuery, connection);
                        addressCmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                        addressCmd.Parameters.AddWithValue("@address2", string.Empty);
                        addressCmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                        addressCmd.Parameters.AddWithValue("@cityId", cityId);
                        addressCmd.Parameters.AddWithValue("@postalCode", string.Empty);

                        addressCmd.ExecuteNonQuery();

                        MySqlCommand getLastIdCmd = new MySqlCommand("SELECT LAST_INSERT_ID()", connection);
                        int addressId = Convert.ToInt32(getLastIdCmd.ExecuteScalar());

                        string customerQuery = "INSERT INTO customer (customerName, addressId, active, createDate, createdBy, lastUpdate, lastUpdateBy) " +
                                               "VALUES (@name, @addressId, @active, NOW(), 'Admin', NOW(), 'Admin')";
                        MySqlCommand customerCmd = new MySqlCommand(customerQuery, connection);
                        customerCmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                        customerCmd.Parameters.AddWithValue("@addressId", addressId);
                        customerCmd.Parameters.AddWithValue("@active", 1);

                        customerCmd.ExecuteNonQuery();
                        MessageBox.Show("Customer added successfully.");
                        LoadCustomerData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error adding customer: " + ex.Message + "\nStack Trace:\n" + ex.StackTrace);
                    }
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (ValidateCustomerData() && dgvCustomers.SelectedRows.Count > 0)
            {
                int customerId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["customerId"].Value);
                int addressId = Convert.ToInt32(dgvCustomers.SelectedRows[0].Cells["addressId"].Value);

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        string addressQuery = "UPDATE address SET address = @address, phone = @phone, lastUpdate = NOW(), lastUpdateBy = 'Admin' WHERE addressId = @addressId";
                        MySqlCommand addressCmd = new MySqlCommand(addressQuery, connection);
                        addressCmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
                        addressCmd.Parameters.AddWithValue("@phone", txtPhone.Text.Trim());
                        addressCmd.Parameters.AddWithValue("@addressId", addressId);

                        Console.WriteLine("Executing address update query...");
                        Console.WriteLine($"Address: {txtAddress.Text.Trim()}, Phone: {txtPhone.Text.Trim()}, AddressId: {addressId}");

                        int addressRowsAffected = addressCmd.ExecuteNonQuery();
                        if (addressRowsAffected == 0)
                        {
                            MessageBox.Show("Address update failed. Please ensure the addressId exists.");
                            return;
                        }

                        string customerQuery = "UPDATE customer SET customerName = @name, active = @active, lastUpdate = NOW(), lastUpdateBy = 'Admin' WHERE customerId = @customerId";
                        MySqlCommand customerCmd = new MySqlCommand(customerQuery, connection);
                        customerCmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                        customerCmd.Parameters.AddWithValue("@active", 1); 
                        customerCmd.Parameters.AddWithValue("@customerId", customerId);

                        Console.WriteLine("Executing customer update query...");
                        Console.WriteLine($"Name: {txtName.Text.Trim()}, Active: 1, CustomerId: {customerId}");

                        int customerRowsAffected = customerCmd.ExecuteNonQuery();
                        if (customerRowsAffected == 0)
                        {
                            MessageBox.Show("Customer update failed. Please ensure the customerId exists.");
                            return;
                        }

                        MessageBox.Show("Customer updated successfully.");
                        LoadCustomerData();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error updating customer: " + ex.Message + "\nStack Trace:\n" + ex.StackTrace);
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

        private void btnViewReports_Click(object sender, EventArgs e)
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

                    MessageBox.Show("All records have been deleted.");
                    LoadCustomerData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting records: " + ex.Message);
            }
        }
    }
}
