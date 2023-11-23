using Microsoft.AspNetCore.Mvc;
using xyzboutique.api.Helper;
using xyzboutique.businesslayer.Core;
using xyzboutique.businesslayer.Manager.OrderManagement;
using xyzboutique.businesslayer.Manager.RoleManagement;
using xyzboutique.common.BusinessObjects.Security;
using xyzboutique.common.Configuration;
using xyzboutique.common.Core;
using xyzboutique.common.Facade;
using xyzboutique.Common.Configuration;

namespace xyzboutique.api.Controllers;

// [ProducesResponseType(typeof(CheckStatus), 401)]
// [ProducesResponseType(500)]
[Route("v1/orders")]
[ApiController]
public class OrderController : BaseController
{
  IOrderManager _manager;
  ILogger<OrderController> _logger;
  public OrderController(IOrderManager manager, ILogger<OrderController> logger) : base(manager, logger)
  {
    _manager = manager;
    _logger = logger;
  }
  [HttpGet]
  public IActionResult Get(int state = 0, string texto = "", int page = 1, string orderby = "", int quantity = 0)
  {
    try
    {
      RoleQueryInput input = new RoleQueryInput();
      input.State = state;
      input.Text = texto;
      input.Page = page;
      input.OrderBy = orderby;
      input.Quantity = quantity;
      // input.IdUser = WebHelper.GetToken(Request);
      DataQuery data = _manager.Search(input);
      if (data.ApiState.Equals(Status.Error))
      {
        return NotFound(data);
      }
      else
      {
        return Ok(data);
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex);
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

  [HttpPost("/ChangeStatus")]
  [ProducesResponseType(typeof(CheckStatus), 201)]
  [ProducesResponseType(typeof(CheckStatus), 404)]
  public IActionResult ChangeStatus(string idOrder)
  {
    try
    {
      CheckStatus checkStatus = null;
      if (ModelState.IsValid)
      {
        checkStatus = _manager.ChangeOrderStatus(idOrder);

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
}