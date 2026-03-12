using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskTracker_KR.Models
{
    [Table("DevGroups")]
    public class DevGroup : BaseModel
    {
        [Column("group_id")]
        public int Id { get; set; }
        [Column("group_title")]
        public string? Title { get; set; } = string.Empty;
        [Column("group_ended_tasks_count")]
        public long EndCount { get; set; }

        [Column("group_months_ended_tasks_count")]
        public short MonthEndCount { get; set; }

        [Column("group_count_of_accounts")]
        public short AccountsCount { get; set; }

        [Column("group_email")]
        public string? Email { get; set; } = string.Empty;

        [Column("group_ready_tasks")]
        public int[] ReadyTasks { get; set; } = Array.Empty<int>();

    }
}
