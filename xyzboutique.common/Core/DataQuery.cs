using Newtonsoft.Json;
using System.Collections.Generic;

namespace xyzboutique.common.Core;

public class DataQuery
{
  public string ApiState { get; set; }
  public string ApiMessage { get; set; }
  public IList<IDictionary<string, object>>? data { get; set; }
  public int total { get; set; }
  public DataQuery()
  {
    data = new List<IDictionary<string, object>>();
    this.ApiState = Status.Ok;
    total = 0;
    this.ApiMessage = string.Empty;
  }

  public DataQuery(string ApiStatus, string ApiMessage)
  {
    data = new List<IDictionary<string, object>>();
    total = 0;
    this.ApiState = ApiStatus;
    this.ApiMessage = ApiMessage;
  }
  public DataQuery(Dictionary<string, object> diccionario)
  {
    this.ApiState = (string)diccionario["ApiState"];
    this.ApiMessage = (string)diccionario["ApiMessage"];
    long total = (long)(diccionario["total"]);
    this.total = (int)total;

    this.data = JsonConvert.DeserializeObject<IList<IDictionary<string, object>>>(diccionario["data"].ToString());
  }
}