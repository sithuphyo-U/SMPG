using DIS.Infrastructure.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DIS.DataAccess.Entity
{
    [Table("log")]
    public class Log : BaseEntity
    {
        public int user_id { get; set; }
        [ForeignKey("user_id")]
        public virtual User? User { get; set; }
        public string? controller { get; set; } = string.Empty;
        public string? table { get; set; } = string.Empty;
        public string? action { get; set; } = string.Empty;
        public string? url { get; set; } = string.Empty;
        public DateTime? date { get; set; }
        public string? ip { get; set; } = string.Empty;

    }
}
