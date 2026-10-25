using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JellyBlindTest.Controllers;

[ApiController]
[Authorize]
[Route("jellyblindtest")]
public sealed class GamePageController : ControllerBase
{
    private const string PageResourceName = "JellyBlindTest.Web.blindtest.html";
    private const string GameScriptStart = "<script>";
    private const string GameScriptEnd = "</script>";
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

    [HttpGet("game.js")]
    [AllowAnonymous]
    [Produces("application/javascript")]
    public IActionResult GetGameScript()
    {
        using var stream = typeof(GamePageController).Assembly.GetManifestResourceStream(PageResourceName);
        if (stream is null) return NotFound();

        using var reader = new StreamReader(stream);
        var page = reader.ReadToEnd();
        var start = page.IndexOf(GameScriptStart, StringComparison.OrdinalIgnoreCase);
        if (start < 0) return NotFound();
        start += GameScriptStart.Length;

        var end = page.IndexOf(GameScriptEnd, start, StringComparison.OrdinalIgnoreCase);
        if (end < 0) return NotFound();

        return Content(page[start..end], "application/javascript; charset=utf-8");
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