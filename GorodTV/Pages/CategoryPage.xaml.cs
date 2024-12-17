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
            if (!vm.Categories.Any())
            {
                vm.LoadCategoriesCommand.Execute(null);
            }
        }
    }
}