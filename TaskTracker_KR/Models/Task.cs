using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace TaskTracker_KR.Models
{
    [Table("Tasks")]
    public class Task : BaseModel
    {
        [Column("task_id")]
        public int Id { get; set; }

        [Column("task_title")]
        public string? Title { get; set; } = string.Empty;

        [Column("task_text")]
        public string? Text { get; set; } = string.Empty;

        [Column("task_create_date")]
        public DateOnly CreateDate { get; set; }

        [Column("task_end_date")]
        public DateOnly EndDate { get; set; }

        [Column("task_end_fact_date")]
        public DateOnly FactEndDate { get; set; }

        [Column("task_status")]
        public bool Status { get; set; }

        [Column("account_employee_FK")]
        public int EmployeeFK { get; set; }
        public Account? Account { get; set; }


    }
}
