using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JellyBlindTest.Controllers;

[ApiController]
[Authorize]
[Route("jellyblindtest")]
public sealed class GamePageController : ControllerBase
{
    private const string PageResourceName = "JellyBlindTest.Web.blindtest.html";

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
}