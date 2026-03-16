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
using TaskTracker_KR.Models;
using TaskTracker_KR.Services;
using static System.Net.Mime.MediaTypeNames;

namespace TaskTracker_KR
{
    /// <summary>
    /// Логика взаимодействия для CreateTaskWindow.xaml
    /// </summary>
    public partial class CreateTaskWindow : Window
    {
        private DispatcherTimer _timer;
        // Минимальная дата - завтра
        private DateTime minimalDateTomorrow = DateTime.Today.AddDays(1);
        public CreateTaskWindow()
        {
            InitializeComponent();
            this.WindowState = Cookie.windowState;
            // Изменение иконки кнопки на требуемую (по состоянию окна)
            SameActions.ControlWindowStateStatus(
                this.WindowState,
                FullOpenIcon);


            // Установка завтрашней даты как начальной
            ChooseDateCalendar.SelectedDate = minimalDateTomorrow;

            _timer = new DispatcherTimer();
            // Тик раз в 300 мс
            _timer.Interval = TimeSpan.FromMilliseconds(300);
            _timer.Tick += Timer_Tick;
            _timer.Start();

            SetInfoToCombobox();

            // Установка разрешений для кнопок
            SameActions.SetLeftSideButtonsEnableStatus([WorkButton, LookButton, ChatButton, CreateButton, AcceptButton]);
        }

        // Установка данных в Combobox
        private async void SetInfoToCombobox()
        {
            var progList = await SupabaseHelper.GetAvailableProgrammersAsync();
            SameActions.PutProgrammersList(progList, ProgrammersList);
        }



        // Действие при тике таймера
        private void Timer_Tick(object sender, EventArgs e) => 
            UpdateDateTime();

        // Обновление поля с датой
        private void UpdateDateTime()
        {
            TaskEndDateTextBlock.Text = "Дата сдачи:\n " + (ChooseDateCalendar.SelectedDate != null ? ChooseDateCalendar.SelectedDate.Value.ToString("yyyy-MM-dd") : "Дата не выбрана");
            TaskCreateDateTextBlock.Text = "Дата регистрации:\n " + DateTime.Today.ToString("yyyy-MM-dd");
        }


        // Проверка изменений в календаре
        private void ChooseDateCalendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ChooseDateCalendar.SelectedDate.HasValue)
            {
                DateTime selectedDate = ChooseDateCalendar.SelectedDate.Value;

                // ВЫбранная дата раньше минимальнйо
                if (selectedDate < minimalDateTomorrow)
                {
                    SameActions.SendCustomMessageBox(
                        $"⚠️ Минимальная дата — завтра!\n\n" +
                        $"Вы выбрали: {selectedDate:dd.MM.yyyy}\n" +
                        $"Минимальная дата: {minimalDateTomorrow:dd.MM.yyyy}",
                        MessageBoxImage.Exclamation
                        );

                    // Сбрасываем на завтра
                    ChooseDateCalendar.SelectedDate = minimalDateTomorrow;
                }
            }

            UpdateDateTime();
        }

        // Ручная остановка таймера
        private void StopTimerTick()
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

        /// <summary>
        /// Создание задачи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CreateTaskOnClick(object sender, RoutedEventArgs e)
        {
            TextRange descriptionContent = new TextRange(
                TaskInputDescription.Document.ContentStart,
                TaskInputDescription.Document.ContentEnd);
            // Проверка введеных данных на SQLI
            if (SameActions.VerifyUserInputDataForSQLI(
                [TaskInputTitle.Text,
                descriptionContent.Text]))
            {
                if (SameActions.SendCustomMessageBox(
                    $"Проверьте данные:\n\n" +
                    $"Название: {TaskInputTitle.Text}\n" +
                $"Описание задачи: {descriptionContent.Text}\n" +
                $"Дата создания задачи:{TaskCreateDateTextBlock.Text[19..]}\n" +
                $"Дата дэдлайна:{TaskEndDateTextBlock.Text[13..]}\n" +
                $"Исполнитель: {ProgrammersList.Text.Split(".")[0]}",
                    MessageBoxImage.Exclamation,
                    true
                    ))
                {
                    // Добавление данных
                    // Формирование словаря для параметров
                    var parameters = new Dictionary<string, object>
                    {
                        { "p_title", TaskInputTitle.Text },
                        { "p_text", descriptionContent.Text },
                        { "p_create_date", TaskCreateDateTextBlock.Text[19..] },
                        { "p_end_date", TaskEndDateTextBlock.Text[13..] },
                        { "p_end_fact_date", (object)DBNull.Value },
                        { "p_employee_id", ProgrammersList.Text.Split(".")[0] },
                        { "p_manager_id", Cookie.currentAccountId }
                    };
                    SupabaseHelper.CreateTask(parameters);
                }
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
                StopTimerTick();
                Environment.Exit(0);
            }
                
        }
        // Изменить размер окна
        public void FullSizeWindow(object sender, RoutedEventArgs e) =>
            SameActions.ResizeWindowState(createTaskWindow, FullOpenIcon);
        // Свернуть окно
        public void HideWindow(object sender, RoutedEventArgs e) =>
            this.WindowState = WindowState.Minimized;

        public void ShowNotify(object sender, RoutedEventArgs e) =>
            SameActions.SendCustomMessageBox(
                 "Войдите в аккаунт",
                 MessageBoxImage.Information);
        

        private void BackToMainWindow(object sender, RoutedEventArgs e) =>
            SameActions.OpenNextWindowInterface<HomeWindow>(
                this,
                this.Height,
                this.Width,
                this.Left,
                this.Top);

    }
}
