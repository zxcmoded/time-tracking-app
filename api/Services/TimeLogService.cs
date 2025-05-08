using tracking_app.Dto;
using tracking_app.Mappers;
using tracking_app.Repository;

namespace tracking_app.Services
{
    public interface ITimeLogService
    {
        Task<IEnumerable<TimeLogDto>> GetUserLogs(Guid userId);
        Task<TimeLogDto?> GetCurrentLog(Guid userId, DateTime date);
        Task<TimeLogResponseDto> SaveTime(TimeLogDto timeLogDto);
    }

    public class TimeLogService : ITimeLogService
    {
        private readonly ITimeLogRepository timeLogRepository;
        public TimeLogService(ITimeLogRepository timeLogRepository)
        {
            this.timeLogRepository = timeLogRepository;
        }

        public async Task<TimeLogDto?> GetCurrentLog(Guid userId, DateTime date)
        {
            var log = await timeLogRepository.GetByDate(date, userId);

            return log == null ? null : log.MapToDto();
        }

        public async Task<IEnumerable<TimeLogDto>> GetUserLogs(Guid userId)
        {
            return (await timeLogRepository.GetUserLogs(userId)).MapToDto();
        }

        public async Task<TimeLogResponseDto> SaveTime(TimeLogDto timeLogDto)
        {
            var timeLog = await GetCurrentLog(Guid.Parse(timeLogDto.UserId), timeLogDto.DateCreated);

            // If a time log exists, assign the LogId
            if (timeLog != null)
            {
                timeLogDto.LogId = timeLog.LogId;
            }
            // If no time in log and TimeOut exists, return a failure response
            else if (timeLogDto.TimeIn == null && timeLogDto.TimeOut.HasValue)
            {
                return new TimeLogResponseDto
                {
                    Success = false,
                    Message = "Please clock in before clocking out."
                };
            }

            // Save the time log if valid
            await timeLogRepository.SaveTimeLog(timeLogDto.MapToDbo());

            return new TimeLogResponseDto
            {
                Success = true,
                Message = "Saved Successfully"
            };
        }
    }
}