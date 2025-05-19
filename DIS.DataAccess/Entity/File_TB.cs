using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity
{
    [Table("File")]
    public class File_TB : BaseEntity
    {
        public int? disastercategory_id { get; set; }
        public string? file_name {  get; set; }
        public string? file_type { get; set; }
        public string? originalfile_name { get; set; }
        public string? path { get; set; }
    }
}
