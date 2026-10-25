using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace JellyBlindTest.Frontend;

public sealed class WebClientTransformationRegistration : BackgroundService
{
    private static readonly Guid TransformationId =
        Guid.Parse("d5da6325-d9e1-4ef4-ab68-ed2b36ae2e21");
    private readonly ILogger<WebClientTransformationRegistration> _logger;

    public WebClientTransformationRegistration(ILogger<WebClientTransformationRegistration> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        for (var attempt = 0; attempt < 60 && !stoppingToken.IsCancellationRequested; attempt++)
        {
            var assembly = AssemblyLoadContext.All
                .SelectMany(context => context.Assemblies)
                .FirstOrDefault(candidate => candidate.FullName?.Contains(
                    "FileTransformation", StringComparison.OrdinalIgnoreCase) == true);
            var pluginInterface = assembly?.GetType("Jellyfin.Plugin.FileTransformation.PluginInterface");
            var register = pluginInterface?.GetMethod("RegisterTransformation", BindingFlags.Public | BindingFlags.Static);

            if (register is not null)
            {
                Register(register);
                return;
            }

            await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken).ConfigureAwait(false);
        }

        if (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogWarning("File Transformation was not found; the Blind Test Home tab is unavailable.");
        }
    }

    private void Register(MethodInfo register)
    {
        try
        {
            var payloadType = register.GetParameters().FirstOrDefault()?.ParameterType;
            var parse = payloadType?.GetMethod("Parse", new[] { typeof(string) });
            if (payloadType is null || parse is null)
            {
                _logger.LogWarning("File Transformation registration API was not recognized.");
                return;
            }

            var payloadJson = JsonSerializer.Serialize(new
            {
                id = TransformationId,
                fileNamePattern = "index.html",
                callbackAssembly = typeof(WebClientScriptTransformation).Assembly.FullName,
                callbackClass = typeof(WebClientScriptTransformation).FullName,
                callbackMethod = nameof(WebClientScriptTransformation.TransformIndexHtml),
            });
            var payload = parse.Invoke(null, new object[] { payloadJson });
            register.Invoke(null, new[] { payload });
            _logger.LogInformation("Registered the Blind Test native Home tab script.");
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception, "Could not register the Blind Test native-tab transformation.");
        }
    }
}

public static class WebClientScriptTransformation
{
    private const string InjectionMarker = "data-jellyblindtest-navigation";
    private const string ScriptResourceName = "JellyBlindTest.Web.navigation.js";

    public static string TransformIndexHtml(object payload)
    {
        using var payloadDocument = JsonDocument.Parse(payload.ToString() ?? "{}");
        if (!payloadDocument.RootElement.TryGetProperty("contents", out var contentsElement))
        {
            return string.Empty;
        }

        var contents = contentsElement.GetString() ?? string.Empty;
        if (contents.Contains(InjectionMarker, StringComparison.Ordinal)) return contents;

        using var stream = typeof(WebClientScriptTransformation).Assembly.GetManifestResourceStream(ScriptResourceName);
        if (stream is null) return contents;

        using var reader = new StreamReader(stream, Encoding.UTF8);
        var script = "<script " + InjectionMarker + "=\"true\">" + reader.ReadToEnd() + "</script>";
        var bodyEnd = contents.LastIndexOf("</body>", StringComparison.OrdinalIgnoreCase);
        return bodyEnd < 0 ? contents : contents.Insert(bodyEnd, script);
    }
}