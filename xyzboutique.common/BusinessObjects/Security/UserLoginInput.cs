using System;
using System.Collections.Generic;
using System.Text;
using xyzboutique.common.Configuration;
using xyzboutique.common.Core;

namespace xyzboutique.common.BusinessObjects.Security;
public class UserLoginInput : BaseInputEntity
{
    [TextValidator(IsObligatory = true, ObligatoryError = "Message.YouShouldSendA|Message.User", SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 40)]
    public string user { get; set; }

    [TextValidator(IsObligatory = true, ObligatoryError = "Message.YouShouldSendA|Message.Password")]
    public string password { get; set; }
}
