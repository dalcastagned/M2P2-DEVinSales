using Microsoft.AspNetCore.Identity;

namespace DevInSales.Core.Entities
{
    public class User : IdentityUser<int>
    {
        public List<UserRole> UserRoles { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string PasswordExpired { get; set; }
    }
}
