#pragma warning disable ASP0026 // [Authorize] overridden by [AllowAnonymous] from farther away

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShortUri.Login.Commands;
using ShortUri.Login.Commands.Models;

namespace ShortUri.Login.Controllers;

/// <summary>
/// 登录相关接口.
/// </summary>
[ApiController]
[Route("/account")]
[AllowAnonymous]
public class LoginController : ControllerBase
{
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="LoginController"/> class.
    /// </summary>
    /// <param name="mediator"></param>
    public LoginController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// 用户登录.
    /// </summary>
    /// <param name="req">登录请求体.</param>
    /// <param name="ct">取消令牌.</param>
    /// <returns>返回 <see cref="LoginCommandResponse"/>.</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<LoginCommandResponse> Login([FromBody] LoginCommand req, CancellationToken ct)
    {
        return await _mediator.Send(req, ct);
    }

    /// <summary>
    /// 刷新 token.
    /// </summary>
    /// <param name="req">刷新 token 请求体.</param>
    /// <param name="ct">取消令牌.</param>
    /// <returns>返回 <see cref="RefreshTokenCommandResponse"/>.</returns>
    [HttpPost("refresh_token")]
    [AllowAnonymous]
    public async Task<RefreshTokenCommandResponse> RefreshToken([FromBody] RefreshTokenCommand req, CancellationToken ct)
    {
        return await _mediator.Send(req, ct);
    }
}