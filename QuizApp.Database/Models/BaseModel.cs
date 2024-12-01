using System.ComponentModel.DataAnnotations;

namespace QuizApp.Database.Models;

public class BaseModel
{
    [Key]
    public int Id { get; set; }
}