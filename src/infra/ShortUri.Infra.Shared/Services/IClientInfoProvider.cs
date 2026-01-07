namespace ShortUri.Infra.Services;

/// <summary>
/// IClientInfoProvider.
/// </summary>
public interface IClientInfoProvider
{
    /// <summary>
    /// 获取客户端信息.
    /// </summary>
    /// <returns></returns>
    ShortUri.Infra.Models.ClientInfo GetClientInfo();
}