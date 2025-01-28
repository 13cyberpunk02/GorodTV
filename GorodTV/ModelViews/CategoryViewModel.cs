using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GorodTV.Models.Responses.Category;
using GorodTV.Pages;
using GorodTV.Services;
using GorodTV.Services.Interfaces;

namespace GorodTV.ModelViews;

public partial class CategoryViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<Category> _categories;
    
    [ObservableProperty]
    private Category _selectedCategory;
    
    private readonly IRestService _restService;
    public IAsyncRelayCommand LoadCategoriesCommand { get; }

    public CategoryViewModel()
    {
        Categories = new ObservableCollection<Category>();
        LoadCategoriesCommand = new AsyncRelayCommand(LoadCategoriesAsync);
        _restService = new RestService();
    }

    private async Task LoadCategoriesAsync()
    {
        var categories = await _restService.GetCategoriesRequest();

        if (!categories.Categories.Any())
        {
            await Shell.Current.DisplayAlert("Ошибка", "Похоже, что вы не авторизованы", "OK");
            return;
        }
        Categories.Clear();
        foreach (var category in categories.Categories)
        {
            Categories.Add(category);
        }
    }

    [RelayCommand]
    private async Task SelectCategoryAsync()
    {
        if(SelectedCategory is null)
            return;

        var parameters = new Dictionary<string, object>
        {
            { "categoryId", SelectedCategory.Id },
            { "categoryName", SelectedCategory.Name }
        };
        SelectedCategory = null;
        await Shell.Current.GoToAsync("category/channel", parameters);
    }

    partial void OnSelectedCategoryChanged(Category value)
    {
        if (value is not null)
        {
            SelectCategoryCommand.Execute(null);
        }
    }
}
