using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MafiatorApp.Dtos;
using MafiatorApp.Enums;
using MafiatorApp.Extentions;
using MafiatorApp.Models.Api;
using Xamarin.Forms.Internals;

namespace MafiatorApp.Services.Impl
{
    public class WebApiService : IWebApiService
    {
        [Preserve(AllMembers = true)]
        public WebApiService()
        {
        }

        public Task<HttpResponseMessage> ConfirmPhoneNo(ConfirmPhoneDto confirmPhoneDto)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(
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

        public Task<HttpResponseMessage> UpdateProfile(UpdateProfileDto profile)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/User/UpdateProfile"),
                profile);
        }

        public Task<ApiResult<UserDto>> GetUser()
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<UserDto>>(
                new Uri(Constants.BaseUrl + "api/User/GetUser"));
        }

        public Task<ApiResult<UserStatusDto>> GetUserStatus()
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<UserStatusDto>>(
                new Uri(Constants.BaseUrl + "api/GameMember/GetMemberStatus"));
        }

        public Task<ApiResult<ValidateUserDto>> ValidateUser(string userCode)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<ValidateUserDto>>(
                new Uri(Constants.BaseUrl + "api/User/ValidateUser?username=" + userCode));
        }

        public Task<ApiResult<IEnumerable<AvatarDto>>> GetAllAvatars()
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<AvatarDto>>>(
                new Uri(Constants.BaseUrl + "api/Avatar/GetAll"));
        }

        public Task<ApiResult<RoomDto>> GetRoom(string roomId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<RoomDto>>(
                new Uri(Constants.BaseUrl + "api/Room/GetRoom?roomId=" + roomId));
        }

        public Task<ApiResult<IEnumerable<RoomDto>>> GetRoomPage(int skip)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<RoomDto>>>(
                new Uri(Constants.BaseUrl + "api/Room/GetPage?skip=" + skip));
        }

        public Task<ApiResult<IEnumerable<RoomDto>>> GetMyRooms()
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<RoomDto>>>(
                new Uri(Constants.BaseUrl + "api/Room/GetMyRooms"));
        }

        public Task<HttpResponseMessage> AddRoom(RoomCreateDto room)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(new Uri(Constants.BaseUrl + "api/Room/Add"),
                room);
        }

        public Task<HttpResponseMessage> UpdateRoomImage(UpdateRoomImageDto roomImage)
        {
            return BaseHttpClient.Instance.PutAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/Room/UpdateRoomImage"), roomImage);
        }

        public Task<ApiResult<string>> IsRoomJoined(string roomId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<string>>(
                new Uri(Constants.BaseUrl + "api/Room/IsJoined?roomId=" + roomId));
        }

        public Task<HttpResponseMessage> AddGame(GameCreateDto game)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(new Uri(Constants.BaseUrl + "api/Game/Add"),
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

        public Task<ApiResult<string>> IsGameJoined(string roomId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<string>>(
                new Uri(Constants.BaseUrl + "api/Game/IsJoined?gameId=" + roomId));
        }

        public Task<HttpResponseMessage> JoinGame(string gameId)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/Game/Join"), gameId);
        }

        public Task<HttpResponseMessage> LeaveGame(string gameId)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/Game/Leave"), gameId);
        }

        public Task<ApiResult<IEnumerable<GameMemberDto>>> GetMembersOfGame(string gameId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<GameMemberDto>>>(
                new Uri(Constants.BaseUrl + "api/GameMember/GetByGame?gameId=" + gameId));
        }

        public Task<ApiResult<IEnumerable<WaitingPlayerDto>>> GetWaitingPlayersByGame(string gameId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<WaitingPlayerDto>>>(
                new Uri(Constants.BaseUrl + "api/GameMember/GetWaitingPlayersByGame?gameId=" + gameId));
        }

        public Task<ApiResult<IEnumerable<PlayerRoleDto>>> GetMafiaPartners(string gameId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<PlayerRoleDto>>>(
                new Uri(Constants.BaseUrl + "api/Game/GetMafiaPartners?gameId=" + gameId));
        }

        public Task<ApiResult<PlayerRoleDto>> GetPlayerRole(string gameId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<PlayerRoleDto>>(
                new Uri(Constants.BaseUrl + "api/Game/GetRoleOfPlayer?gameId=" + gameId));
        }

        public Task<ApiResult<IEnumerable<GameEventResultDto>>> GetNightResult(string gameId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<GameEventResultDto>>>(
                new Uri(Constants.BaseUrl + "api/GameEvent/GetNightResult?gameId=" + gameId));
        }

        public async Task<HttpResponseMessage> AddMember(AddMemberDto member)
        {
            return await BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/RoomMember/AddMember"), member);
        }

        public Task<HttpResponseMessage> JoinRoom(string code)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/Room/Join"), code);
        }

        public Task<HttpResponseMessage> LeaveRoom(string code)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/Room/Leave"), code);
        }

        public async Task<ApiResult<IEnumerable<RoomMemberDto>>> GetMembersByRoom(Ulid roomId)
        {
            return await BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<RoomMemberDto>>>(
                new Uri(Constants.BaseUrl + "api/RoomMember/GetByRoom?roomId=" + roomId));
        }

        public Task<HttpResponseMessage> SendVotes(VoteDto vote)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/Vote/AddVotes"), vote);
        }

        public Task<ApiResult<IEnumerable<VoteStatusDto>>> GetVotesStatus(string gameId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<VoteStatusDto>>>(
                new Uri(Constants.BaseUrl + "api/Vote/GetVoteStatus?gameId=" + gameId));
        }

        public Task<HttpResponseMessage> FireGameEvent(GameEventDto gameEvent)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/GameEvent/Add"), gameEvent);
        }

        public Task<HttpResponseMessage> Cure(GameEventDto gameEvent)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/GameEvent/Cure"), gameEvent);
        }

        public Task<HttpResponseMessage> Inquiry(GameEventDto gameEvent)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri(Constants.BaseUrl + "api/GameEvent/Inquiry"), gameEvent);
        }

        public Task<ApiResult<IEnumerable<GameEventStatusDto>>> GetEventStatus(string gameId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<GameEventStatusDto>>>(
                new Uri(Constants.BaseUrl + "api/GameEvent/GetStatus?gameId=" + gameId));
        }

        public Task<ApiResult<IEnumerable<GemDto>>> GetAllGems()
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<GemDto>>>(
                new Uri(Constants.BaseUrl + "api/Gem/GetAll"));
        }

        public Task<ApiResult<IEnumerable<ChatMessageDto>>> GetChatByRoom(string roomId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<ChatMessageDto>>>(
                new Uri(Constants.BaseUrl + "api/Chat/GetByRoom?roomId=" + roomId));
        }
    }
}