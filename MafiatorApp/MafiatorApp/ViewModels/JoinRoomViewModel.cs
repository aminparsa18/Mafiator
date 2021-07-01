using System;
using System.Linq;
using System.Threading.Tasks;
using MafiatorApp.Dtos;
using MafiatorApp.Extentions;
using MafiatorApp.Models.Api;
using MafiatorApp.Services;
using MafiatorApp.Validations;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.ViewModels.Base.Interfaces;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class JoinRoomViewModel:ViewModelBase
    {
        private ValidatableObject<string> code;
        public ValidatableObject<string> Code
        {
            get => code;
            set
            {
                code = value;
                RaisePropertyChanged(() => Code);
            }
        }
        public IAsyncCommand PopCommand { get; set; }
        public IAsyncCommand JoinRoomCommand { get; set; }

        public JoinRoomViewModel()
        {
            Code=new ValidatableObject<string>();
            PopCommand = new AsyncCommand(Pop);
            JoinRoomCommand=new AsyncCommand(JoinRoom);

        }

        private async Task JoinRoom()
        {
            await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Joining Room...");
            var request = await WebApiService.JoinRoom(Code.Value);
            if (request.IsSuccessStatusCode)
            {
                var result = await request.Content.ReadAsMessagePackAsync<ApiResult<string>>();
                if (result.IsSuccess)
                {
                    await NavigationService.RemovePopupAsync();
                    await NavigationService.RemovePopupAsync();
                    await NavigationService.NavigateToAsync<MyRoomViewModel>(Ulid.Parse(result.Data));
                }
                else
                {
                    await NavigationService.RemovePopupAsync();
                    DependencyService.Get<IAlert>().ShortAlert(result.Errors.FirstOrDefault(),MessageType.Error);
                }
            }
            else
            {
                var result = await request.Content.ReadAsStringAsync();
                await NavigationService.RemovePopupAsync();
                DependencyService.Get<IAlert>().ShortAlert(result, MessageType.Error);
            }
        }

        private async Task Pop()
        {
            await NavigationService.RemovePopupAsync();
        }
    }
}
