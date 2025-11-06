using Microsoft.EntityFrameworkCore;
using UserStudentMgmt.Domain.Entities;
using UserStudentMgmt.Domain.Interfaces;
using UserStudentMgmt.Infrastructure.Data;

namespace UserStudentMgmt.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(UserStudentMgmtDbContext context) : base(context) {}

        public async Task<User> GetByUserNameAsync(string userName)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.UserName == userName);
        }
    }
}