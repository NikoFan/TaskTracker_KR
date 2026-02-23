using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TaskTracker_KR.Properties;

namespace TaskTracker_KR.Services
{
    public static class Cookie
    {
        // id активного аккаунта
        private static int _id = -1;
       
        // Размер окна
        private static WindowState _windowState = WindowState.Normal;

        // Разрешения для пользователя
        private static Dictionary<String, Boolean> _accountRights = new Dictionary<string, bool>();
        public static int currentAccountId
        {
            get 
            {
                return _id;
            } 
            set 
            {
                _id = value;
            } 
        }

        public static WindowState windowState { get { return _windowState; } set { _windowState = value; } }

        public static Dictionary<String, Boolean> accountRights { get { return _accountRights; } set { _accountRights = value; } }

    }
}
