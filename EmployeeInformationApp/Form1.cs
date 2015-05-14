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
using System.Windows.Forms.VisualStyles;

namespace EmployeeInformationApp
{
    public partial class EmployeeInformationUI : Form
    {
        public EmployeeInformationUI()
        {
            InitializeComponent();
        }

        string connectionString = "Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=UniversityStudentDB;Data Source=SHIPLU";
        private bool isUpdateMode = false;
        private int employeeId;
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

            if (isUpdateMode)
            {
                SqlConnection connection = new SqlConnection(connectionString);
                string query = "update  Employee set [First Name]= '" + employee.firstName + "',[Middle Name]= '" +
                               employee.middleName + "',[Last Name]= '" + employee.lastName + "', Address= '" +
                               employee.address + "', Email= '" + employee.email + "', Phone = '" + employee.phone +
                               "', Salary= '" + employee.salary + "' where ID = '" + employeeId + "'";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                int rowcount = command.ExecuteNonQuery();
                connection.Close();
                if (rowcount > 0)
                {
                    MessageBox.Show("Updated Successfully");
                    saveButton.Text = "Save";
                    employeeId = 0;
                    isUpdateMode = false;
                    emailTextBox.Enabled = true;
                    ShowAllInformation();

                }
                else
                {
                    MessageBox.Show("Updated failed!");
                }
            }
            else
            {


                if (IsEmailExists(employee.email))
                {
                    MessageBox.Show("Email already exists!");
                }
                else
                {
                    SqlConnection connection = new SqlConnection(connectionString);
                    string query = "Insert into Employee values('" + employee.firstName + "','" + employee.middleName +
                                   "','" +
                                   employee.lastName + "','" + employee.address + "','" + employee.email + "','" +
                                   employee.phone + "','" + employee.salary + "')";
                    SqlCommand command = new SqlCommand(query, connection);

                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Inserted Successfully");
                    ShowAllInformation();
                    connection.Close();
                    ClearAllTextBox();

                }

            }
        }

        private bool IsEmailExists(string email)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "select * from Employee where Email='" + email + "'";
            SqlCommand command =new SqlCommand(query,connection);

          connection.Open();
            bool isEmailExists = false;
           SqlDataReader reader=command.ExecuteReader();

            while (reader.Read())
            {
                isEmailExists = true;
                break;
                
            }
            reader.Close();
            connection.Close();
            return isEmailExists;
        }



        public void ShowAllInformation()
        {
            SqlConnection connection =new SqlConnection(connectionString);
            string query = "select * from Employee";
            SqlCommand command =new SqlCommand(query,connection);

            connection.Open();
            List<Employee> employeeInformationList = new List<Employee>();
           SqlDataReader reader= command.ExecuteReader();

            while (reader.Read())
            {
                Employee employee = new Employee();
                employee.id = Convert.ToInt32(reader["ID"].ToString());
                employee.firstName = reader["First Name"].ToString();
                employee.middleName = reader["Middle Name"].ToString();
                employee.lastName = reader["Last Name"].ToString();
                employee.address = reader["Address"].ToString();
                employee.email = reader["Email"].ToString();
                employee.phone = reader["Phone"].ToString();
                employee.salary = Convert.ToDouble(reader["Salary"].ToString());

                employeeInformationList.Add(employee);

            }
            reader.Close();
            connection.Close();
            LoadAllInformationIntoListView(employeeInformationList);
        }

        private void LoadAllInformationIntoListView(List<Employee> aEmployees)
        {
            showOutputListView.Items.Clear();

            foreach (Employee emp in aEmployees)
            {
                ListViewItem item =new ListViewItem();
                item.Text = emp.id.ToString(); 
                item.SubItems.Add(emp.firstName);
                item.SubItems.Add(emp.middleName);
                item.SubItems.Add(emp.lastName);
                item.SubItems.Add(emp.address);
                item.SubItems.Add(emp.email);
                item.SubItems.Add(emp.phone);
                item.SubItems.Add(Convert.ToDouble(emp.salary).ToString());

                showOutputListView.Items.Add(item);
            }
        }
        private void ClearAllTextBox()
        {
            firstNameTextBox.Clear();
            middleNameTextBox.Clear();
            lastNameTextBox.Clear();
            addressTextBox.Clear();
            emailTextBox.Clear();
            phoneTextBox.Clear();
            salaryTextBox.Clear();
        }

        private void EmployeeInformationUI_Load(object sender, EventArgs e)
        {
            ShowAllInformation();
        }

        private Employee GetEmployeeById(int id)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "select * from Employee where ID ='" + id + "' ";
            SqlCommand command = new SqlCommand(query, connection);

            connection.Open();
            SqlDataReader reader = command.ExecuteReader();

            List<Employee> employeeIdList = new List<Employee>();

            while (reader.Read())
            {
                Employee employee = new Employee();
                employee.id = Convert.ToInt32(reader["ID"].ToString());
                employee.firstName = reader["first Name"].ToString();
                employee.middleName = reader["Middle Name"].ToString();
                employee.lastName = reader["Last Name"].ToString();
                employee.address = reader["Address"].ToString();
                employee.email = reader["Email"].ToString();
                employee.phone = reader["Phone"].ToString();
                employee.salary = Convert.ToDouble(reader["Salary"].ToString());

                employeeIdList.Add(employee);
            }
            reader.Close();
            connection.Close();
            return employeeIdList.SingleOrDefault();
        }

       

        private void showOutputListView_SelectedIndexChanged(object sender, EventArgs e)
        {
          
            ListViewItem item = showOutputListView.SelectedItems[0];
            int id = Convert.ToInt32(item.Text);
            Employee employee = GetEmployeeById(id);
            if (employee!=null)
            {
                isUpdateMode = true;
                saveButton.Text = "Update";
                emailTextBox.Enabled = false;
                employeeId= employee.id;
                firstNameTextBox.Text = employee.firstName;
                middleNameTextBox.Text = employee.middleName;
                lastNameTextBox.Text = employee.lastName;
                addressTextBox.Text = employee.address;
                emailTextBox.Text = employee.email;
                phoneTextBox.Text = employee.phone;
                salaryTextBox.Text = (employee.salary).ToString();

            }

        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            SqlConnection connection = new SqlConnection(connectionString);
            string query = "delete from Employee where ID = '" + employeeId + "'";
            SqlCommand command= new SqlCommand(query,connection);

            connection.Open();
            
            if (command.ExecuteNonQuery()>0)
            {
                MessageBox.Show("Deleted Successfully");
                ShowAllInformation();
                ClearAllTextBox();
                employeeId = 0;
               saveButton.Text = "Save";
                emailTextBox.Enabled = true;
                isUpdateMode = false;
            }
            else
            {
                MessageBox.Show("Not found");
            }
        }


    }
}
