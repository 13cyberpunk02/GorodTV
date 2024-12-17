using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        Shell.Current.GoToAsync($"///{nameof(CategoryPage)}");
        return true;    
    }
}