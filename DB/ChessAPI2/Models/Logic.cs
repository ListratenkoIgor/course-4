using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChessRules;
namespace ChessAPI2.Models
{
    public class Logic
    {

        private ModelChessDB db;
        public Logic() {
            db = new ModelChessDB();
        
        }
        public Game GetCurrentGame() {
            Game game = db
                .Games
                .Where(g => g.Status == "play")
                .OrderBy(g => g.GameID)
                .FirstOrDefault();
            if (game == null)
                game = CreateNewGame();
            return game;
        
        }
        public IQueryable<Game> GetActiveGames()
        {
            return  db
                .Games
                .Where(g => g.Status == "play");
                

            //return game;

        }
        private Game CreateNewGame()
        {
            Game game = new Game();
            Chess chess = new Chess();
            game.FEN = chess.fen;
            game.Status = "play";
            db.Games.Add(game);
            db.SaveChanges();
            return game;
        }

        internal Game GetGame(int id) {

            return db
                .Games
                .Where(g => g.GameID == id)
                .First();

        }
        internal Game GetGame(int id,string user)
        {

            Game game = new Game();
            game = db.Games.Where(g => g.GameID == id).First();

            game.YourColor = PlayerColor(user, game);
            return game;



        }
        internal Game MakeMove(int id, string move)
        {

            //DropPrimaryKey("dbo.ProductTypes");
            Game game = GetGame(id);
            if (game == null) return game;
            if (game.Status != "play") return game;


            Chess chess = new Chess(game.FEN);
            Chess chessNext = chess.Move(move);

            if (chess.fen == chessNext.fen) return game;
            if (chessNext.IsCheckMate || chessNext.IsStalemate)
              game.Status = "done";       
           
            game.FEN = chessNext.fen;
            //db.Entry(game);
            db.Entry(game).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return game;
        }

        string PlayerColor(string user,Game game ) {
            if (game.White == user)
            {
                return "White";
            }
            else
            {
                if (game.Black == user)
                {
                    return  "Black";
                }
                else
                {
                    return "NULL";
                }
            }
        }
        string MoveColor(string move){
            switch (move[0]) {
                case 'p':
                case 'r':
                case 'n':
                case 'b':
                case 'q':
                case 'k':
                    return "Black";
                case 'P':
                case 'R':
                case 'N':
                case 'B':
                case 'Q':
                case 'K':
                    return "White";
                default:
                    return "NULL";

            }
        
        
        }
        internal Game MakeMove(string user, int id, string move)
        {
            

            //DropPrimaryKey("dbo.ProductTypes");
            Game game = GetGame(id);
            if (game == null) return game;
            if (game.Status != "play") return game;
            if ((PlayerColor(user, game) == "NULL") || (MoveColor(move)!= PlayerColor(user, game))) {
                return game;
            }

            Chess chess = new Chess(game.FEN);
            Chess chessNext = chess.Move(move);

            if (chess.fen == chessNext.fen) return game;
            if (chessNext.IsCheckMate || chessNext.IsStalemate)
                game.Status = "done";

            game.FEN = chessNext.fen;
            //db.Entry(game);
            db.Entry(game).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return game;
        }
    }
}