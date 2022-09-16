using Mafiator.Common.Data.Dtos.Avatars;
using System.Collections.Generic;

namespace Mafiator.Repository.Contracts;

/// <summary>
/// Repository provides methods to retrieve/handle avatar data.
/// </summary>
public interface IAvatarRepository : IBaseRepository<Avatar>
{
    /// <summary>
    /// Retrieves all avatars data.
    /// </summary>
    /// <returns>List of avatars</returns>
    Task<IEnumerable<AvatarResult>> GetAllDtosFast();

    /// <summary>
    /// Retrieves all avatars data.
    /// </summary>
    /// <returns>List of avatars</returns>
    Task<List<AvatarResult>> GetAllDtos();
}