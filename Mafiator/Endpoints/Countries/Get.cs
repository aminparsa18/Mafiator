using Mafiator.Common.Data.Dtos.Countries;
using Mafiator.Service.Contracts.Countries;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.Countries;

public class Get : EndpointWithoutRequest<ApiResult<IEnumerable<CountryResult>>>
{
    private readonly ICountryService _countryService;

    public Get(ICountryService countryService)
    {
        _countryService = countryService;
    }

    public override void Configure()
    {
        Get(ApiUrls.Countries);
        Summary(s =>
        {
            s.Summary = "Get countries";
            s.Description = "Retrieves all country data";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Countries));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var countries = await _countryService.GetAll();
        await SendMemoryPackAsync(new ApiResult<IEnumerable<CountryResult>>
        {
            Data = countries,
            IsSuccess = true
        }, cancellation: ct);
    }
}