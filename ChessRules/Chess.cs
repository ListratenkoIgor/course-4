using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessRules
{
    public class Chess
    {
		Board board;
		Moves moves;
		public string LastMove { get; private set; }
		public bool IsCheckMate { get; private set; }
		public bool IsStalemate { get; private set; }
		List<FigureMoving> allMoves;
		public string fen { get; private set; }
		public Chess(string fen = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPPP/RNBQKBNR w KQkq - 0 1")
		{
			this.fen = fen;
			board = new Board(this.fen);
			moves = new Moves(board);
			if (this.GetAllMoves().Count() == 0)
			{
				this.IsStalemate = true;
				if (this.board.IsSelfCheck())
				{
					this.IsStalemate = false;
					this.IsCheckMate = true;
				}
			}
		}
		~Chess() {
			
			//Console.WriteLine("Chess Destruction\nMemory used before collection:       {0:N0}", GC.GetTotalMemory(false));
			GC.Collect(2, GCCollectionMode.Forced, false);
			//Console.WriteLine("Memory used after full collection:   {0:N0}", GC.GetTotalMemory(true));
		}
		Chess(Board board)
		{
			this.board = board;
			this.LastMove = board.LastMove;
			this.fen = board.fen;
			moves = new Moves(board);
			if (this.GetAllMoves().Count() == 0)
			{
				this.IsStalemate = true;
				if (this.board.IsSelfCheck())
				{
					this.IsStalemate = false;
					this.IsCheckMate = true;
				}
			}
		}
		public Chess Move(string move)
		{

			FigureMoving fm = new FigureMoving(move);
			if (!(moves.CanMove(fm) && !board.IsCheckAfterMove(fm)))
				return this;
			Board nextBoard = board.Move(fm);
			Chess nextChess = new Chess(nextBoard);
			if (nextChess.GetAllMoves().Count() == 0) {
				nextChess.IsStalemate = true;
				if (nextBoard.IsSelfCheck()) {
					nextChess.IsStalemate = false;
					nextChess.IsCheckMate = true;
				}
			}
			return nextChess;
		}
		public char GetFigureAt(int x, int y)
		{
			//return '.';     
			Square square = new Square(x, y);
			Figure f = board.GetFigureAt(square);
			
			return f == Figure.none ? '.' : (char)f;
		}
		void FindAllMoves()
		{

			allMoves = new List<FigureMoving>();
			foreach (FigureOnSquare fs in board.YieldFigures())
			{
				foreach (Square to in Square.YieldSquares())
				{
					FigureMoving fm = new FigureMoving(fs, to);
					if (moves.CanMove(fm))
					{
						if (!board.IsCheckAfterMove(fm))
							allMoves.Add(fm);
					}
				}
			}
		}
		public List<string> GetAllMoves()
		{
			FindAllMoves();
			List<string> list = new List<string>();
			foreach (FigureMoving fm in allMoves)
			{
				list.Add(fm.ToString());
			}
			//Console.WriteLine("after get all moves\nMemory used before collection:       {0:N0}", GC.GetTotalMemory(false));
			GC.Collect(2, GCCollectionMode.Forced, false);
			//Console.WriteLine("Memory used after full collection:   {0:N0}", GC.GetTotalMemory(true));
			return list;
		}

	}
}
