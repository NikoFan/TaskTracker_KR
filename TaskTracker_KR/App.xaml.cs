using DotNetEnv;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using TaskTracker_KR.Services;

namespace TaskTracker_KR
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            try
            {
                // 🔹 1. УМНЫЙ ПОИСК .env (поднимается вверх, пока не найдет)
                string? envPath = FindEnvFile();

                if (string.IsNullOrEmpty(envPath) || !File.Exists(envPath))
                {
                    MessageBox.Show(
                        $"❌ Файл .env НЕ НАЙДЕН!\n\n" +
                        $"Текущая папка запуска: {AppContext.BaseDirectory}\n\n" +
                        $"Убедись, что файл .env лежит в корне проекта\n" +
                        $"(в одной папке с файлом .csproj).",
                        "Ошибка файла",
                        MessageBoxButton.OK,
                        MessageBoxImage.Error
                    );
                    Shutdown();
                    return;
                }

                Debug.WriteLine($"✅ .env найден: {envPath}");

                // 🔹 2. Загружаем файл
                Env.Load(envPath);

                // 🔹 3. Читаем переменные
                string? url = Env.GetString("url");
                string? key = Env.GetString("api");

                Debug.WriteLine($"🔑 URL: {(string.IsNullOrEmpty(url) ? "ПУСТО" : "OK")}");
                Debug.WriteLine($"🔑 KEY: {(string.IsNullOrEmpty(key) ? "ПУСТО" : "OK")}");

                // 🔹 4. Проверка
                if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(key))
                {
                    MessageBox.Show(
                        $"❌ Переменные в .env пустые!\n\n" +
                        $"Проверь содержимое файла (без пробелов вокруг =).",
                        "Ошибка данных",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    Shutdown();
                    return;
                }

                // 🔹 5. Инициализация
                SupabaseHelper.Initialize(url, key);

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ ОШИБКА:\n\n{ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
                Shutdown();
            }
        }

        /// <summary>
        /// Ищет файл .env, поднимаясь от папки запуска вверх по дереву.
        /// Это надежнее, чем рассчитывать количество уровней вручную.
        /// </summary>
        private string? FindEnvFile()
        {
            // Начинаем поиск от папки, где лежит запущенный .exe
            DirectoryInfo? currentDir = new DirectoryInfo(AppContext.BaseDirectory);

            // Поднимаемся максимум на 10 уровней вверх
            for (int i = 0; i < 10; i++)
            {
                if (currentDir == null) break;

                // Проверяем, есть ли .env в текущей папке
                string envPath = Path.Combine(currentDir.FullName, ".env");
                if (File.Exists(envPath))
                {
                    return envPath;
                }

                // Поднимаемся на уровень выше
                currentDir = currentDir.Parent;
            }

            return null;
        }
    }

}
