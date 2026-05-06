using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TaskTracker_KR.Models
{
    [Table("Accounts")]
    public class Account : BaseModel
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("login")]
        public string? Login { get; set; } = string.Empty;

        [Column("password")]
        public string? Password { get; set; } = string.Empty;

        [Column("name")]
        public string? Name { get; set; } = string.Empty;

        [Column("role")]
        public string? Role { get; set; } = string.Empty;
    }
}
