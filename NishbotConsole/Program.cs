using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nishbot;

namespace NishbotConsole
{
    class Program
    {
        private static Dictionary<string, Nishbot.Nishbot> ircbot = new Dictionary<string, Nishbot.Nishbot>();


        static void Main(string[] args)
        {
            bool bRunning = true;


            Console.WriteLine("### Welcome to the Nishbot Console ###");
            Console.WriteLine("'h' for more info");
            Console.WriteLine("'q' to quit");
            Console.WriteLine("");
            Console.WriteLine("");

            while (bRunning)
            {
                Console.Write("> ");
                string msg = Console.ReadLine();
                if (msg.StartsWith("q"))
                {
                    bRunning = false;
                }
                else
                {
                    ProcessCommand(msg);
                }
            }
        }

        static void ProcessCommand(string msg)
        {
            if (msg.Length == 0 && msg.Contains(' ') == false)
                return;

            string[] settings = msg.Split(' ');

            switch (msg.Substring(0, 1).ToLower())
            {
                case "c":
                    if (settings.Length >= 2)
                    {
                        string serverident = settings[1].Split('.')[1];
                        int i = 1;
                        while(ircbot.ContainsKey(serverident))
                        {
                            serverident = serverident + i;
                            i++;
                        }
                        Nishbot.Nishbot irc = new Nishbot.Nishbot();



                        ircbot.Add(serverident, irc);
                        Console.WriteLine("Connecting to server " + settings[1] + " as '" + serverident + "'");
                    }
                    break;
                case "d":
                    if (settings.Length >= 2)
                    {
                        if (ircbot.ContainsKey(settings[1]))
                        {
                            ircbot.Remove(settings[1]);
                        }
                        else
                        {
                            Console.WriteLine("Connection not found");
                        }
                    }
                    break;
                case "j":
                    if (settings.Length >= 3)
                    {
                        if (ircbot.ContainsKey(settings[1]))
                        {

                        }
                        else
                        {
                            Console.WriteLine("Connection not found");
                        }

                    }
                    break;
                case "l":
                    if (settings.Length >= 3)
                    {
                        if (ircbot.ContainsKey(settings[1]))
                        {

                        }
                        else
                        {
                            Console.WriteLine("Connection not found");
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Invalid chat command");
                    break;
            }
        }
    }
}
