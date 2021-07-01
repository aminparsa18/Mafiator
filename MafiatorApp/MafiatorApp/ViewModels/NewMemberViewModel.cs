using MafiatorApp.Services;
using MafiatorApp.Validations;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.ViewModels.Base.Interfaces;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using GoogleVisionBarCodeScanner;
using MafiatorApp.Dtos;
using MafiatorApp.Extentions;
using MafiatorApp.Models.Api;
using MafiatorApp.Models.PipeEvents;
using MafiatorApp.Views;
using MessagePack;
using MessagePipe;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class NewMemberViewModel : ViewModelBase
    {
        private readonly IPublisher<UpdateRoomEvent> publisher;
        public ObservableRangeCollection<ValidateUserDto> Members { get; set; }
        private ValidatableObject<string> name;

        public ValidatableObject<string> Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private bool isNameValid;

        public bool IsNameValid
        {
            get => isNameValid;
            set
            {
                isNameValid = value;
                RaisePropertyChanged(() => IsNameValid);
            }
        }

        public IAsyncCommand AddMemberCommand { get; set; }
        public IAsyncCommand PopCommand { get; set; }
        public IAsyncCommand ScanQrCommand { get; set; }
        public IAsyncCommand SearchMemberCommand { get; set; }

        private Ulid roomId;


        public NewMemberViewModel(IPublisher<UpdateRoomEvent> publisher)
        {
            this.publisher = publisher;
            Members = new ObservableRangeCollection<ValidateUserDto>();
            Name = new ValidatableObject<string>();
            AddValidations();
            AddMemberCommand = new AsyncCommand(AddMember);
            PopCommand = new AsyncCommand(Pop);
            ScanQrCommand = new AsyncCommand(ScanQr);
            SearchMemberCommand = new AsyncCommand(SearchMember);
            if (SystemConstant.Members != null && SystemConstant.Members.Any())
                Members.AddRange(SystemConstant.Members);
        }

        private async Task SearchMember()
        {
            //  if (Ulid.TryParse(Name.Value, out var id))
            //{
            try
            {
                await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Finding User...");
                var member = await WebApiService.ValidateUser(Name.Value);
                if (Members.Any(m => m.Id == member.Data.Id))
                {
                    await NavigationService.RemovePopupAsync();
                    DependencyService.Get<IAlert>()
                        .ShortAlert("User already exist in your collection", MessageType.Error);
                    return;
                }

                Members.Add(member.Data);
                Name.Value = "";
                await NavigationService.RemovePopupAsync();
            }
            catch (MessagePackSerializationException)
            {
                await NavigationService.RemovePopupAsync();
                DependencyService.Get<IAlert>().ShortAlert("No such user", MessageType.Error);
            }

            //}
            //else
            //{
            //    DependencyService.Get<IAlert>().ShortAlert("Invalid User Code", MessageType.Error);
            //}
        }


        private async Task Pop()
        {
            await NavigationService.RemovePopupAsync();
        }

        private async Task AddMember()
        {
            if (roomId == Ulid.Empty)
            {
                SystemConstant.Members = Members.ToList();
                await Pop();
                return;
            }

            if (!Members.Any())
            {
                DependencyService.Get<IAlert>().ShortAlert("No user selected", MessageType.Error);
                return;
            }
            await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Adding members...");
            var response = await WebApiService.AddMember(new AddMemberDto()
            {
                RoomId = roomId,
                Users = Members.Select(s=>s.Id).ToList()
            });

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsMessagePackAsync<ApiResult<string>>();
                if (result.IsSuccess)
                {
                    await NavigationService.RemovePopupAsync();
                    await NavigationService.RemovePopupAsync();
                    DependencyService.Get<IAlert>().ShortAlert("Member(s) joined room", MessageType.Success);
                   // MessagingCenter.Send(this, "RefreshMembers");
                   publisher.Publish(new UpdateRoomEvent());
                }
                else
                {
                    await NavigationService.RemovePopupAsync();
                    DependencyService.Get<IAlert>().ShortAlert(string.Join(',', result.Errors), MessageType.Error);
                }
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                await NavigationService.RemovePopupAsync();
                DependencyService.Get<IAlert>().ShortAlert("No such user found", MessageType.Error);
            }
            else
            {
                await NavigationService.RemovePopupAsync();
                var content = await response.Content.ReadAsStringAsync();
                DependencyService.Get<IAlert>().ShortAlert(content, MessageType.Error);
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is Ulid id)
                roomId = id;
            return base.InitializeAsync(navigationData);
        }

        private async Task ScanQr()
        {
            Methods.SetSupportBarcodeFormat(BarcodeFormats.QRCode);
            await NavigationService.NavigateToAsync<BarcodeScannerViewModel>();
        }

        private void AddValidations()
        {
            Name.Validations.Add(new IsNotNullOrEmptyRule<string> {ValidationMessage = "نام کاربری را وارد کنید"});
        }

        private bool Validate()
        {
            IsNameValid = Name.Validate();
            return IsNameValid;
        }

        public void RemoveMember(Ulid userId)
        {
            Members.Remove(Members.FirstOrDefault(m => m.Id == userId));
        }
    }
}