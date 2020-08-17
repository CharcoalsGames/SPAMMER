using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Net.Mail;
using System.Text;
using System.Reflection.Metadata;

namespace EMAIL_SPAMMER
{
    class Program
    {
        public static string accountstxtpath, targetemail, smtpserver, domain;
        public static string[] user, password, accounts;

        static int threads = 100;
        static bool work = false;

        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Gray;

            Thread counter = new Thread(setTitle)
            {
                IsBackground = false
            };
            counter.Start();

            EmailSpam.LoadData();

            Console.Write("Accounts path: ");

            accountstxtpath = Console.ReadLine().Replace("\"", "").Trim();
            accounts = File.ReadAllLines(accountstxtpath);

            user = new string[accounts.Length];
            password = new string[accounts.Length];

            for (int i = 0; i < accounts.Length; i++)
            {
                string help = (accounts[i].ToString());

                user[i] = help.Split(':')[0];
                password[i] = help.Split(':')[1];
            }

            Console.WriteLine($"Loaded {user.Length} accounts and {password.Length} passwords!");
            Console.Write("Target email: ");
            targetemail = Console.ReadLine();
            Console.Write("SMTP Server (smtp.gmail.com): ");
            smtpserver = Console.ReadLine();
            Console.Write("Domain (@gmail.com): ");
            domain = Console.ReadLine();
            Console.Write("Threads: ");
            threads = int.Parse(Console.ReadLine());

            work = true;

            List<Thread> workers = new List<Thread>();

            for (int i = 0; i < threads; i++)
            {
                Thread t = new Thread(Worker);
                t.Start();
                workers.Add(t);
            }

            while (true)
            {
                if (Console.ReadLine().Trim().ToLower() == "stop")
                {
                    Console.WriteLine("Stopping threads...");
                    work = false;
                    foreach (Thread t in workers)
                    {
                        t.Join();
                    }
                    break;
                }
            }

            Console.WriteLine("All threads stopped!");
            Console.ReadKey();
        }

        static void setTitle()
        {
            while (true)
            {
                Console.Title = $"Succes: {EmailSpam.succes} Failure: {EmailSpam.failure}";
            }
        }
        static void Worker()
        {
            while (work)
            {
                 EmailSpam.SendEmail();
            }
        }
    }
}