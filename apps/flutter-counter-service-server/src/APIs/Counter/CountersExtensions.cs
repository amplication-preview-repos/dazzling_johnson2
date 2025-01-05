using FlutterCounterService.APIs.Dtos;
using FlutterCounterService.Infrastructure.Models;

namespace FlutterCounterService.APIs.Extensions;

public static class CountersExtensions
{
    public static Counter ToDto(this CounterDbModel model)
    {
        return new Counter
        {
            CreatedAt = model.CreatedAt,
            Id = model.Id,
            UpdatedAt = model.UpdatedAt,
            Users = model.Users?.Select(x => x.Id).ToList(),
            Value = model.Value,
        };
    }

    public static CounterDbModel ToModel(
        this CounterUpdateInput updateDto,
        CounterWhereUniqueInput uniqueId
    )
    {
        var counter = new CounterDbModel { Id = uniqueId.Id, Value = updateDto.Value };

        if (updateDto.CreatedAt != null)
        {
            counter.CreatedAt = updateDto.CreatedAt.Value;
        }
        if (updateDto.UpdatedAt != null)
        {
            counter.UpdatedAt = updateDto.UpdatedAt.Value;
        }

        return counter;
    }
}
