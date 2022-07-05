using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSystemVisitor
{
    public class Notifications
    {
        public delegate void EventHandler(string message);
        public EventHandler Notify;
        public void Start()
        {
            Notify?.Invoke($"\n*** Start ***\n");   // 2.Вызов события 
        }

        public void Finish()
        {
            Notify?.Invoke($"\n*** Finish ***\n");   // 2.Вызов события 
        }
        public void DisplayMessage(string message) => Console.WriteLine(message);


    }
}
