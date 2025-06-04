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
    
    public class ImageFileService : BaseService<Image, int, IImageRepository>
    {
        public ImageFileService(IImageRepository repo, IUnitOfWork _uom) : base(repo, _uom, new Logger(typeof(ImageFileService)))
        {
        }
    }
}
