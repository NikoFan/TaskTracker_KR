using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using TaskTracker_KR.Models;

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
        /// <param name="height">Высота окна</param>
        /// <param name="width">Ширина окна</param>
        /// <param name="leftDistance">Расстояние от левой части экрана</param>
        /// <param name="topDistance">Расстояние от верхней части экрана</param>
        public static void OpenNextWindowInterface<T>(
            Window oldWindow,
            double height,
            double width,
            double leftDistance,
            double topDistance) where T : Window, new() 
        {
            
            var window = new T()
            {
                WindowStartupLocation = WindowStartupLocation.Manual,
                Height = height,
                Width = width,
                Left = leftDistance,
                Top = topDistance
            };
            oldWindow.Visibility = Visibility.Collapsed;
            window.Show();
        }

        /// <summary>
        /// Метод проверки введенных пользователем данных на наличие SQLI
        /// </summary>
        /// <param name="stringDataSet">Массив данных от пользователя, 
        /// который проверяется на SQLI</param>
        public static Boolean VerifyUserInputDataForSQLI(
            List<String> stringDataSet)
        {
            foreach(var item in stringDataSet)
            {
                if (item.ToString().Contains("'")
                    || item.ToString().Contains("-")
                    || item.ToString().Contains(";"))
                    return false;
             
            }
            return true;
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


        /// <summary>
        /// Установка в ComboBox данных о программистах из группы
        /// </summary>
        /// <param name="progList">Список программистов</param>
        /// <param name="combobox">Выпаадющий список</param>
        public static void PutProgrammersList(
            List<ProgrammerBusyInfo> progList,
            ComboBox combobox)
        {
            foreach (var element in progList)
            {
                combobox.Items.Add(new ProgrammersBusyToCombobox(
                    element.ProgrammerId,
                    element.ProgrammerName,
                    element.IsBusy).ToString());
                
            }
        }
    }

    // Модель для установки текста в выпадающий список
    public class ProgrammersBusyToCombobox(long id, string name, bool status)  // Со скобками
    {
        public long ProgrammerId { get; set; } = id;
        public string ProgrammerName { get; set; } = name;
        public bool ProgrammerStatus { get; set; } = status;
        public override string ToString() => $"{ProgrammerId}. {ProgrammerName} | Статус: {ProgrammerStatus} {(ProgrammerStatus == true ? "Занят" : "Свободен")}";
    }
}
