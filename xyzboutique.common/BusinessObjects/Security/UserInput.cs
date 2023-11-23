using System;
using System.Collections.Generic;
using System.Text;
using xyzboutique.common.Configuration;
using xyzboutique.common.Core;

namespace xyzboutique.common.BusinessObjects.Security;
public class UserInput : BaseInputEntity
{
    [TextValidator(IsObligatory = true, ObligatoryError = "Message.YouShouldSendA|Message.Id", Action = "update", SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 50)]
    public string id { get; set; }

    [TextValidator(IsObligatory = true, ObligatoryError = "Message.YouShouldSendA|Message.Code", SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 50)]
    public string Code { get; set; }

    [TextValidator(IsObligatory = true, ObligatoryError = "Message.YouShouldSendA|Message.Name", XSS = true, SQLInjection = true, IsMinMaxLength = true, MaxLength = 40)]
    public string name { get; set; }
    [TextValidator(IsObligatory = true, ObligatoryError = "Message.YouShouldSendA|Message.Password", XSS = true, SQLInjection = true, IsMinMaxLength = true, MaxLength = 100)]
    public string password { get; set; }

    [TextValidator(SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 50)]
    public string idRole { get; set; }

    [TextValidator(SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 250)]
    public string email { get; set; }

    [StateValidaor(Code = 1055, MessageError = "Message.YouShouldSelectAValid|Message.State")]
    public int state { get; set; }
    public bool changePassowrd { get; set; }

    [TextValidator(SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 50)]
    public string phone { get; set; }


    public UserInput()
    {
        id = string.Empty;
        name = string.Empty;
        // type = 0;
        state = 0;
        password = string.Empty;
        idRole = string.Empty;
        email = string.Empty;
        changePassowrd = false;
        phone = string.Empty;
    }
}