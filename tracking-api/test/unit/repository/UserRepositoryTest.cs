using Microsoft.EntityFrameworkCore;
using tracking_app.Models;
using tracking_app.Repository;

namespace tracking_app.test.unit.repository
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private DbContextOptions<TrackingAppDbContext> _dbContextOptions;
        private TrackingAppDbContext _dbContext;
        private IUserRepository _userRepository;

        [SetUp]
        public void SetUp()
        {
            // Set up in-memory database
            _dbContextOptions = new DbContextOptionsBuilder<TrackingAppDbContext>()
                                    .UseInMemoryDatabase("TestDatabase") // Use an in-memory database
                                    .Options;

            // Create the context with the in-memory database
            _dbContext = new TrackingAppDbContext(_dbContextOptions);
            _userRepository = new UserRepository(_dbContext);
        }

        [Test]
        public async Task GetByUserName_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userName = "john_doe";
            var user = new User { Id = Guid.NewGuid(), Username = userName, Password = "123456", DateCreated = DateTime.Now, Name = "John Doe" };

            // Add user to the in-memory database
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();

            // Act
            var result = await _userRepository.GetByUserName(userName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(userName, result?.Username);
        }

        [TearDown]
        public void TearDown()
        {
            // Clean up the in-memory database after each test
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
