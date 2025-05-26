using DIS.DataAccess.Interfaces;
using DIS.Infrastructure.Logging;
using DIS.Infrastructure.Utilities;
using DMS.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DIS.Application.Service
{
    public  class FileService
    {
        IUnitOfWork uom;
        Logger logger;

        IDisasterInfoFileRepository fileRepo;
        string rootFolder = DIS.Infrastructure.Utilities.Constants.NY_FILES_ROOT;
        public static string FolderPath = ConfigManager.GetRootFolderPath();

        public FileService(IUnitOfWork uom,
            IDisasterInfoFileRepository fileRepo)
        {
            logger = new Logger(typeof(FileService));
            this.fileRepo = fileRepo;
            this.uom = uom;
        }
        public static string ReadAsBase64String(string fileName)
        {
            string base64Str = string.Empty;
            try
            {
                string path = Path.Join(FolderPath, fileName);
                if (File.Exists(path))
                {
                    byte[] bytes = File.ReadAllBytes(path);
                    base64Str = Convert.ToBase64String(bytes);
                }
            }
            catch (Exception ex)
            {

            }
            return base64Str;
        }

        //public bool CreatedPhysicalFile(string filepath, string fileName, IFormFile file)
        //{
        //    bool sucess = false;

        //    try
        //    {
        //        if (!Directory.Exists(filepath))
        //        {
        //            Directory.CreateDirectory(filepath);
        //        }

        //        var path = Path.Combine(filepath, fileName);
        //        using (FileStream fs = System.IO.File.Create(path))
        //        {
        //            file.CopyTo(fs);
        //        }

        //        sucess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        //logger.Log(ex);
        //    }

        //    return sucess;
        //}
        public bool CreatedPhysicalFile(string filepath, string fileName, IFormFile file)
        {
            bool success = false;
            try
            {
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var path = Path.Combine(filepath, fileName);

                using (var memoryStream = new MemoryStream())
                {
                    file.CopyTo(memoryStream);
                    memoryStream.Position = 0;

                    using (FileStream fs = new FileStream(path, FileMode.Create))
                    {
                        memoryStream.CopyTo(fs);
                    }
                }

                success = true;
            }
            catch (Exception ex)
            {
                // logger.Log(ex.Message);
            }

            return success;
        }


    }


}
