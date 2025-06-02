using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity.Settings
{
    [Table("Label")]
    public  class Label : BaseEntity
    {
        public string? category_name { get; set; }
        public string? sub_category { get; set; }
        public string? country_type { get; set; }
        public string? country { get; set; }
        public string? statedivison { get; set; }
        public string? district { get; set; }
        public string? township { get; set; }
        public string? casedate { get; set; }
        public string? headline { get; set; }
        public string? File_Name { get; set; }
        public string? totalNews { get; set; }


    }
}
