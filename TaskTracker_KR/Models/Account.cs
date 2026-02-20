using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Supabase.Postgrest.Attributes;

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
        public string Login { get; set; }

        // Разрешение на создание задачи
        [Column("account_password")]
        public string Password { get; set; }

        // Разрешение на зачет задачи
        [Column("account_email")]
        public string Email { get; set; }

        // Разрешение на работу над задачами
        [Column("account_name")]
        public string Name { get; set; }

        // Разрешение на просмотр статистики
        [Column("account_role_fk")]
        public int Role_FK { get; set; }

        // Разрешение на отправку сообщений
        [Column("dev_group_id_fk")]
        public int Group_FK { get; set; }
    }
}
