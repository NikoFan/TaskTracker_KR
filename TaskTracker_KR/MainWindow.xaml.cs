using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TaskTracker_KR.Models;
using TaskTracker_KR.Services;

namespace TaskTracker_KR
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WindowState _previousWindowState = WindowState.Normal;
        public MainWindow()
        {
            InitializeComponent();
        }

        public void DragWindow(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error drug window\n{ex.Message}");
            }
        }
        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            if ((MessageBox.Show(
                "Выйти из приложения?",
                "Трекер", 
                MessageBoxButton.YesNo, 
                MessageBoxImage.Question) == MessageBoxResult.Yes))
                Environment.Exit(0);
        }

        public void FullSizeWindow(object sender, RoutedEventArgs e)
        {
            BitmapImage icon = new BitmapImage();
            icon.BeginInit();
            if (this.WindowState == WindowState.Maximized)
            {
                // Если уже развернуто → восстанавливаем
                this.WindowState = WindowState.Normal;
                
                icon.UriSource = new Uri("/icons/square.png", UriKind.RelativeOrAbsolute);
                
            }
            else
            {
                // Если нормально → запоминаем состояние и разворачиваем
                _previousWindowState = this.WindowState;
                this.WindowState = WindowState.Maximized;
                icon.UriSource = new Uri("/icons/double_square.png", UriKind.RelativeOrAbsolute);
            }
            
            icon.EndInit();
            FullOpenIcon.Source = icon;
        }

        public void HideWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        private async void AccountSignIn(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("🔄 Начинаю загрузку ролей...");

                var accountExist = await SupabaseHelper.GetCurrentAccount(
                    LoginInput.Text,
                    PasswordInput.Text);

                MessageBox.Show($"📊 Статус: {accountExist}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ИСКЛЮЧЕНИЕ: {ex.Message}");
                Console.WriteLine($"📄 Stack: {ex.StackTrace}");

                // Если есть InnerException — выводим тоже
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"📄 Inner: {ex.InnerException.Message}");
                }

                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }

        private async void GetRoles(object sender, RoutedEventArgs e)
        {
            try
            {
                Console.WriteLine("🔄 Начинаю загрузку ролей...");

                var roles = await SupabaseHelper.GetAllRolesAsync();

                Console.WriteLine($"📊 Получено записей: {roles?.Count ?? 0}");

                // Если данные есть — выводим их
                foreach (var role in roles)
                {
                    Console.WriteLine($"✅ ID: {role.Id}, Title: {role.RoleName}");
                }
                MessageBox.Show($"✅ ID: {roles[0].Id}, Title: {roles[0].RoleName}");
                MessageBox.Show($"✅ ID: {roles[1].Id}, Title: {roles[1].RoleName}");
                MessageBox.Show($"✅ ID: {roles[2].Id}, Title: {roles[2].RoleName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ ИСКЛЮЧЕНИЕ: {ex.Message}");
                Console.WriteLine($"📄 Stack: {ex.StackTrace}");

                // Если есть InnerException — выводим тоже
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"📄 Inner: {ex.InnerException.Message}");
                }

                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}