using System;
using System.Collections.Generic;
using System.Text;
using xyzboutique.common.Configuration;
using xyzboutique.common.Core;
using xyzboutique.common.Entities.Administration;

namespace xyzboutique.common.BusinessObjects.Security;
public class OrderInput : BaseInputEntity
{
    [TextValidator(IsObligatory = true, ObligatoryError = "Message.YouShouldSendA|Message.Id", Action = "update", SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 50)]
    public string id { get; set; }

    [TextValidator(SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 50)]
    public string IdCustomer { get; set; }

    [TextValidator(SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 50)]
    public string IdEmployee { get; set; }

    [TextValidator(SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 50)]
    public string IdOrderState { get; set; }

    [TextValidator(IsObligatory = true, ObligatoryError = "Message.YouShouldSendA|Message.OrderNumber", SQLInjection = true, XSS = true, IsMinMaxLength = true, MaxLength = 50)]
    public string OrderNumber { get; set; }

    public string OrderDate { get; set; }
    public string ReceptionDate { get; set; }
    public string ShippingDate { get; set; }
    public string DeliveryDate { get; set; }

    public IList<OrderProductInput> orderProductInputs { get; set; }
    public OrderInput()
    {
        id = string.Empty;
        IdCustomer = string.Empty;
        IdEmployee = string.Empty;
        IdOrderState = string.Empty;
        OrderNumber = string.Empty;

        OrderDate = string.Empty;
        ReceptionDate = string.Empty;
        ShippingDate = string.Empty;
        DeliveryDate = string.Empty;
        orderProductInputs = new List<OrderProductInput>();
    }
}