using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MafiatorApp.Cache;
using MafiatorApp.Dtos;
using MafiatorApp.Enums;
using MafiatorApp.Extentions;
using MafiatorApp.Models.Api;
using MafiatorApp.Views;
using Polly;
using Polly.Retry;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MafiatorApp.Services.Impl
{
    public class WebApiService : IWebApiService
    {
        [Preserve(AllMembers = true)]
        public WebApiService()
        {
        }

        public async Task<HttpResponseMessage> ConfirmPhoneNo(ConfirmPhoneDto confirmPhoneDto)
        {
            return await BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/User/ConfirmPhoneNo"), confirmPhoneDto);
        }

        public Task<HttpResponseMessage> Login(UserLoginDto userLogin)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(new Uri(Constants.BaseUrl + "api/User/Login"),
                userLogin);
        }

        public Task<HttpResponseMessage> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(new Uri(Constants.BaseUrl + "api/User/RefreshToken"),
                refreshTokenRequest);
        }

        public Task<HttpResponseMessage> RegisterUser(RegisterUserDto registerUserDto)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(new Uri(Constants.BaseUrl + "api/User/RegisterUser"),
                registerUserDto);
        }

        public async Task<HttpResponseMessage> UpdateProfilePicture(string name)
        {
            return await BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/User/UpdateProfilePicture"),
                name);
        }

        public async Task<ApiResult<UserDto>> GetUser()
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<UserDto>>(
                new Uri(Constants.BaseUrl + "api/User/GetUser"));
        }

        public async Task<ApiResult<UserStatusDto>> GetUserStatus()
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<UserStatusDto>>(
                new Uri(Constants.BaseUrl + "api/GameMember/GetMemberStatus"));
        }

        public async Task<ApiResult<ValidateUserDto>> ValidateUser(string userCode)
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<ValidateUserDto>>(
                new Uri(Constants.BaseUrl + "api/User/ValidateUser?userCode=" + userCode));
        }

        public async Task<ApiResult<IEnumerable<AvatarDto>>> GetAllAvatars()
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<AvatarDto>>>(
                new Uri(Constants.BaseUrl + "api/Avatar/GetAll"));
        }

        public async Task<ApiResult<RoomDto>> GetRoom(string roomId)
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<RoomDto>>(
                new Uri(Constants.BaseUrl + "api/Room/GetRoom?roomId=" + roomId));
        }

        public async Task<ApiResult<IEnumerable<RoomDto>>> GetRoomPage(int skip)
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<RoomDto>>>(
                new Uri(Constants.BaseUrl + "api/Room/GetPage?skip=" + skip));
        }

        public async Task<ApiResult<IEnumerable<RoomDto>>> GetMyRooms()
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<RoomDto>>>(
                new Uri(Constants.BaseUrl + "api/Room/GetMyRooms"));
        }

        public async Task<HttpResponseMessage> AddRoom(RoomCreateDto room)
        {
            return await BaseHttpClient.Instance.PostAsMessagePackAsync(new Uri(Constants.BaseUrl + "api/Room/Add"),
                room);
        }

        public async Task<HttpResponseMessage> UpdateRoomImage(UpdateRoomImageDto roomImage)
        {
            return await BaseHttpClient.Instance.PutAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/Room/UpdateRoomImage"), roomImage);
        }

        public async Task<ApiResult<string>> IsRoomJoined(string roomId)
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<string>>(
                new Uri(Constants.BaseUrl + "api/Room/IsJoined?roomId=" + roomId));
        }

        public async Task<HttpResponseMessage> AddGame(GameCreateDto game)
        {
            return await BaseHttpClient.Instance.PostAsMessagePackAsync(new Uri(Constants.BaseUrl + "api/Game/Add"),
                game);
        }

        public Task<ApiResult<IEnumerable<RoomGameDto>>> GetGamesByRoom(string roomId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<RoomGameDto>>>(
                new Uri(Constants.BaseUrl + "api/Game/GetByRoom?roomId=" + roomId));
        }

        public Task<ApiResult<IEnumerable<GameDto>>> GetAvailableGames()
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<GameDto>>>(
                new Uri(Constants.BaseUrl + "api/Game/GetAvailable"));
        }

        public Task<ApiResult<WaitingGameDto>> GetWaitingGameByRoom(string roomId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<WaitingGameDto>>(
                new Uri(Constants.BaseUrl + "api/Game/GetWaitingGameByRoom?roomId=" + roomId));
        }
        public Task<ApiResult<WaitingGameDto>> GetWaitingGameByGame(string gameId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<WaitingGameDto>>(
                new Uri(Constants.BaseUrl + "api/Game/GetWaitingGameByGame?gameId=" + gameId));
        }
        public Task<ApiResult<IEnumerable<GameRole>>> GetAllRoles()
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<GameRole>>>(
                new Uri(Constants.BaseUrl + "api/Game/GetAllRoles"));
        }

        public async Task<ApiResult<string>> IsGameJoined(string roomId)
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<string>>(
                new Uri(Constants.BaseUrl + "api/Game/IsJoined?gameId=" + roomId));
        }

        public async Task<HttpResponseMessage> JoinGame(string gameId)
        {
            return await BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/Game/Join"), gameId);
        }

        public async Task<HttpResponseMessage> LeaveGame(string gameId)
        {
            return await BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/Game/Leave"), gameId);
        }

        public async Task<ApiResult<IEnumerable<GameMemberDto>>> GetMembersOfGame(string gameId)
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<GameMemberDto>>>(
                new Uri(Constants.BaseUrl + "api/GameMember/GetByGame?gameId=" + gameId));
        }

        public async Task<ApiResult<IEnumerable<WaitingPlayerDto>>> GetWaitingPlayersByGame(string gameId)
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<WaitingPlayerDto>>>(
                new Uri(Constants.BaseUrl + "api/GameMember/GetWaitingPlayersByGame?gameId=" + gameId));
        }

        public async Task<ApiResult<IEnumerable<PlayerRoleDto>>> GetMafiaPartners(string gameId)
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<PlayerRoleDto>>>(
                new Uri(Constants.BaseUrl + "api/Game/GetMafiaPartners?gameId=" + gameId));
        }

        public async Task<ApiResult<PlayerRoleDto>> GetPlayerRole(string gameId)
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<PlayerRoleDto>>(
                new Uri(Constants.BaseUrl + "api/Game/GetRoleOfPlayer?gameId=" + gameId));
        }

        public async Task<HttpResponseMessage> AddMember(AddMemberDto member)
        {
            return await BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/RoomMember/AddMember"), member);
        }

        public async Task<HttpResponseMessage> JoinRoom(string code)
        {
            return await BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/Room/Join"), code);
        }

        public async Task<HttpResponseMessage> LeaveRoom(string code)
        {
            return await BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/Room/Leave"), code);
        }

        public async Task<ApiResult<IEnumerable<RoomMemberDto>>> GetMembersByRoom(Ulid roomId)
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<RoomMemberDto>>>(
                new Uri(Constants.BaseUrl + "api/RoomMember/GetByRoom?roomId=" + roomId));
        }

        public async Task<HttpResponseMessage> SendVotes(VoteDto vote)
        {
            return await BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/Vote/AddVotes"), vote);
        }

        public async Task<ApiResult<IEnumerable<VoteStatusDto>>> GetVotesStatus(string gameId)
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<VoteStatusDto>>>(
                new Uri(Constants.BaseUrl + "api/Vote/GetVoteStatus?gameId=" + gameId));
        }

        public async Task<HttpResponseMessage> FireGameEvent(GameEventDto gameEvent)
        {
            return await BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/GameEvent/Add"), gameEvent);
        }

        public async Task<HttpResponseMessage> Cure(GameEventDto gameEvent)
        {
            return await BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/GameEvent/Cure"), gameEvent);
        }

        public async Task<HttpResponseMessage> Inquiry(GameEventDto gameEvent)
        {
            return await BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/GameEvent/Inquiry"), gameEvent);
        }

        public async Task<ApiResult<IEnumerable<GameEventStatusDto>>> GetEventStatus(string gameId)
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<GameEventStatusDto>>>(
                new Uri(Constants.BaseUrl + "api/GameEvent/GetStatus?gameId=" + gameId));
        }
    }
}