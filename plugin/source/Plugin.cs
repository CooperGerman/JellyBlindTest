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
    }

    public override string Name => "JellyBlindTest";

    public override Guid Id => Guid.Parse("dcfb6e4c-e418-4bca-9a4a-f960e991ad00");

    public override string Description =>
        "Play a short random song preview and guess the title.";

    public IEnumerable<PluginPageInfo> GetPages()
    {
        yield return new PluginPageInfo
        {
            Name = "JellyBlindTest",
            DisplayName = "Blind Test",
            EnableInMainMenu = true,
            EmbeddedResourcePath = "JellyBlindTest.Web.blindtest.html",
        };
    }
}

public sealed class PluginConfiguration : BasePluginConfiguration
{
}