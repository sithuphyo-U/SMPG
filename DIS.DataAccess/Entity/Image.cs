using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity
{
    [Table("Image")]
    public class Image : BaseEntity
    {
        public string image_name {  get; set; }
        public string image_type {  get; set; }
        public string original_image_name {  get; set; }
        public string? path {  get; set; }

    }
}
