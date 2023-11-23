using xyzboutique.businesslayer.Manager.HelperManagement;

namespace xyzboutique.businesslayer.Core;
public class BusinessManagerFactory
{
  IHelperManager _helperManager;
  public BusinessManagerFactory(
      IHelperManager helperManager = null)
  {
    _helperManager = helperManager;
  }
  public IHelperManager GetHelperManager()
  {
    return _helperManager;
  }
}
