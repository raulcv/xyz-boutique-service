using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using xyzboutique.common.Core;
using xyzboutique.common.Configuration;
using xyzboutique.dataaccess.Core;

namespace xyzboutique.businesslayer.Core;
class GeneralValidator
{
  CheckStatus _checkStatus;
  bool _error;
  IList<GeneralValidatorMessage> _messages;
  IConfigurationRoot Configuration { get; } = ConfigurationHelper.GetConfiguration(Directory.GetCurrentDirectory());
  IRepository _repository;
  IUnitOfWork _unitOfWork;

  public GeneralValidator(IRepository repository, IUnitOfWork unitOfWork) : base()
  {
    _repository = repository;
    _unitOfWork = unitOfWork;
    _messages = new List<GeneralValidatorMessage>();
    _error = false;
    _checkStatus = new CheckStatus(Status.Ok);
  }

  public GeneralValidator()
  {
    _messages = new List<GeneralValidatorMessage>();
    _error = false;
    _checkStatus = new CheckStatus(Status.Ok);
  }

  private void GenerateMessage<T>(T entity) where T : class
  {
    _messages = new List<GeneralValidatorMessage>();
    if (entity != null)
    {
      foreach (PropertyInfo propertyInfo in entity.GetType().GetProperties())
      {
        _messages.Add(new GeneralValidatorMessage()
        {
          key = propertyInfo.Name,
          messages = new List<string>()
        });
      }
    }
  }

  private string GetMessage(string property)
  {
    return Configuration.GetSection("Messages")[property.Replace("Menssage.", "")];
  }

  private void AddMessage(string key, string message)
  {
    string _value = string.Empty;
    //Validate Message

    if (string.IsNullOrEmpty(message))
    {
      _value = string.Format(Message.YouShouldSend, key);
    }
    else
    {
      //Look up code
      var arrayMessage = message.Split("|");

      if (arrayMessage.Length == 3)
      {
        _value = string.Format(GetMessage(arrayMessage[0]), GetMessage(arrayMessage[1]), GetMessage(arrayMessage[2]));
      }
      else if (arrayMessage.Length == 2)
      {
        _value = string.Format(GetMessage(arrayMessage[0]), GetMessage(arrayMessage[1]));
      }
      else if (arrayMessage.Length == 1)
      {
        //Validate if exists Message.
        if (arrayMessage[0].Contains("Message."))
        {
          _value = GetMessage(arrayMessage[0]);
        }
        else
        {
          _value = arrayMessage[0];
        }
      }
    }

    bool _existsKey = false;
    //Look for
    foreach (var item in _messages)
    {
      if (item.key.ToLower().Equals(key.ToLower()))
      {
        item.messages.Add(_value);
        _existsKey = true;
      }
    }

    if (!_existsKey)
    {
      _messages.Add(new GeneralValidatorMessage()
      {
        key = key,
        messages = new List<string>() { _value }
      });
    }

    _error = true;
    _checkStatus.ApiState = Status.Error;
  }

  private void UpdateCheckStatus()
  {
    if (_error)
    {
      _checkStatus.ApiState = Status.Error;

      //lista 
      StringBuilder build = new StringBuilder();


      foreach (var item in _messages.Where(p => p.messages.Any()))
      {
        foreach (var message in item.messages)
        {
          build.Append(message);
        }
      }

      _checkStatus.ApiState = build.ToString();
    }
  }

  #region public
  public void AddMessage<T>(T entity, string key, string message) where T : class
  {
    if (!_messages.Any())
    {
      GenerateMessage(entity);
    }
    string _value = string.Empty;

    //valiate message
    if (string.IsNullOrEmpty(message))
    {
      _value = string.Format(Message.YouShouldSend, key);
    }
    else
    {
      //Find code
      var arrayMessage = message.Split("|");

      if (arrayMessage.Length == 3)
      {
        _value = string.Format(GetMessage(arrayMessage[0]), GetMessage(arrayMessage[1]), GetMessage(arrayMessage[2]));
      }
      else if (arrayMessage.Length == 2)
      {
        _value = string.Format(GetMessage(arrayMessage[0]), GetMessage(arrayMessage[1]));
      }
      else if (arrayMessage.Length == 1)
      {
        //Validate if message exists.
        if (arrayMessage[0].Contains("Message."))
        {
          _value = GetMessage(arrayMessage[0]);
        }
        else
        {
          _value = arrayMessage[0];
        }
      }
    }

    bool existsKey = false;

    //buscar
    foreach (var item in _messages)
    {
      if (item.key.Equals(key))
      {
        item.messages.Add(_value);
        existsKey = true;
      }
    }

    if (!existsKey)
    {
      _messages.Add(new GeneralValidatorMessage()
      {
        key = key,
        messages = new List<string>() { _value }
      });
    }

    _error = true;
    _checkStatus.ApiState = Status.Error;
    UpdateCheckStatus();
  }

  public CheckStatus Validate<T>(T entity, string action = "") where T : class
  {
    GenerateMessage(entity);
    CheckStatus checkStatus = new CheckStatus();

    if (entity != null)
    {
      foreach (PropertyInfo propertyInfo in entity.GetType().GetProperties())
      {
        // string Type
        #region String Type
        if (propertyInfo.PropertyType == typeof(string))
        {
          object[] textAttribute = propertyInfo.GetCustomAttributes(typeof(TextValidator), true);
          if (textAttribute.Length > 0)
          {
            string value = (string)propertyInfo.GetValue(entity);
            value = string.IsNullOrEmpty(value) ? string.Empty : value;

            TextValidator validator = (TextValidator)textAttribute[0];

            if (validator.IsObligatory && string.IsNullOrEmpty(value) && string.IsNullOrEmpty(validator.Action))
            {
              AddMessage(propertyInfo.Name, validator.ObligatoryError);
            }

            if (validator.IsObligatory && string.IsNullOrEmpty(value) && !string.IsNullOrEmpty(validator.Action) && action.Equals(validator.Action))
            {
              AddMessage(propertyInfo.Name, validator.ObligatoryError);
            }

            if (validator.IsExactLength && value.Length != validator.ExactLength)
            {
              AddMessage(propertyInfo.Name, validator.ExactLengthError);
            }

            //Only Max
            if (validator.IsMinMaxLength && validator.MinLength > 0 && validator.MaxLength > 0 && value.Length < validator.MinLength && value.Length > validator.MaxLength)
            {
              AddMessage(propertyInfo.Name,
                          string.IsNullOrEmpty(validator.MinMaxLengthError) ?
                              string.Format(Message.MaxMinLengthCharacterField, propertyInfo.Name, validator.MinLength, validator.MaxLength) :
                              validator.MinMaxLengthError
                              );
            }
            else
            {
              if (validator.IsMinMaxLength && validator.MinLength > 0 && validator.MaxLength <= 0 && value.Length < validator.MinLength)
              {
                AddMessage(propertyInfo.Name,
                        string.IsNullOrEmpty(validator.MinMaxLengthError) ? string.Format(Message.MinLenghtCharacterField, propertyInfo.Name, validator.MinLength) : validator.MinMaxLengthError
                            );
              }

              if (validator.IsMinMaxLength && validator.MaxLength > 0 && validator.MinLength <= 0 && value.Length > validator.MaxLength)
              {
                AddMessage(propertyInfo.Name,
                        string.IsNullOrEmpty(validator.MinMaxLengthError) ? string.Format(Message.FieldCharacterMaxLenth, propertyInfo.Name, validator.MaxLength) : validator.MinMaxLengthError
                            );
              }
            }


            //ethical hacking
            if (!string.IsNullOrEmpty(value) && ((validator.XSS && !IsXSS(value)) || (validator.SQLInjection && !IsSQL(value))))
            {
              string messageError = string.Empty;

              if (!string.IsNullOrEmpty(validator.XSSError))
              {
                messageError = validator.XSSError;
              }
              if (!string.IsNullOrEmpty(validator.SQLInjectionError))
              {
                messageError = validator.SQLInjectionError;
              }

              if (string.IsNullOrEmpty(messageError))
              {
                messageError = string.Format(Message.NoEnterValueField, propertyInfo.Name);
              }

              AddMessage(propertyInfo.Name, messageError);
            }
          }
        }
        #endregion

        //Int Type
        #region Type Int
        if (propertyInfo.PropertyType == typeof(int) ||
            propertyInfo.PropertyType == typeof(int?)
            )
        {
          object[] intAttribute = propertyInfo.GetCustomAttributes(typeof(IntegerValidator), true);
          if (intAttribute.Length > 0)
          {
            int? value = (int?)propertyInfo.GetValue(entity);

            if (intAttribute.Length > 0)
            {
              IntegerValidator validator = (IntegerValidator)intAttribute[0];

              if (validator.IsObligatory && value == null && string.IsNullOrEmpty(validator.Action))
              {
                AddMessage(propertyInfo.Name, validator.ObligatoryError);
              }

              if (validator.IsObligatory && value == null && !string.IsNullOrEmpty(validator.Action) && action.Equals(validator.Action))
              {
                AddMessage(propertyInfo.Name, validator.ObligatoryError);
              }
            }
          }
        }
        #endregion
      }

      UpdateCheckStatus();
    }
    else
    {
      checkStatus.ApiState = Status.Error;
      checkStatus.ApiMessage = Message.YouShouldEnterValues;
    }

    return checkStatus;
  }

  public CheckStatus ValidateExistsBD(string key, string message, string consult, string table, string innerjoin = "", string where = "")
  {
    int total = 0;

    if (_repository != null)
    {
      try
      {
        SqlParameter OutputParam = new SqlParameter("@total", SqlDbType.Int);
        OutputParam.Direction = ParameterDirection.InputOutput;
        OutputParam.Value = 0;

        SqlParameter[] parameters =
        {
                        new SqlParameter("@consult",consult),
                        new SqlParameter("@entity",table),
                        new SqlParameter("@innerjoin", innerjoin),
                        new SqlParameter("@where",where),
                        OutputParam
                    };

        var data = _repository.ExecuteProcedureScalar(DataBaseQuery.ConValidadorDataExistencia,
            parameters);

        int TT = Convert.ToInt32(OutputParam.Value);

        if (OutputParam.Value != DBNull.Value)
        {
          total = Convert.ToInt32(OutputParam.Value);
        }
        if (data != null)
        {
          _error = true;
          AddMessage(key, message);
        }
      }
      catch (Exception ex)
      {
        _checkStatus.ApiState = Status.Error;
        _checkStatus.ApiMessage += "Error." + ex.Message;
      }
    }

    UpdateCheckStatus();

    return _checkStatus;
  }

  public CheckStatus ValidateNotExistsBD(string key, string message, string consult, string table, string innerjoin = "", string where = "")
  {
    int total = 0;

    if (_repository != null)
    {
      try
      {
        SqlParameter OutputParam = new SqlParameter("@total", SqlDbType.Int);
        OutputParam.Direction = ParameterDirection.InputOutput;
        OutputParam.Value = 0;

        SqlParameter[] parameters =
        {
                        new SqlParameter("@consult",consult),
                        new SqlParameter("@entity",table),
                        new SqlParameter("@innerjoin", innerjoin),
                        new SqlParameter("@where",where),
                        OutputParam
                    };

        var data = _repository.ExecuteProcedureScalar(DataBaseQuery.ConValidadorDataExistencia, parameters);

        int TT = Convert.ToInt32(OutputParam.Value);

        if (OutputParam.Value != DBNull.Value)
        {
          total = Convert.ToInt32(OutputParam.Value);
        }

        if (data == null)
        {
          _error = true;
          AddMessage(key, message);
        }

      }
      catch (Exception ex)
      {
        _checkStatus.ApiState = Status.Error;
        _checkStatus.ApiMessage += "Error." + ex.Message;
      }
    }

    UpdateCheckStatus();

    return _checkStatus;
  }

  public bool Success()
  {
    return !_error;
  }

  public CheckStatus GetStatus()
  {
    return _checkStatus;
  }

  public override string ToString()
  {
    StringBuilder build = new StringBuilder();

    if (_error)
    {
      foreach (var item in _messages.Where(p => p.messages.Any()))
      {
        foreach (var message in item.messages)
        {
          build.Append(string.Format("{0}: {1} ", item.key, message));
        }
      }
    }
    else
    {
      build.Append("OK");
    }
    return build.ToString();
  }

  #endregion


  private bool IsName(String verifyText)
  {
    var output = true;
    Regex reg = new Regex("[A-Za-zñÑ0-9ÁáÀàÉéÈèÍíÌìÓóÒòÚúÙùÑñüÜ\\s'&\\-_\"/.:]");

    MatchCollection mc = reg.Matches(verifyText);
    int lengthText = 0;
    lengthText = verifyText.Length;

    int lengthMatch = 0;
    lengthMatch = mc.Count;

    if (lengthText != lengthMatch)
    {
      output = false;
    }
    return output;
  }

  private bool IsXSS(String verifyText)
  {
    Regex reg = new Regex(@"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>");
    if (reg.IsMatch(verifyText))
    {
      return false;
    }
    return true;
  }

  private bool IsSQL(String verifyText)
  {
    string text = verifyText.ToUpper();
    Regex reg = new Regex("(DECLARE |GRANT |REVOKE|ROLLBACK|INSERT INTO|UPDATE|SELECT |WITH |DELETE|CREATE |ALTER TABLE|ALTER COLUM|ALTER ASSEMBLY|ALTER ASYMMETRIC|ALTER CERTIFICATE|ALTER CREDENTIAL|ALTER DATABASE ENCRYPTION|ALTER EVENT SESSION|ALTER SECURITY|ALTER SERVER|ALTER PROCEDURE|ALTER FUNCTION|ALTER DATABASE|FUNCTION|EXEC |EXECUTE|TABLE |DROP INDEX|DROP TABLE|DROP DATABASE|INNER JOIN|LEFT JOIN|LEFT OUTER JOIN|RIGHT JOIN|RIGHT OUTER JOIN|TRUNCATE|DATABASE|UNION ALL SELECT |GROUP BY|ORDER BY|WHERE |FROM |VIEW |SCHEMA )");
    if (reg.IsMatch(text))
    {
      return false;
    }

    if (text.IndexOf("CHAR(") >= 0)
    {
      return false;
    }
    Regex regV2 = new Regex("(UNICODE|SUBSTRING|DB_NAME|LOWER|tyNO|PRINT |SCHEMA_NAME|TABLE_NAME|TABLE_SCHEMA|INFORMATION_SCHEMA.TABLES|AUTHORIZATION|EXISTS|CONTRACT|XML SCHEMA COLLECTION|SYMMETRIC KEY|SEARCH PROPERTY LIST|FULLTEXT CATALOG|GRANT CONTROL TO|SERVER ROLE|SCHEMA_NAME|PERMISSION_SET|ALTER ASSEMBLY|ALTER ASYMMETRIC|ALTER CERTIFICATE|ALTER CREDENTIAL|ALTER DATABASE ENCRYPTION|ALTER EVENT SESSION|CURRENT_USER|PARTITION |UNION SELECT )");
    if (regV2.IsMatch(text))
    {
      return false;
    }

    if (text.IndexOf(";") >= 0 ||
        text.IndexOf("‘") >= 0 ||
        text.IndexOf("/* */") >= 0 ||
        text.IndexOf("/**/") >= 0 ||
        text.IndexOf("/*") >= 0 ||
        text.IndexOf("*/") >= 0 ||
        text.IndexOf("'") >= 0 ||
        text.IndexOf("=") >= 0 ||
        text.IndexOf("’") >= 0 ||
        text.IndexOf("--") >= 0
    )
    {
      return false;
    }
    return true;
  }
}