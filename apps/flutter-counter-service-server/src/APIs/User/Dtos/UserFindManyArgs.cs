using FlutterCounterService.APIs.Common;
using FlutterCounterService.Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace FlutterCounterService.APIs.Dtos;

[BindProperties(SupportsGet = true)]
public class UserFindManyArgs : FindManyInput<User, UserWhereInput> { }
