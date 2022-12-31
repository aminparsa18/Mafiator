using Mafiator.Common.Data.Dtos.Countries;

namespace Mafiator.Repository.Contracts;

/// <summary>
/// Repository provides methods to retrieve/handle country data.
/// </summary>
public interface ICountryRepository : IBaseRepository<Country>
{
    /// <summary>
    /// Retrieves all country data.
    /// </summary>
    /// <returns>List of countries</returns>
    Task<IEnumerable<CountryResult>> GetAllDtoFast();
}