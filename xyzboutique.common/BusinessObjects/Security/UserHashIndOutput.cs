using System;
using System.Collections.Generic;
using System.Text;
using xyzboutique.common.Core;

namespace xyzboutique.common.BusinessObjects.Security;
public class UserHashIndOutput : SingleQuery
{
    public string hash { get; set; }
    public string salt { get; set; }

    public UserHashIndOutput()
    {
        hash = string.Empty;
        salt = string.Empty;
    }
}