using Microsoft.EntityFrameworkCore;
using tracking_app.Models;

namespace tracking_app.Repository
{
    public interface ITimeLogRepository
    {
        Task<bool> SaveTimeLog(TimeLog timeLog);
        Task<TimeLog?> GetByDate(DateTime dateTime, Guid userId);
        Task<IEnumerable<TimeLog>> GetUserLogs(Guid userId);
    }

    public class TimeLogRepository : ITimeLogRepository
    {
        private readonly TrackingAppDbContext db;
        public TimeLogRepository(TrackingAppDbContext db)
        {
            this.db = db;
        }

        public async Task<TimeLog?> GetByDate(DateTime dateTime, Guid userId)
        {
            return await db.TimeLogs
                .Include(x => x.User)
                .Where(x => x.DateCreated.Date == dateTime.Date && x.UserId == userId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<TimeLog>> GetUserLogs(Guid userId)
        {
            return await db.TimeLogs
                .Include(x => x.User)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.DateCreated)
                .ToListAsync();
        }

        public async Task<bool> SaveTimeLog(TimeLog timeLog)
        {
            var userTimeLog = await db.TimeLogs.FindAsync(timeLog.LogId);
            if (userTimeLog != null)
            {
                userTimeLog.TimeOut = timeLog.TimeOut;
            }
            else
            {
                userTimeLog = new()
                {
                    DateCreated = timeLog.DateCreated,
                    UserId = timeLog.UserId,
                    TimeIn = timeLog.TimeIn
                };
                await db.TimeLogs.AddAsync(userTimeLog);
            }

            var result = await db.SaveChangesAsync();
            return result > 0;
        }
    }
}