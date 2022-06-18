using Microsoft.AspNetCore.Identity;

namespace DevInSales.Core.Entities
{
    public class Role : IdentityRole<int>
    {
        public List<UserRole> UserRoles { get; set; }
    }
}
