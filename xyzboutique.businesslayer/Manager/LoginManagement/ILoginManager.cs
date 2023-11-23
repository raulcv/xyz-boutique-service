using System.Collections.Generic;
using System.Text;
using xyzboutique.businesslayer.Core;
using xyzboutique.common.BusinessObjects.Security;

namespace xyzboutique.BusinessLayer.Manager.LoginManagement;
public interface ILoginManager : IActionManager
{
    UserLoginIndOutput Login(UserLoginInput input);

}