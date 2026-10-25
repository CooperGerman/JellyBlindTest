using MediaBrowser.Common.Configuration;
using MediaBrowser.Common.Plugins;
using MediaBrowser.Model.Plugins;
using MediaBrowser.Model.Serialization;

namespace JellyBlindTest;

public sealed class Plugin : BasePlugin<PluginConfiguration>, IHasWebPages
{
    public Plugin(IApplicationPaths applicationPaths, IXmlSerializer xmlSerializer)
        : base(applicationPaths, xmlSerializer)
    {
        Instance = this;
    }

    public static Plugin? Instance { get; private set; }

    public override string Name => "JellyBlindTest";

    public override Guid Id => Guid.Parse("dcfb6e4c-e418-4bca-9a4a-f960e991ad00");

    public override string Description =>
        "Play a short random song preview and guess the title.";

    public IEnumerable<PluginPageInfo> GetPages()
    {
        yield return new PluginPageInfo
        {
            Name = "JellyBlindTestSettings",
            DisplayName = "Blind Test Settings",
            EnableInMainMenu = false,
            EmbeddedResourcePath = "JellyBlindTest.Web.settings.html",
        };
    }
}

public sealed class PluginConfiguration : BasePluginConfiguration
{
    public int RoundCount { get; set; } = 5;

    public int PreviewSeconds { get; set; } = 5;

    public int AnswerSeconds { get; set; } = 15;

    public int GuessingSeconds { get; set; }

    public int PopularTrackPoolSize { get; set; } = 500;

    public int RevealSeconds { get; set; } = 5;
}