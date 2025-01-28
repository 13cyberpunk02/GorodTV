using GorodTV.ModelViews;

namespace GorodTV.Pages;

public partial class ChannelPage : ContentPage
{
    public ChannelPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is ChannelViewModel vm)
        {
            vm.LoadChannelsCommand.Execute(null);
        }
    }    

    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync("..");        
        return true;    
    }
}