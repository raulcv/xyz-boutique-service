using System;
using System.Collections.Generic;
using System.Text;
using xyzboutique.common.Core;

namespace xyzboutique.common.BusinessObjects.Security;
public class  UserHashQuery : SingleQuery
{
  public string id { get; set; }
  public string hash { get; set; }
  public string salt { get; set; }
  public int type { get; set; }
  public string creationdate { get; set; }
  public string user { get; set; }
  public  UserHashQuery()
  {
    id = string.Empty;
    hash = string.Empty;
    salt = string.Empty;
    type = 0;
    creationdate = string.Empty;
    user = string.Empty;
  }
}
