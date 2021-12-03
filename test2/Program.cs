using System;
using ChessRules;
namespace test2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Chess chess = new Chess("rnbqkbnr/ppppp2p/5p2/6p1/8/8/1b6/R2QK2R w KQkq d6 0 4");
            Chess chess = new Chess("k7/PP6/K7/8/8/8/8/8 b ---- -- 0 4");
            while (true)
            {
                Console.WriteLine(chess.fen);
                if (chess.IsStalemate)
                    Console.WriteLine("IS STALEMATE");
                if (chess.IsCheckMate)
                    Console.WriteLine("IS CHECKMATE");
                Console.WriteLine(ChessToAscii(chess));
                foreach (string moves in chess.GetAllMoves())
                {
                    Console.Write(moves + "\n");
                }
                Console.WriteLine();
                string move = Console.ReadLine();
                if (move == "exit") 
                    break;
                chess = chess.Move(move);
                if(chess.IsStalemate)
                    Console.WriteLine("IS STALEMATE");
                if (chess.IsCheckMate)
                    Console.WriteLine("IS CHECKMATE");              


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

            //Console.WriteLine("Hello World!");
        }
    }
}
