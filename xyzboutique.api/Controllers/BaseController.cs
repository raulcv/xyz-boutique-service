using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using xyzboutique.businesslayer.Core;

namespace xyzboutique.api.Controllers;
public class BaseController : Controller
{
  private IActionManager _manager;
  private ILogger _logger;
  public BaseController(IActionManager manager, ILogger logger)
  {
    _manager = manager;
    _logger = logger;
  }
  public IActionManager ActionManager { get { return _manager; } }
  public ILogger Logger { get { return _logger; } }

}