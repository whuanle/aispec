using Maomi;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using ShortUri.Infra;

namespace ShortUri.Modules;

/// <summary>
/// 可观察性.
/// </summary>
public class ConfigureOpenTelemetryModule : IModule
{
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="ConfigureOpenTelemetryModule"/> class.
    /// </summary>
    /// <param name="systemOptions"></param>
    public ConfigureOpenTelemetryModule(SystemOptions systemOptions)
    {
        _systemOptions = systemOptions;
    }

    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        context.Services.AddOpenTelemetry()
              .ConfigureResource(resource => resource.AddService(AppConst.ActivitySource.Name))
              .WithTracing(tracing =>
              {
                  tracing
                  .AddAspNetCoreInstrumentation()
                  .AddEntityFrameworkCoreInstrumentation()
                  .AddHttpClientInstrumentation()
                  .AddRedisInstrumentation()
                  .AddOtlpExporter(options =>
                  {
                      options.Endpoint = new Uri(_systemOptions.OTLP.Trace);
                      options.Protocol = (OtlpExportProtocol)_systemOptions.OTLP.Protocol;
                  });
              })
              .WithMetrics(metrices =>
              {
                  metrices.AddAspNetCoreInstrumentation()
                  .AddHttpClientInstrumentation()
                  .AddAspNetCoreInstrumentation()
                  .AddRuntimeInstrumentation()
                  .AddOtlpExporter(options =>
                  {
                      options.Endpoint = new Uri(_systemOptions.OTLP.Metrics);
                      options.Protocol = (OtlpExportProtocol)_systemOptions.OTLP.Protocol;
                  });
              });
    }
}
