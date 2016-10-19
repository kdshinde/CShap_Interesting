using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;
using System.Data.SqlClient;

namespace LearnDotNet
{
    class Employee
    {
        public string LastName { get; set; }
        public int Salary { get; set; }
        public string Address { get; set; }
    }

    class Program
    {
        static void Main()
        {
            var emp = new Employee
            {
                LastName = "Shinde",
                Salary = 10,
                Address = "Boston"
            };

            var emptwo = new Employee
            {
                LastName = "Shinde",
                Salary = 20,
                Address = "Mumbai"
            };

            List<Employee> list = new List<Employee> { emp, emptwo };

            var properties = typeof(Employee).GetProperties();

            var dataTable = new DataTable();
            foreach (var info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (var entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    values[i] = properties[i].GetValue(entity);
                }

                dataTable.Rows.Add(values);
            }

            AddEmployee(dataTable);
        }


        public static void AddEmployee(DataTable dataTable)
        {
            using (var conn = new SqlConnection(
                            @"Data Source = (local); Initial Catalog = MyCompany; Integrated Security = True;"))
            {
                conn.Open();
                using (var cmd = new SqlCommand("AddEmployee"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = conn;
                    cmd.Parameters.AddWithValue("@EmpList", dataTable);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}


