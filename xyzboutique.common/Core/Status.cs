using System;
using System.Collections.Generic;
using System.Text;

namespace xyzboutique.common.Core;
public class Status
{
  public static string Ok = "ok";
  public static string Error = "error";
  public static string Information = "information";
  public static string Warning = "warning";
  public static string NoAuthorized = "noauthorized";

  public static string Pending = "pending";
  public static string Active = "active";
  public static string Inactive = "inactive";

  public static string Create = "create";
  public static string Update = "update";

  public static string WithoutAccess = "withoutaccess";
  public static string Deleted = "Deleted";

  public static string ApiState = "apistate";
  public static string ApiMessage = "apimessage";
  public static string Blocked = "blocked";
  public static string Get = "get";
  public static string Post = "post";
  public static string Put = "put";
  public static string Delete = "delete";
}
