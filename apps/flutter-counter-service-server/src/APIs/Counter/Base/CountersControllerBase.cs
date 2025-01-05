using FlutterCounterService.APIs;
using FlutterCounterService.APIs.Common;
using FlutterCounterService.APIs.Dtos;
using FlutterCounterService.APIs.Errors;
using Microsoft.AspNetCore.Mvc;

namespace FlutterCounterService.APIs;

[Route("api/[controller]")]
[ApiController()]
public abstract class CountersControllerBase : ControllerBase
{
    protected readonly ICountersService _service;

    public CountersControllerBase(ICountersService service)
    {
        _service = service;
    }

    /// <summary>
    /// Create one Counter
    /// </summary>
    [HttpPost()]
    public async Task<ActionResult<Counter>> CreateCounter(CounterCreateInput input)
    {
        var counter = await _service.CreateCounter(input);

        return CreatedAtAction(nameof(Counter), new { id = counter.Id }, counter);
    }

    /// <summary>
    /// Delete one Counter
    /// </summary>
    [HttpDelete("{Id}")]
    public async Task<ActionResult> DeleteCounter([FromRoute()] CounterWhereUniqueInput uniqueId)
    {
        try
        {
            await _service.DeleteCounter(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find many Counters
    /// </summary>
    [HttpGet()]
    public async Task<ActionResult<List<Counter>>> Counters(
        [FromQuery()] CounterFindManyArgs filter
    )
    {
        return Ok(await _service.Counters(filter));
    }

    /// <summary>
    /// Meta data about Counter records
    /// </summary>
    [HttpPost("meta")]
    public async Task<ActionResult<MetadataDto>> CountersMeta(
        [FromQuery()] CounterFindManyArgs filter
    )
    {
        return Ok(await _service.CountersMeta(filter));
    }

    /// <summary>
    /// Get one Counter
    /// </summary>
    [HttpGet("{Id}")]
    public async Task<ActionResult<Counter>> Counter([FromRoute()] CounterWhereUniqueInput uniqueId)
    {
        try
        {
            return await _service.Counter(uniqueId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update one Counter
    /// </summary>
    [HttpPatch("{Id}")]
    public async Task<ActionResult> UpdateCounter(
        [FromRoute()] CounterWhereUniqueInput uniqueId,
        [FromQuery()] CounterUpdateInput counterUpdateDto
    )
    {
        try
        {
            await _service.UpdateCounter(uniqueId, counterUpdateDto);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Connect multiple Users records to Counter
    /// </summary>
    [HttpPost("{Id}/users")]
    public async Task<ActionResult> ConnectUsers(
        [FromRoute()] CounterWhereUniqueInput uniqueId,
        [FromQuery()] UserWhereUniqueInput[] usersId
    )
    {
        try
        {
            await _service.ConnectUsers(uniqueId, usersId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Disconnect multiple Users records from Counter
    /// </summary>
    [HttpDelete("{Id}/users")]
    public async Task<ActionResult> DisconnectUsers(
        [FromRoute()] CounterWhereUniqueInput uniqueId,
        [FromBody()] UserWhereUniqueInput[] usersId
    )
    {
        try
        {
            await _service.DisconnectUsers(uniqueId, usersId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }

    /// <summary>
    /// Find multiple Users records for Counter
    /// </summary>
    [HttpGet("{Id}/users")]
    public async Task<ActionResult<List<User>>> FindUsers(
        [FromRoute()] CounterWhereUniqueInput uniqueId,
        [FromQuery()] UserFindManyArgs filter
    )
    {
        try
        {
            return Ok(await _service.FindUsers(uniqueId, filter));
        }
        catch (NotFoundException)
        {
            return NotFound();
        }
    }

    /// <summary>
    /// Update multiple Users records for Counter
    /// </summary>
    [HttpPatch("{Id}/users")]
    public async Task<ActionResult> UpdateUsers(
        [FromRoute()] CounterWhereUniqueInput uniqueId,
        [FromBody()] UserWhereUniqueInput[] usersId
    )
    {
        try
        {
            await _service.UpdateUsers(uniqueId, usersId);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
