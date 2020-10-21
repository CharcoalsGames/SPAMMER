using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace EMAIL_SPAMMER
{
    public static class Program
    {
        public static string accountstxtpath, targetemail, smtpserver, domain;
        public static string[] user, password, accounts;

        public static int smtpporttls;
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

            Console.WriteLine("S P A M M E R");

            EmailSpam.LoadData();

            Console.Write("Target email: ");
            targetemail = Console.ReadLine();

            if (!File.Exists("config.txt"))
            {
                Console.WriteLine("No config file detected, creating new one!");

                Console.Write("Accounts path: ");
                accountstxtpath = Console.ReadLine().Replace("\"", "").Trim();                

                Console.Write("SMTP Server (smtp.gmail.com): ");
                smtpserver = Console.ReadLine();
                Console.Write("Domain (@gmail.com): ");
                domain = Console.ReadLine();
                Console.Write("Threads: ");
                threads = int.Parse(Console.ReadLine());
                Console.Write("SMTP Port: ");
                smtpporttls = int.Parse(Console.ReadLine());
                File.WriteAllText("config.txt", string.Join("\r\n", new[] { smtpserver, domain, threads.ToString(), accountstxtpath, smtpporttls.ToString() }));
            }
            else
                LoadConfig();

            accounts = File.ReadAllLines(accountstxtpath);

            user = new string[accounts.Length];
            password = new string[accounts.Length];

            for (int i = 0; i < accounts.Length; i++)
            {
                string help = (accounts[i].ToString());

                user[i] = help.Split(':')[0];
                password[i] = help.Split(':')[1];
            }

            if (user.Length == password.Length)
            {
                Console.WriteLine($"Loaded {user.Length} users and {password.Length} passwords! Everything are good!");
            }
            else
            {
                Console.WriteLine($"Something went wrong with accounts, detected {user.Length} users and {password.Length} :O");
                return;
            }

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
                    Console.WriteLine();
                    Console.WriteLine("Stopping... this is can take a while...");
                    Console.WriteLine();
                    work = false;
                    foreach (Thread t in workers)
                    {
                        t.Join();
                    }
                    break;
                }
            }
            Console.WriteLine("SPAMMER Stoped!");
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

        static void LoadConfig()
        {
            var ConfigTXT = File.ReadAllLines("config.txt");
            smtpserver = ConfigTXT[0];
            domain = ConfigTXT[1];
            threads = int.Parse(ConfigTXT[2]);
            accountstxtpath = ConfigTXT[3];
            smtpporttls = int.Parse(ConfigTXT[4]);
            Console.WriteLine("Config loaded!");
        }
    }
} 