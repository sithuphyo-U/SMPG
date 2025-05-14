using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity
{
    public class ProgramCode :BaseEntity
    {
public string program_name { get; set; }
        public string program_code {  get; set; }
        public int parent_id { get; set; }
        public string url { get; set; }
        public string icon { get; set; }
        public string permission { get; set; }
    }
}
