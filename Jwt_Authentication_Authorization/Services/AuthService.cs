using Jwt_Authentication_Authorization.Context;
using Jwt_Authentication_Authorization.Interfaces;
using Jwt_Authentication_Authorization.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Jwt_Authentication_Authorization.Services
{
    public class AuthService : IAuthService
    {
        private readonly JwtContext _jwtContext;
        private readonly IConfiguration _configuration;

        public AuthService(JwtContext context, IConfiguration configuration)
        {
            _jwtContext = context;
            _configuration = configuration;
        }

        public Role AddRole(Role role)
        {
            var addedRole = _jwtContext.Roles.Add(role);
            _jwtContext.SaveChanges();
            return addedRole.Entity;
        }

        public User AddUser(User user)
        {
            var addedUser = _jwtContext.Add(user);
            _jwtContext.SaveChanges();
            return addedUser.Entity;
        }

        public bool AssignRoleToUser(AddUserRole obj)
        {
            try
            {
                var addRoles = new List<UserRole>();
                var user = _jwtContext.Users.SingleOrDefault(s => s.Id == obj.Userid);
                if (user == null)
                {
                    throw new Exception("User is not valid");
                }
                foreach (var role in obj.RoleIds)
                {
                    var userRole = new UserRole
                    {
                        RoleId = role,
                        UserId = user.Id
                    };
                    addRoles.Add(userRole);
                }
                _jwtContext.UserRoles.AddRange(addRoles);
                _jwtContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Add logging or additional exception handling here if needed
                return false;
            }
        }

        public string Login(LoginRequest loginRequest)
        {
            if (loginRequest.Username != null && loginRequest.Password != null)
            {
                var user = _jwtContext.Users.SingleOrDefault(s => s.Username == loginRequest.Username && s.Password == loginRequest.Password);
                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, _configuration["jwt:Subject"]),
                        new Claim("id", user.Id.ToString()), // Ensure this conversion
                        new Claim("Username", user.Username)
                    };

                    var userRoles = _jwtContext.UserRoles.Where(u => u.UserId == user.Id).ToList();
                    var rolesIds = userRoles.Select(u => u.RoleId).ToList();
                    var roles = _jwtContext.Roles.Where(r => rolesIds.Contains(r.Id)).ToList();

                    foreach (var role in roles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, role.Name));
                    }

                    var claimsIdentity = new ClaimsIdentity(claims);
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["jwt:Key"]));
                    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(
                        issuer: _configuration["jwt:Issuer"],
                        audience: _configuration["jwt:Audience"],
                        claims: claimsIdentity.Claims,
                        expires: DateTime.Now.AddMinutes(30),
                        signingCredentials: creds
                    );

                    return new JwtSecurityTokenHandler().WriteToken(token);
                }
                else
                {
                    throw new Exception("User is not valid");
                }
            }
            else
            {
                throw new Exception("Credentials are not valid");
            }
        }
    }
}
