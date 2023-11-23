using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using xyzboutique.dataaccess.Core;

namespace xyzboutique.dataaccess.Core;
public interface IDbFactory
{
  DataContext GetDataContext { get; }
}