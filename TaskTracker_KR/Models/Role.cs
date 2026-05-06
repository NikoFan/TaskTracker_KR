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
    public class TopProgrammer
    {
        [JsonPropertyName("worker_id")]
        public long WorkerId { get; set; }

        [JsonPropertyName("worker_name")]
        public string WorkerName { get; set; }

        [JsonPropertyName("total_score")]
        public int TotalScore { get; set; }
    }
}
