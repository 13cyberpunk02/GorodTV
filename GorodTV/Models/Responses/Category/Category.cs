namespace GorodTV.Models.Responses.Category;

public record Category(
    string Id, 
    string Name, 
    string Icon);

public record CategoriesList(List<Category> Categories);