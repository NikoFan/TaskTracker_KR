using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TaskTracker_KR.Models
{
    [Table("ReadyTasks")]
    public class TaskModel : BaseModel
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("title")]
        public string? Title { get; set; } = string.Empty;

        [Column("comment")]
        public string? Comment { get; set; } = string.Empty;

        [Column("result")]
        public int Result { get; set; }

        [Column("worker_id")]
        public int WorkerId { get; set; }
        public Account? Account { get; set; }


    }
}
