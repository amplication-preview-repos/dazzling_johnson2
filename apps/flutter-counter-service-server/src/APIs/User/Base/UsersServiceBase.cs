using FlutterCounterService.APIs;
using FlutterCounterService.APIs.Common;
using FlutterCounterService.APIs.Dtos;
using FlutterCounterService.APIs.Errors;
using FlutterCounterService.APIs.Extensions;
using FlutterCounterService.Infrastructure;
using FlutterCounterService.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace FlutterCounterService.APIs;

public abstract class UsersServiceBase : IUsersService
{
    protected readonly FlutterCounterServiceDbContext _context;

    public UsersServiceBase(FlutterCounterServiceDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Create one User
    /// </summary>
    public async Task<User> CreateUser(UserCreateInput createDto)
    {
        var user = new UserDbModel
        {
            CreatedAt = createDto.CreatedAt,
            Email = createDto.Email,
            FirstName = createDto.FirstName,
            LastName = createDto.LastName,
            Password = createDto.Password,
            Roles = createDto.Roles,
            UpdatedAt = createDto.UpdatedAt,
            Username = createDto.Username
        };

        if (createDto.Id != null)
        {
            user.Id = createDto.Id;
        }
        if (createDto.Counter != null)
        {
            user.Counter = await _context
                .Counters.Where(counter => createDto.Counter.Id == counter.Id)
                .FirstOrDefaultAsync();
        }

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var result = await _context.FindAsync<UserDbModel>(user.Id);

        if (result == null)
        {
            throw new NotFoundException();
        }

        return result.ToDto();
    }

    /// <summary>
    /// Delete one User
    /// </summary>
    public async Task DeleteUser(UserWhereUniqueInput uniqueId)
    {
        var user = await _context.Users.FindAsync(uniqueId.Id);
        if (user == null)
        {
            throw new NotFoundException();
        }

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Find many Users
    /// </summary>
    public async Task<List<User>> Users(UserFindManyArgs findManyArgs)
    {
        var users = await _context
            .Users.Include(x => x.Counter)
            .ApplyWhere(findManyArgs.Where)
            .ApplySkip(findManyArgs.Skip)
            .ApplyTake(findManyArgs.Take)
            .ApplyOrderBy(findManyArgs.SortBy)
            .ToListAsync();
        return users.ConvertAll(user => user.ToDto());
    }

    /// <summary>
    /// Meta data about User records
    /// </summary>
    public async Task<MetadataDto> UsersMeta(UserFindManyArgs findManyArgs)
    {
        var count = await _context.Users.ApplyWhere(findManyArgs.Where).CountAsync();

        return new MetadataDto { Count = count };
    }

    /// <summary>
    /// Get one User
    /// </summary>
    public async Task<User> User(UserWhereUniqueInput uniqueId)
    {
        var users = await this.Users(
            new UserFindManyArgs { Where = new UserWhereInput { Id = uniqueId.Id } }
        );
        var user = users.FirstOrDefault();
        if (user == null)
        {
            throw new NotFoundException();
        }

        return user;
    }

    /// <summary>
    /// Update one User
    /// </summary>
    public async Task UpdateUser(UserWhereUniqueInput uniqueId, UserUpdateInput updateDto)
    {
        var user = updateDto.ToModel(uniqueId);

        if (updateDto.Counter != null)
        {
            user.Counter = await _context
                .Counters.Where(counter => updateDto.Counter == counter.Id)
                .FirstOrDefaultAsync();
        }

        _context.Entry(user).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.Users.Any(e => e.Id == user.Id))
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
    /// Get a Counter record for User
    /// </summary>
    public async Task<Counter> GetCounter(UserWhereUniqueInput uniqueId)
    {
        var user = await _context
            .Users.Where(user => user.Id == uniqueId.Id)
            .Include(user => user.Counter)
            .FirstOrDefaultAsync();
        if (user == null)
        {
            throw new NotFoundException();
        }
        return user.Counter.ToDto();
    }
}
