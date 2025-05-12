using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.Application
{
    public class FileModel
    {
        public string name { get; set; } = string.Empty;

        public string uuid { get; set; } = string.Empty;

        public string ext { get; set; } = string.Empty;

        public DateTime created_date { get; set; }
        public bool success { get; set; } = false;
    }
}
