using Supabase.Gotrue.Mfa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace TaskTracker_KR.Services
{
    public static class SameActions
    {
        /// <summary>
        /// Установка прав доступка к кнопкам
        /// на левой части интерфейса
        /// </summary>
        /// <param name="leftSideButtons">Список с кнопками</param>
        public static void SetLeftSideButtonsEnableStatus(
            List<Button> leftSideButtons)
        {
            foreach (var button in leftSideButtons) {
                // Определение активности
                button.IsEnabled = Cookie.accountRights[button.Uid];
            }
        }

        /// <summary>
        /// Контроль перед открытием окна его размеров и изменение иконки кнопки
        /// </summary>
        /// <param name="currentWindowState">Текущий размер окна</param>
        /// <param name="fullOpenIcon">Иконка кнопки раскрытия окна</param>
        public static void ControlWindowStateStatus(
            WindowState currentWindowState,
            Image fullOpenIcon)
        {
            // Создание объекта для обработки png изображения
            BitmapImage icon = new BitmapImage();
            icon.BeginInit();
            // Определение размеров окна
            if (currentWindowState == WindowState.Normal)
                icon.UriSource = new Uri(
                    "/icons/square.png",
                    UriKind.RelativeOrAbsolute);
            else
                icon.UriSource = new Uri(
                    "/icons/double_square.png",
                    UriKind.RelativeOrAbsolute);
            icon.EndInit();
            // Установка новой иконки
            fullOpenIcon.Source = icon;
        }

        /// <summary>
        /// Метод изменения размеров окна по нажатию кнопки
        /// </summary>
        /// <param name="pastWindowState">Текущий размер окна</param>
        /// <param name="fullOpenIcon">Иконка кнопки раскрытия окна</param>
        /// 
        public static void ResizeWindowState(
            Window pastWindowState,
            Image fullOpenIcon)
        {
            if (pastWindowState.WindowState == WindowState.Maximized)
                // Из раскрытого -> в обычное
                pastWindowState.WindowState = WindowState.Normal;
            else
                // Из обычного -> в раскрытое
                pastWindowState.WindowState = WindowState.Maximized;
            Cookie.windowState = pastWindowState.WindowState;
            ControlWindowStateStatus(pastWindowState.WindowState, fullOpenIcon);
        }

        /// <summary>
        /// Открывание окна интерфейса
        /// </summary>
        /// <param name="oldWindow">Старое окно</param>
        /// <param name="leftDistance">Расстояние от левой части экрана</param>
        /// <param name="topDistance">Расстояние от верхней части экрана</param>
        public static void OpenNextWindowInterface<T>(
            Window oldWindow,
            double leftDistance,
            double topDistance) where T : Window, new() 
        {
            
            var window = new T()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Left = leftDistance,
                Top = topDistance
            };
            oldWindow.Visibility = Visibility.Collapsed;
            window.Show();
        }

        /// <summary>
        /// Отправка MessageBox пользователю
        /// </summary>
        /// <param name="message">Текст сообщения</param>
        /// <param name="icon">Иконка сообщения</param>
        public static Boolean SendCustomMessageBox(
            string message,
            MessageBoxImage icon,
            bool? buttons = null)
        {
            return MessageBox.Show(
                message,
                "Трекер",
                buttons == null ? MessageBoxButton.OK : MessageBoxButton.YesNo,
                icon) == MessageBoxResult.Yes;
        }
    }
}
