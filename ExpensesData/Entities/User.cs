using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpensesData.Entities;

[Table("users")]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    public long  ChatId { get; set; }
    public int Step { get; set; }
    
    [MaxLength(50)]
    [Required]
    public string?  Name { get; set; }

    [MaxLength(50)]
    public string? UserName { get; set; }

    [MaxLength(50)]
    public string?  Phone { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    public int? RoomId { get; set; }
    
    [ForeignKey(nameof(RoomId))]
    public Room? Room { get; set; }
    public bool isAdmin { get; set; }
    
    //it's the same as list
    public ICollection<Outlay>? Outlays { get; set; }

    public string? FullName => UserName ?? Name;
    
}