using DIS.Application.Services.Common;
using DIS.DataAccess.Entity;
using DIS.DataAccess.Interfaces;
using DIS.Infrastructure.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.Application.Services
{
   
    public class DisasterInfoFileService : BaseService<File_TB, int, IDisasterInfoFileRepository>
    {
        public DisasterInfoFileService(IDisasterInfoFileRepository repo, IUnitOfWork _uom) : base(repo, _uom, new Logger(typeof(DisasterInfoFileService)))
        {
        }
    }
}
