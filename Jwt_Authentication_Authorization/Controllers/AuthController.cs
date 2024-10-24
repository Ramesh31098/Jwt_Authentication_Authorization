using Jwt_Authentication_Authorization.Interfaces;
using Jwt_Authentication_Authorization.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Jwt_Authentication_Authorization.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
         private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        // GET: api/<AuthController>
        [HttpPost("Login")]
        public string Login( [FromBody] LoginRequest obj)
        {
            var token = _authService.Login(obj);
            return token;   
        }

        // GET api/<AuthController>/5

      


        [HttpPost("AddUser")]
        public User AddUser([FromBody] User  user)
        {
             var addedUser= _authService.AddUser(user);

            return addedUser;
        }

       

        // PUT api/<AuthController>/5
        [HttpPost("AddRole")]
        public Role AddRole( [FromBody] Role role)
        {
             var addedRole=_authService.AddRole(role);
             return addedRole;
        }

        // DELETE api/<AuthController>/5
        [HttpPost("AssignRole")]
        public bool AssignRoleToUser([FromBody]AddUserRole userRole)
        {
            var addedUserRole=_authService.AssignRoleToUser(userRole);
            return addedUserRole;   
        }
    }
}
