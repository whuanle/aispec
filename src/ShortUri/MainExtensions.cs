using Maomi;
using Serilog;
using ShortUri.Infra;

namespace ShortUri;

/// <summary>
/// MainExtensions.
/// </summary>
public static partial class MainExtensions
{
    /// <summary>
    /// UseShortUri.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IHostApplicationBuilder UseShortUri(this IHostApplicationBuilder builder)
    {
        if (!Directory.Exists(AppConst.ConfigsPath))
        {
            InitConfigurationDirectory();
        }

        ImportSystemConfiguration(builder);

        var systemOptions = builder.Configuration.GetSection("ShortUri").Get<SystemOptions>() ?? throw new FormatException("The system configuration cannot be loaded.");

        builder.Services.AddSingleton(systemOptions);

        builder.Services.AddSingleton<IConfigurationManager>(builder.Configuration);

        builder.Logging.ClearProviders();
        builder.Services.AddSerilog((services, configuration) =>
        {
            configuration.ReadFrom.Services(services);
            configuration.ReadFrom.Configuration(builder.Configuration);
        });

        builder.Services.AddModule<MainModule>();

        return builder;
    }

    /// <summary>
    /// UseShortUri.
    /// </summary>
    /// <param name="builder"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseShortUri(this IApplicationBuilder builder)
    {
        return builder;
    }
}