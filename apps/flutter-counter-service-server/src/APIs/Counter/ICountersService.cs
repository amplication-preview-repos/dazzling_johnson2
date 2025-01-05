using FlutterCounterService.APIs.Common;
using FlutterCounterService.APIs.Dtos;

namespace FlutterCounterService.APIs;

public interface ICountersService
{
    /// <summary>
    /// Create one Counter
    /// </summary>
    public Task<Counter> CreateCounter(CounterCreateInput counter);

    /// <summary>
    /// Delete one Counter
    /// </summary>
    public Task DeleteCounter(CounterWhereUniqueInput uniqueId);

    /// <summary>
    /// Find many Counters
    /// </summary>
    public Task<List<Counter>> Counters(CounterFindManyArgs findManyArgs);

    /// <summary>
    /// Meta data about Counter records
    /// </summary>
    public Task<MetadataDto> CountersMeta(CounterFindManyArgs findManyArgs);

    /// <summary>
    /// Get one Counter
    /// </summary>
    public Task<Counter> Counter(CounterWhereUniqueInput uniqueId);

    /// <summary>
    /// Update one Counter
    /// </summary>
    public Task UpdateCounter(CounterWhereUniqueInput uniqueId, CounterUpdateInput updateDto);

    /// <summary>
    /// Connect multiple Users records to Counter
    /// </summary>
    public Task ConnectUsers(CounterWhereUniqueInput uniqueId, UserWhereUniqueInput[] usersId);

    /// <summary>
    /// Disconnect multiple Users records from Counter
    /// </summary>
    public Task DisconnectUsers(CounterWhereUniqueInput uniqueId, UserWhereUniqueInput[] usersId);

    /// <summary>
    /// Find multiple Users records for Counter
    /// </summary>
    public Task<List<User>> FindUsers(
        CounterWhereUniqueInput uniqueId,
        UserFindManyArgs UserFindManyArgs
    );

    /// <summary>
    /// Update multiple Users records for Counter
    /// </summary>
    public Task UpdateUsers(CounterWhereUniqueInput uniqueId, UserWhereUniqueInput[] usersId);
}
