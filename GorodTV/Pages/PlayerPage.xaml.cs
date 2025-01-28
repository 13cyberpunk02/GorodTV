
namespace GorodTV.Pages;

public partial class PlayerPage : ContentPage
{
    public PlayerPage()
    {
        InitializeComponent();
    }
    
    protected override bool OnBackButtonPressed()
    {
        Shell.Current.GoToAsync("..");        
        Player.Stop();                
        return true;
    }
    
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Player.Play();             
    }
}