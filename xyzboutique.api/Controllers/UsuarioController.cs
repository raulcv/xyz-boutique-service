using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xyzboutique.api.Helper;
using xyzboutique.businesslayer.Manager.UserManagement;
using xyzboutique.common.BusinessObjects.Security;
using xyzboutique.common.Configuration;
using xyzboutique.common.Core;
using xyzboutique.common.Entities.Security;
using xyzboutique.common.Facade;
using xyzboutique.Common.Configuration;

namespace xyzboutique.api.Controllers;

[ProducesResponseType(typeof(CheckStatus), 401)]
[ProducesResponseType(500)]
// [Authorize(Policy = "AutorizacionHomologado")]AutorizacionHomologado
// [LoggingActionFilter]
[Route("v1/users")]
[ApiController]
public class UserController : BaseController
{
    IUserManager _manager;
    ILogger<UserController> _logger;

    public UserController(IUserManager manager, ILogger<UserController> logger) : base(manager, logger)
    {
        _manager = manager;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(typeof(DataQuery), 200)]
    [ProducesResponseType(typeof(DataQuery), 404)]
    public IActionResult Get(int status = 0, string text = "", int page = 1, string orderBy = "", int quantity = 0,
    string user = "", string emmail = "", string idRole = "", string phoneNumber = ""
    )
    {
        try
        {
            string _idUser = WebHelper.GetToken(Request);

            UserQueryInput input = new UserQueryInput()
            {
                Text = text,
                Email = emmail,
                IdRole = idRole,
                OrderBy = orderBy,
                Page = page,
                Quantity = quantity,
                IdUser = _idUser
            };

            DataQuery data = _manager.Search(input);
            if (data.ApiState.Equals(Status.Error))
            {
                return NotFound(data);
            }
            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.SERVICE_ERROR, ex, ex.Message);
            return new EmptyResult();
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(UserIndOutput), 200)]
    [ProducesResponseType(typeof(UserIndOutput), 404)]
    public IActionResult Get(string id)
    {
        try
        {
            UserIndOutput data = (UserIndOutput)_manager.SingleById(id, WebHelper.GetToken(Request));

            if (data.ApiState.Equals(Status.Error))
            {
                return NotFound(data);
            }
            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.SERVICE_ERROR, ex, ex.Message);
            return new EmptyResult();
        }
    }


    [HttpPost]
    [ProducesResponseType(typeof(CheckStatus), 201)]
    [ProducesResponseType(typeof(CheckStatus), 404)]
    public IActionResult Post([FromBody] UserInput input)
    {
        try
        {
            CheckStatus checkStatus = null;
            if (ModelState.IsValid)
            {
                input.IdUser = WebHelper.GetToken(Request);
                checkStatus = _manager.Create(input);

                if (checkStatus.ApiState.Equals(Status.Error))
                {
                    return StatusCode(WebApiNames.CodeErrorProcess, checkStatus);
                }
                return StatusCode(WebApiNames.CodeCreated, checkStatus);
            }
            else
            {
                checkStatus = new CheckStatus(Status.Error, Message.NoValidInput);
                return StatusCode(WebApiNames.CodeErrorProcess, checkStatus);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.SERVICE_ERROR, ex, ex.Message);
            return new EmptyResult();
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(typeof(CheckStatus), 200)]
    [ProducesResponseType(typeof(CheckStatus), 404)]
    public IActionResult Put(string id, [FromBody] UserInput input)
    {
        try
        {
            CheckStatus checkStatus = null;
            if (ModelState.IsValid)
            {
                input.id = id;
                input.IdUser = WebHelper.GetToken(Request);
                checkStatus = _manager.Update(input);

                if (checkStatus.ApiState.Equals(Status.Error))
                {
                    return StatusCode(422, checkStatus);
                }

                return Ok(checkStatus);
            }
            else
            {
                checkStatus = new CheckStatus(Status.Error, Message.NoValidInput);
                return StatusCode(WebApiNames.CodeErrorProcess, checkStatus);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.SERVICE_ERROR, ex, ex.Message);
            return new EmptyResult();
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(CheckStatus), 200)]
    [ProducesResponseType(typeof(CheckStatus), 404)]
    public IActionResult Delete(string id)
    {
        try
        {
            CheckStatus checkStatus = _manager.Delete(new BaseInputDelete(id, WebHelper.GetToken(Request)));

            if (checkStatus.ApiMessage.Equals(Status.Error))
            {
                return StatusCode(WebApiNames.CodeErrorProcess, checkStatus);
            }
            return Ok(checkStatus);
        }
        catch (Exception ex)
        {
            _logger.LogError(LoggingEvents.SERVICE_ERROR, ex, ex.Message);
            return new EmptyResult();
        }
    }
}