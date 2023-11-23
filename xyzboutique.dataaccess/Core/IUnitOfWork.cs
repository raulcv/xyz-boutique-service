using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace xyzboutique.dataaccess.Core;

public interface IUnitOfWork
{
  void BeginTransaction();

  void RollbackTransaction();

  void CommitTransaction();

  void SaveChanges();
}