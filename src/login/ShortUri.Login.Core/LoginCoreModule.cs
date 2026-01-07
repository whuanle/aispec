using Maomi;
using Microsoft.Extensions.DependencyInjection;
using ShortUri.Infra.Models;
using ShortUri.Infra.Services;
using ShortUri.Login.Services;

namespace ShortUri.Login;

/// <summary>
/// LoginCoreModule.
/// </summary>
[InjectModule<LoginSharedModule>]
[InjectModule<LoginApiModule>]
public class LoginCoreModule : IModule
{
    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        context.Services.AddScoped<IUserContextProvider, UserContextProvider>();
        context.Services.AddScoped<UserContext>(s =>
        {
            return s.GetRequiredService<IUserContextProvider>().GetUserContext();
        });
    }
}
