using MessagePack;
using System.Collections.Generic;

namespace Mafiator.Common.Api;

/// <summary>
/// Api result returning in controllers.
/// </summary>
[MessagePackObject]
public class ApiResult
{
    /// <summary>
    /// Flag indicating api call has been successfull.
    /// </summary>
    [Key(0)] public bool IsSuccess { get; set; }

    /// <summary>
    /// Api result status code.
    /// </summary>
    [Key(1)] public ApiResultStatusCode StatusCode { get; set; }

    /// <summary>
    /// Api result errrors.
    /// </summary>
    [Key(2)] public IEnumerable<string> Errors { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResult"/> class.
    /// </summary>
    public ApiResult()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ApiResult"/> class.
    /// </summary>
    public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, IEnumerable<string> erros = null)
    {
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Errors = erros;
    }
}

/// <summary>
/// Generic typed api result returning in controllers.
/// </summary>
[MessagePackObject]
public class ApiResult<TData> : ApiResult
    where TData : class
{
    /// <summary>
    /// Api result data.
    /// </summary>
    [Key(3)] public TData Data { get; set; }
}