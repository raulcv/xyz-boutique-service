using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using xyzboutique.businesslayer.Manager.HelperManagement;
using xyzboutique.BusinessLayer.Manager.LoginManagement;
using xyzboutique.common.BusinessObjects.Security;
using xyzboutique.common.Core;
using xyzboutique.common.Facade;
using xyzboutique.Common.Configuration;

namespace xyzboutique.api.Controllers;

// [Produces("application/json")]
// [Consumes("application/json")]
// [LoggingActionFilter]
[Route("v1/login")]
[ApiController]
public class LoginController : BaseController
{
    ILoginManager _manager;
    ILogger<LoginController> _logger;
    public LoginController(ILoginManager manager, ILogger<LoginController> logger) :
        base(manager, logger)
    {
        _manager = manager;
        _logger = logger;
    }

    [AllowAnonymous]
    [HttpPost]
    [ProducesResponseType(typeof(UserLoginIndOutput), 201)]
    [ProducesResponseType(typeof(UserLoginIndOutput), 422)]
    public IActionResult Post([FromBody] UserLoginInput input)
    {
        try
        {
            UserLoginIndOutput user = _manager.Login(input);

            if (user.ApiState.Equals(Status.Error))
            {
                return StatusCode(WebApiNames.CodeErrorProcess, user);
            }
            return StatusCode(WebApiNames.CodeCreated, user);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.SERVICE_ERROR, ex, ex.Message);
            return new EmptyResult();
        }
    }

}