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

        return Ok(new GameSettingsResponse(
            Math.Clamp(configuration.RoundCount, 1, 20),
            Math.Clamp(configuration.PreviewSeconds, 1, 30),
            Math.Clamp(configuration.AnswerSeconds, 1, 120),
            Math.Clamp(configuration.RevealSeconds, 1, 30)));
    }
}

public sealed record GameSettingsResponse(
    int RoundCount,
    int PreviewSeconds,
    int AnswerSeconds,
    int RevealSeconds);