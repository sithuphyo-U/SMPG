using DIS.DataAccess.Entity;
using DIS.DataAccess.Interfaces;
using DIS.Web.ViewModels;
using DIS.DataAccess.Entity.Settings;
using DIS.DataAccess.Interfaces.Settings;
using DIS.Infrastructure.Enumerations;
using DIS.Infrastructure.Utilities;
using DIS.Web.ViewModels;
using DIS.Web.ViewModels;
using Newtonsoft.Json;

namespace DIS.Web.Mappers
{
    public class ProgramCodeMapper
    {
        public static List<ProgramCodeViewModel> MapModelToListViewModel(List<ProgramCode> list, IRoleRepository _roleRepo)
        {
            List<ProgramCodeViewModel> vmList = new List<ProgramCodeViewModel>();
            List<Role> roleList = _roleRepo.Get().Where(x => x.deleted == false).ToList();
            foreach (var item in list)
            {
                ProgramCodeViewModel vm = new ProgramCodeViewModel();
                vm.id = item.id;
                vm.program_name = item.program_name;
                vm.program_code = item.program_code;
                vm.url = item.url;
                vm.icon = item.icon;
                vm.parent_id = item.parent_id;
                if (!string.IsNullOrEmpty(item.permission))
                {
                    List<PermissionViewModel>? permissionList = JsonConvert.DeserializeObject<List<PermissionViewModel>>(item.permission);
                    if (permissionList != null)
                    {
                        int count = permissionList.Count;
                        foreach (var role in roleList)
                        {
                            PermissionViewModel? permission = permissionList.Where(x => x.role_id == role.id).FirstOrDefault();
                            if (permission != null)
                            {
                                vm.permissions.Add(permission);
                            }
                            else
                            {
                                count++;
                                PermissionViewModel permissionView = new PermissionViewModel();
                                permissionView.id = count;
                                permissionView.program_name = item.program_name;
                                permissionView.program_code_id = item.id;
                                permissionView.role_id = role.id;
                                permissionView.role_name = role.name;
                                permissionView.read = false;
                                permissionView.write = false;
                                permissionView.delete = false;
                                vm.permissions.Add(permissionView);
                            }
                        }
                    }
                    else
                    {
                        int count = permissionList.Count;
                        foreach (var role in roleList)
                        {
                            count++;
                            PermissionViewModel permissionView = new PermissionViewModel();
                            permissionView.id = count;
                            permissionView.program_name = item.program_name;
                            permissionView.program_code_id = item.id;
                            permissionView.role_id = role.id;
                            permissionView.role_name = role.name;
                            permissionView.read = false;
                            permissionView.write = false;
                            permissionView.delete = false;
                            vm.permissions.Add(permissionView);
                        }
                    }
                }
                else
                {
                    int count = 0;
                    foreach (var role in roleList)
                    {
                        count++;
                        PermissionViewModel permissionView = new PermissionViewModel();
                        permissionView.id = count;
                        permissionView.program_name = item.program_name;
                        permissionView.program_code_id = item.id;
                        permissionView.role_id = role.id;
                        permissionView.role_name = role.name;
                        permissionView.read = false;
                        permissionView.write = false;
                        permissionView.delete = false;
                        vm.permissions.Add(permissionView);
                    }
                }
                vmList.Add(vm);
            }
            return vmList;
        }
        public static List<PermissionViewModel> MapModelToPermissionViewModel(List<ProgramCode> list, IRoleRepository _roleRepo, int role_id)
        {
            List<PermissionViewModel> vmList = new List<PermissionViewModel>();
            Role? role = _roleRepo.Get(role_id);
            int count = 0;
            foreach (var item in list)
            {
                var vm = new PermissionViewModel();
                if (item != null)
                {


                    if (!string.IsNullOrEmpty(item.permission))
                    {
                        List<PermissionViewModel>? pVm = JsonConvert.DeserializeObject<List<PermissionViewModel>>(item.permission);
                        vm = pVm.Where(x => x.role_id == role_id).FirstOrDefault();
                        if (vm == null)
                        {
                            vm = new PermissionViewModel();
                            vm.id = pVm.Count + 1;
                            vm.program_name = item.program_name;
                            vm.program_code_id = item.id;
                            vm.role_id = role.id;
                            vm.role_name = role.name;
                            vm.read = false;
                            vm.write = false;
                            vm.delete = false;
                        }
                    }
                    else
                    {
                        count++;
                        vm = new PermissionViewModel();
                        vm.id = count;
                        vm.program_name = item.program_name;
                        vm.program_code_id = item.id;
                        vm.role_id = role.id;
                        vm.role_name = role.name;
                        vm.read = false;
                        vm.write = false;
                        vm.delete = false;
                    }
                }
                vmList.Add(vm);
            }
            return vmList;
        }
    }
}

