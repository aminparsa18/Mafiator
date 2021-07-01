using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MafiatorApp.Models;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.ViewModels.Base.Interfaces;
using Xamarin.Essentials;

namespace MafiatorApp.ViewModels
{
    public class MenuViewModel : ViewModelBase
    {
        private HomeMenuItem selectedItem;

        public HomeMenuItem SelectedItem
        {
            get => selectedItem;
            set
            {
                selectedItem = value;
                RaisePropertyChanged(() => SelectedItem);
            }
        }

        public ObservableCollection<HomeMenuItem> MenuItems { get; set; }
        public IAsyncCommand SelectionChangedCommand { get; set; }
        public MenuViewModel()
        {
            MenuItems = new ObservableCollection<HomeMenuItem>
            {
                new HomeMenuItem {Id = MenuItemType.Profile, Title = "پروفایل", ImageUrl = "user.png"},
                new HomeMenuItem {Id = MenuItemType.Wallet, Title = "کیف پول", ImageUrl = "wallet.png"},
                new HomeMenuItem {Id = MenuItemType.Instruction, Title = "راهنمای بازی", ImageUrl = "instruction.png"},
                new HomeMenuItem {Id = MenuItemType.Events, Title = "رویداد ها", ImageUrl = "events.png"},
                new HomeMenuItem {Id = MenuItemType.Invitation, Title = "دعوت از دوستان", ImageUrl = "invitation.png"},
                new HomeMenuItem {Id = MenuItemType.Rule, Title = "قوانین و مقررات", ImageUrl = "rules.png"},
                new HomeMenuItem {Id = MenuItemType.About, Title = "درباره ما", ImageUrl = "about.png"},
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