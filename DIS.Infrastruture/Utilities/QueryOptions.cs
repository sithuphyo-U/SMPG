using DIS.Infrastructure.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DIS.Infrastructure.Utilities
{
    public class QueryOptions<TEntity>
    {
        public int legth { get; set; } = 0;
        public int Page { get; set; } = 0;
        public int RecordPerPage { get; set; } = 10;
        public string? SearchValue { get; set; }
        public string? SortColumnName { get; set; }
        public List<string>? SortColumnsName { get; set; }
        public QueryOptions()
        {
            SortOrder = SortOrder.ASC;
            SortBy = new List<Func<TEntity, object>>();
        }

        public Expression<Func<TEntity, bool>>? FilterBy { get; set; }
        public List<Func<TEntity, object>> SortBy { get; set; }
        public SortOrder SortOrder { get; set; }
    }
}
