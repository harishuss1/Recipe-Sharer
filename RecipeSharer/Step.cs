namespace Recipes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Step
{
    [Key]
    public int StepId { get; set; }

    [ForeignKey("RecipeId")]
    public Recipe Recipe { get; set; }

    public int Number { get; set; }

    public string Description { get; set; }


    public Step()
    {
    }
}