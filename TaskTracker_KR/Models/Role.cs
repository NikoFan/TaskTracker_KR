using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;

namespace TaskTracker_KR.Models
{
    [Table("Roles")]
    public class Role : BaseModel
    {
        // Id создаётся автоматически в БД, поэтому не помечаем его как Column для вставки
        [Column("role_id")]
        public int Id { get; set; }

        // Название роли
        [Column("role_name")]
        public string? RoleName { get; set; } = string.Empty;

        // Разрешение на создание задачи
        [Column("create_approval")]
        public bool CreateApproval { get; set; }

        // Разрешение на зачет задачи
        [Column("accept_approval")]
        public bool AcceptApproval { get; set; }

        // Разрешение на работу над задачами
        [Column("work_approval")]
        public bool WorkApproval { get; set; }

        // Разрешение на просмотр статистики
        [Column("look_approval")]
        public bool LookApproval { get; set; }

        // Разрешение на отправку сообщений
        [Column("send_approval")]
        public bool SendApproval { get; set; }
    }
}
