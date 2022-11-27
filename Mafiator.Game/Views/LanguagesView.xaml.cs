using Mafiator.Game.ViewModels;
using Mopups.Pages;

namespace Mafiator.Game.Views;

public partial class LanguagesView : PopupPage
{

	public LanguagesView(LanguagesViewModel languagesViewModel)
	{
		InitializeComponent();
        BindingContext = languagesViewModel;// MauiApplication.Current.Services.GetService(typeof(LanguagesViewModel)) as LanguagesViewModel;
	}
}