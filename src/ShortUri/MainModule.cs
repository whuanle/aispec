using Maomi;
using Maomi.I18n;
using ShortUri.Database;
using ShortUri.Filters;
using ShortUri.Infra;
using ShortUri.Login;
using ShortUri.Modules;

namespace ShortUri;

/// <summary>
/// MainModule.
/// </summary>
[InjectModule<InfraCoreModule>]
[InjectModule<DatabaseCoreModule>]
[InjectModule<LoginCoreModule>]
[InjectModule<ApiModule>]
public partial class MainModule : IModule
{
    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        // 添加HTTP上下文访问器
        context.Services.AddHttpContextAccessor();
        context.Services.AddExceptionHandler<MaomiExceptionHandler>();
        context.Services.AddI18nAspNetCore();
    }
}