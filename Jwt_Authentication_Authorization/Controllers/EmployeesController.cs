using Jwt_Authentication_Authorization.Interfaces;
using Jwt_Authentication_Authorization.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jwt_Authentication_Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   //[Authorize(Roles ="Admin")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeesController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        // GET: api/<EmployeesController>
        [HttpGet]
        [Authorize(Roles ="User")]
        public List<Employee> GetEmployee()
        {
            return _employeeService.GetEmployeeDetails();

        }

       

        // POST api/<EmployeesController>
        [HttpPost]
        [Authorize(Roles ="Admin,User")]
        public Employee AddEmployee([FromBody] Employee emp)
        {

            var employe= _employeeService.AddEmployee(emp); 
                return employe;
        }

      

       
    }
}
