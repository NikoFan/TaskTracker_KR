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
    /// Логика взаимодействия для TopProgrammersDashboard.xaml
    /// </summary>
    public partial class TopProgrammersDashboard : Window
    {
        public TopProgrammersDashboard()
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
            SameActions.ResizeWindowState(topProgrammersDashboard, FullOpenIcon);
        // Свернуть окно
        public void HideWindow(object sender, RoutedEventArgs e) =>
            this.WindowState = WindowState.Minimized;

        // Переход в окно создания задачи
        public void AddTask(object sender, RoutedEventArgs e) =>
            SameActions.OpenNextWindowInterface<AddReadyTask>(
                        this,
                        this.Height,
                        this.Width,
                        this.Left,
                        this.Top);


        private async void GenerateWorkersCards()
        {

            var data = await SupabaseHelper.GetTop5SuccessfulProgrammers();

            DataPanel.Children.Clear();
            foreach (var task in data)
            {
                // Безопасное чтение данных
                string workerName = task["worker_name"].ToString();
                int workerScore = Convert.ToInt32(task["total_score"]);


                // Содержимое карточки
                StackPanel content = new();
                content.Children.Add(new TextBlock { Text = workerName, FontWeight = FontWeights.Bold, FontSize = 22, Margin = new Thickness(0, 0, 0, 4) });
                content.Children.Add(new TextBlock { Text = "Общее число рейтинга: " + workerScore.ToString(), TextWrapping = TextWrapping.Wrap, Margin = new Thickness(0, 0, 0, 4) });

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
