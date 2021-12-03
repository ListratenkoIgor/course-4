using System;
using ChessClient;
using static System.Console;
using ChessRules;
namespace ConsoleChess
{
    class Program
    {
        public string HOST = "https://localhost:44378/api/Games";
        public string USER = "Black";
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            Program program = new Program();
            program.Start();
        }
        Client client;

        void Start() {
            string move;
            string[] parts;
            GameInfo gi;
            client = new Client(HOST,USER);
            WriteLine(client.host);
            WriteLine(client.GetCurrentGame("1"));
            while (true) {
                move=ReadLine();
                parts = move.Split();
                if (parts[0] == "exit")
                    break;
                WriteLine(parts[1] + "/" + parts[2]);
                gi =client.GetCurrentGame(parts[1] + "/"+parts[2]);
                WriteLine(ChessToAscii(new Chess(gi.FEN)));

            }
            //client.GetCurrentGame("2");
        }
        static string ChessToAscii(Chess chess)
        {
            string text = "+-----------------+\n ";
            for (int y = 7; y >= 0; y--)
            {
                text += y + 1;
                text += " | ";

                for (int x = 0; x < 8; x++)
                {
                    text += chess.GetFigureAt(x, y) + " ";
                }
                text += " |\n ";
            }
            text += "+-----------------+\n";
            text += "  a b c d e f g h \n";
            return text;
        }

    }
}
