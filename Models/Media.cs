using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Interfaces.IReadable;

namespace Models;

[Table("Media")]
public class Media : IReadable
{
  [Key]
  public int Id { get; set; }

  [Column("Author")]
  [Required(ErrorMessage = "Il faut un auteur")]
  public required string Author { get; set; }

  [Column("Title")]
  [Required(ErrorMessage = "Entrez un titre")]
  public required string Title { get; set; }

  public virtual string DisplayInformation()
  {
    return $"Title: {Title}, Author: {Author}";
  }
}