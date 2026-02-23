using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TaskTracker_KR.Services;

namespace TaskTracker_KR
{
    /// <summary>
    /// Логика взаимодействия для HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        private WindowState _previousWindowState = Cookie.windowState;
        public HomeWindow()
        {
            InitializeComponent();
            this.WindowState = _previousWindowState;

            // Изменение иконки кнопки на требуемую (по состоянию окна)
            SameActions.ControlWindowStateStatus(
                this.WindowState,
                FullOpenIcon);

            // Отображение данных от аккаунта
            DisplayAccountInformation();
        }

        private async void DisplayAccountInformation()
        {
            
            var response = await SupabaseHelper.GetAccountApprovals();

            // Данные для вставки в форму аккаунта
            string? accountName = response!=null ? response.Name : "-";
            string? accountRole = response != null ? response.Role.RoleName : "-"; ;
            Cookie.accountRights = new Dictionary<String, Boolean>()
            {
                {"accept", response!=null ? response.Role.AcceptApproval : false},
                {"work", response!=null ? response.Role.WorkApproval : false},
                {"look", response!=null ? response.Role.LookApproval : false},
                {"create", response!=null ? response.Role.CreateApproval : false},
                {"send", response!=null ? response.Role.SendApproval : false}
            };

            // Установка данных в поля
            AccountName.Text = accountName;
            AccountRole.Text = accountRole;

            
            // Установка разрешений для кнопок
            SameActions.SetLeftSideButtonsEnableStatus([WorkButton, LookButton, ChatButton, CreateButton, AcceptButton]);

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
                this.WindowState = WindowState.Maximized;
                icon.UriSource = new Uri("/icons/double_square.png", UriKind.RelativeOrAbsolute);
            }
            // Установка состояния окна в кэш
            Cookie.windowState = this.WindowState;
            icon.EndInit();
            FullOpenIcon.Source = icon;
        }
        // Свернуть окно
        public void HideWindow(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        public void ShowNotify(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                "Войдите в аккаунт",
                "Трекер",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        // Выйти из аккаунта
        private async void LogOut(object sender, RoutedEventArgs e)
        {
            SameActions.OpenNextWindowInterface<MainWindow>(
                this,
                this.Left,
                this.Top);
        }
    }
}
