using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using xyzboutique.common.Core;

namespace xyzboutique.common.BusinessObjects.Security;
public class UserLoginIndOutput : SingleQuery
{
    public string id { get; set; }
    public string code { get; set; }
    public string user { get; set; }
    public string email { get; set; }
    public string state { get; set; }
    public string role { get; set; }

    [NotMapped]
    public string token { get; set; }
    public bool isExpiredPassword { get; set; }

    public UserLoginIndOutput()
    {
        id = string.Empty;
        code = string.Empty;
        user = string.Empty;
        email = string.Empty;
        role = string.Empty;
        token = string.Empty;
        isExpiredPassword = false;
    }
}