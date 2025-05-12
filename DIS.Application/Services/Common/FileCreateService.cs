using DIS.Infrastructure.Utilities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.Application.Service.Common
{
    public class FileCreateService
    {
        static string rootFolder = DIS.Infrastructure.Utilities.Constants.NY_FILES_ROOT;
        public static SaveFile SaveFormFile(IFormFile formFile)
        {
            SaveFile savedFile = new SaveFile();
            try
            {
                savedFile.created_date = DateTime.Now;
                savedFile.ext = formFile.ContentType;
                savedFile.name = formFile.FileName;
                var uuid = Guid.NewGuid().ToString();
                savedFile.uuid = uuid;
                savedFile.success = CreatedPhysicalFile(rootFolder, uuid, formFile);
            }
            catch (Exception err)
            {
                savedFile.success = false;
            }
            return savedFile;
        }
        public static bool CreatedPhysicalFile(string filepath, string fileName, IFormFile file)
        {
            bool sucess = false;

            try
            {
                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var path = Path.Combine(filepath, fileName);
                using (FileStream fs = System.IO.File.Create(path))
                {
                    file.CopyTo(fs);
                }

                sucess = true;
            }
            catch (Exception ex)
            {
                sucess = false;
            }

            return sucess;
        }
        public static string ConvertImageToBase64(string fileName)
        {
            string base64String = string.Empty;
            var path = Path.Combine(rootFolder, fileName);
            // Read the image file as a byte array
            if (System.IO.File.Exists(path))
            {
                byte[] imageBytes = System.IO.File.ReadAllBytes(path);

                // Convert byte array to Base64 string
                base64String = Convert.ToBase64String(imageBytes);
                base64String = "data:image/jpeg;base64," + base64String;

            }
            return base64String;
        }
    }
}
