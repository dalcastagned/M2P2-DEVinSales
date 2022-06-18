using DevInSales.Core.Data.Context;
using DevInSales.EFCoreApi.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DevInSales.Core.Entities
{
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public UserService(
            DataContext context,
            UserManager<User> userManager,
            SignInManager<User> signInManager
        )
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IdentityResult> SignUp(User user, string password) =>
            await _userManager.CreateAsync(user, password);

        public async Task<SignInResult> Login(User user, string password) =>
            await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);

        public async Task<User> GetUser(string email) => await _userManager.FindByEmailAsync(email);

        public async Task<User> GetById(int id) => await _userManager.FindByIdAsync(id.ToString());

        public async Task<IdentityResult> RemoveUser(User user) =>
            await _userManager.DeleteAsync(user);

        public async Task<IEnumerable<User>> GetUsers(
            string? name,
            string? DataMin,
            string? DataMax
        )
        {
            var query = _userManager.Users.AsQueryable();
            if (!string.IsNullOrEmpty(name))
                query = query.Where(p => p.UserName.ToUpper().Contains(name.ToUpper()));
            if (!string.IsNullOrEmpty(DataMin))
                query = query.Where(p => p.BirthDate >= DateTime.Parse(DataMin));
            if (!string.IsNullOrEmpty(DataMax))
                query = query.Where(p => p.BirthDate <= DateTime.Parse(DataMax));

            return await query.ToListAsync();
        }
    }
}
