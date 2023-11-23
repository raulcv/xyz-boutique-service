using System;

namespace xyzboutique.common.Configuration;

public class StateValidaor : Attribute
{
    public int Code { get; set; }
    public string MessageError { get; set; }
    public StateValidaor()
    {
        Code = 0;
        MessageError = string.Empty;
    }
}
