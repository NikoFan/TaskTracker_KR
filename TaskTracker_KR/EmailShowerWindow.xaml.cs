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
    /// Логика взаимодействия для EmailShowerWindow.xaml
    /// </summary>
    public partial class EmailShowerWindow : Window
    {
        public EmailShowerWindow()
        {
            InitializeComponent();
            this.WindowState = Cookie.windowState;

            // Изменение иконки кнопки на требуемую (по состоянию окна)
            SameActions.ControlWindowStateStatus(
                this.WindowState,
                FullOpenIcon);
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
        public void FullSizeWindow(object sender, RoutedEventArgs e) =>
            SameActions.ResizeWindowState(emailShowerWindow, FullOpenIcon);
        // Свернуть окно
        public void HideWindow(object sender, RoutedEventArgs e) =>
            this.WindowState = WindowState.Minimized;

        public void ShowNotify(object sender, RoutedEventArgs e) =>
            SameActions.SendCustomMessageBox(
                 "Войдите в аккаунт",
                 MessageBoxImage.Information);
        
        private void add(object sender, RoutedEventArgs e)
        {
            int index = ShowEmailMessages.Children.Count;
            TextBlock text = new TextBlock();
            text.Text = $"TEXT {index}";
            ShowEmailMessages.Children.Insert(index, text);
        }

        private void BackToMainWindow(object sender, RoutedEventArgs e) =>
            SameActions.OpenNextWindowInterface<HomeWindow>(this, this.Left, this.Top);

    }
}
