using System.Collections.Generic;
using MessagePack;
using Newtonsoft.Json;

namespace MafiatorApp.Models.Api
{
    [MessagePackObject]
   public class ApiResult
    {
        [Key(0)]
        public bool IsSuccess { get; set; }
        [Key(1)]
        public ApiResultStatusCode StatusCode { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [Key(2)]
        public IEnumerable<string> Errors { get; set; }
    }
    [MessagePackObject]
    public class ApiResult<TData> : ApiResult
        where TData : class
    {
        [Key(3)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public TData Data { get; set; }
    }
}
