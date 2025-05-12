using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.Infrastructure.Utilities
{
    public class CommandResult<TEntity>
    {
        public int id { get; set; } = 0;
        public int code { get; set; } = 0;
        public bool success { get; set; } = false;
        public TEntity? entity { get; set; }
        public List<string> messages { get; set; }
        public CommandResult()
        {
            messages = new List<string>();
        }
    }
}
