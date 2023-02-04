using FastEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Avatars;

namespace Mafiator.FastApi.Endpoints.Avatars;

public class AvatarsEndpoint : EndpointWithoutRequest<ApiResult<IEnumerable<AvatarResult>>>
{
    public override void Configure()
    {
        Post("v1/avatars"+"ddd");
        //Get(ApiUrls.Avatars + "ddd");
        Version(2);
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d =>
        {
            d.Produces(200);
          
        });
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var avatars = new List<AvatarResult>();
        await SendAsync(new ApiResult<IEnumerable<AvatarResult>>
        {
            Data = avatars,
            IsSuccess = true
        }, cancellation: ct);
    }
}