using System.Collections.Generic;
using System.Threading.Tasks;
using Mafiator.Data.Dtos;
using Mafiator.Entities;

namespace Mafiator.Repository.Contracts
{
    public interface IVoteRepository : IRepository<Vote>
    {
        Task<int> Validate(string gameId);
        Task<IEnumerable<VoteValidateDto>> GetNonValidatedTargets(string gameId);
        Task<IEnumerable<VoteStatusDto>> GetVoteStatus(string gameId);
    }
}