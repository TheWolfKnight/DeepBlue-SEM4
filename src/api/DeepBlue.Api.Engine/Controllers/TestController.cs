
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DeepBlue.Api.Engine.Controllers;

[EnableCors(CorsPolicies.AllowMoveValidator)]
public class TestController : ControllerBase
{

}
