namespace FlutterCounterService.APIs.Dtos;

public class CounterUpdateInput
{
    public DateTime? CreatedAt { get; set; }

    public string? Id { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public List<string>? Users { get; set; }

    public int? Value { get; set; }
}
