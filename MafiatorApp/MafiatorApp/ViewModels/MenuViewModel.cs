using MafiatorApp.Models;
using MafiatorApp.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;

namespace MafiatorApp.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private HomeMenuItem selectedItem;

        public HomeMenuItem SelectedItem
        {
            get => selectedItem;
            set => SetProperty(ref selectedItem, value);
        }

        public ObservableCollection<HomeMenuItem> MenuItems { get; set; }
        public IAsyncCommand SelectionChangedCommand { get; set; }
        public MenuViewModel()
        {
            MenuItems = new ObservableCollection<HomeMenuItem>
            {
                new() {Id = MenuItemType.Profile, Title = "پروفایل", ImageUrl = "user.png"},
                new() {Id = MenuItemType.Wallet, Title = "کیف پول", ImageUrl = "wallet.png"},
                new() {Id = MenuItemType.Instruction, Title = "راهنمای بازی", ImageUrl = "instruction.png"},
                new() {Id = MenuItemType.Events, Title = "رویداد ها", ImageUrl = "events.png"},
                new() {Id = MenuItemType.Invitation, Title = "دعوت از دوستان", ImageUrl = "invitation.png"},
                new() {Id = MenuItemType.Rule, Title = "قوانین و مقررات", ImageUrl = "rules.png"},
                new() {Id = MenuItemType.About, Title = "درباره ما", ImageUrl = "about.png"},
            };
            SelectionChangedCommand = new AsyncCommand(SelectionChanged);
        }

        private async Task SelectionChanged()
        {
            if (SelectedItem == null) return;
            switch (SelectedItem.Id)
            {
                case MenuItemType.Rule:
                    await Launcher.OpenAsync("https://www.termsfeed.com/blog/sample-terms-of-service-template/");
                    SelectedItem = null;
                    break;
            }
        }
    }
}