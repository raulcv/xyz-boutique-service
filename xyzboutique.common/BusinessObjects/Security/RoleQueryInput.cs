using System;
using System.Collections.Generic;
using System.Text;
using xyzboutique.common.Core;

namespace xyzboutique.common.BusinessObjects.Security;
public class RoleQueryInput : DataQueryInput
{
  public int State { get; set; }

  public RoleQueryInput()
  {
    this.State = 0;
  }
}
