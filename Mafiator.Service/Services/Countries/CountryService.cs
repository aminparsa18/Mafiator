using Mafiator.Common.Data.Dtos.Countries;
using Mafiator.Repository;
using Mafiator.Service.Contracts.Countries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.Countries;

public class CountryService : ICountryService
{
    private readonly IUnitOfWork _unitOfWork;

    public CountryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public Task<IEnumerable<CountryResult>> GetAll() =>
        _unitOfWork.Country.GetAllDtoFast();
}