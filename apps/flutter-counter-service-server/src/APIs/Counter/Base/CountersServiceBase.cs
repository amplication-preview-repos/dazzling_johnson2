using FlutterCounterService.APIs;
using FlutterCounterService.APIs.Common;
using FlutterCounterService.APIs.Dtos;
using FlutterCounterService.APIs.Errors;
using FlutterCounterService.APIs.Extensions;
using FlutterCounterService.Infrastructure;
using FlutterCounterService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace FlutterCounterService.APIs;

public abstract class CountersServiceBase : ICountersService
{
    protected readonly FlutterCounterServiceDbContext _context;

    public CountersServiceBase(FlutterCounterServiceDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one Counter
    /// </summary>
    public async Task<Counter> CreateCounter(CounterCreateInput createDto)
    {
        var counter = new CounterDbModel
        {
            CreatedAt = createDto.CreatedAt,
            UpdatedAt = createDto.UpdatedAt,
            Value = createDto.Value
        };

        if (createDto.Id != null)
        {
            counter.Id = createDto.Id;
        }
        if (createDto.Users != null)
        {
            counter.Users = await _context
                .Users.Where(user => createDto.Users.Select(t => t.Id).Contains(user.Id))
                .ToListAsync();
        }

        _context.Counters.Add(counter);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<CounterDbModel>(counter.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one Counter
    /// </summary>
    public async Task DeleteCounter(CounterWhereUniqueInput uniqueId)
    {
        var counter = await _context.Counters.FindAsync(uniqueId.Id);
        if (counter == null)
        {
            throw new NotFoundException();
        }

        _context.Counters.Remove(counter);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Counters
    /// </summary>
    public async Task<List<Counter>> Counters(CounterFindManyArgs findManyArgs)
    {
        var counters = await _context
            .Counters.Include(x => x.Users)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return counters.ConvertAll(counter => counter.ToDto());
    }

    /// <summary>
    /// Meta data about Counter records
    /// </summary>
    public async Task<MetadataDto> CountersMeta(CounterFindManyArgs findManyArgs)
    {
        var count = await _context.Counters.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one Counter
    /// </summary>
    public async Task<Counter> Counter(CounterWhereUniqueInput uniqueId)
    {
        var counters = await this.Counters(
            new CounterFindManyArgs { Where = new CounterWhereInput { Id = uniqueId.Id } }
        );
        var counter = counters.FirstOrDefault();
        if (counter == null)
        {
            throw new NotFoundException();
        }

        return counter;
    }

    /// <summary>
    /// Update one Counter
    /// </summary>
    public async Task UpdateCounter(CounterWhereUniqueInput uniqueId, CounterUpdateInput updateDto)
    {
        var counter = updateDto.ToModel(uniqueId);

        if (updateDto.Users != null)
        {
            counter.Users = await _context
                .Users.Where(user => updateDto.Users.Select(t => t).Contains(user.Id))
                .ToListAsync();
        }

        _context.Entry(counter).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Counters.Any(e => e.Id == counter.Id))
            {
                throw new NotFoundException();
            }
            else
            {
                throw;
            }
        }
    }

    /// <summary>
    /// Connect multiple Users records to Counter
    /// </summary>
    public async Task ConnectUsers(
        CounterWhereUniqueInput uniqueId,
        UserWhereUniqueInput[] childrenIds
    )
    {
        var parent = await _context
            .Counters.Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Users.Where(t => childrenIds.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();
        if (children.Count == 0)
        {
            throw new NotFoundException();
        }

        var childrenToConnect = children.Except(parent.Users);

        foreach (var child in childrenToConnect)
        {
            parent.Users.Add(child);
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Disconnect multiple Users records from Counter
    /// </summary>
    public async Task DisconnectUsers(
        CounterWhereUniqueInput uniqueId,
        UserWhereUniqueInput[] childrenIds
    )
    {
        var parent = await _context
            .Counters.Include(x => x.Users)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (parent == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Users.Where(t => childrenIds.Select(x => x.Id).Contains(t.Id))
            .ToListAsync();

        foreach (var child in children)
        {
            parent.Users?.Remove(child);
        }
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find multiple Users records for Counter
    /// </summary>
    public async Task<List<User>> FindUsers(
        CounterWhereUniqueInput uniqueId,
        UserFindManyArgs counterFindManyArgs
    )
    {
        var users = await _context
            .Users.Where(m => m.CounterId == uniqueId.Id)
            .ApplyWhere(counterFindManyArgs.Where)
            .ApplySkip(counterFindManyArgs.Skip)
            .ApplyTake(counterFindManyArgs.Take)
            .ApplyOrderBy(counterFindManyArgs.SortBy)
            .ToListAsync();

        return users.Select(x => x.ToDto()).ToList();
    }

    /// <summary>
    /// Update multiple Users records for Counter
    /// </summary>
    public async Task UpdateUsers(
        CounterWhereUniqueInput uniqueId,
        UserWhereUniqueInput[] childrenIds
    )
    {
        var counter = await _context
            .Counters.Include(t => t.Users)
            .FirstOrDefaultAsync(x => x.Id == uniqueId.Id);
        if (counter == null)
        {
            throw new NotFoundException();
        }

        var children = await _context
            .Users.Where(a => childrenIds.Select(x => x.Id).Contains(a.Id))
            .ToListAsync();

        if (children.Count == 0)
        {
            throw new NotFoundException();
        }

        counter.Users = children;
        await _context.SaveChangesAsync();
    }
}
