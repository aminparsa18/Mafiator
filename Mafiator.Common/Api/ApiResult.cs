using System.Collections.Generic;
using MessagePack;

namespace Mafiator.Common.Api
{
    [MessagePackObject]
    public class ApiResult
    {
        [Key(0)] public bool IsSuccess { get; set; }
        [Key(1)] public ApiResultStatusCode StatusCode { get; set; }
        [Key(2)] public IEnumerable<string> Errors { get; set; }

        public ApiResult()
        {
        }

        public ApiResult(bool isSuccess, ApiResultStatusCode statusCode, IEnumerable<string> erros = null)
        {
            IsSuccess = isSuccess;
            StatusCode = statusCode;
            Errors = erros;
        }
    }

    [MessagePackObject]
    public class ApiResult<TData> : ApiResult
        where TData : class
    {
        [Key(3)] public TData Data { get; set; }
    }
}