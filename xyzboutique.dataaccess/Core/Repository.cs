using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using xyzboutique.common.Configuration;
using xyzboutique.common.Core;
using xyzboutique.Common.Configuration;
namespace xyzboutique.dataaccess.Core;

public class Repository : IRepository
{
  DataContext _context;
  public Repository(IDbFactory dbFactory)
  {
    _context = dbFactory.GetDataContext;
  }
  public IQueryable<T> All<T>() where T : class
  {
    return _context.Set<T>().AsQueryable();
  }

  public bool Contains<T>(Expression<Func<T, bool>> predicate) where T : class
  {
    throw new NotImplementedException();
  }

  public void Create<T>(T TObject) where T : class
  {
    string codeEntity = string.Empty;

    Type _type = TObject.GetType();
    PropertyInfo propCode = _type.GetProperty("codeEntity");

    if (propCode != null)
    {
      string entidad = "";
      var nombre = typeof(T).Name;

      Type type = typeof(EntityNomenclature);

      foreach (FieldInfo field in type.GetFields())
      {
        if (field.Name.Equals(nombre))
        {
          entidad = field.GetValue(null) as string;
        }
      }

      if (!String.IsNullOrWhiteSpace(entidad))
      {

        // SqlParameter[] sqlParams = { new SqlParameter("@entidad", entidad) };
        // code = (string)ExecuteProcedureScalar(DataBaseQuery.ConCodeGenerator, sqlParams);

        propCode.SetValue(TObject, codeEntity);
      }
    }


    var newEntry = _context.Set<T>().Add(TObject);
  }

  public void Delete<T>(T TObject) where T : class
  {
    throw new NotImplementedException();
  }

  public void Delete<T>(Expression<Func<T, bool>> predicate) where T : class
  {
    throw new NotImplementedException();
  }

  public object ExecuteFuncion(string functionCommand, params SqlParameter[] sqlParams)
  {
    throw new NotImplementedException();
  }

  public void ExecuteProcedure(string procedureCommand, object id)
  {
    SqlParameter[] sqlParams =
     {
      new SqlParameter("@id", id)
     };

    var cmd = _context.Database.GetDbConnection().CreateCommand();
    cmd.CommandText = procedureCommand;
    cmd.CommandType = System.Data.CommandType.StoredProcedure;

    cmd.Parameters.Clear();
    cmd.Parameters.AddRange(sqlParams);
    _context.Database.OpenConnection();

    cmd.ExecuteNonQuery();
    _context.Database.CloseConnection();
  }

  public void ExecuteProcedure(string procedureCommand, params SqlParameter[] sqlParams)
  {
    var cmd = _context.Database.GetDbConnection().CreateCommand();

    cmd.CommandText = procedureCommand;
    cmd.CommandType = CommandType.StoredProcedure;

    cmd.Parameters.Clear();
    cmd.Parameters.AddRange(sqlParams);
    _context.Database.OpenConnection();

    cmd.ExecuteNonQuery();

    _context.Database.CloseConnection();
  }

  public IList<T> ExecuteProcedureQuery<T>(string procedureCommand, object id) where T : class
  {
    SqlParameter[] parameters = { new SqlParameter("@id", id) };
    return ExecuteProcedureQuery<T>(procedureCommand, parameters);
  }

  public IList<T> ExecuteProcedureQuery<T>(string procedureCommand, params SqlParameter[] sqlParams) where T : class
  {
    var cmd = _context.Database.GetDbConnection().CreateCommand();
    cmd.CommandText = procedureCommand;
    cmd.CommandType = System.Data.CommandType.StoredProcedure;

    cmd.Parameters.AddRange(sqlParams);
    _context.Database.OpenConnection();
    DbDataReader dr = cmd.ExecuteReader();

    var objList = new List<T>();
    var props = typeof(T).GetFilteredProperties();

    var colMapping = dr.GetColumnSchema()
      .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
      .ToDictionary(key => key.ColumnName.ToLower());

    if (dr.HasRows)
    {
      while (dr.Read())
      {
        T obj = Activator.CreateInstance<T>();
        foreach (var prop in props)
        {
          var val =
            dr.GetValue(colMapping[prop.Name.ToLower()].ColumnOrdinal.Value);
          prop.SetValue(obj, val == DBNull.Value ? null : val);
        }
        objList.Add(obj);
      }
    }
    _context.Database.CloseConnection();
    return objList;
  }

  public DataQuery ExecuteProcedureQuery(string procedureCommand, DataQueryInput input, string entityError)
  {
    SqlParameter OutputParam = new SqlParameter("@total", SqlDbType.Int);
    OutputParam.Direction = ParameterDirection.InputOutput;
    OutputParam.Value = 0;
    SqlParameter[] parameters =
    {
      new SqlParameter("@text",input.Text),
      new SqlParameter("@orderby",input.OrderBy),
      new SqlParameter("@page", input.Page),
      OutputParam
    };

    DataQuery data = new DataQuery();

    int total = 0;
    var cmd = _context.Database.GetDbConnection().CreateCommand();
    cmd.CommandText = procedureCommand;
    cmd.CommandType = System.Data.CommandType.StoredProcedure;

    cmd.Parameters.AddRange(parameters);
    _context.Database.OpenConnection();

    SqlParameter return_Value = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
    return_Value.Direction = ParameterDirection.ReturnValue;
    cmd.Parameters.Add(return_Value);

    IList<IDictionary<string, object>> objDict = new List<IDictionary<string, object>>();

    using (DbDataReader dr = cmd.ExecuteReader())
    {
      while (dr.Read())
      {
        Dictionary<string, object> objFilaDicy = new Dictionary<string, object>();

        objFilaDicy = Enumerable.Range(0, dr.FieldCount).ToDictionary(dr.GetName, dr.GetValue);
        objDict.Add(objFilaDicy);
      }
    }

    /* Output and return values are not available here yet*/
    if (OutputParam.Value != DBNull.Value)
    {
      total = Convert.ToInt32(OutputParam.Value);
    }

    if (objDict.Count != 0)
    {
      data.data = objDict;
      data.total = total;
    }
    else
    {
      data.ApiState = Status.Error;
      if (entityError != null && String.IsNullOrEmpty(entityError))
      {
        data.ApiMessage = Message.NotExistDefault;
      }
      else
      {
        data.ApiMessage = String.Format(Message.NotExists, entityError);
      }
    }
    _context.Database.CloseConnection();
    return data;
  }

  public DataQuery ExecuteProcedureQuery(string procedureCommand, SqlParameter[] sqlParams, string entityError)
  {
    SqlParameter OutputParam = new SqlParameter("@total", SqlDbType.Int);
    OutputParam.Direction = ParameterDirection.InputOutput;
    OutputParam.Value = 0;

    sqlParams = sqlParams.Append(OutputParam).ToArray();

    DataQuery data = new DataQuery();

    int total = 0;

    var cmd = _context.Database.GetDbConnection().CreateCommand();
    cmd.CommandText = procedureCommand;
    cmd.CommandType = System.Data.CommandType.StoredProcedure;

    cmd.Parameters.AddRange(sqlParams);

    _context.Database.OpenConnection();

    SqlParameter return_Value = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
    return_Value.Direction = ParameterDirection.ReturnValue;
    cmd.Parameters.Add(return_Value);

    IList<IDictionary<string, object>> objDict = new List<IDictionary<string, object>>();

    using (DbDataReader dr = cmd.ExecuteReader())
    {
      while (dr.Read())
      {
        Dictionary<string, object> objFilaDicy = new Dictionary<string, object>();

        objFilaDicy = Enumerable.Range(0, dr.FieldCount).ToDictionary(dr.GetName, dr.GetValue);
        objDict.Add(objFilaDicy);
      }
    }

    if (OutputParam.Value != DBNull.Value)
    {
      total = Convert.ToInt32(OutputParam.Value);
    }

    if (objDict.Count != 0)
    {
      data.data = objDict;
      data.total = total;
    }
    else
    {
      data.ApiState = Status.Error;
      if (entityError != null && String.IsNullOrEmpty(entityError))
      {
        data.ApiState = Message.NotExistDefault;
      }
      else
      {
        data.ApiMessage = String.Format(Message.NotExists, entityError);
      }
    }
    _context.Database.CloseConnection();
    return data;
  }

  public virtual T ExecuteProcedureSingle<T>(String procedureCommand, object id) where T : class
  {
    SqlParameter[] parameters = { new SqlParameter("@id", id) };
    return ExecuteProcedureSingle<T>(procedureCommand, parameters);
  }
  public virtual T ExecuteProcedureSingle<T>(String procedureCommand, params SqlParameter[] sqlParams) where T : class
  {

    var cmd = _context.Database.GetDbConnection().CreateCommand();
    cmd.CommandText = procedureCommand;
    cmd.CommandType = CommandType.StoredProcedure;

    cmd.Parameters.AddRange(sqlParams);

    _context.Database.OpenConnection();
    var dr = cmd.ExecuteReader();

    T obj = Activator.CreateInstance<T>();
    var props = typeof(T).GetFilteredProperties();
    Type tipo = obj.GetType();
    var colMapping = dr.GetColumnSchema()
      .Where(x => props.Any(y => y.Name.ToLower() == x.ColumnName.ToLower()))
      .ToDictionary(key => key.ColumnName.ToLower());

    if (dr.Read() && dr.FieldCount != 0)
    {
      foreach (var prop in props)
      {
        var val =
                dr.GetValue(colMapping[prop.Name.ToLower()].ColumnOrdinal.Value);
        prop.SetValue(obj, val == DBNull.Value ? null : val);
      }
    }
    else
    {
      obj = null;
    }

    while (dr.NextResult()) { /* ignore subsequent result sets here */ }
    _context.Database.CloseConnection();
    return obj;


  }

  public object ExecuteProcedureScalar(string ExecuteProcedureScalar, SqlParameter[] parameters)
  {
    throw new NotImplementedException();
  }

  public IEnumerable<T> Filter<T>(Expression<Func<T, bool>> predicate) where T : class
  {
    throw new NotImplementedException();
  }

  public IEnumerable<T> Filter<T>(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50) where T : class
  {
    throw new NotImplementedException();
  }

  public T Find<T>(Expression<Func<T, bool>> predicate) where T : class
  {
    throw new NotImplementedException();
  }

  public T Single<T>(Expression<Func<T, bool>> expression) where T : class
  {
    throw new NotImplementedException();
  }

  public void Update<T>(T TObject) where T : class
  {
    try
    {
      var entry = _context.Entry(TObject);
      _context.Set<T>().Attach(TObject);
      entry.State = EntityState.Modified;
    }
    catch (Exception ex)
    {
      throw ex;
    }
  }
}

public static class TypeExtensions
{
  public static PropertyInfo[] GetFilteredProperties(this Type type)
  {
    return type.GetProperties().Where(pi => pi.GetCustomAttributes(typeof(NotMappedAttribute), true).Length == 0).ToArray();
  }
}