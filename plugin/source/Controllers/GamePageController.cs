using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JellyBlindTest.Controllers;

[ApiController]
[Authorize]
[Route("jellyblindtest")]
public sealed class GamePageController : ControllerBase
{
    private const string PageResourceName = "JellyBlindTest.Web.blindtest.html";
    private const string IconResourceName = "JellyBlindTest.Web.blindtest-icon.svg";

    [HttpGet("page")]
    [AllowAnonymous]
    [Produces("text/html")]
    public IActionResult GetGamePage()
    {
        using var stream = typeof(GamePageController).Assembly.GetManifestResourceStream(PageResourceName);
        if (stream is null) return NotFound();

        using var reader = new StreamReader(stream);
        return Content(reader.ReadToEnd(), "text/html; charset=utf-8");
    }

    [HttpGet("icon.svg")]
    [AllowAnonymous]
    [Produces("image/svg+xml")]
    public IActionResult GetIcon()
    {
        using var stream = typeof(GamePageController).Assembly.GetManifestResourceStream(IconResourceName);
        if (stream is null) return NotFound();

        using var reader = new StreamReader(stream);
        return Content(reader.ReadToEnd(), "image/svg+xml; charset=utf-8");
    }
}