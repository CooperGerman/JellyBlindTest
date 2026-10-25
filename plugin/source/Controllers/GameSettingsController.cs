using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JellyBlindTest.Controllers;

[ApiController]
[Authorize]
[Route("jellyblindtest")]
public sealed class GameSettingsController : ControllerBase
{
    [HttpGet("settings")]
    public ActionResult<GameSettingsResponse> GetSettings()
    {
        var configuration = Plugin.Instance?.Configuration;
        if (configuration is null)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable);
        }

        var guessingSeconds = configuration.GuessingSeconds > 0
            ? configuration.GuessingSeconds
            : configuration.PreviewSeconds + configuration.AnswerSeconds;

        return Ok(new GameSettingsResponse(
            Math.Clamp(configuration.RoundCount, 1, 20),
            Math.Clamp(guessingSeconds, 1, 150),
            configuration.EnablePopularityMode,
            Math.Clamp(configuration.PopularTrackPoolSize, 1, 5000),
            Math.Clamp(configuration.RevealSeconds, 1, 30)));
    }
}

public sealed record GameSettingsResponse(
    int RoundCount,
    int GuessingSeconds,
    bool EnablePopularityMode,
    int PopularTrackPoolSize,
    int RevealSeconds);