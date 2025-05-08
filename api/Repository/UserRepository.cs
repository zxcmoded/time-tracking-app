using Microsoft.EntityFrameworkCore;
using tracking_app.Models;

namespace tracking_app.Repository
{
    public interface IUserRepository
    {
        Task<User?> GetByUserName(string userName);
    }

    public class UserRepository : IUserRepository
    {
        private readonly TrackingAppDbContext db;
        public UserRepository(TrackingAppDbContext db)
        {
            this.db = db;
        }

        public async Task<User?> GetByUserName(string userName)
        {
            return await db.Users
                .Where(x => x.Username == userName)
                .FirstOrDefaultAsync();
        }
    }
}