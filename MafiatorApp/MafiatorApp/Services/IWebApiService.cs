using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using MafiatorApp.Dtos;
using MafiatorApp.Enums;
using MafiatorApp.Models.Api;
using MafiatorApp.ViewModels;

namespace MafiatorApp.Services
{
    public interface IWebApiService
    {
        //User
        Task<HttpResponseMessage> ConfirmPhoneNo(ConfirmPhoneDto confirmPhoneDto);
        Task<HttpResponseMessage> Login(UserLoginDto userLogin);
        Task<HttpResponseMessage> RefreshToken(RefreshTokenRequest refreshTokenRequest);
        Task<HttpResponseMessage> RegisterUser(RegisterUserDto registerUserDto);
        Task<HttpResponseMessage> UpdateProfile(UpdateProfileDto profile);
        Task<ApiResult<UserDto>> GetUser();
        Task<ApiResult<UserStatusDto>> GetUserStatus();
        Task<ApiResult<ValidateUserDto>> ValidateUser(string userCode);
        //Avatar
        Task<ApiResult<IEnumerable<AvatarDto>>> GetAllAvatars();

        //Room
        Task<ApiResult<RoomDto>> GetRoom(string roomId);
        Task<ApiResult<IEnumerable<RoomDto>>> GetRoomPage(int skip);
        Task<ApiResult<IEnumerable<RoomDto>>> GetMyRooms();
        Task<HttpResponseMessage> AddRoom(RoomCreateDto room);
        Task<HttpResponseMessage> UpdateRoomImage(UpdateRoomImageDto roomImage);
        Task<ApiResult<string>> IsRoomJoined(string roomId);

        //Game
        Task<HttpResponseMessage> AddGame(GameCreateDto game);
        Task<ApiResult<IEnumerable<RoomGameDto>>> GetGamesByRoom(string roomId);
        Task<ApiResult<IEnumerable<GameDto>>> GetAvailableGames();
        Task<ApiResult<WaitingGameDto>> GetWaitingGameByRoom(string roomId);
        Task<ApiResult<WaitingGameDto>> GetWaitingGameByGame(string gameId);
        Task<ApiResult<IEnumerable<GameRole>>> GetAllRoles();
        Task<ApiResult<string>> IsGameJoined(string roomId);
        Task<HttpResponseMessage> JoinGame(string gameId);
        Task<HttpResponseMessage> LeaveGame(string gameId);
        Task<ApiResult<IEnumerable<GameMemberDto>>> GetMembersOfGame(string gameId);
        Task<ApiResult<IEnumerable<WaitingPlayerDto>>> GetWaitingPlayersByGame(string gameId);
        Task<ApiResult<IEnumerable<PlayerRoleDto>>> GetMafiaPartners(string gameId);
        Task<ApiResult<PlayerRoleDto>> GetPlayerRole(string gameId);
        Task<ApiResult<IEnumerable<GameEventResultDto>>> GetNightResult(string gameId);

        //Members
        Task<HttpResponseMessage> AddMember(AddMemberDto member);
        Task<HttpResponseMessage> JoinRoom(string code);
        Task<HttpResponseMessage> LeaveRoom(string code);
        Task<ApiResult<IEnumerable<RoomMemberDto>>> GetMembersByRoom(Ulid roomId);

        //votes
        Task<HttpResponseMessage> SendVotes(VoteDto vote);
        Task<ApiResult<IEnumerable<VoteStatusDto>>> GetVotesStatus(string gameId);

        //game events
        Task<HttpResponseMessage> FireGameEvent(GameEventDto gameEvent);
        Task<HttpResponseMessage> Cure(GameEventDto gameEvent);
        Task<HttpResponseMessage> Inquiry(GameEventDto gameEvent);
        Task<ApiResult<IEnumerable<GameEventStatusDto>>> GetEventStatus(string gameId);

        //gem
        Task<ApiResult<IEnumerable<GemDto>>> GetAllGems();

        //chat
        Task<ApiResult<IEnumerable<ChatMessageDto>>> GetChatByRoom(string roomId);
    }
}
