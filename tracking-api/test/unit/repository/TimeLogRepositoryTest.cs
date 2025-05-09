using Microsoft.EntityFrameworkCore;
using tracking_app.Models;
using tracking_app.Repository;

namespace tracking_app.test.unit.repository;

public class TimeLogRepositoryTest
{
    private DbContextOptions<TrackingAppDbContext> _dbContextOptions;
    private TrackingAppDbContext _dbContext;
    private TimeLogRepository _timeLogRepository;

    [SetUp]
    public void SetUp()
    {
        // Use an in-memory database for testing
        _dbContextOptions = new DbContextOptionsBuilder<TrackingAppDbContext>()
            .UseInMemoryDatabase("TestDatabase") // Ensures each test uses a unique in-memory DB
            .Options;
        _dbContext = new TrackingAppDbContext(_dbContextOptions);
        _timeLogRepository = new TimeLogRepository(_dbContext);
    }

    [Test]
    public async Task SaveTimeLog_ShouldSaveNewTimeLog()
    {
        var log = new TimeLog
        {
            LogId = 1,
            DateCreated = DateTime.UtcNow,
            UserId = Guid.NewGuid(),
            TimeIn = DateTime.UtcNow
        };

        var result = await _timeLogRepository.SaveTimeLog(log);

        Assert.IsTrue(result);
        Assert.AreEqual(1, _dbContext.TimeLogs.Count());
    }

    [Test]
    public async Task GetByDate_ShouldReturnCorrectLog()
    {
        var userId = Guid.NewGuid();
        var log = new TimeLog
        {
            LogId = 1,
            DateCreated = DateTime.Today,
            UserId = userId,
            TimeIn = DateTime.Now
        };

        _dbContext.TimeLogs.Add(log);
        await _dbContext.SaveChangesAsync();

        var result = await _timeLogRepository.GetByDate(DateTime.Today, userId);

        Assert.IsNotNull(result);
        Assert.AreEqual(userId, result.UserId);
    }

    [Test]
    public async Task GetUserLogs_ShouldReturnLogsForUser()
    {
        var userId = Guid.NewGuid();

        _dbContext.TimeLogs.AddRange(new[]
        {
                new TimeLog { LogId = 1, UserId = userId, DateCreated = DateTime.Today.AddDays(-1), TimeIn = DateTime.Now },
                new TimeLog { LogId = 2, UserId = userId, DateCreated = DateTime.Today, TimeIn = DateTime.Now },
                new TimeLog { LogId = 3, UserId = Guid.NewGuid(), DateCreated = DateTime.Today, TimeIn = DateTime.Now } // another user
            });

        await _dbContext.SaveChangesAsync();

        var result = await _timeLogRepository.GetUserLogs(userId);

        Assert.AreEqual(2, result.Count());
        Assert.IsTrue(result.All(r => r.UserId == userId));
    }
}