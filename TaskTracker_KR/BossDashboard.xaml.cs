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
    /// Логика взаимодействия для BossDashboard.xaml
    /// </summary>
    public partial class BossDashboard : Window
    {
        public BossDashboard()
        {
            InitializeComponent();
            this.WindowState = Cookie.windowState;

            // Изменение иконки кнопки на требуемую (по состоянию окна)
            SameActions.ControlWindowStateStatus(
                this.WindowState,
                FullOpenIcon);

            GenerateWorkersCards();
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
            SameActions.ResizeWindowState(bossDashboard, FullOpenIcon);
        // Свернуть окно
        public void HideWindow(object sender, RoutedEventArgs e) =>
            this.WindowState = WindowState.Minimized;


        private async void GenerateWorkersCards()
        {

            var data = await SupabaseHelper.GetProgrammerStatistics();

            DataPanel.Children.Clear();
            foreach (var task in data)
            {
                // Безопасное чтение данных
                //long workerID = task.WorkerId;
                //string workerName = task.WorkerName;
                //int taskCount = task.TaskCount;
                //double avgScore = task.AvgScore;
                //int score5 = task.Score5Count;
                //int score4 = task.Score4Count;
                //int score3 = task.Score3Count;
                //int score2 = task.Score2Count;
                //int score1 = task.Score1Count;


                long workerID = Convert.ToInt64(task["worker_id"]);
                string workerName = task["worker_name"]?.ToString() ?? "";
                int taskCount = Convert.ToInt32(task["task_count"]);
                double avgScore = Convert.ToDouble(task["avg_score"]);
                int score5 = Convert.ToInt32(task["score_5_count"]);
                int score4 = Convert.ToInt32(task["score_4_count"]);
                int score3 = Convert.ToInt32(task["score_3_count"]);
                int score2 = Convert.ToInt32(task["score_2_count"]);
                int score1 = Convert.ToInt32(task["score_1_count"]);

                int sum = (score5 + score4 + score3 + score2 + score1) == 0 ? 1 : score5 + score4 + score3 + score2 + score1;




                // Содержимое карточки
                StackPanel content = new();
                content.Children.Add(new TextBlock { Text = workerID.ToString(), FontWeight = FontWeights.Bold, FontSize = 14, Margin = new Thickness(0, 0, 0, 4) });
                content.Children.Add(new TextBlock { Text = workerName, FontWeight = FontWeights.Bold, FontSize = 22, Margin = new Thickness(0, 0, 0, 4) });
                content.Children.Add(new TextBlock { Text = "Общее число задач: " + taskCount.ToString(), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 4) });
                content.Children.Add(new TextBlock { Text = "Средняя оценка: " + avgScore.ToString(), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 4) });

                content.Children.Add(ProgressBarCreate(score5, sum, 5));
                content.Children.Add(ProgressBarCreate(score4, sum, 4));
                content.Children.Add(ProgressBarCreate(score3, sum, 3));
                content.Children.Add(ProgressBarCreate(score2, sum, 2));
                content.Children.Add(ProgressBarCreate(score1, sum, 1));

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


        private Border ProgressBarCreate(int number, int summa, int content)
        {
            return new Border
            {
                BorderBrush = Brushes.Gray,
                BorderThickness = new Thickness(1),
                CornerRadius = new CornerRadius(5),
                Margin = new Thickness(0, 5, 0, 0),
                Child = new Grid
                {
                    Children =
                    {
                        // Основной ProgressBar
                        new ProgressBar
                        {
                            Maximum = summa,
                            Value = number,  // Преобразуем оценку в проценты
                            Height = 20,
                            Margin = new Thickness(5),
                            Foreground = new SolidColorBrush(Color.FromRgb(76, 175, 80)),
                            Background = new SolidColorBrush(Color.FromRgb(240, 240, 240)),
                            HorizontalAlignment = HorizontalAlignment.Stretch
                        },
                        // Текст внутри ProgressBar
                        new TextBlock
                        {
                            Text = $"Соотношение оценок: {content}",  // Отображаем значение в процентах
                            HorizontalAlignment = HorizontalAlignment.Center,
                            VerticalAlignment = VerticalAlignment.Center,
                            FontWeight = FontWeights.Bold,
                            Foreground = Brushes.Black,
                            
                            TextWrapping = TextWrapping.NoWrap
                        }
                    }
                }
            };
        }
    } 
}
