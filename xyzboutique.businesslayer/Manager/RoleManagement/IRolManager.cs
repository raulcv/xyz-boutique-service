using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using xyzboutique.businesslayer.Core;
using xyzboutique.common.Core;

namespace xyzboutique.businesslayer.Manager.RoleManagement;
public interface IRoleManager : IActionManager
{
  CheckStatus RolImport(BaseInputEntity entity);
  MemoryStream SearchExport(DataQueryInput input);
  CheckStatus RoleStatus(BaseInputEntity entity);
}
