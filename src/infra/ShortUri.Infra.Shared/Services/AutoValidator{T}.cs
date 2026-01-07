using FluentValidation;
using Maomi;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ShortUri.Infra.Defaults;
using ShortUri.Infra.Models;
using ShortUri.Infra.Services;

namespace ShortUri.Infra;

/// <summary>
/// 自动模型验证器.
/// </summary>
/// <typeparam name="T">类型.</typeparam>
public class AutoValidator<T> : AbstractValidator<T>, IValidator<T>
    where T : class, IModelValidator<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AutoValidator{T}"/> class.
    /// </summary>
    public AutoValidator()
    {
        T.Validate(this);
    }
}