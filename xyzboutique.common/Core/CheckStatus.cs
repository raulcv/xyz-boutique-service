using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace xyzboutique.common.Core;
public class CheckStatus
{
  [NotMapped]
  public string Id { set; get; }
  [NotMapped]
  public string Code { set; get; }
  public string ApiState { get; set; }
  public string ApiMessage { get; set; }

  public CheckStatus(string apiState)
  {
    this.Id = string.Empty;
    this.Code = string.Empty;
    this.ApiState = apiState;
    this.ApiMessage = string.Empty;
  }
  public CheckStatus(string apiState, string apiMessage)
  {
    this.Id = string.Empty;
    this.Code = string.Empty;
    this.ApiState = apiState;
    this.ApiMessage = apiMessage;
  }
  public CheckStatus()
  {
    this.Id = "0";
    this.ApiState = Status.Error;
    this.Code = string.Empty;
    this.ApiMessage = string.Empty;
  }
  public CheckStatus(string id, string code, string apiState, string apiMessage)
  {
    this.Id = id;
    this.Code = code;
    this.ApiState = apiState;
    this.ApiMessage = apiMessage;
  }
  public CheckStatus(Dictionary<string, object> diccionario)
  {
    this.Id = (string)(diccionario["id"]);
    this.Code = (string)diccionario["code"];
    this.ApiState = (string)diccionario["apiState"];
    this.ApiMessage = (string)diccionario["apiMessage"];

  }
}