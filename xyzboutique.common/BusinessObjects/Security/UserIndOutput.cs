using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using xyzboutique.common.Core;

namespace xyzboutique.common.BusinessObjects.Security;
public class UserIndOutput : SingleQuery
{
    public string id { get; set; }
    public string code { get; set; }
    public string name { get; set; }
    public string idRole { get; set; }
    public string role { get; set; }
    public string email { get; set; }
    public string state { get; set; }
    public string dateUpdate { get; set; }
    public string phone { get; set; }

    public UserIndOutput()
    {
        id = string.Empty;
        code = string.Empty;
        name = string.Empty;
        idRole = string.Empty;
        role = string.Empty;
        email = string.Empty;
        state = string.Empty;
        dateUpdate = string.Empty;
        phone = string.Empty;
    }
}