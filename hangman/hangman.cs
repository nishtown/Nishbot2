using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;

namespace Hangman
{
    public class Hangman
    {
        public bool Active { get; private set; }
        private Settings settings { get; set; }

        public Hangman()
        {
            settings.Players = new List<string>();
        }

        public void NewGame()
        {
            Active = true;
            settings.Players.Clear();
            generateWord();
        }

        public string EndGame()
        {
            string oldword = settings.Word;
            Active = false;
            settings.Word = "";
            settings.Players.Clear();
            settings.Hashedword = "";

            return oldword;
        }

        public void JoinGame(string user)
        {
            if (settings.Players.Contains(user) == false)
                settings.Players.Add(user);
        }

        public void LeaveGame(string user)
        {
            if (settings.Players.Contains(user))
                settings.Players.Remove(user);
        }

        public bool CanPlay(string user)
        {
            return settings.Players.Contains(user);
        }

        public bool Checkwin()
        {
            if (settings.Hashedword.ToLower() == settings.Word.ToLower())
            {
                Active = false;
                return true;
            }
            else
            {
                return true;
            }
        }

        public bool Guess(char c)
        {
            if (settings.Word.ToLower().Contains(c))
            {
                for (int i = 0; i< settings.Word.Length; i++)
                {
                    if (settings.Word[i] == c)
                    {
                        StringBuilder sb = new StringBuilder(settings.Hashedword);
                        sb[i] = c;
                        settings.Hashedword = sb.ToString();
                    }
                }
                return true;
            }
            else
            {
                settings.AttemptsRemaining--;
                return false;
            }
        }

        public bool Solve(string guess)
        {
            if(guess == settings.Word)
            {
                return true;
            }
            else
            {
                settings.AttemptsRemaining--;
                return false;
            }
        }

        private void generateWord()
        {
            string url = "http://www.vocabula.com/feature/definitions.aspx";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            settings.Wordurl = url;
            if (response.StatusCode == HttpStatusCode.OK)
            {
                settings.Wordurl = response.ResponseUri.ToString();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;
                if (response.CharacterSet == null)
                    readStream = new StreamReader(receiveStream);
                else
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));

                string data = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                var regex = new Regex(@"<span id=""lblWord"" class=""Word"">(.*?)</span>");
                var match = regex.Match(data);
                settings.Word = match.Groups[1].Value;

                settings.AttemptsRemaining = Convert.ToInt32(settings.Word.Length * 0.75);

                regex = new Regex(@"<span id=""lblDefinition"" class=""Definition"">(.*?)</span>");
                match = regex.Match(data);
                var defs = match.Groups[1].Value.Split(' ');

                Random random = new Random((int)DateTime.Now.Ticks);
                int randomNumber = random.Next(1, defs.Count());

                int y = 0;
                for (int x = 0; x < defs.Count(); x++)
                {
                    y++;
                    if (y == 3)
                    {
                        defs[x] = "";
                        y = 0;
                    }

                    settings.Definition += defs[x] + " ";
                }

                settings.Word = settings.Word.Replace(" ", "");
            }
        }

    }

    public class Settings
    {
        public string Word { get; set; }
        public string Hashedword { get; set; }
        public int Length { get; set; }
        public string Wordurl { get; set; }
        public string Definition { get; set; }
        public List<string> Players { get; set; }
        public int AttemptsRemaining { get; set; }
    }
}
