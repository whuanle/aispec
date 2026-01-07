using Microsoft.AspNetCore.Http;
using ShortUri.Infra.Defaults;
using ShortUri.Infra.Exceptions;
using ShortUri.Infra.Models;
using ShortUri.Infra.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ShortUri.Login.Services;

/// <summary>
/// 用户上下文提供者.
/// </summary>
public class UserContextProvider : IUserContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserContextProvider"/> class.
    /// </summary>
    /// <param name="httpContextAccessor"></param>
    public UserContextProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;

        _userContext = new Lazy<UserContext>(() =>
        {
            return Parse();
        });
    }

    private readonly Lazy<UserContext> _userContext;

    /// <inheritdoc/>
    public UserContext GetUserContext() => _userContext.Value;

    private DefaultUserContext Parse()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        var user = httpContext?.User;
        if (httpContext == null || user == null || user.Identity == null || !user.Identity.IsAuthenticated)
        {
            return new DefaultUserContext
            {
                UserId = 0,
                UserName = "Anonymous",
                NickName = "Anonymous",
                Email = string.Empty
            };
        }

        var userId = user.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = user.FindFirstValue(JwtRegisteredClaimNames.Name);
        var nickName = user.FindFirstValue(JwtRegisteredClaimNames.Nickname);
        var email = user.FindFirstValue(ClaimTypes.Email);

        return new DefaultUserContext
        {
            UserId = int.TryParse(userId, out var guid) ? guid : throw new BusinessException("Token 格式错误") { StatusCode = 401 },
            UserName = userName ?? string.Empty,
            NickName = nickName ?? string.Empty,
            Email = email ?? string.Empty
        };
    }
}
