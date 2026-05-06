using Serilog;
using Sprache;
using Supabase;                    // Основной клиент Supabase
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Exceptions;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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

            // Создаём экземпляр клиента и сохраняем в статическое свойство
            Client = new Client(url, key, new SupabaseOptions
            {
                AutoRefreshToken = true,
            });
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
                .Select("id, role")
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
            Cookie.currentAccountRole = response.Role;
            return true;
        }

        /// <summary>
        /// Получение списка неоцененных задач
        /// </summary>
        public static async Task<Boolean> GetListOfNewTasks()
        {
            if (Client == null)
                throw new InvalidOperationException("Supabase client not initialized.");
            var response = await Client
                .From<TaskModel>()
                .Select("*")
                .Where(x => x.Result == 0)
                .Get();
            Log.Information($"response: {response?.ToString()}");
            if (response == null)
            {
                Cookie.currentAccountId = -1;
                return false;
            }
            return true;
        }


        


        /// <summary>
        /// Метод добавления задачи для проверки
        /// </summary>
        /// <param name="parameters">Словарь данных для добавления</param>
        public static async Task<Boolean> SendTask(Dictionary<string, object> parameters)
        {
            try
            {
                var result = await Client
                    .Rpc<int>("add_ready_task", parameters);

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

        ///<summary>
        /// Выгрузка не проверенных задач
        /// </summary>
        /// 
        public static async Task<List<Dictionary<string, object>>> GetNewTasksToCheck()
        {
            try
            {
                // Вызываем функцию и получаем список словарей
                var response = await Client
                    .Rpc<List<Dictionary<string, object>>>(
                        "get_tasks_with_result_zero",
                        new { });
                return response;
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

        /// <summary>
        /// Обновление оценки у задачи
        /// </summary>
        /// 
        public static async Task<Boolean> UpdateTaskResult(long taskId, long workerId, short newResult)
        {
            try
            {
                // Передаем параметры в функцию
                var parameters = new Dictionary<string, object>
                {
                    { "p_task_id", taskId },
                    { "p_worker_id", workerId },
                    { "p_new_result", newResult }
                };

                // Вызываем функцию
                await Client.Rpc("update_task_result", parameters);
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

        ///<summary>
        /// Получение топа программистов для доски почета
        /// </summary>
        /// 
        public static async Task<List<Dictionary<string, object>>> GetTop5SuccessfulProgrammers()
        {
            try
            {
                return await Client
                    .Rpc<List<Dictionary<string, object>>>(
                        "get_top_5_successful_programmers",
                        new { });
            }
            catch (PostgrestException ex)
            {
                Console.WriteLine($"Supabase error: {ex.Message}");
                throw;
            }
        }

        ///<summary>
        ///получение всех программистов с их успеваемостью
        /// </summary>
        /// 
        public static async Task<List<Dictionary<string, object>>> GetProgrammerStatistics()
        {
            try
            {
                // ВАЖНО: Получаем список словарей вместо моделей
                var response = await Client
                    .Rpc<List<Dictionary<string, object>>>(
                        "get_programmer_statistics",
                        new { });

                // Проверка данных (для отладки)
                foreach (var stat in response)
                {
                    Console.WriteLine($"ID: {stat["worker_id"]}, Name: {stat["worker_name"]}, Avg: {stat["avg_score"]}");
                }

                return response;
            }
            catch (PostgrestException ex)
            {
                Console.WriteLine($"Supabase error: {ex.Message}");
                throw;
            }
        }

    }
}
