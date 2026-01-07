using Maomi;
using Microsoft.AspNetCore.HttpLogging;
using ShortUri.Infra;

namespace ShortUri.Modules;

/// <summary>
/// 配置日志.
/// </summary>
public class ConfigureLoggerModule : IModule
{
    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        // todo: 忽略 swagger
        context.Services.AddHttpLogging(logging =>
        {
            logging.LoggingFields = HttpLoggingFields.RequestPath | HttpLoggingFields.RequestQuery | HttpLoggingFields.RequestProtocol
#if DEBUG
            | HttpLoggingFields.RequestBody | HttpLoggingFields.ResponseBody
#endif
            | HttpLoggingFields.ResponseStatusCode
            ;

            logging.CombineLogs = true;
            logging.RequestBodyLogLimit = 4096;
            logging.ResponseBodyLogLimit = 4096;
        });
    }
}
