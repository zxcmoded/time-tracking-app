using tracking_app.Dto;
using tracking_app.Models;

namespace tracking_app.Mappers
{
    public static class TimeLogMapper
    {

        public static TimeLog MapToDbo(this TimeLogDto timeLogDto)
        {
            return new TimeLog
            {
                LogId = timeLogDto.LogId ?? 0,
                UserId = Guid.Parse(timeLogDto.UserId),
                DateCreated = timeLogDto.DateCreated,
                TimeIn = timeLogDto.TimeIn,
                TimeOut = timeLogDto.TimeOut,
            };
        }

        public static TimeLogDto MapToDto(this TimeLog timeLog)
        {
            return new TimeLogDto
            {
                LogId = timeLog.LogId,
                UserId = timeLog.UserId.ToString(),
                Name = timeLog.User.Name,
                TimeIn = timeLog.TimeIn,
                DateCreated = timeLog.DateCreated,
                TimeOut = timeLog.TimeOut,
                TotalHours = timeLog.TimeOut.HasValue && timeLog.TimeIn.HasValue ? (timeLog.TimeOut.Value - timeLog.TimeIn.Value).TotalHours : 0
            };
        }

        public static IEnumerable<TimeLogDto> MapToDto(this IEnumerable<TimeLog> timeLogs)
        {
            return timeLogs.Select(MapToDto);
        }
    }
}