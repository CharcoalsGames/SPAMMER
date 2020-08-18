using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace EMAIL_SPAMMER
{
    public static class EmailSpam
    {
        static string[] topics, subjects;
        public static uint succes, failure;

        public static void SendEmail()
        {
            Random rnd = new Random();

            for (int i = 0; i < Program.accounts.Length; i++)
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient(Program.smtpserver);

                mail.From = new MailAddress(Program.user[i] + Program.domain);
                mail.To.Add(Program.targetemail);

                mail.Subject = topics[rnd.Next(1, topics.Length - 1)].ToString();
                mail.Body = subjects[rnd.Next(1, subjects.Length - 1)].ToString();

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(Program.user[i], Program.password[i]);
                SmtpServer.EnableSsl = true;

                try
                {
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    SmtpServer.Send(mail);
                    Console.WriteLine($"{DateTime.Now} - Email sended from {Program.user[i] + Program.domain} - {mail.Subject}");
                    succes++;
                }
                catch(Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    Console.WriteLine($"{DateTime.Now} - Email NOT sended! ({Program.user[i] + Program.domain} - {e.Message})");
                    failure++;
                }
            }
        }

        public static void LoadData()
        {
            if (File.Exists("topics.txt") || File.Exists("subjects.txt"))
            {
                topics = new string[File.ReadAllLines("topics.txt").Length];
                subjects = new string[File.ReadAllLines("subjects.txt").Length];
                topics = File.ReadAllLines("topics.txt");
                subjects = File.ReadAllLines("subjects.txt");
            }
            else
            {
                File.Create("topics.txt");
                File.Create("subjects.txt");
                File.WriteAllText("topics.txt", string.Join("\r\n", new[] { "example topic", "second topic", "etc..."}));
                File.WriteAllText("subjects.txt", string.Join("\r\n", new[] { "example subject", "next random subject", "etc..." }));
                LoadData();
            }
        }
    }
}
