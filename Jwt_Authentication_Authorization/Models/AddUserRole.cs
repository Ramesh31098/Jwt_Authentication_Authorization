namespace Jwt_Authentication_Authorization.Models
{
    public class AddUserRole
    {
        public int Userid { get; set; } 
        public List<int> RoleIds { get; set; }
    }
}
