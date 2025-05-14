
﻿using DIS.Infrastructure.Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity.Settings
{

    [Table("subCategory")]
    public class DisasterSubCategory : BaseEntity
    {
        public string? name { get; set; }
        public int disaster_category_id { get; set; }
        [ForeignKey("disaster_category_id")]

        public virtual DisasterCategory? disastercategory { get; set; }
    }
}