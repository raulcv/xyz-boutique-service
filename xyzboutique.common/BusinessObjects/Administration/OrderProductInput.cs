using System;
using System.Collections.Generic;
using System.Text;
using xyzboutique.common.Configuration;
using xyzboutique.common.Core;
using xyzboutique.common.Entities.Administration;

namespace xyzboutique.common.BusinessObjects.Security;
public class OrderProductInput : BaseInputEntity
{
    [TextValidator(IsObligatory = true, ObligatoryError = "Message.YouShouldSendA|Message.Id", Action = "update", SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 50)]
    public string id { get; set; }

    [TextValidator(SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 50)]
    public string IdProduct { get; set; }

    [TextValidator(SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 50)]
    public string IdOrder { get; set; }
    
    public OrderProductInput()
    {
        id = string.Empty;
        IdProduct = string.Empty;
        IdOrder = string.Empty;
    }
}