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
    public static class FileService
    {
        public static string FolderPath = ConfigManager.GetRootFolderPath();
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
        public static string ReadCMSFileAsBase64String(string fileName)
        {
            string base64Str = string.Empty;
            try
            {
                string path = Path.Join(Constants.NY_FILES_ROOT, fileName);
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
        public static bool WriteBase64StringAsFile(string fileName, string base64Data)
        {
            bool success = false;
            try
            {
                string filePath = Path.Combine(FolderPath, fileName);
                string pattern = @"^data:image\/[a-zA-Z]+;base64,";
                string result = Regex.Replace(base64Data, pattern, "");
                byte[] fileData = Convert.FromBase64String(result);
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
                File.WriteAllBytes(filePath, fileData);
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
            }
            return success;
        }
        public static bool WriteFile(string fileName, IFormFile file)
        {
            bool success = false;
            try
            {
                string filePath = Path.Combine(FolderPath, fileName);
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                }
                using (FileStream fs = System.IO.File.Create(filePath))
                {
                    file.CopyTo(fs);
                }
                success = true;
            }
            catch (Exception ex)
            {

            }
            return success;
        }
        public static FileModel SaveFormFile(IFormFile formFile)
        {
            FileModel savedFile = new FileModel();
            try
            {
                savedFile.created_date = DateTime.Now;
                savedFile.ext = formFile.ContentType;
                savedFile.name = formFile.FileName;
                var uuid = Guid.NewGuid().ToString();
                savedFile.uuid = uuid;
                savedFile.success = CreatedPhysicalFile(FolderPath, uuid, formFile);
            }
            catch (Exception err)
            {
                savedFile.success = false;
            }
            return savedFile;
        }
        public static FileModel SaveCMSFormFile(IFormFile formFile)
        {
            FileModel savedFile = new FileModel();
            try
            {
                savedFile.created_date = DateTime.Now;
                savedFile.ext = formFile.ContentType;
                savedFile.name = formFile.FileName;
                var uuid = Guid.NewGuid().ToString();
                savedFile.uuid = uuid;
                savedFile.success = CreatedCMSPhysicalFile(uuid, formFile);
            }
            catch (Exception err)
            {
                savedFile.success = false;
            }
            return savedFile;
        }
        public static void DeleteFile(string fileName)
        {
            var path = Path.Join(FolderPath, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }
        public static void DeleteCMSFile(string fileName)
        {
            var path = Path.Join(Constants.NY_FILES_ROOT, fileName);
            if (File.Exists(path))
            {
                File.Delete(path);
            }
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
        public static bool CreatedCMSPhysicalFile(string fileName, IFormFile file)
        {
            bool sucess = false;

            try
            {
                if (!Directory.Exists(Constants.NY_FILES_ROOT))
                {
                    Directory.CreateDirectory(Constants.NY_FILES_ROOT);
                }

                var path = Path.Combine(Constants.NY_FILES_ROOT, fileName);
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
    }
}
