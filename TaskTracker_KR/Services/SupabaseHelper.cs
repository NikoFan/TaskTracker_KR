using Serilog;
using Sprache;
using Supabase;                    // Основной клиент Supabase
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Exceptions;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskTracker_KR.Models;       // Наша модель TaskItem
using TaskTracker_KR.Properties;

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
                .Select("account_id")
                .Where(x => x.Login == inputLogin)
                .Where(x => x.Password == inputPassword)
                .Single();
            Log.Information($"response: {response?.ToString()}");
            if (response == null)
            {
                Cookie.currentAccountId = -1;
                return false;
            }  
            Cookie.currentAccountId = response.Id;
            return true;
        }

        /// <summary>
        /// Получение данных accounts + roles
        /// </summary>
        public static async Task<Account> GetAccountApprovals()
        {
            try
            {
                return await Client
                    .From<Account>()
                    .Select("*, role:Roles(*), " +
                                "devgroup:DevGroups(*)")
                    .Where(x => x.Id == Cookie.currentAccountId)
                    .Single();             // Бросает exception если не найдено
            }
            catch (PostgrestException ex)
            {
                // Ошибка RLS, синтаксиса запроса и т.д.
                Console.WriteLine($"Supabase error: {ex.Message}");
                throw; // Пробрасываем дальше, чтобы приложение могло обработать
            }
            catch (Exception ex)
            {
                // Сетевые ошибки, таймауты
                Console.WriteLine($"Connection error: {ex.Message}");
                throw;
            }
        }


        /// <summary>
        /// Получение списка ФИО и id исполнителей
        /// </summary>
        public static async Task<List<ProgrammerBusyInfo>> GetAvailableProgrammersAsync()
        {
            try
            {
                // В версии 4.1.0 Rpc возвращает ОДИН объект
                // Для RETURNS TABLE нужно использовать List<Dictionary> как тип
                var response = await Client.Postgrest
                    .Rpc<List<Dictionary<string, object>>>(
                        "get_available_programmers", // Функция SQL
                        new { p_manager_id = Cookie.currentAccountId });

                var response2 = await Client.Postgrest
                    .Rpc<List<ProgrammerBusyInfo>>(
                        "get_available_programmers",
                        new { p_manager_id = Cookie.currentAccountId });

                // Конвертируем в типизированный список
                var programmers = new List<ProgrammerBusyInfo>();

                foreach(var item in response)
                {
                    MessageBox.Show($"item: {item["programmer_name"]} {item["is_busy"]}");
                }

                if (response2 != null)
                {
                    
                    foreach (var modelInstance in response2)
                    {

                        MessageBox.Show($"{modelInstance.IsBusy} -> busy");
                        programmers.Add(modelInstance); 
                        //programmers.Add(new ProgrammerBusyInfo
                        //{
                        //    ProgrammerId = Convert.ToInt64(dict["programmer_id"]),
                        //    ProgrammerName = dict["programmer_name"]?.ToString() ?? string.Empty,
                        //    IsBusy = ParseBoolean(dict["is_busy"])
                        //});
                    }
                }

                return programmers;
            }
            catch (PostgrestException ex)
            {
                Console.WriteLine($"Supabase error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Надёжно конвертирует различные форматы в bool.
        /// </summary>
        private static bool ParseBoolean(object? value)
        {
            if (value == null) return false;

            // Если уже булево значение
            if (value is bool b) return b;

            // Если строка
            if (value is string s)
            {
                s = s.Trim().ToLowerInvariant();
                return s == "true" || s == "1" || s == "yes" || s == "t";
            }

            // Если число
            if (value is int i) return i != 0;
            if (value is long l) return l != 0;
            if (value is short sh) return sh != 0;
            if (value is byte by) return by != 0;

            // Попытка универсальной конвертации
            try
            {
                return Convert.ToBoolean(value);
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Метод создания задачи для программиста
        /// </summary>
        /// <param name="parameters">Словарь данных для добавления</param>
        public static async Task<Boolean> CreateTask(Dictionary<string, object> parameters)
        {
            try
            {
                var result = await Client
                    .Rpc<int>("create_task_safe", parameters);

                MessageBox.Show(result.ToString());
                return true;

            }
            catch (PostgrestException ex)
            {
                Console.WriteLine($"Supabase error: {ex.Message}");
                MessageBox.Show($"Supabase error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection error: {ex.Message}");
                MessageBox.Show($"Connection error: {ex.Message}");
                throw;
                
            }
            
        }
    }
}
