namespace FlutterCounterService.APIs.Dtos;

public class CounterCreateInput
{
    public DateTime CreatedAt { get; set; }

    public string? Id { get; set; }

    public DateTime UpdatedAt { get; set; }

    public List<User>? Users { get; set; }

    public int? Value { get; set; }
}
