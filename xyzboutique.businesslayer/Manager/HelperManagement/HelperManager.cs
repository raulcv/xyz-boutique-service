using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using xyzboutique.common.BusinessObjects.Security;
using xyzboutique.common.Configuration;
using xyzboutique.common.Core;
using xyzboutique.common.Entities.Security;

using xyzboutique.dataaccess.Core;

namespace xyzboutique.businesslayer.Manager.HelperManagement;

public class HelperManager : IHelperManager
{
  private IRepository _repository;
  private ILogger<HelperManager> _logger;
  private IUnitOfWork _unitOfWork;

  public HelperManager(
        IRepository repository,
        ILogger<HelperManager> logger,
        IUnitOfWork unitOfWork) : base()
  {
    _repository = repository;
    _logger = logger;
    _unitOfWork = unitOfWork;
  }

  public IUnitOfWork UnitOfWork
  {
    get
    {
      return _unitOfWork;
    }
  }

  #region CRUD

  public DataQuery Search(DataQueryInput input)
  {
    throw new NotImplementedException();
  }

  public SingleQuery SingleById(string id, string token)
  {
    throw new NotImplementedException();
  }

  public CheckStatus Create(BaseInputEntity entity)
  {
    throw new NotImplementedException();
  }

  public CheckStatus Update(BaseInputEntity entity)
  {
    throw new NotImplementedException();
  }

  public CheckStatus Delete(BaseInputDelete entity)
  {
    throw new NotImplementedException();
  }

  #endregion CRUD

  #region Helper date time
  public DateTime ReturnCurrentDate()
  {
    DateTime currentDateTime = new DateTime();

    currentDateTime = DateTime.UtcNow;
    return currentDateTime;
  }
  public DateTime ConvertDate(string stringdate)
  {
    DateTime dt = DateTime.ParseExact(stringdate, "d/M/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None);
    return dt;
  }
  public string ReturnCorrectDate(string date)
  {
    string convertedDate = "";
    try
    {
      date = date.Replace('/', '-');
      string[] fechaNumeros = date.Split('-');
      convertedDate = fechaNumeros[2] + "-" + fechaNumeros[1] + "-" + fechaNumeros[0];
    }
    catch (Exception)
    {
      return "";
    }

    return convertedDate;
  }
  public bool ValidateConvertDate(string stringdate)
  {
    bool valid = true;
    DateTime dt;
    if (!DateTime.TryParseExact(stringdate,
                              "d/M/yyyy",
                              CultureInfo.InvariantCulture,
                              DateTimeStyles.None,
      out dt))
    {
      valid = false;
    }

    return valid;
  }
  public string ValidateOrdering(string ordering)
  {
    string message = string.Empty;
    string[] parametroOrden = { "asc", "desc" };

    if (!string.IsNullOrEmpty(ordering))
    {
      var listado = ordering.Split(' ');
      if (listado.Length > 2)
      {
        message = Message.InvalidOrdering;
      }
      else
      {
        if (listado.Length > 1)
        {
          if (!parametroOrden.Contains(listado[1].ToLower()))
          {
            message = Message.InvalidOrdering;
          }
        }
      }
    }
    return message;
  }
  #endregion Helper date time

  #region ValidateDB
  // public int ValidateEntityRegister(string token, string entity, int type, string id)
  // {
  //   try
  //   {
  //     SqlParameter[] sqlParams =
  //     {
  //                   new SqlParameter("@token", token),
  //                   new SqlParameter("@entity", entity),
  //                   new SqlParameter("@type", type),
  //                   new SqlParameter("@id", id)
  //               };

  //     return (int)_repository.ExecuteFuncion(DataBaseQuery.SegEntidadRegistroDevolver, sqlParams);
  //   }
  //   catch (Exception ex)
  //   {
  //     _logger.LogError(string.Format("error: {0}", ex.Message));
  //   }
  //   return -1;
  // }
  public bool ValidateIdRol(string id)
  {
    return _repository.Contains<Role>
        (p => p.Id.ToString().Equals(id) && !p.Deleted);
  }
  public bool ValidateIdUser(string id)
  {
    return _repository.Contains<User>
      (p => p.Id.ToString().Equals(id) && !p.Deleted);
  }
  // public bool ValidateUserAdministrator(User user)
  // {
  //   bool valido = false;

  //   if (user.Type == 2)
  //   {
  //     valido = true;
  //   }
  //   return valido;
  // }
  public string ReturnUserEmail(string id)
  {
    string email = "";

    User user = _repository.Single<User>
        (p => p.Id.ToString().Equals(id));

    if (user != null)
    {
      email = user.Email;
    }
    return email;
  }
  public bool ReturnPasswordIsExpired(string id, int type)
  {
    bool isExpiredPassword = true;
    SqlParameter[] sqlParams =
    {
        new SqlParameter("@id", id),
        new SqlParameter("@type", type)
    };
    isExpiredPassword = (bool)_repository.ExecuteFuncion(
        DataBaseQuery.SegEsContraseniaExpiradaDevolver, sqlParams);
    return isExpiredPassword;
  }
  public bool ReturnPasswordIsRepeated(string id, string password, int type)
  {
    bool isPasswordRepeated = false;

    IList<UserHashQuery> userHash = new List<UserHashQuery>();
    SqlParameter[] sqlParams =
                {
                    new SqlParameter("@id", id),
                    new SqlParameter("@type", type)
                };

    userHash = _repository.ExecuteProcedureQuery<UserHashQuery>(
        DataBaseQuery.SegUsuarioHashBuscarPorIdTipo, sqlParams
        );

    if (userHash != null)
    {
      if (userHash.Count != 0)
      {
        foreach (var user in userHash)
        {
          if ((user.type == 1 &&
            DecryptText(user.hash, user.salt).Equals(password)) ||
            (user.type == 2 &&
            EncryptTextMD5(user.user).Equals(user.salt) &&
            EncryptTextMD5(password).Equals(user.hash)))
          {
            isPasswordRepeated = true;
          }
        }
      }
    }

    return isPasswordRepeated;
  }

  #endregion ValidateDB

  #region Password Encrypt

  public string GenerateSalt()
  {
    byte[] bytes = new byte[128 / 8];
    using (var keyGenerator = RandomNumberGenerator.Create())
    {
      keyGenerator.GetBytes(bytes);
      return BitConverter.ToString(bytes).Replace("-", "").ToLower();
    }
  }

#pragma warning disable SYSLIB0041
  public string EncryptText(string text, string salt)
  {
    string EncryptionKey = salt;  //we can change the code converstion key as per our requirement
    byte[] clearBytes = Encoding.Unicode.GetBytes(text);
    using (Aes encryptor = Aes.Create())
    {
      Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });

      encryptor.Key = pdb.GetBytes(32);
      encryptor.IV = pdb.GetBytes(16);
      using (MemoryStream ms = new MemoryStream())
      {
        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
        {
          cs.Write(clearBytes, 0, clearBytes.Length);
          cs.Close();
        }
        text = Convert.ToBase64String(ms.ToArray());
      }
    }
    return text;
  }
  public string DecryptText(string cipherText, string salt)
  {
    string EncryptionKey = salt;  //we can change the code converstion key as per our requirement, but the decryption key should be same as encryption key
    cipherText = cipherText.Replace(" ", "+");
    byte[] cipherBytes = Convert.FromBase64String(cipherText);
    using (Aes encryptor = Aes.Create())
    {
      Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
                0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
                });
      encryptor.Key = pdb.GetBytes(32);
      encryptor.IV = pdb.GetBytes(16);
      using (MemoryStream ms = new MemoryStream())
      {
        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
        {
          cs.Write(cipherBytes, 0, cipherBytes.Length);
          cs.Close();
        }
        cipherText = Encoding.Unicode.GetString(ms.ToArray());
      }
    }
    return cipherText;
  }
#pragma warning restore SYSLIB0041
  public string EncryptTextMD5(string text)
  {
    MD5 md5 = MD5.Create();
    ASCIIEncoding encoding = new ASCIIEncoding();
    StringBuilder sb = new StringBuilder();
    byte[] stream = md5.ComputeHash(encoding.GetBytes(text));
    for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
    return sb.ToString();
  }

  #endregion Password Encrypt

  #region Password Secure

  public bool SecurePasswordList(string password)
  {
    bool valid = true;
    //Bulnerable Passwords
    IList<String> vulnerables = new List<String>();
    vulnerables.Add("123456");
    vulnerables.Add("p@ssword");
    vulnerables.Add("123456789");
    vulnerables.Add("qwerty");
    vulnerables.Add("password");
    vulnerables.Add("111111");
    vulnerables.Add("12345678");
    vulnerables.Add("abc123");
    vulnerables.Add("1234567");
    vulnerables.Add("password1");
    vulnerables.Add("passwords");
    vulnerables.Add("12345");
    vulnerables.Add("master");
    vulnerables.Add("login");

    int match = 0;

    foreach (var cv in vulnerables)
    {
      if (cv == password.ToLower())
      {
        match = 1;
        break;
      }
    }

    if (match != 0)
    {
      valid = false;
    }
    return valid;
  }
  public bool SecurePasswordNumber(string passwordNotVerified)
  {
    //Words from A to Z, upper and lower cases
    //Numbers 0 al 9
    Regex numeros = new Regex(@"[0-9]");
    //No has numbers return false
    if (!numeros.IsMatch(passwordNotVerified))
    {
      return false;
    }
    //Everything is right
    return true;
  }
  public bool SecurePasswordCharacter(String passwordNotVerified)
  {
    //, * & ( ^ % $
    Regex SpecialChars = new Regex("(?=.*[@#$%^&+=*?¿¡!|])");
    //If it Not conatins special characters return false
    if (!SpecialChars.IsMatch(passwordNotVerified))
    {
      return false;
    }
    //Otherwise return true
    return true;
  }
  public bool SecurePasswordWord(String passwordNotVerified)
  {
    //Words from A a la Z, Upper and lower cases
    Regex words = new Regex(@"[a-z]");
    //If it not caontains words return false
    if (!words.IsMatch(passwordNotVerified))
    {
      return false;
    }
    return true;
  }
  public bool SecurePasswordUpperCaseWord(String passwordNotVerified)
  {
    //Words from A a la Z, Upper and lower cases
    Regex letras = new Regex(@"[A-Z]");
    //If it not caontains words return false
    if (!letras.IsMatch(passwordNotVerified))
    {
      return false;
    }
    return true;
  }
  private static string DecryptTextMD5(string text, string sKey, string sIV)
  {
    byte[] key = Encoding.ASCII.GetBytes(sKey);
    byte[] IV = Encoding.ASCII.GetBytes(sIV);

    byte[] inputBytes = Convert.FromBase64String(text);
    byte[] resultBytes = new byte[inputBytes.Length];
    string cleantext = String.Empty;
    var cripto = Aes.Create(); //raulcv
    using (MemoryStream ms = new MemoryStream(inputBytes))
    {
      using (CryptoStream objCryptoStream = new CryptoStream(ms, cripto.CreateDecryptor(key, IV), CryptoStreamMode.Read))
      {
        using (StreamReader sr = new StreamReader(objCryptoStream, true))
        {
          cleantext = sr.ReadToEnd();
        }
      }
    }
    return cleantext;
  }

  #endregion Password Secure

  #region user Account
  public string ReturnPersonNameByToken(string token)
  {
    string personName = string.Empty;
    personName = (string)_repository.ExecuteFuncion(DataBaseQuery.SegNombrePersonaPorTokenDevolver,
              new SqlParameter("@token", token)
           );
    return personName;
  }
  public string ReturnIdUserByToken(string token)
  {
    string idUser = string.Empty;
    idUser = (string)_repository.ExecuteFuncion(DataBaseQuery.SegIdUsuarioPorTokenDevolver,
              new SqlParameter("@token", token)
           );
    return idUser;
  }
  public string ReturnUserNameByToken(string token)
  {
    string idUser = string.Empty;
    idUser = (string)_repository.ExecuteFuncion(DataBaseQuery.SegNombreUsuarioPorTokenDevolver,
              new SqlParameter("@token", token)
           );
    return idUser;
  }

  #endregion user account
  public bool ValidateUserRoleState(string id)
  {
    bool state = false;

    UserRole _userRole = _repository.Single<UserRole>(p => p.IdUser.ToString().Equals(id) && !p.Deleted);

    if (_userRole != null)
    {
      Role _rol = _repository.Single<Role>(p => p.Id.ToString().Equals(_userRole.IdRole.ToString()) && !p.Deleted);
      if (_rol != null)
      {
        if (_rol.State == 1)
        {
          state = true;
        }
      }
    }

    return state;
  }


  #region Validate Text

  public bool ValidateIsXSS(string verifyText)
  {
    Regex reg = new Regex(@"</?\w+((\s+\w+(\s*=\s*(?:"".*?""|'.*?'|[^'"">\s]+))?)+\s*|\s*)/?>");
    if (reg.IsMatch(verifyText))
    {
      return false;
    }
    return true;
  }

  public bool ValidateIsSQL(string verifyText)
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
    //si cumple con todo, regresa true
    return true;
  }

  public bool ValidateTextLength(string text, int quantity)
  {
    if (text.Length > quantity)
    {
      return false;
    }
    return true;
  }
  public bool ValidateEmail(string email)
  {
    try
    {
      var addr = new System.Net.Mail.MailAddress(email);
      return addr.Address == email;
    }
    catch
    {
      return false;
    }
  }

  #endregion Validate Text

  public void SaveChanges()
  {
    _unitOfWork.SaveChanges();
  }


}
