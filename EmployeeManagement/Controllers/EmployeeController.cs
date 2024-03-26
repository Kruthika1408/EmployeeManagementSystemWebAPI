using EmployeeBLL;
using EmployeeManagement.Models;
using System;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : ApiController
    {
        private readonly EmployeeBusiness eb;
        public EmployeeController()
        {
            var dbcontext = new EmployeeDAL.EmployeeDBEntities();
            this.eb = new EmployeeBusiness(dbcontext) ?? throw new ArgumentNullException(nameof(eb));
        }

        // GET api/Employee
        [HttpGet]
        [Route("api/Employee")]
        public IHttpActionResult GetAllEmployee()
        {
            var emps = eb.GetAllEmployee().Select(emp => new EmployeeModel
            {
                EmpName = emp.EmpName,
                EmpCode = emp.EmpCode,
                Email = emp.Email,
                DeptCode = emp.DeptCode,
                DateOfBirth = emp.DateOfBirth
            })
            .ToList();

            return Ok(emps);
        }

        //GET api/Employee/101
        [HttpGet]
        [Route("api/Employee/{id}")]
        public IHttpActionResult GetEmployeeById(int id)
        {
            var emp = eb.GetEmployeeById(id);
            if (emp == null)
            {
                return BadRequest("Given ID data not exist");
            }
            var emps = new EmployeeModel
            {
                EmpName = emp.EmpName,
                EmpCode = emp.EmpCode,
                Email = emp.Email,
                DeptCode = emp.DeptCode,
                DateOfBirth = emp.DateOfBirth
            };
            return Ok(emps);
        }


        // POST api/Employee
        [HttpPost]
        [Route("api/Employee")]
        public IHttpActionResult AddEmployee([FromBody] EmployeeModel employee)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var emp = new EmployeeDTO
            {
                EmpName = employee.EmpName,
                Email = employee.Email,
                DeptCode = employee.DeptCode,
                DateOfBirth = employee.DateOfBirth
            };
            var result = eb.AddEmployee(emp);
            if (result!= null)
                return Content(HttpStatusCode.OK, "Employee added successfully!");
            else
                return BadRequest("Failed to add the employee. Please check your input or try again later.");
        }


        // PUT api/Employee/101
        [HttpPut]
        [Route("api/Employee/{empcode}")]
        public IHttpActionResult UpdateEmployee(int empcode, EmployeeModel empmodel)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var empDTO = new EmployeeDTO
            {
                EmpCode = empcode,
                EmpName = empmodel.EmpName,
                Email = empmodel.Email,
                DateOfBirth = (System.DateTime)empmodel.DateOfBirth,
                DeptCode = empmodel.DeptCode
            };


            eb.UpdateEmployee(empDTO);
            return Ok($"Employee with code {empcode} updated successfully");


        }


        // DELETE api/Employee/101
        [HttpDelete]
        [Route("api/Employee/{id}")]
        public IHttpActionResult DeleteTask(int id)
        {
            var emp = eb.GetEmployeeById(id);

            if (emp == null)
            {
                return BadRequest("Given ID data not exist to update");
            }

            eb.DeleteEmpProfile(id);
            try
            {
                return Content(HttpStatusCode.OK, "Employee Deleted successfully!");
            }
            catch
            {
                return BadRequest("Failed to delete the employee. Please check your input or try again later.");
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                eb.Dispose();
            }

            base.Dispose(disposing);
        }

    }
}
