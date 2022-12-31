using Mafiator.Common.Client.Constants;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Countries;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.Countries;

/// <inheritdoc/>
public class CountriesApiService : ICountriesApiService
{
    /// <inheritdoc/>
    public Task<ApiResult<IEnumerable<CountryResult>>> GetAllCountries() =>
        BaseHttpClient.Instance.GetFromMemoryPackAsync<ApiResult<IEnumerable<CountryResult>>>(new Uri($"{UrlConstants.BaseUrl}countries"));
}