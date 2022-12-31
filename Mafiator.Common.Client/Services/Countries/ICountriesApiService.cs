using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Avatars;
using Mafiator.Common.Data.Dtos.Countries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.Countries;

/// <summary>
/// API service provides methods to retrieve/handle countries.
/// </summary>
public interface ICountriesApiService
{
    /// <summary>
    /// Retrieves all country data.
    /// </summary>
    /// <returns></returns>
    Task<ApiResult<IEnumerable<CountryResult>>> GetAllCountries();
}