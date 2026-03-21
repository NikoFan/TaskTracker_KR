using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TaskTracker_KR.Models
{
    // НЕ наследуемся от BaseModel (не нужно для Dictionary подхода)
    public class ProgrammerBusyInfo
    {
        [JsonPropertyName("programmer_id")]
        public long ProgrammerId { get; set; }

        [JsonPropertyName("programmer_name")]
        public string ProgrammerName { get; set; } = string.Empty;

        [JsonPropertyName("is_busy")]
        public bool IsBusy { get; set; }
    }
}
