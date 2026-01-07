using FluentValidation;
using MediatR;
using ShortUri.Login.Commands.Models;

namespace ShortUri.Login.Commands;

/// <summary>
/// 登录.
/// </summary>
public class LoginCommand : IRequest<LoginCommandResponse>, IModelValidator<LoginCommand>
{
    /// <summary>
    /// 用户名或邮箱.
    /// </summary>
    public string UserName { get; init; } = default!;

    /// <summary>
    /// 密码.
    /// </summary>
    public string Password { get; init; } = default!;

    /// <inheritdoc/>
    public static void Validate(AbstractValidator<LoginCommand> validate)
    {
        validate.RuleFor(x => x.UserName).NotEmpty();
        validate.RuleFor(x => x.Password).NotEmpty();
    }
}
