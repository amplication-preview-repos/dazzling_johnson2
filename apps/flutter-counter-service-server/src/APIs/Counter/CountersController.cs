using Microsoft.AspNetCore.Mvc;

namespace FlutterCounterService.APIs;

[ApiController()]
public class CountersController : CountersControllerBase
{
    public CountersController(ICountersService service)
        : base(service) { }
}
