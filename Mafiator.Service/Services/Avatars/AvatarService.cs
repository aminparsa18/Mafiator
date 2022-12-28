using Mafiator.Common.Data.Dtos.Avatars;
using Mafiator.Repository;
using Mafiator.Service.Contracts.Avatars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.Avatars;

public class AvatarService : IAvatarService
{
    private readonly IUnitOfWork _unitOfWork;

    public AvatarService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AvatarResult>> GetAll()
    {
        IEnumerable<AvatarResult> avatars = await _unitOfWork.Avatar.GetAllDtosFast();

        // Set full uri path on avatar name.
        foreach (var avatar in avatars)
        {
            avatar.Name = string.Join(Data.Constants.BlobStorageEndpoint, avatar.Name);
        }
        return avatars;
    }
}