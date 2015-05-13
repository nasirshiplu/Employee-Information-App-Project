using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeInformationApp
{
    public partial class EmployeeInformationUI : Form
    {
        public EmployeeInformationUI()
        {
            InitializeComponent();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
           Employee employee = new Employee();
            employee.firstName = firstNameTextBox.Text;
            employee.middleName = middleNameTextBox.Text;
            employee.lastName = lastNameTextBox.Text;
            employee.address = addressTextBox.Text;
            employee.email = emailTextBox.Text;
            employee.phone = phoneTextBox.Text;
            employee.salary = Convert.ToDouble(salaryTextBox.Text);

            string connectionString = @"SERVER=.\SQLEXPRESS; DATABASE=EmployeeDB; Integrated Security= True";
            SqlConnection connection= new SqlConnection(connectionString);
            string query = "Insert into Employee values('" + employee.firstName + "','" + employee.middleName + "','" +
                           employee.lastName + "','" + employee.address + "','" + employee.email + "','" +
                           employee.phone + "','" + employee.salary + "')";
            SqlCommand command =new SqlCommand(query,connection);

            connection.Open();
            command.ExecuteNonQuery();
            MessageBox.Show("Inserted Successfully");
            connection.Close();
            
        }

        public string ClearAllTextBox()
        {
            firstNameTextBox.Clear();
            middleNameTextBox.Clear();
            lastNameTextBox.
        }



    }
}
