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
        private static int _id = -1;
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
    }
}
