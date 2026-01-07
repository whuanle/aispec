# CQRS 模块开发规范

## 模块三层架构

每个业务领域遵循三层模块化架构：

```
src/{domain}/
├── ShortUri.{Domain}.Shared/     # 共享层 - DTO、Command、Query 定义
├── ShortUri.{Domain}.Core/       # 核心层 - Handler 实现、业务逻辑
└── ShortUri.{Domain}.Api/        # API 层 - Controller/Endpoint 暴露
```

依赖关系：`Api → Core → Shared`

## 目录结构规范

### Shared 层 (ShortUri.{Domain}.Shared)

```
ShortUri.{Domain}.Shared/
├── Commands/                  # Command 定义
│   └── {Action}{Entity}Command.cs
├── Queries/                   # Query 定义
│   ├── {Query}{Entity}Command.cs
│   └── Models/             # Query 响应模型和子对象
│       ├── {Query}{Entity}CommandResponse.cs
│       └── {Query}{Entity}CommandResponseItem.cs
├── Models/                    # 共享模型、DTO
├── Services/                  # 服务接口定义
├── {Domain}SharedModule.cs    # 模块注册
└── ShortUri.{Domain}.Shared.csproj
```

### Core 层 (ShortUri.{Domain}.Core)

```
ShortUri.{Domain}.Core/
├── Handlers/                  # Command Handler 实现
│   └── {Action}{Entity}CommandHandler.cs
├── Queries/                   # Query Handler 实现
│   └── {Query}{Entity}CommandHandler.cs
├── Services/                  # 服务实现
├── {Domain}CoreModule.cs      # 模块注册
└── ShortUri.{Domain}.Core.csproj
```

### Api 层 (ShortUri.{Domain}.Api)

```
ShortUri.{Domain}.Api/
├── Controllers/               # API Controller
│   └── {Entity}Controller.cs
├── {Domain}ApiModule.cs       # 模块注册
└── ShortUri.{Domain}.Api.csproj
```

## 命名规范

### Command 命名

- 文件名：`{动作}{实体}Command.cs`
- 类名：`{动作}{实体}Command`
- 命名空间：`ShortUri.{Domain}.Commands`
- 示例：`UpdateUserInfoCommand.cs`

### Query 命名

- 文件名：`Query{实体}{描述}Command.cs` 或 `{Query}{Entity}Command.cs`
- 类名：与文件名一致
- 命名空间：`ShortUri.{Domain}.Queries`
- 示例：`QueryUserBindAccountCommand.cs`

### Handler 命名

- 文件名：`{Command/Query名}Handler.cs`
- 类名：`{Command/Query名}Handler`
- 命名空间：`ShortUri.{Domain}.Handlers` (Command) 或 `ShortUri.{Domain}.Queries` (Query)
- 示例：`UpdateUserInfoCommandHandler.cs`

### Response 命名

- 文件名：`{Query名}Response.cs`
- 列表项：`{Query名}ResponseItem.cs`
- 命名空间：`ShortUri.{Domain}.Queries.Models`

## 代码模板

### Command 定义

```csharp
using MediatR;
using ShortUri.Infra.Models;

namespace ShortUri.{Domain}.Commands;

/// <summary>
/// {功能描述}.
/// </summary>
public class {Action}{Entity}Command : IRequest<EmptyCommandResponse>
{
    /// <summary>
    /// {属性描述}.
    /// </summary>
    public int Id { get; set; }
}
```

### Query 定义

```csharp
using MediatR;
using ShortUri.{Domain}.Queries.Models;

namespace ShortUri.{Domain}.Queries;

/// <summary>
/// {查询描述}.
/// </summary>
public class Query{Entity}Command : IRequest<Query{Entity}CommandResponse>
{
    /// <summary>
    /// {参数描述}.
    /// </summary>
    public int Id { get; init; }
}
```

### Command Handler 实现

```csharp
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShortUri.Database;
using ShortUri.Infra.Exceptions;
using ShortUri.Infra.Models;
using ShortUri.{Domain}.Commands;

namespace ShortUri.{Domain}.Handlers;

/// <summary>
/// <inheritdoc cref="{Action}{Entity}Command"/>
/// </summary>
public class {Action}{Entity}CommandHandler : IRequestHandler<{Action}{Entity}Command, EmptyCommandResponse>
{
    private readonly DatabaseContext _databaseContext;

    public {Action}{Entity}CommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<EmptyCommandResponse> Handle({Action}{Entity}Command request, CancellationToken cancellationToken)
    {
        // 业务逻辑实现
        return EmptyCommandResponse.Default;
    }
}
```

### Query Handler 实现

```csharp
using MediatR;
using Microsoft.EntityFrameworkCore;
using ShortUri.Database;
using ShortUri.{Domain}.Queries.Models;

namespace ShortUri.{Domain}.Queries;

/// <summary>
/// <inheritdoc cref="Query{Entity}Query"/>
/// </summary>
public class Query{Entity}CommandHandler : IRequestHandler<Query{Entity}Command, Query{Entity}CommandResponse>
{
    private readonly DatabaseContext _databaseContext;

    public Query{Entity}CommandHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<Query{Entity}QueryResponse> Handle(Query{Entity}Query request, CancellationToken cancellationToken)
    {
        // 查询逻辑实现
        return new Query{Entity}QueryResponse();
    }
}
```

### Controller 实现

```csharp
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShortUri.Infra.Models;
using ShortUri.{Domain}.Commands;
using ShortUri.{Domain}.Queries;

namespace ShortUri.{Domain}.Controllers;

/// <summary>
/// {领域}相关接口.
/// </summary>
[ApiController]
[Route("/{domain}/{entity}")]
public partial class {Entity}Controller : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly UserContext _userContext;

    public {Entity}Controller(IMediator mediator, UserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
    }

    /// <summary>
    /// {接口描述}.
    /// </summary>
    [HttpPost("{action}")]
    public async Task<EmptyCommandResponse> {Action}([FromBody] {Action}{Entity}Command req, CancellationToken ct = default)
    {
        return await _mediator.Send(req, ct);
    }
}
```

## 用户信息传递

不允许在 Handler 直接注入 IUserContext，如果需要根据用户 id 查询信息或限制范围，需要在 Command 继承 IUserIdContext，由 Command 传入。

```csharp
/// <summary>
/// 查询能看到的提示词列表.
/// </summary>
public class QueryPromptListCommand : IUserIdContext, IRequest<QueryPromptListCommandResponse>
{
    /// <inheritdoc/>
    public int ContextUserId { get; init; }

    /// <inheritdoc/>
    public UserType ContextUserType { get; init; }
}
```

默认命令或查询模型用不到用户信息，请不要继承 IUserIdContext.


程序会在 ASP.NET Core 做模型绑定后，自动注入用户信息。

## 审计属性
审计输入由框架自动注入，不需要自己在插入或更新实体时手动传递。
实体的以下审计属性会自动赋值，不需要在 Handler 里面手动设置。

```
    /// <summary>
    /// 创建人.
    /// </summary>
    public int CreateUserId { get; set; }

    /// <summary>
    /// 创建时间.
    /// </summary>
    public DateTimeOffset CreateTime { get; set; }

    /// <summary>
    /// 更新人.
    /// </summary>
    public int UpdateUserId { get; set; }

    /// <summary>
    /// 更新时间.
    /// </summary>
    public DateTimeOffset UpdateTime { get; set; }

    /// <summary>
    /// 软删除.
    /// </summary>
    public long IsDeleted { get; set; }
```


## 模块注册

### Shared 模块

```csharp
using Maomi;

namespace ShortUri.{Domain};

public class {Domain}SharedModule : IModule
{
    public void ConfigureServices(ServiceContext context)
    {
    }
}
```

### Core 模块

```csharp
using Maomi;

namespace ShortUri.{Domain};

[InjectModule<{Domain}SharedModule>]
[InjectModule<{Domain}ApiModule>]
public class {Domain}CoreModule : IModule
{
    public void ConfigureServices(ServiceContext context)
    {
    }
}
```

## 子领域组织

对于复杂模块，可按子领域组织目录：

```
ShortUri.Plugin.Shared/
├── Classify/
│   ├── Commands/
│   └── Queries/
├── CustomPlugins/
│   ├── Commands/
│   └── Queries/
├── NativePlugins/
│   ├── Commands/
│   ├── Queries/
│   └── Models/
└── ...

ShortUri.Plugin.Core/
├── Classify/
│   ├── Handlers/
│   └── Queries/
├── CustomPlugins/
│   ├── Handlers/
│   └── Queries/
└── ...
```

## 异常处理

使用 `BusinessException` 抛出业务异常：

```csharp
throw new BusinessException("错误消息") { StatusCode = 404 };
```

常用状态码：
- 400: 请求参数错误
- 403: 无权限
- 404: 资源不存在
- 409: 资源冲突
