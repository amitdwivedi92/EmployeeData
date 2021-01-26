using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EmployeeData.Models
{
    public class EmployeeRole
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool Active { get; set; }

        public List<EmployeeRole> GetEmployeeRole()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            List<EmployeeRole> employeeRole = new List<EmployeeRole>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetEmployeeRole", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EmployeeRole role = new EmployeeRole();
                    role.RoleId = Convert.ToInt32(rdr["RoleId"]);
                    role.RoleName = rdr["RoleName"].ToString();
                    role.Active = Convert.ToBoolean(rdr["Active"]);
                    employeeRole.Add(role);
                }

            }
            return employeeRole;
        }
    }
}