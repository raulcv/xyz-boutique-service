using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xyzboutique.api.Helper;
public static class WebHelper
{
    public static string GetToken(HttpRequest _request)
    {
        string authHeader = _request.Headers["Authorization"];
        string token = "";
        if (authHeader != null && authHeader.StartsWith("Bearer"))
        {
            token = authHeader.Substring("Bearer ".Length).Trim();
        }
        return token;
    }
}
