using GorodTV.ModelViews;

namespace GorodTV.Pages;

public partial class EpgsPage : ContentPage
{
    public EpgsPage()
    {
        InitializeComponent();
    }

    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync($"..");
        return true;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is EpgsViewModel vm) 
        {
            vm.LoadEpgsCommand.Execute(null);
        }
    }
}