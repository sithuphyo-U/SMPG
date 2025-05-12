using DIS.Infrastruture.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.Infrastructure.Utilities
{
    public static class Constants
    {
        public static string NY_FILES_ROOT = ConfigManager.AppSetting["FileSettings:Folder"];
        public static string XSD_FILES_ROOT = ConfigManager.AppSetting["FileSettings:XSD_FILES_ROOT"];
        public static string PGP_KEY_FILE_NAME = ConfigManager.AppSetting["FileSettings:PGP_KEY_FILE_NAME"];

        public const string SaveSucessMessage = "Successfully Saved! ";
        public const string PrintTicketSuccessMessage = "Successfully Printed! ";
        public const string DeleteSuccessMessage = "Delete Success";
        public const string IncorrectPasswordMessage = "Incorrect Password";
        public const string IncorrectUserNameMessage = "Incorrect User Name";
        public const string RoleNotFoundMessage = "Role Not Found!";
        public const string DuplicateMessage = "Dupicate!";
        public const string Create = "Create";
        public const string Update = "Update";
        public const string Delete = "Delete";
        public const string ExportExcel = "Export Excel";
        public const string CreateAction = "Create";
        public const string UpdateAction = "Update";
        public const string DeleteAction = "Delete";
        public const string ReportPrintAction = "Print Report";
        public const string LoginAction = "Login";
        public const string LogoutAction = "Logout";
        public const string UnassignCardAction = "Unassign";
        public const string PermissionDenied = "Permission denied!";
        public const string InactiveUser = "Inactive user!";
        public const string ExpiredUser = "User is expired";
        public const string InCorrectOldPassword = "Incorrect Old Password!";
        public const string ChangePasswordSuccess = "Successfully Change Password!";
        public const string UpdateReadStatus = "Update Read Status";
        public const string Updatestatus = "UpdateReadStatus";

        #region duplicate messages
        public const string DuplicateUserName = "Duplicate User Name!";
        #endregion

    }
}
