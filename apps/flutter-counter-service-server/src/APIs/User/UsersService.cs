using FlutterCounterService.Infrastructure;

namespace FlutterCounterService.APIs;

public class UsersService : UsersServiceBase
{
    public UsersService(FlutterCounterServiceDbContext context)
        : base(context) { }
}
