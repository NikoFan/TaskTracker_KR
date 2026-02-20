using Supabase;                    // Основной клиент Supabase
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTracker_KR.Models;       // Наша модель TaskItem

namespace TaskTracker_KR.Services
{
    public static class SupabaseHelper
    {
        // Статический клиент — доступен из любого места в приложении
        public static Client? Client { get; private set; }

        /// <summary>
        /// Инициализирует клиент Supabase.
        /// Вызывается один раз при запуске приложения.
        /// </summary>
        /// <param name="url">URL проекта Supabase (из настроек API)</param>
        /// <param name="key">Анонимный public key (anon key) из настроек API</param>
        public static void Initialize(string url, string key)
        {
            Console.WriteLine(url + " " + key);
            // Создаём опции клиента (по умолчанию — стандартные)
            var options = new SupabaseOptions
            {
                // Можно включить авто-подписку на realtime, если нужно
                AutoRefreshToken = true,
            };

            // Создаём экземпляр клиента и сохраняем в статическое свойство
            Client = new Client(url, key, options);
        }

        /// <summary>
        /// Получает все роли из таблицы "Roles"
        /// </summary>
        public static async Task<List<Role>> GetAllRolesAsync()
        {
            if (Client == null)
                throw new InvalidOperationException("Supabase client not initialized.");

            // Запрос: SELECT * FROM roles
            var response = await Client
                .From<Role>()
                .Get();         // Выполняем SELECT

            // Возвращаем список моделей (или пустой список, если null)
            return response?.Models ?? new List<Role>();
        }

        /// <summary>
        /// Получение подтверждения наличия аккаунта по Login и Password
        /// </summary>
        public static async Task<Boolean> GetCurrentAccount(
            string inputLogin,
            string inputPassword)
        {
            if (Client == null)
                throw new InvalidOperationException("Supabase client not initialized.");
            var response = await Client
                .From<Account>()
                .Where(x => x.Login == inputLogin)
                .Where(x => x.Password == inputPassword)
                .Single();

            Console.WriteLine("RESPONSE: " + response);
            if (response == null)
                return false;
            return true;
        }


    }
}
