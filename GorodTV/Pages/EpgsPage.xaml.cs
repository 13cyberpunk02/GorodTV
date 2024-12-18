using GorodTV.ModelViews;
namespace GorodTV.Pages;

public partial class EpgsPage : ContentPage
{
    private EpgsViewModel _viewModel;
    public EpgsPage()
    {
        InitializeComponent();
        _viewModel = new EpgsViewModel();
        BindingContext = _viewModel;
    }

    protected override bool OnBackButtonPressed()
    {
        _viewModel.ClearBackwardsFromEpg();
        _viewModel.GoBackToChannelsPage();
        return true;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.ClearBackwardsFromPlayer();
    }
}