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
    public class ProgrammerStatistics
    {
        [JsonPropertyName("worker_id")]
        public long WorkerId { get; set; }

        [JsonPropertyName("worker_name")]
        public string WorkerName { get; set; }

        [JsonPropertyName("task_count")]
        public int TaskCount { get; set; }

        [JsonPropertyName("avg_score")]
        public double AvgScore { get; set; }

        [JsonPropertyName("score_5_count")]
        public int Score5Count { get; set; }

        [JsonPropertyName("score_4_count")]
        public int Score4Count { get; set; }

        [JsonPropertyName("score_3_count")]
        public int Score3Count { get; set; }

        [JsonPropertyName("score_2_count")]
        public int Score2Count { get; set; }

        [JsonPropertyName("score_1_count")]
        public int Score1Count { get; set; }
    }
}
