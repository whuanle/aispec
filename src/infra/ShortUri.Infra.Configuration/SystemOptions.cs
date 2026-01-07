using Microsoft.AspNetCore.Mvc.Formatters;
using ShortUri.Infra.Models;

namespace ShortUri.Infra;

/// <summary>
/// 系统配置.
/// </summary>
public class SystemOptions
{
    /// <summary>
    /// 开启调试输出.
    /// </summary>
    public bool Debug { get; init; }

    /// <summary>
    /// 系统名称.
    /// </summary>
    public string Name { get; init; } = "ShortUri";

    /// <summary>
    /// 监听端口.
    /// </summary>
    public int Port { get; init; }

    /// <summary>
    /// 服务访问地址.
    /// </summary>
    public string Server { get; init; } = string.Empty;

    /// <summary>
    /// 前端地址.
    /// </summary>
    public string WebUI { get; init; } = string.Empty;

    /// <summary>
    /// AES 密钥.
    /// </summary>
    public string AES { get; init; } = string.Empty;

    /// <summary>
    /// 系统数据库类型.
    /// </summary>
    public string DBType { get; init; } = string.Empty;

    /// <summary>
    /// 系统数据库连接字符串.
    /// </summary>
    public string Database { get; init; } = string.Empty;

    /// <summary>
    /// Redis 连接字符串.
    /// </summary>
    public string Redis { get; init; } = string.Empty;

    /// <summary>
    /// 最大上传文件大小，单位为字节，默认 100MB.
    /// </summary>
    public int MaxUploadFileSize { get; init; } = 1024 * 1024 * 100;

    /// <summary>
    /// 可观察性.
    /// </summary>
    public OpenTelemetryOptions OTLP { get; init; } = new();
}


public class OpenTelemetryOptions
{
    public string Trace { get; init; } = string.Empty;
    public string Metrics { get; init; } = string.Empty;
    public int Protocol { get; init; }
}