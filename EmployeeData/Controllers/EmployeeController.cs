using EmployeeData.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EmployeeData.Controllers
{
    public class EmployeeController : Controller
    {

        // GET: Employee
        public ActionResult Index()
        {
            List<EmployeeModel> employees = this.GetEmployee();

            return View("~/Views/Employee/Index.cshtml", employees);
        }
        public List<EmployeeModel> GetEmployee()
        {
            EmployeeModel employeeModel = new EmployeeModel();
            List<EmployeeModel> employeeModels = employeeModel.Employees();

            return employeeModels;
        }
        [HttpGet]
        public ActionResult HelloPage()
        {
            return View("~/Views/Product/MyName.cshtml");
        }

        [HttpGet]
        public ActionResult Create()
        {
            EmployeeModel employeeModel = new EmployeeModel();
            employeeModel.Country = employeeModel.GetCountry();
            EmployeeRole role = new EmployeeRole();
            employeeModel.Role = role.GetEmployeeRole();
            return View(employeeModel);
        }

        [HttpPost]
        public ActionResult Create(EmployeeModel employee)
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
                SqlParameter parameterEmail = new SqlParameter();
                parameterEmail.ParameterName = "@EmailId";
                parameterEmail.Value = employee.EmailId;
                cmd.Parameters.Add(parameterEmail);
                SqlParameter parameterAdd = new SqlParameter();
                parameterAdd.ParameterName = "@Address";
                parameterAdd.Value = employee.Address;
                cmd.Parameters.Add(parameterAdd);
                SqlParameter parameterGndr = new SqlParameter();
                parameterGndr.ParameterName = "@Gender";
                parameterGndr.Value = employee.EmployeeGender;
                cmd.Parameters.Add(parameterGndr);
                SqlParameter parameterAct = new SqlParameter();
                parameterAct.ParameterName = "@Active";
                parameterAct.Value = employee.Active;
                cmd.Parameters.Add(parameterAct);
                SqlParameter paraCountry = new SqlParameter();
                paraCountry.ParameterName = "@CountryId";
                paraCountry.Value = employee.CountryId;
                cmd.Parameters.Add(paraCountry);
                SqlParameter sqlRole = new SqlParameter();
                sqlRole.ParameterName = "@RoleId";
                sqlRole.Value = employee.RoleId;
                cmd.Parameters.Add(sqlRole);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            EmployeeModel emp = new EmployeeModel();
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;
           
            DataTable dataTable = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = @"Select * From tbl_UserDetails where Id = @Id";
                SqlDataAdapter sqlData = new SqlDataAdapter(query, conn);
                sqlData.SelectCommand.Parameters.AddWithValue("Id", id);
                sqlData.Fill(dataTable);

            }
            emp.Id = Convert.ToInt32(dataTable.Rows[0][0].ToString());
            emp.Name = dataTable.Rows[0][1].ToString();
            emp.EmailId = dataTable.Rows[0][2].ToString();
            emp.Address = dataTable.Rows[0][3].ToString();
            emp.EmployeeGender = dataTable.Rows[0][4].ToString();
            emp.Country = emp.GetCountry();
            emp.CountryId = Convert.ToInt32(dataTable.Rows[0][5].ToString());
            emp.Active = Convert.ToBoolean(dataTable.Rows[0][6]);
            return View(emp);

        }
        [HttpPost]
        public ActionResult Edit(int id, EmployeeModel model)
        {
           
                EmployeeModel employee = new EmployeeModel();
                employee.Id = id;
                employee.Edit(model);
                return RedirectToAction("Index");
           
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            EmployeeModel employee = new EmployeeModel();
            employee.Delete(id);

            return RedirectToAction("Index");
        }



    }
}