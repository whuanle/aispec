using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ShortUri.Database;
using ShortUri.Infra;
using ShortUri.Infra.Defaults;
using ShortUri.Infra.Exceptions;
using ShortUri.Infra.Helpers;
using ShortUri.Infra.Services;
using ShortUri.Login.Commands;
using ShortUri.Login.Commands.Models;
using ShortUri.Login.Services;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace ShortUri.Login.Handlers;

/// <summary>
/// 登录命令处理程序.
/// </summary>
public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginCommandResponse>
{
    private readonly DatabaseContext _databaseContext;
    private readonly SystemOptions _systemOptions;
    private readonly IRedisDatabase _redisDatabase;
    private readonly ITokenProvider _tokenProvider;
    private readonly ILogger<LoginCommandHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginCommandHandler"/> class.
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="systemOptions"></param>
    /// <param name="redisDatabase"></param>
    /// <param name="rsaProvider"></param>
    /// <param name="tokenProvider"></param>
    /// <param name="logger"></param>
    public LoginCommandHandler(DatabaseContext dbContext, SystemOptions systemOptions, IRedisDatabase redisDatabase, ITokenProvider tokenProvider, ILogger<LoginCommandHandler> logger)
    {
        _databaseContext = dbContext;
        _systemOptions = systemOptions;
        _redisDatabase = redisDatabase;
        _tokenProvider = tokenProvider;
        _logger = logger;
    }

    /// <summary>
    /// 处理登录命令.
    /// </summary>
    /// <param name="request">命令请求.</param>
    /// <param name="cancellationToken">取消令牌.</param>
    /// <returns>登录结果.</returns>
    public async Task<LoginCommandResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var cacheKey = $"login:fail:{request.UserName}";
        var failCount = await _redisDatabase.GetAsync<int>(cacheKey);

        if (failCount >= 5)
        {
            throw new BusinessException("登录失败次数过多，请稍后再试.") { StatusCode = 403 };
        }

        var user = await _databaseContext.Users.Where(u =>
                                  u.UserName == request.UserName || u.Email == request.UserName)
                              .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            await IncrementLoginFailCountAsync(cacheKey);
            throw new BusinessException("用户名或密码错误") { StatusCode = 401 };
        }

        try
        {
            if (!PBKDF2Helper.VerifyHash(request.Password, user.Password, user.PasswordSalt))
            {
                await IncrementLoginFailCountAsync(cacheKey);
                throw new BusinessException("用户名或密码错误") { StatusCode = 401 };
            }
        }
        catch (Exception ex) when (ex is not BusinessException)
        {
            await IncrementLoginFailCountAsync(cacheKey);
            throw new BusinessException("用户名或密码错误") { StatusCode = 401 };
        }

        // 登录成功，清除失败计数
        await _redisDatabase.Database.KeyDeleteAsync(cacheKey);

        var userContext = new DefaultUserContext
        {
            UserId = user.Id,
            UserName = user.UserName,
            NickName = user.NickName,
            Email = user.Email
        };

        var (accessToken, refreshToken) = _tokenProvider.GenerateTokens(userContext);

        var result = new LoginCommandResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            UserId = user.Id,
            UserName = user.UserName,
            ExpiresIn = DateTimeOffset.Now.AddMinutes(30).ToUnixTimeMilliseconds()
        };

        _logger.LogInformation("User login.{@Message}", new { user.Id, user.UserName, user.NickName });

        return result;
    }

    /// <summary>
    /// 增加登录失败计数.
    /// </summary>
    /// <param name="cacheKey">缓存键.</param>
    /// <returns>异步任务.</returns>
    private async Task IncrementLoginFailCountAsync(string cacheKey)
    {
        var failCount = await _redisDatabase.Database.StringIncrementAsync(cacheKey);
        await _redisDatabase.Database.KeyExpireAsync(cacheKey, TimeSpan.FromMinutes(5));
    }
}