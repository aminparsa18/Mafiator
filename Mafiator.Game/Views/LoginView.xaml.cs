namespace Mafiator.Game.Views;

public partial class LoginView : ContentPage
{
    private bool isInit;

    public LoginView()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        //LazyVideo.LoadViewAsync();
        Dispatcher.Dispatch(() =>
        {
            var a = new Animation
            {
                {0, 1, new Animation(v => Logo.TranslationY = v, 200, 0, Easing.SpringOut)},
            };
            a.Commit(
                Logo,
                "flip1",
                length: 1200);
        });
    }

    private void RegisterBtn_OnClicked(object sender, EventArgs e)
    {
        RegisterBtn.BackgroundColor = Colors.Black;
        LoginBtn.BackgroundColor = Color.FromArgb("#232228");
        if (isInit)
        {
            RegisterPanel.IsVisible = true;
            RegisterPanel.FadeTo(1, 400, Easing.Linear);
            LoginPanel.FadeTo(0, 400, Easing.Linear);
            LoginPanel.IsVisible = false;
            return;
        }

        RegisterPanel.IsVisible = true;
        Dispatcher.Dispatch(() =>
        {
            var a = new Animation
            {
                {0, 1, new Animation(v => Body.TranslationY = v, 0, -120, Easing.SpringOut)},
                {0, 1, new Animation(v => Logo.RotationX = v, 0, 360, Easing.Linear)},
                {0, 1, new Animation(v => TitlePanel.Opacity = v, 1, 0, Easing.Linear)},
                {0, 1, new Animation(v => RegisterPanel.Opacity = v, 0, 1, Easing.Linear)}
            };
            a.Commit(
                Logo,
                "flip2",
                length: 1200,
                finished: (x, y) => { TitlePanel.IsVisible = false; });
        });
        isInit = true;
    }

    private void LoginBtn_OnClicked(object sender, EventArgs e)
    {
        LoginBtn.BackgroundColor = Colors.Black;
        RegisterBtn.BackgroundColor = Color.FromArgb("#707070");
        if (isInit)
        {
            LoginPanel.IsVisible = true;
            LoginPanel.FadeTo(1, 400, Easing.Linear);
            RegisterPanel.IsVisible = false;
            return;
        }

        LoginPanel.IsVisible = true;
        Dispatcher.Dispatch(() =>
        {
            var a = new Animation
            {
                {0, 1, new Animation(v => Body.TranslationY = v, 0, -120, Easing.SpringOut)},
                {0, 1, new Animation(v => Logo.RotationX = v, 0, 360, Easing.Linear)},
                {0, 1, new Animation(v => TitlePanel.Opacity = v, 1, 0, Easing.Linear)},
                {0, 1, new Animation(v => LoginPanel.Opacity = v, 0, 1, Easing.Linear)},
            };
            a.Commit(
                Logo,
                "flip3",
                length: 1200,
                finished: (x, y) => { TitlePanel.IsVisible = false; });
        });
        isInit = true;
    }

    //protected override bool OnBackButtonPressed() => true;

    private void LoginPassVis_OnTapped(object sender, EventArgs e)
    {
        LoginPassEntry.IsPassword = false;
        ShowLoginPass.IsVisible = false;
        HideLoginPass.IsVisible = true;
    }

    private void LoginPassInvis_OnTapped(object sender, EventArgs e)
    {
        LoginPassEntry.IsPassword = true;
        ShowLoginPass.IsVisible = true;
        HideLoginPass.IsVisible = false;
    }

    private void RegisterPassInvis_OnTapped(object sender, EventArgs e)
    {
        RegisterPassEntry.IsPassword = true;
        ShowRegisterPass.IsVisible = true;
        HideRegisterPass.IsVisible = false;
    }

    private void RegisterPassVis_OnTapped(object sender, EventArgs e)
    {
        RegisterPassEntry.IsPassword = false;
        ShowRegisterPass.IsVisible = false;
        HideRegisterPass.IsVisible = true;
    }

    private void ConfRegisterPassInvis_OnTapped(object sender, EventArgs e)
    {
        ConfPassEntry.IsPassword = true;
        ShowRegisterConfirmPass.IsVisible = true;
        HideRegisterConfirmPass.IsVisible = false;
    }

    private void ConfRegisterPassVis_OnTapped(object sender, EventArgs e)
    {
        ConfPassEntry.IsPassword = false;
        ShowRegisterConfirmPass.IsVisible = false;
        HideRegisterConfirmPass.IsVisible = true;
    }
}