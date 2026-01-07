using Maomi;
using Microsoft.Extensions.DependencyInjection;
using ShortUri.Infra;
using StackExchange.Redis.Extensions.Core;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;
using StackExchange.Redis.Extensions.System.Text.Json;

namespace ShortUri.Database;

/// <summary>
/// DatabaseCoreModule.
/// </summary>
[InjectModule<DatabaseMysqlModule>]
public class DatabaseCoreModule : IModule
{
    private readonly SystemOptions _systemOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="DatabaseCoreModule"/> class.
    /// </summary>
    /// <param name="systemOptions"></param>
    public DatabaseCoreModule(SystemOptions systemOptions)
    {
        _systemOptions = systemOptions!;
    }

    /// <inheritdoc/>
    public void ConfigureServices(ServiceContext context)
    {
        // 添加 redis
        AddStackExchangeRedis(context.Services, new RedisConfiguration
        {
            ConnectionString = _systemOptions.Redis,
            PoolSize = 10,
            KeyPrefix = "short:",
            ConnectTimeout = 5000,
            IsDefault = true
        });
    }

    private static void AddStackExchangeRedis(IServiceCollection services, RedisConfiguration redisConfiguration)
    {
        services.AddSingleton<ISerializer, SystemTextJsonSerializer>();

        services.AddSingleton<IRedisClientFactory, RedisClientFactory>();

        services.AddSingleton((provider) => provider
            .GetRequiredService<IRedisClientFactory>()
            .GetDefaultRedisClient()
            .GetDefaultDatabase());

        services.AddSingleton(redisConfiguration);
    }
}