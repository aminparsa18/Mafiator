using Mafiator.Common.Data.Dtos.Countries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Countries;

public interface ICountryService
{
    Task<IEnumerable<CountryResult>> GetAll();
}