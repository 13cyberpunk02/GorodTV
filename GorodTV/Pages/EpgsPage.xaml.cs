using GorodTV.ModelViews;
namespace GorodTV.Pages;

public partial class EpgsPage : ContentPage
{
    private EpgsViewModel viewModel;
    public EpgsPage()
    {
        InitializeComponent();
        viewModel = new EpgsViewModel();
        BindingContext = viewModel;
    }

    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync($"///{nameof(ChannelPage)}");
        viewModel.ClearBackwardsFromEpg();
        return true;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        viewModel.ClearBackwardsFromPlayer();
    }
}