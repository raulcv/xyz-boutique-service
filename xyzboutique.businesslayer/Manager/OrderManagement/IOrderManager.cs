using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using xyzboutique.businesslayer.Core;
using xyzboutique.common.Core;

namespace xyzboutique.businesslayer.Manager.OrderManagement;
public interface IOrderManager : IActionManager
{
  CheckStatus ChangeOrderStatus(string id);

}
