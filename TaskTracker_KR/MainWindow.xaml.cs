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
        private async void AccountSignIn(object sender, RoutedEventArgs e)
        {
            try
            {
                // Авторизация пользователя
                var accountExist = await SupabaseHelper.GetCurrentAccount(
                    LoginInput.Text,
                    PasswordInput.Text);
                SameActions.SendCustomMessageBox(
                    $"Результат авторизации: {(Cookie.currentAccountId == -1 ? "Неверные данные" : "Авторизован")}",
                    Cookie.currentAccountId == -1 ? MessageBoxImage.Stop : MessageBoxImage.Information);
                // Открытие окна
                if (Cookie.currentAccountRole == "Программист")
                    SameActions.OpenNextWindowInterface<AddReadyTask>(
                        this,
                        this.Height,
                        this.Width,
                        this.Left,
                        this.Top);
               else if (Cookie.currentAccountRole == "Менеджер")
                    SameActions.OpenNextWindowInterface<ShowNewTasks>(
                        this,
                        this.Height,
                        this.Width,
                        this.Left,
                        this.Top);
                else if (Cookie.currentAccountRole == "Начальник")
                    SameActions.OpenNextWindowInterface<BossDashboard>(
                        this,
                        this.Height,
                        this.Width,
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

        //private async void setData()
        //{
        //    //int index = TaskStackPanel.Children.Count;
        //    //for (int i = 0; i < 5; i++) {
        //    //    Border block = new Border();
        //    //    block.HorizontalAlignment = HorizontalAlignment.Center;

        //    //    TextBlock textBlock = new TextBlock();
        //    //    textBlock.Text = $"Задача {i}";
        //    //    block.Child = textBlock;
        //    //    TaskStackPanel.Children.Add(block);
        //    //}

        //    foreach (TaskModel task in await SupabaseHelper.GetTasksByEmployeeAsync())
        //    {
        //        var border = new Border
        //        {
        //            BorderBrush = Brushes.LightGray,
        //            BorderThickness = new Thickness(1),
        //            Margin = new Thickness(4),
        //            Padding = new Thickness(8),
        //            CornerRadius = new CornerRadius(4)
        //        };
        //        var grid = new Grid();
        //        grid.ColumnDefinitions.Add(new()); grid.ColumnDefinitions.Add(new());


        //        grid.RowDefinitions.Add(new());
        //        var title = new TextBlock
        //        {
        //            Text = $"{task.Title}",
        //            FontSize = 32,
        //            Margin = new Thickness(2)
        //        };
        //        Grid.SetRow(title, 0); Grid.SetColumn(title, 0);
        //        grid.Children.Add(title);

        //        grid.RowDefinitions.Add(new());
        //        var description = new TextBlock
        //        {
        //            Text = $"{task.Text}",
        //            Margin = new Thickness(2)
        //        };
        //        Grid.SetRow(description, 1); Grid.SetColumn(description, 0);
        //        grid.Children.Add(description);

        //        grid.RowDefinitions.Add(new());
        //        var date_Start = new TextBlock
        //        {
        //            Text = $"{task.CreateDate.ToString()}",
        //            Margin = new Thickness(2)
        //        };
        //        Grid.SetRow(date_Start, 2); Grid.SetColumn(date_Start, 0);
        //        grid.Children.Add(date_Start);

        //        grid.RowDefinitions.Add(new());
        //        var dateEnd = new TextBlock
        //        {
        //            Text = $"{task.EndDate.ToString()}",
        //            Margin = new Thickness(2)
        //        };
        //        Grid.SetRow(dateEnd, 3); Grid.SetColumn(dateEnd, 0);
        //        grid.Children.Add(dateEnd);




        //        var btns = new StackPanel { Orientation = Orientation.Vertical, Margin = new Thickness(6, 0, 0, 0) };
        //        btns.Children.Add(new Button { Content = "✅" });
        //        btns.Children.Add(new Button { Content = "❌" });
        //        Grid.SetColumn(btns, 1); Grid.SetRowSpan(btns, 4);
        //        grid.Children.Add(btns);

        //        border.Child = grid;
        //        DataPanel.Children.Add(border);
        //    }
        //}
    }
}