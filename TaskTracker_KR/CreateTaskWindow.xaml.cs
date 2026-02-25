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
using System.Windows.Threading;
using TaskTracker_KR.Services;

namespace TaskTracker_KR
{
    /// <summary>
    /// Логика взаимодействия для CreateTaskWindow.xaml
    /// </summary>
    public partial class CreateTaskWindow : Window
    {
        private DispatcherTimer _timer;
        public CreateTaskWindow()
        {
            InitializeComponent();
            this.WindowState = Cookie.windowState;
            // Установка выбранной датой - сегодня
            DateTime tomorrow = DateTime.Today.AddDays(1);
            ChooseDateCalendar.SelectedDate = tomorrow;

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(300); // 300 мс
            _timer.Tick += Timer_Tick;
            _timer.Start();
            

            // Изменение иконки кнопки на требуемую (по состоянию окна)
            SameActions.ControlWindowStateStatus(
                this.WindowState,
                FullOpenIcon);
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateDateTime();
        }
        private void UpdateDateTime()
        {
            TaskCreateDateTextBlock.Text = ChooseDateCalendar.SelectedDate != null ? ChooseDateCalendar.SelectedDate.Value.ToString() : "Дата не выбрана";
        }
        private void ChooseDateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChooseDateCalendar.SelectedDate.HasValue)
            {
                DateTime selectedDate = ChooseDateCalendar.SelectedDate.Value;

                // 🔹 Минимальная дата — ЗАВТРА
                DateTime minDate = DateTime.Today.AddDays(1);

                if (selectedDate < minDate)
                {
                    MessageBox.Show(
                        $"⚠️ Минимальная дата — завтра!\n\n" +
                        $"Вы выбрали: {selectedDate:dd.MM.yyyy}\n" +
                        $"Минимальная дата: {minDate:dd.MM.yyyy}",
                        "Ошибка выбора даты",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );

                    // Сбрасываем на завтра
                    ChooseDateCalendar.SelectedDate = minDate;
                }
            }

            UpdateDateTime();
        }


        private void Window_Closed(object sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Tick -= Timer_Tick;
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
            {
                _timer.Stop();
                _timer.Tick -= Timer_Tick;
                Environment.Exit(0);
            }
                
        }
        // Изменить размер окна
        public void FullSizeWindow(object sender, RoutedEventArgs e) =>
            SameActions.ResizeWindowState(createTaskWindow, FullOpenIcon);
        // Свернуть окно
        public void HideWindow(object sender, RoutedEventArgs e) =>
            this.WindowState = WindowState.Minimized;

        public void ShowNotify(object sender, RoutedEventArgs e)
        {
            SameActions.SendCustomMessageBox(
                 "Войдите в аккаунт",
                 MessageBoxImage.Information);
        }
    }
}
