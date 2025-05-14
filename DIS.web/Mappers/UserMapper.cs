using DIS.DataAccess.Entity;
using DIS.Infrastructure.Enumerations;
using DIS.Infrastructure.Utilities;
using DIS.Infrastruture.Utilities;
using DIS.Web.ViewModels;

namespace DIS.Web.Mappers
{
    public class UserMapper
    {
        public QueryOptions<User> PrepareQueryOptionForRepository(QueryOptions<User> queryOption, UserViewModel vm)
        {

            if (!string.IsNullOrEmpty(vm.name))
            {
                queryOption.FilterBy = (x => x.name.Contains(vm.name));
            }
            if (!string.IsNullOrEmpty(vm.username))
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (a => a.username.Contains(vm.username)));
            }
            if (vm.status.HasValue)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (a => a.status == vm.status));
            }
            if (vm.role_id > 0)
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (a => a.role_id == vm.role_id));
            }

            if (queryOption.SortColumnName != null)
            {
                queryOption.SortBy = new List<Func<User, object>>();
                if (queryOption.SortColumnName == "name")
                {
                    queryOption.SortBy.Add((x => x.name));
                }
                else if (queryOption.SortColumnName == "username")
                {
                    queryOption.SortBy.Add((x => x.username));
                }
                else if (queryOption.SortColumnName == "role")
                {
                    queryOption.SortBy.Add((x => x.role.name));
                }
                else
                {
                    queryOption.SortOrder = SortOrder.DESC;
                    queryOption.SortBy.Add((x => x.id));
                }
            }
            else
            {
                queryOption.SortBy.Add((x => x.id));
            }
            return queryOption;
        }
        public PagedResult<UserViewModel> MapModelToListViewMode(PagedResult<User> userList)
        {
            PagedResult<UserViewModel> vmList = new PagedResult<UserViewModel>();
            foreach (var user in userList.data)
            {
                UserViewModel vm = new UserViewModel();
                vm.id = user.id;
                vm.name = user.name;
                vm.username = user.username;
                vm.role_id = user.role_id;
                if (user.role != null)
                {
                    vm.role_name = user.role.name;
                }
                vm.status = user.status;
                vmList.data.Add(vm);
            }
            vmList.total = userList.total;
            return vmList;
        }
        public User? MapViewModelToModel(User? user, UserViewModel vm)
        {
            if (user != null)
            {
                user.name = vm.name;
                user.username = vm.username;
                if (vm.status.HasValue)
                {
                    user.status = vm.status.Value;
                }
                if (vm.role_id > 0)
                {
                    user.role_id = vm.role_id.Value;
                }
            }
            return user;
        }
        public UserViewModel MapModelToViewModel(User? user, UserViewModel vm)
        {
            if (user != null)
            {
                vm.id = user.id;
                vm.name = user.name;
                vm.username = user.username;
                vm.role_id = user.role_id;
                vm.password = user.password;
                if (user.role != null)
                {
                    vm.role_name = user.role.name;
                }
                vm.status = user.status;
            }
            return vm;
        }

    }
}
