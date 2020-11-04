using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMAIL_SPAMMER
{
    public static class logs
    {
        public static void CreateFile()
        {
            if (!File.Exists("log.txt")) 
                File.Create("log.txt");
            AddLog($"[{DateTime.Now}] SPAMMER Started");
        }

        public static void AddLog(string x)
        {
            File.AppendAllText("log.txt", x);
        }
    }
}
