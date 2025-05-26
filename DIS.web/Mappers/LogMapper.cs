using DIS.DataAccess.Entity;
using DIS.Infrastructure.Enumerations;
using DIS.Infrastructure.Utilities;
using DIS.Infrastruture.Utilities;
using DIS.Web.ViewModels;

namespace DIS.Web.Mappers
{
    public class LogMapper
    {
        public QueryOptions<Log> PrepareQueryOptionForRepository(QueryOptions<Log> queryOption, LogViewModel vm)
        {

            if (!string.IsNullOrEmpty(vm.user_name))
            {
                queryOption.FilterBy = (x => x.User.name.Contains(vm.user_name) || x.action.Contains(vm.user_name) || x.controller.Contains(vm.user_name));
            }
            if (!string.IsNullOrEmpty(vm.action))
            {
                queryOption.FilterBy = LinqExpressionHelper.AppendAnd(queryOption.FilterBy, (a => a.action.Contains(vm.action)));
            }
            if (queryOption.SortColumnName != null)
            {
                queryOption.SortBy = new List<Func<Log, object>>();
                if (queryOption.SortColumnName == "ip")
                {
                    queryOption.SortBy.Add((x => x.ip));
                }
                else if (queryOption.SortColumnName == "action")
                {
                    queryOption.SortBy.Add((x => x.action));
                }
                else if (queryOption.SortColumnName == "url")
                {
                    queryOption.SortBy.Add((x => x.url));
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
        public PagedResult<LogViewModel> MapModelToListViewMode(PagedResult<Log> list)
        {
            PagedResult<LogViewModel> vmList = new PagedResult<LogViewModel>();
            foreach (var data in list.data)
            {
                LogViewModel vm = new LogViewModel();
                vm.id = data.id;
                vm.ip = data.ip;
                vm.action = data.action;
                vm.user_id = data.user_id;
                vm.controller = data.controller;
                vm.table = data.table;
                vm.date = string.Format("{0:dd-MM-yyyy hh:mm tt}", data.date);
                if (data.User != null)
                {
                    vm.user_name = data.User.name;
                }
                vm.url = data.url;
                vmList.data.Add(vm);
            }
            vmList.total = list.total;
            return vmList;
        }
    }
}
