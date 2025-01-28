using GorodTV.ModelViews;

namespace GorodTV.Pages;

public partial class CategoryPage : ContentPage
{    
    public CategoryPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CategoryViewModel vm)
        {
            if (vm.Categories.Count <= 0)
            {
                vm.LoadCategoriesCommand.Execute(null);
            }
        }
    }

    protected override bool OnBackButtonPressed()
    {
        Application.Current.Quit();
        return true;        
    }
}