using Mafiator.Data.Dtos.Vote;
using Mafiator.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Repository.Contracts
{
    public interface IVoteRepository : IRepository<Vote>
    {
        Task<int> Validate(string gameId);
        Task<IEnumerable<VoteValidateDto>> GetNonValidatedTargets(string gameId);
        Task<IEnumerable<VoteStatusDto>> GetVoteStatus(string gameId);
    }
}