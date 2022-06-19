using DevInSales.Core.Data.Dtos;
using DevInSales.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace DevInSales.EFCoreApi.Core.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUser(string email);
        Task<IdentityResult> SignUp(User user, string password);
        Task<SignInResult> Login(User user, string password);
        Task<User> GetById(int id);
        Task<IdentityResult> RemoveUser(User user);
        Task<IEnumerable<User>> GetUsers(string? name, string? DataMin, string? DataMax);
        Task<IdentityResult> ChangePassword(User user, string oldPassword, string newPassword);
        Task<IList<string>> GetRoles(User user);
        Task<IdentityResult> AddUserRole(User user, string role);
    }
}