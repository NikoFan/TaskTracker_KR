using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для ShowNewTasks.xaml
    /// </summary>
    public partial class ShowNewTasks : Window
    {
        public ShowNewTasks()
        {
            InitializeComponent();
            this.WindowState = Cookie.windowState;

            // Изменение иконки кнопки на требуемую (по состоянию окна)
            SameActions.ControlWindowStateStatus(
                this.WindowState,
                FullOpenIcon);

            GenerateTaskCards();
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
            SameActions.ResizeWindowState(showNewTasks, FullOpenIcon);
        // Свернуть окно
        public void HideWindow(object sender, RoutedEventArgs e) =>
            this.WindowState = WindowState.Minimized;
        
        private async void UpdateTaskResult(int taskId, long workerId, short newResult)
        {
            MessageBox.Show($"{taskId}, {workerId}, result: {newResult}");
            await SupabaseHelper.UpdateTaskResult(taskId, workerId, newResult);
            GenerateTaskCards();
        }
        

        private async void GenerateTaskCards()
        {

            var data = await SupabaseHelper.GetNewTasksToCheck();

            DataPanel.Children.Clear();
            foreach (var task in data)
            {
                // Безопасное чтение данных
                string title = $"{task["title"]}";
                string desc = $"{task["comment"]}";
                string assignee = $"{task["worker_name"]}";
                short rating =  Convert.ToInt16(task["result"]);

                // Элемент рейтинга
                TextBlock ratingTxt = new() { Text = rating.ToString(), FontSize = 16, FontWeight = FontWeights.Bold, VerticalAlignment = VerticalAlignment.Center, MinWidth = 20, TextAlignment = TextAlignment.Center };
                Button btnDown = new() { Content = "▼", Margin = new Thickness(0, 0, 4, 0) };
                Button btnUp = new() { Content = "▲", Margin = new Thickness(4, 0, 0, 0) };

                // Логика изменения (замыкание хранит текущее значение)
                void UpdateRating(int delta)
                {
                    rating = (short)Math.Clamp(rating + delta, 1, 5);
                    ratingTxt.Text = rating.ToString();
                    task["result"] = rating.ToString(); // Синхронизируем с источником
                }

                btnDown.Click += (s, e) => UpdateRating(-1);
                btnUp.Click += (s, e) => UpdateRating(1);

                StackPanel ratingPanel = new() { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 6, 0, 2) };
                ratingPanel.Children.Add(btnDown);
                ratingPanel.Children.Add(ratingTxt);
                ratingPanel.Children.Add(btnUp);

                // Содержимое карточки
                StackPanel content = new();
                content.Children.Add(new TextBlock { Text = title, FontWeight = FontWeights.Bold, FontSize = 14, Margin = new Thickness(0, 0, 0, 4) });
                content.Children.Add(new TextBlock { Text = desc, TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 4) });
                content.Children.Add(new TextBlock { Text = $"👤 {assignee}", FontStyle = FontStyles.Italic, Margin = new Thickness(0, 0, 0, 6) });
                content.Children.Add(new Separator { Margin = new Thickness(0, 2, 0, 4), Opacity = 0.3 });
                content.Children.Add(ratingPanel);

                Button btnRate = new() { Content = "⭐ Оценить", Margin = new Thickness(0, 4, 0, 0), HorizontalAlignment = HorizontalAlignment.Right };
                btnRate.Click += (s, e) => UpdateTaskResult(
                    Convert.ToInt32(task["id"]),
                    Convert.ToInt64(task["worker_id"]),
                    Convert.ToInt16(task["result"])); //  MessageBox.Show($"Задача: {title}\nФинальная оценка: {rating}/5", "Подтверждение", MessageBoxButton.OK, MessageBoxImage.Information));
                content.Children.Add(btnRate);

                // Обёртка Border
                Border border = new()
                {
                    Child = content,
                    Background = Brushes.White,
                    BorderBrush = Brushes.LightGray,
                    BorderThickness = new Thickness(1),
                    CornerRadius = new CornerRadius(6),
                    Padding = new Thickness(10),
                    Margin = new Thickness(0, 5, 0, 5)
                };

                // Добавляем ПОСЛЕ существующего заголовка
                DataPanel.Children.Add(border);
            }
        }

    }

}
