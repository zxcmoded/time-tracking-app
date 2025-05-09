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
            DateCreated = DateTime.UtcNow,
            UserId = Guid.NewGuid(),
            TimeIn = DateTime.UtcNow
        };

        var result = await _timeLogRepository.SaveTimeLog(log);

        Assert.IsTrue(result);
    }

    [Test]
    public async Task GetByDate_ShouldReturnCorrectLog()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Username = "testuser",
            Name = "John Doe",
            DateCreated = DateTime.Now,
            Password = "123456"
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        var log = new TimeLog
        {
            DateCreated = DateTime.Now,
            UserId = userId,
            TimeIn = DateTime.Now
        };

        _dbContext.TimeLogs.Add(log);

        await _dbContext.SaveChangesAsync();

        var result = await _timeLogRepository.GetByDate(log.DateCreated, userId);

        Assert.IsNotNull(result);
        Assert.AreEqual(userId, result.UserId);
    }

    [Test]
    public async Task GetUserLogs_ShouldReturnLogsForUser()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Username = "testuser",
            Name = "John Doe",
            DateCreated = DateTime.Now,
            Password = "123456"
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        _dbContext.TimeLogs.AddRange(new[]
        {
                new TimeLog { UserId = userId, DateCreated = DateTime.Now, TimeIn = DateTime.Now },
        });

        await _dbContext.SaveChangesAsync();

        var result = await _timeLogRepository.GetUserLogs(userId);

        Assert.AreEqual(1, result.Count());
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up the in-memory database after each test
        _dbContext.Database.EnsureDeleted();
        _dbContext.Dispose();
    }
}