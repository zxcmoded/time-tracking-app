using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using tracking_app.Controllers;
using tracking_app.Dto;
using tracking_app.Services;

namespace tracking_app.test.integration
{
    public class TimeLogControllerTests
    {
        private Mock<ITimeLogService> _mockTimeLogService;
        private Mock<ILogger<TimeLogController>> _mockLogger;
        private TimeLogController _controller;

        [SetUp]
        public void Setup()
        {
            _mockTimeLogService = new Mock<ITimeLogService>();
            _mockLogger = new Mock<ILogger<TimeLogController>>();
            _controller = new TimeLogController(_mockLogger.Object, _mockTimeLogService.Object);
        }

        [Test]
        public async Task GetUserTimeLogs_ShouldReturnLogs()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var logs = new List<TimeLogDto>
            {
                new() { UserId = userId.ToString(), DateCreated = DateTime.Now, TimeIn = DateTime.Now }
            };

            _mockTimeLogService.Setup(s => s.GetUserLogs(userId)).ReturnsAsync(logs);

            // Act
            var result = await _controller.GetUserTimeLogs(userId.ToString());

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedLogs = okResult.Value as IEnumerable<TimeLogDto>;
            Assert.IsNotNull(returnedLogs);
            Assert.AreEqual(1, ((List<TimeLogDto>)returnedLogs).Count);
        }

        [Test]
        public async Task GetCurrenTimeLog_ShouldReturnCurrentLog()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var date = DateTime.Today;
            var log = new TimeLogDto
            {
                UserId = userId.ToString(), DateCreated = date, TimeIn = DateTime.Now
            };

            _mockTimeLogService.Setup(s => s.GetCurrentLog(userId, date)).ReturnsAsync(log);

            // Act
            var result = await _controller.GetCurrenTimeLog(userId.ToString(), date);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnedLogs = okResult.Value as TimeLogDto;
            Assert.IsNotNull(returnedLogs);
        }

        [Test]
        public async Task SaveTimeLog_ShouldReturnSuccessResponse()
        {
            // Arrange
            var timeLogDto = new TimeLogDto
            {
                UserId = Guid.NewGuid().ToString(),
                DateCreated = DateTime.Now,
                TimeIn = DateTime.Now
            };

            var response = new TimeLogResponseDto { Success = true };

            _mockTimeLogService.Setup(s => s.SaveTime(timeLogDto)).ReturnsAsync(response);

            // Act
            var result = await _controller.SaveTimeLog(timeLogDto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var responseDto = okResult.Value as TimeLogResponseDto;
            Assert.IsNotNull(responseDto);
            Assert.IsTrue(responseDto.Success);
        }
    }
}
