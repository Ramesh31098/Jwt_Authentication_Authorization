using Jwt_Authentication_Authorization.Context;
using Jwt_Authentication_Authorization.Interfaces;
using Jwt_Authentication_Authorization.Models;

namespace Jwt_Authentication_Authorization.Services
{
    public class EmployeeService : IEmployeeService

    {
        private readonly JwtContext _jwtContext;
        public EmployeeService(JwtContext  jwtContext)
        {
             _jwtContext = jwtContext;
        }
        public Employee AddEmployee(Employee employee)
        {
            var  emp = _jwtContext.Add(employee);   

            _jwtContext.SaveChanges();
            return emp.Entity;
        }

        public List<Employee> GetEmployeeDetails()
        {
            var emp= _jwtContext.Employees.ToList();    
            return emp;
        }
    }
}

