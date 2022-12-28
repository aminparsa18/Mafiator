using Mafiator.Common.Data.Dtos.Avatars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Avatars;

public interface IAvatarService
{
    Task<IEnumerable<AvatarResult>> GetAll();
}