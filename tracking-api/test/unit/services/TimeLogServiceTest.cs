using Moq;

using tracking_app.Dto;
using tracking_app.Models;
using tracking_app.Repository;
using tracking_app.Services;

namespace tracking_app.test.unit.services
{
    [TestFixture]
    public class TimeLogServiceTests
    {
        private Mock<ITimeLogRepository> _timeLogRepositoryMock;
        private ITimeLogService _timeLogService;

        [SetUp]
        public void SetUp()
        {
            _timeLogRepositoryMock = new Mock<ITimeLogRepository>();
            _timeLogService = new TimeLogService(_timeLogRepositoryMock.Object);
        }

        [Test]
        public async Task SaveTime_ShouldReturnFailure_WhenNoTimeInAndTimeOutExists()
        {
            // Arrange
            var timeLogDto = new TimeLogDto
            {
                UserId = Guid.NewGuid().ToString(),
                DateCreated = DateTime.Now,
                TimeIn = null,
                TimeOut = DateTime.Now.AddHours(1)
            };

            _timeLogRepositoryMock
                .Setup(repo => repo.GetByDate(It.IsAny<DateTime>(), It.IsAny<Guid>()))
                .ReturnsAsync((TimeLog?)null); // Simulate no log found

            // Act
            var result = await _timeLogService.SaveTime(timeLogDto);

            // Assert
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Please clock in before clocking out.", result.Message);
        }

        [Test]
        public async Task SaveTime_ShouldSaveSuccessfully_WhenValidTimeLogIsProvided()
        {
            // Arrange
            var timeLogDto = new TimeLogDto
            {
                UserId = Guid.NewGuid().ToString(),
                DateCreated = DateTime.Now,
                TimeIn = DateTime.Now.AddMinutes(-30),
                TimeOut = DateTime.Now
            };

            var existingTimeLog = new TimeLog
            {
                LogId = 1,
                UserId = Guid.Parse(timeLogDto.UserId),
                DateCreated = timeLogDto.DateCreated,
                TimeIn = timeLogDto.TimeIn
            };

            _timeLogRepositoryMock
                .Setup(repo => repo.GetByDate(It.IsAny<DateTime>(), It.IsAny<Guid>()))
                .ReturnsAsync(existingTimeLog); // Simulate existing log

            _timeLogRepositoryMock
                .Setup(repo => repo.SaveTimeLog(It.IsAny<TimeLog>()))
                .ReturnsAsync(true); // Simulate successful save

            // Act
            var result = await _timeLogService.SaveTime(timeLogDto);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Saved Successfully", result.Message);
        }

        [Test]
        public async Task SaveTime_ShouldCreateNewTimeLog_WhenNoExistingLogFound()
        {
            // Arrange
            var timeLogDto = new TimeLogDto
            {
                UserId = Guid.NewGuid().ToString(),
                DateCreated = DateTime.Now,
                TimeIn = DateTime.Now.AddMinutes(-30),
                TimeOut = DateTime.Now
            };

            _timeLogRepositoryMock
                .Setup(repo => repo.GetByDate(It.IsAny<DateTime>(), It.IsAny<Guid>()))
                .ReturnsAsync((TimeLog?)null); // Simulate no log found

            _timeLogRepositoryMock
                .Setup(repo => repo.SaveTimeLog(It.IsAny<TimeLog>()))
                .ReturnsAsync(true); // Simulate successful save

            // Act
            var result = await _timeLogService.SaveTime(timeLogDto);

            // Assert
            Assert.IsTrue(result.Success);
            Assert.AreEqual("Saved Successfully", result.Message);
        }

        [Test]
        public async Task GetCurrentLog_ShouldReturnNull_WhenNoLogExists()
        {
            // Arrange
            _timeLogRepositoryMock
                .Setup(repo => repo.GetByDate(It.IsAny<DateTime>(), It.IsAny<Guid>()))
                .ReturnsAsync((TimeLog?)null); // Simulate no log found

            // Act
            var result = await _timeLogService.GetCurrentLog(Guid.NewGuid(), DateTime.Now);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetUserLogs_ShouldReturnLogs_WhenLogsExist()
        {
            // Arrange
            var logs = new List<TimeLog>
            {
                new TimeLog
                {
                    LogId = 1,
                    UserId = Guid.NewGuid(),
                    DateCreated = DateTime.Now,
                    TimeIn = DateTime.Now.AddMinutes(-30),
                    TimeOut = DateTime.Now
                }
            };

            _timeLogRepositoryMock
                .Setup(repo => repo.GetUserLogs(It.IsAny<Guid>()))
                .ReturnsAsync(logs); // Simulate logs found

            // Act
            var result = await _timeLogService.GetUserLogs(Guid.NewGuid());

            // Assert
            Assert.IsNotEmpty(result);
            Assert.AreEqual(logs.Count, result.Count());
        }
    }
}
