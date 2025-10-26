using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BedeLotteryConsoleUI.Services
{
    public class ConsoleUserInterface : IUserInterface
    {
        public string ReadLine() => Console.ReadLine() ?? string.Empty;
        public void WriteLine(string message) => Console.WriteLine(message);
        public void Write(string message) => Console.Write(message);
    }
}
