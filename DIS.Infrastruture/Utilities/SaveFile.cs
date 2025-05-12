using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace DIS.Infrastructure.Utilities
{
    public class SaveFile
    {
        public string? name { get; set; }

        public string? uuid { get; set; }

        public string? ext { get; set; }

        public DateTime created_date { get; set; }
        public bool success { get; set; } = false;
    }
}
