using TransactionApi.Models;

namespace TransactionApi.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<User> CreateAsync(User user);
        Task<User?> GetByIdAsync(int id);
    }
}