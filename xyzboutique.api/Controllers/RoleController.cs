using Microsoft.AspNetCore.Mvc;
using xyzboutique.businesslayer.Core;
using xyzboutique.businesslayer.Manager.RoleManagement;
using xyzboutique.common.BusinessObjects.Security;
using xyzboutique.common.Core;

namespace xyzboutique.api.Controllers;

// [ProducesResponseType(typeof(CheckStatus), 401)]
// [ProducesResponseType(500)]
[Route("v1/roles")]
[ApiController]
public class RoleController : BaseController
{
  IRoleManager _manager;
  ILogger<RoleController> _logger;
  public RoleController(IRoleManager manager, ILogger<RoleController> logger) : base(manager, logger)
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
}