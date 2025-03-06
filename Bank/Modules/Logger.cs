using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Modules
{
    public class Logger
    {
        public static bool Logging = false;
        private static string fileName = "transaction_errors.log";

        public static void clear()
        {
            File.WriteAllText(fileName, string.Empty);
        }

        public static void log(string msg)
        {
            if (!Logging)
            {
                return;
            }
            File.AppendAllText(fileName, msg + "\n");
        }

        public static Exception logException(Exception exception)
        {
            if (!Logging)
            {
                return exception;
            }
            log(@$"Вемямя: {TimeOnly.FromDateTime(DateTime.Now)}
Тип: {exception.GetType().ToString()}
Описание: {exception.ToString()}");
            return exception;
        }
    }
}
