using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using xyzboutique.common.Core;

namespace xyzboutique.dataaccess.Core;
public interface IRepository
{
  IQueryable<T> All<T>() where T : class;
  void Create<T>(T TObject) where T : class;
  void Delete<T>(T TObject) where T : class;
  void Delete<T>(Expression<Func<T, bool>> predicate) where T : class;
  void Update<T>(T TObject) where T : class;

  IEnumerable<T> Filter<T>(Expression<Func<T, bool>> predicate) where T : class;
  IEnumerable<T> Filter<T>(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 50) where T : class;
  T Find<T>(Expression<Func<T, bool>> predicate) where T : class;
  T Single<T>(Expression<Func<T, bool>> expression) where T : class;

  bool Contains<T>(Expression<Func<T, bool>> predicate) where T : class;

  void ExecuteProcedure(string procedureCommand, object id);
  void ExecuteProcedure(string procedureCommand, params SqlParameter[] sqlParams);

  IList<T> ExecuteProcedureQuery<T>(string procedureCommand, object id) where T : class;
  IList<T> ExecuteProcedureQuery<T>(string procedureCommand, params SqlParameter[] sqlParams) where T : class;

  DataQuery ExecuteProcedureQuery(string procedureCommand, DataQueryInput input, string entityError);
  DataQuery ExecuteProcedureQuery(string procedureCommand, SqlParameter[] sqlParams, string entityError);


  T ExecuteProcedureSingle<T>(string procedureCommand, object id) where T : class;
  T ExecuteProcedureSingle<T>(string procedureCommand, params SqlParameter[] sqlParams) where T : class;

  object ExecuteFuncion(string functionCommand, params SqlParameter[] sqlParams);
  object ExecuteProcedureScalar(string ExecuteProcedureScalar, SqlParameter[] parameters);
}