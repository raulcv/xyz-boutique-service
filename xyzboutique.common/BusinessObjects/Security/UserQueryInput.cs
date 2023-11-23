using System;
using System.Collections.Generic;
using System.Text;
using xyzboutique.common.Core;
using xyzboutique.common.Configuration;
namespace xyzboutique.common.BusinessObjects.Security;
public class UserQueryInput : DataQueryInput
{
    [TextValidator(SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 250)]
    public string Email { get; set; }
    public string IdRole { get; set; }
    [TextValidator(SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 20)]

    public UserQueryInput()
    {
        Email = string.Empty;
        IdRole = string.Empty;
    }
}