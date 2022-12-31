using Mafiator.Common.Data.Dtos.Countries;
using System.Data;

namespace Mafiator.Repository.Repositories;

/// <inheritdoc/>
public class CountryRepository : BaseRepository<Country>, ICountryRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CountryRepository"/> class.
    /// </summary>
    public CountryRepository(ApplicationDbContext context, IDbConnection connection) : base(context, connection)
    {
    }

    public Task<IEnumerable<CountryResult>> GetAllDtoFast() =>
        Connection.ExecuteQueryAsync<CountryResult>("SELECT Name,Code,Sign FROM [Country] ORDER BY Name");
}