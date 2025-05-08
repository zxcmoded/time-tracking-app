using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tracking_app.Dto;
using tracking_app.Models;
using tracking_app.Services;

namespace tracking_app.Controllers;

[Authorize]
[ApiController]
[ApiVersion("1")]
[Route("api/v{version:apiVersion}/[controller]")]
public class TimeLogController : ControllerBase
{
    private readonly ILogger<AuthController> _logger;
    private readonly ITimeLogService _timeLogService;
    public TimeLogController(ILogger<AuthController> logger, ITimeLogService timeLogService)
    {
        _logger = logger;
        _timeLogService = timeLogService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TimeLogDto>>> GetUserTimeLogs(string userId)
    {
        try
        {
            _logger.LogInformation("Get user time logs: {userId}", userId);

            var result = await _timeLogService.GetUserLogs(Guid.Parse(userId));

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpGet]
    [Route("current")]
    public async Task<ActionResult<IEnumerable<TimeLogDto>>> GetCurrenTimeLog(string userId, DateTime date)
    {
        try
        {
            _logger.LogInformation("Get user time logs: {userId}", userId);

            var result = await _timeLogService.GetCurrentLog(Guid.Parse(userId), date);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<TimeLogResponseDto>> SaveTimeLog([FromBody] TimeLogDto timeLog)
    {
        try
        {
            _logger.LogInformation("Save time log: {timeLog}", timeLog);

            var result = await _timeLogService.SaveTime(timeLog);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}