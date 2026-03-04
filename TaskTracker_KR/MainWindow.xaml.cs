using Microsoft.Extensions.Logging;
using System.Collections.ObjectModel;
using System.Data;
using System.Runtime.CompilerServices;
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
        public MainWindow()
        {
            InitializeComponent();
            
            this.WindowState = Cookie.windowState;

            // Изменение иконки кнопки на требуемую (по состоянию окна)
            SameActions.ControlWindowStateStatus(
                this.WindowState,
                FullOpenIcon);
        }

        // Перетаскивание окна
        public void DragWindow(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error drag window\n{ex.Message}");
            }
        }
        // Закрытие окна
        public void CloseWindow(object sender, RoutedEventArgs e)
        {
            if (SameActions.SendCustomMessageBox(
                    "Вы точно хотите закрыть приложение?",
                    MessageBoxImage.Question,
                    true)
                )
                Environment.Exit(0);
                
        }
        // Изменить размер окна
        public void FullSizeWindow(object sender, RoutedEventArgs e) => 
            SameActions.ResizeWindowState(mainWindow, FullOpenIcon);
        // Свернуть окно
        public void HideWindow(object sender, RoutedEventArgs e) => 
            this.WindowState = WindowState.Minimized;

        public void ShowNotify(object sender, RoutedEventArgs e)
        {
           SameActions.SendCustomMessageBox(
                "Войдите в аккаунт",
                MessageBoxImage.Information);
        }
     

        private async void AccountSignIn(object sender, RoutedEventArgs e)
        {
            try
            {
                // Авторизация пользователя
                var accountExist = await SupabaseHelper.GetCurrentAccount(
                    LoginInput.Text,
                    PasswordInput.Text);
                SameActions.SendCustomMessageBox(
                    $"Результат авторизации: {(Cookie.currentAccountId == -1 ? "Неверные данные": "Авторизован")}",
                    Cookie.currentAccountId == -1 ? MessageBoxImage.Stop : MessageBoxImage.Information);
                // Открытие окна
                if (Cookie.currentAccountId != -1)
                    SameActions.OpenNextWindowInterface<HomeWindow>(
                        this,
                        this.Left,
                        this.Top);
                return;
            }
            catch (Exception ex)
            {
                // Если есть InnerException — выводим тоже
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"📄 InnerException: {ex.InnerException.Message}");
                }
                MessageBox.Show($"Ошибка: {ex.Message}");
            }
        }
    }
}