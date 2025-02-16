
namespace GorodTV.Pages;

public partial class PlayerPage : ContentPage
{
    public PlayerPage()
    {
        InitializeComponent();
        Player.ShouldShowPlaybackControls = false;
    }
    
    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync("..");        
        return true;
    }
    
    protected override void OnAppearing()
    {        
        base.OnAppearing();
        PlayVideo();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Player.Stop();
    }

    public void PauseVideo()
    {
        Player.Pause();
    }

    public void PlayVideo()
    {
        Player.Play();
    }
}