namespace GorodTV.Models.Responses.Category;

public class Category
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Icon { get; set; }
}
    

public class CategoriesList
{
   public required List<Category> Categories { get; set; } 
}