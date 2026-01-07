using ShortUri.Infra.Models;

namespace ShortUri.Infra.Services;

/// <summary>
/// 用户上下文提供者.
/// </summary>
public interface IUserContextProvider
{
    /// <summary>
    /// 获取用户上下文.
    /// </summary>
    /// <returns><see cref="UserContext"/>.</returns>
    UserContext GetUserContext();
}
