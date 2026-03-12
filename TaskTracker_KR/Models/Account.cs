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
        // Id создаётся автоматически в БД, поэтому не помечаем его как Column для вставки
        [Column("account_id")]
        public int Id { get; set; }

        // Название роли
        [Column("account_login")]
        public string? Login { get; set; } = string.Empty;

        // Разрешение на создание задачи
        [Column("account_password")]
        public string? Password { get; set; } = string.Empty;

        // Разрешение на зачет задачи
        [Column("account_email")]
        public string? Email { get; set; } = string.Empty;

        // Разрешение на работу над задачами
        [Column("account_name")]
        public string? Name { get; set; } = string.Empty;

        // Разрешение на просмотр статистики
        [Column("account_role_fk")]
        public int Role_FK { get; set; }

        public Role? Role { get; set; }
       

        // Разрешение на отправку сообщений
        [Column("dev_group_id_fk")]
        public int Group_FK { get; set; }

        // [JsonPropertyName("devgroup")] // Для комбинированного запроса в GetAccountApprovals
        public DevGroup? DevGroup { get; set; }
    }
}
