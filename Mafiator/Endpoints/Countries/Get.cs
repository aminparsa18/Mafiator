using Mafiator.Common.Data.Dtos.Countries;
using Mafiator.Service.Contracts.Countries;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.Countries;

public class Get : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ApiResult<IEnumerable<CountryResult>>>
{
    private readonly ICountryService _countryService;

    public Get(ICountryService countryService)
    {
        _countryService = countryService;
    }

    [ApiVersion("1.0")]
    [HttpGet(ApiUrls.Countries)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(ApiUrls.Countries), Tags = new[] { "Countries Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<CountryResult>>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var countries = await _countryService.GetAll();
        return Ok(new ApiResult<IEnumerable<CountryResult>>
        {
            Data = countries,
            IsSuccess = true
        });
    }
}