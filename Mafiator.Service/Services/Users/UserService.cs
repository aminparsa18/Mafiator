using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Common.Data.Enums;
using Mafiator.Data.Extensions;
using Mafiator.Repository;
using Mafiator.Service.Contracts.Users;
using RepoDb;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.Users;
public class UserService : IUserService
{
    private readonly IDbConnection _dbConnection;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IDbConnection dbConnection, IUnitOfWork unitOfWork)
    {
        _dbConnection = dbConnection;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResult<UserDetailsResult>> GetDetails(string userId)
    {
        var users = await _dbConnection.ExecuteQueryAsync<UserDetailsResult>(
            "SELECT TOP 1 [Image],[Score],[DisplayName],[CountryCode] FROM [Users] WHERE Id = @id",
            new { id = userId });
        if (!users.Any())
        {
            return new ApiResult<UserDetailsResult>()
            {
                StatusCode = ApiResultStatusCode.Unauthorized,
                Errors = new[] { "User does not exist" }
            };
        }
        var user = users.FirstOrDefault();
        user.Image = string.Join(Data.Constants.BlobStorageEndpoint, user.Image);
        return new ApiResult<UserDetailsResult>()
        {
            IsSuccess = true,
            Data = user
        };
    }

    public async Task<ApiResult<UserStatusResult>> GetStatus(string userId)
    {
        var res = await _unitOfWork.GameMember.GetUserStatusFast(userId);
        var status = new UserStatusResult();
        var totalMafia = res.Count(s => s.GameRole.IsMafia());
        var winMafia = res.Count(s => s.GameRole.IsMafia() && s.GameStatus == GameStatus.MafiaWin);
        status.MafiaWin = totalMafia == 0 ? 0 : winMafia / totalMafia;
        var totalCitizen = res.Count(s => s.GameRole.IsCitizen());
        var winCitizen = res.Count(s => s.GameRole.IsCitizen() && s.GameStatus == GameStatus.CitizenWin);
        status.CitizenWin = totalCitizen == 0 ? 0 : winCitizen / totalCitizen;
        status.TotalWin = winMafia + winCitizen;

        return new ApiResult<UserStatusResult>
        {
            IsSuccess = true,
            Data = status
        };
    }
}