using GorodTV.ModelViews;

namespace GorodTV.Pages;

public partial class PlayerPage : ContentPage
{
    private readonly PlayerViewModel _viewModel;
    public PlayerPage()
    {
        InitializeComponent();
        _viewModel = new PlayerViewModel();
        BindingContext = _viewModel;
    }
    
    protected override bool OnBackButtonPressed()
    {       
        _viewModel.GoBackToEpg();
        Player.Stop();        
        return true;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Player.Play();        
    }
}