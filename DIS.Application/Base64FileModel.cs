using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.Application
{
    public class Base64FileModel
    {
        public string name { get; set; } = string.Empty;
        public string file_bytes { get; set; } = string.Empty;
        public string content_type { get; set; } = string.Empty;
    }
}
