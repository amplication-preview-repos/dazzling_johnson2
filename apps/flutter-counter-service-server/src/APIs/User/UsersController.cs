using Microsoft.AspNetCore.Mvc;

namespace FlutterCounterService.APIs;

[ApiController()]
public class UsersController : UsersControllerBase
{
    public UsersController(IUsersService service)
        : base(service) { }
}
