using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FlutterCounterService.Infrastructure.Models;

[Table("Counters")]
public class CounterDbModel
{
    [Required()]
    public DateTime CreatedAt { get; set; }

    [Key()]
    [Required()]
    public string Id { get; set; }

    [Required()]
    public DateTime UpdatedAt { get; set; }

    public List<UserDbModel>? Users { get; set; } = new List<UserDbModel>();

    [Range(-999999999, 999999999)]
    public int? Value { get; set; }
}
