using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


namespace ChessClient
{
    public class Client
    {
        public string host { get; private set; }
        public string user { get; private set; }
        public Client(string host,string user) {
            this.host = host;
            this.user = user;
        
        }
        int CurrentGameID;
        public GameInfo GetCurrentGame(string param = "") {

            GameInfo gameInfo = new GameInfo(ParseJson(CallServer(param)));
            CurrentGameID= gameInfo.GameID;
            return gameInfo;
            var list = ParseJson(CallServer(param));
            foreach (string key in list){
                Console.WriteLine(key + " " + list[key]); 
            }
            //Console.WriteLine(CallServer(param));
        }
        public GameInfo SendMove(string move )
        {
            string json = CallServer(CurrentGameID + "/" + move);
            var list = ParseJson(CallServer(json));
            GameInfo gameInfo = new GameInfo(list);
            CurrentGameID = gameInfo.GameID;
            return gameInfo;

        }
        private string CallServer(string param = "") {
            string adress = host + "/" + param;
            WebRequest request = WebRequest.Create(adress);
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
                return reader.ReadToEnd();

        }
        private NameValueCollection ParseJson(string json) { 
            string pattern = @"""(\w+)\"":""?([^,""]*)""?";
            NameValueCollection list = new NameValueCollection();
            foreach (Match m in Regex.Matches(json, pattern))
            {
                //Console.WriteLine("'{0}' found at index {1}.", m.Value, m.Index);

                if (m.Groups.Count == 3)
                    list[m.Groups[1].Value] = m.Groups[2].Value;
            
            
            }

            return list;
        }


    }
}
