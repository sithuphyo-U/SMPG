using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity
{
    public class Image : BaseEntity
    {
        public string image_name {  get; set; }
        public string image_type {  get; set; }
        public string original_image_name {  get; set; }
        public string? path {  get; set; }

    }
}
