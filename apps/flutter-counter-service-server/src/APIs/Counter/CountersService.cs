using FlutterCounterService.Infrastructure;

namespace FlutterCounterService.APIs;

public class CountersService : CountersServiceBase
{
    public CountersService(FlutterCounterServiceDbContext context)
        : base(context) { }
}
