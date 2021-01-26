using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel;

namespace EmployeeData.Models
{
    public class EmployeeModel
    {
        public int Id { get; set; }

        [DisplayName("Name *")]
        public string Name { get; set; }
        public string Address { get; set; }
        [DisplayName("Email *")]
        public string EmailId { get; set; }
        public string EmployeeGender { get; set; }
        public int CountryId { get; set; }
        public string CountriesName { get; set; }
        public List<CountryModel> Country { get; set; }
        public bool Active { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public List<EmployeeRole> Role { get; set; }

       
        public List<CountryModel> GetCountry()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            List<CountryModel> countries = new List<CountryModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("sp_GetCountry", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();

                while (rdr.Read())
                {
                    CountryModel getcountries = new CountryModel();

                    getcountries.CountryId = Convert.ToInt32(rdr["CountryId"]);
                    getcountries.CountryName = rdr["CountryName"].ToString();
                    countries.Add(getcountries);
                }

            }
            return countries;
        }
        public List<EmployeeModel> Employees()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            List<EmployeeModel> employees = new List<EmployeeModel>();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("spGetEmployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    EmployeeModel employee = new EmployeeModel();
                    employee.Id = Convert.ToInt32(rdr["Id"]);
                    employee.Name = rdr["Name"].ToString();
                    employee.EmailId = rdr["EmailId"].ToString();
                    employee.Address = rdr["Address"].ToString();
                    employee.Country = GetCountry();
                    employee.EmployeeGender = rdr["Gender"].ToString();
                    employee.CountriesName = rdr["CountryName"].ToString();
                    employee.Active = Convert.ToBoolean(rdr["Active"]);
                    employee.RoleName = rdr["RoleName"].ToString();
                    employees.Add(employee);
                }
            }
            return employees;

        }

        public void AddEmployee(EmployeeModel employee)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_AddEmployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                SqlParameter parameter = new SqlParameter();
                parameter.ParameterName = "@Name";
                parameter.Value = employee.Name;
                cmd.Parameters.Add(parameter);
                parameter.ParameterName = "@EmailId";
                parameter.Value = employee.EmailId;
                cmd.Parameters.Add(parameter);
                parameter.ParameterName = "@Address";
                parameter.Value = employee.Address;
                cmd.Parameters.Add(parameter);
                parameter.ParameterName = "@Gender";
                parameter.Value = employee.EmployeeGender;
                cmd.Parameters.Add(parameter);
                parameter.ParameterName = "@Country";
                parameter.Value = employee.Country;
                cmd.Parameters.Add(parameter);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool Edit(EmployeeModel model)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_UpdateEmployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", model.Id);
                cmd.Parameters.AddWithValue("@Name", model.Name);
                cmd.Parameters.AddWithValue("@EmailId", model.EmailId);
                cmd.Parameters.AddWithValue("@Address", model.Address);
                cmd.Parameters.AddWithValue("@Gender", model.EmployeeGender);
                cmd.Parameters.AddWithValue("@Active", model.Active);
                cmd.Parameters.AddWithValue("@CountryId", model.CountryId);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                conn.Close();

                if (i >= 1)
                    return true;
                else
                    return false;
            }
        }
        public bool Delete(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_DeleteEmployee", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", id);

                conn.Open();
                int i = cmd.ExecuteNonQuery();
                conn.Close();

                if (i >= 1)
                    return true;
                else
                    return false;
            }

        }

        //public int GetEmployeeById(int id)
        //{
        //    string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
        //    List<EmployeeModel> employees = new List<EmployeeModel>();
        //    using (SqlConnection conn = new SqlConnection(connectionString))
        //    {
        //        SqlCommand cmd = new SqlCommand("sp_GetEmployeeById", conn);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Id", id);
        //        conn.Open();
        //        SqlDataReader rdr = cmd.ExecuteReader();
        //        while (rdr.Read())
        //        {
        //            EmployeeModel employee = new EmployeeModel();
        //            employee.Id = id;
        //            employee.Name = rdr["Name"].ToString();
        //            employee.EmailId = rdr["EmailId"].ToString();
        //            employee.Address = rdr["Address"].ToString();
        //            employees.Add(employee);
        //        }
        //    }
        //    return ;
        //}
    }
}