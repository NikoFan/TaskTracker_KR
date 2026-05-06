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
    /// Логика взаимодействия для AddReadyTask.xaml
    /// </summary>
    public partial class AddReadyTask : Window
    {
        public AddReadyTask()
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

        // Переход в окно рейтинга
        public void SeeTop(object sender, RoutedEventArgs e) =>
            SameActions.OpenNextWindowInterface<TopProgrammersDashboard>(
                this,
                this.Height,
                this.Width,
                this.Left,
                this.Top);


        // Изменить размер окна
        public void FullSizeWindow(object sender, RoutedEventArgs e) =>
            SameActions.ResizeWindowState(addReadyTask, FullOpenIcon);
        // Свернуть окно
        public void HideWindow(object sender, RoutedEventArgs e) =>
            this.WindowState = WindowState.Minimized;

        public async void SendToCheck(object sender, RoutedEventArgs e)
        {
            Dictionary<string, object> taskData = new Dictionary<string, object>()
            {
                { "p_title", TitleInput.Text },
                { "p_comment", CommentInput.Text.Length == 0 ? "-" : CommentInput.Text },
                { "p_result", 0 },
                { "p_worker_id", Cookie.currentAccountId }
            };

            if (await SupabaseHelper.SendTask(taskData))
            {
                SameActions.SendCustomMessageBox(
                    "Задача отправлена на проверку", MessageBoxImage.Information);
            }
            else
            {
                SameActions.SendCustomMessageBox(
                    "Ошибка при отправке!\nПроверьте интернет и попробуйте позже!", MessageBoxImage.Warning);
            }
        }
    }
}
