using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xyzboutique.common.Core;
using xyzboutique.dataaccess.Core;

namespace xyzboutique.businesslayer.Core;
public interface IActionManager
{
  IUnitOfWork UnitOfWork { get; }
  DataQuery Search(DataQueryInput input);
  SingleQuery SingleById(string id, string token);

  CheckStatus Create(BaseInputEntity entity);
  CheckStatus Update(BaseInputEntity entity);
  CheckStatus Delete(BaseInputDelete entity);

  void SaveChanges();
}