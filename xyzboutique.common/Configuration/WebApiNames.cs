using System;
using System.Collections.Generic;
using System.Text;

namespace xyzboutique.Common.Configuration;
public static class WebApiNames
{
    public const string role = "api/roles";
    public const string user = "api/users";
    public const string product = "api/products";
    public const string order = "api/orders";

    public const int get = 1;
    public const int post = 2;
    public const int put = 3;
    public const int delete = 4;

    public const int CodeUnauthorized = 401;
    public const int CodeNotFound = 404;
    public const int CodeErrorProcess = 422;

    public const int CodeCreated = 201;
    public const int CodeOk = 200;

}