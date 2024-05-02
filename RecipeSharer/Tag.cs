namespace Recipes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Tag
{
    [Key]
    public int TagId {get;set;}

    // [InverseProperty("Tags")]
    public List<Recipe> TaggedRecipes {get; set;}

    public string Name {get; set;}

    public Tag(){
    }
}