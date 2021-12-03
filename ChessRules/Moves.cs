using System;
using System.Collections.Generic;
using System.Linq;



namespace ChessRules
{
	class Moves
	{
		FigureMoving fm;
		Board board;

		public Moves(Board board)
		{
			this.board = board;

		}
		~Moves() { }
		public bool CanMove(FigureMoving fm)
		{
			this.fm = fm;
			bool canMove = CanMoveFrom() && CanMoveTo() && CanFigureMove();
			//Console.WriteLine("Memory used before collection:       {0:N0}", GC.GetTotalMemory(false));
			GC.Collect(2, GCCollectionMode.Forced, false);
			//Console.WriteLine("Memory used after full collection:   {0:N0}", GC.GetTotalMemory(true));
			return canMove;
				

		}
		bool CanMoveFrom()
		{
			return fm.from.OnBoard() && fm.figure.GetColor() == board.moveColor;

		}
		bool CanMoveTo()
		{
			return fm.to.OnBoard() &&fm.to!=fm.from&& board.GetFigureAt(fm.to).GetColor() != board.moveColor;

		}
		bool CanFigureMove()
		{
			switch (fm.figure) {
				case Figure.whiteKing:
				case Figure.blackKing:
					return CanKingMove();
				case Figure.whiteRook:
				case Figure.blackRook:
					return (fm.SignX == 0 || fm.SignY == 0)&&CanStraightMove();
				case Figure.whiteBishop:
				case Figure.blackBishop:
					return (fm.SignX != 0 && fm.SignY != 0) && CanStraightMove();
				case Figure.whiteQueen:
				case Figure.blackQueen:
					return CanStraightMove();
				case Figure.whiteKnight:
				case Figure.blackKnight:
					return CanKnightMove();
				case Figure.whitePawn:
				case Figure.blackPawn:
					return CanPawnMove();
				default: 
					return false;
			}
		}

		private bool CanPawnMove() {
			if (fm.from.y < 1 || fm.from.y > 6) return false;
			int stepY = fm.figure.GetColor() == Color.white ? 1 : -1;
			return CanPawnGo(stepY) || CanPawnJump(stepY) || CanPawnEat(stepY);
		}
		private bool CanPawnGo(int stepY)
		{ if (board.GetFigureAt(fm.to) == Figure.none)
				if (fm.DeltaX == 0 && fm.DeltaY == stepY)
					return true;
			return false;

		}
		private bool CanPawnJump(int stepY)
		{
			if (fm.from.y == 1 || fm.from.y == 6)
				if (board.GetFigureAt(fm.to) == Figure.none)
					if (fm.DeltaX == 0 && fm.DeltaY == 2*stepY)
							if (board.GetFigureAt(new Square(fm.from.x,fm.from.y+stepY))==Figure.none)
						return true;
			return false;
		}
		private bool CanPawnEat(int stepY)
		{
			if (board.GetFigureAt(fm.to) != Figure.none)
				if (fm.AbsDeltaX == 1 && fm.DeltaY == stepY)
					return true;
			//enpassan rule
			string[] parts = board.fen.Split();
			if (parts[3]!="-")
				if (new Square(parts[3])==fm.to)
					if (board.GetFigureAt(fm.to) == Figure.none)
						if (fm.AbsDeltaX == 1 && fm.DeltaY == stepY)
							return true;
			return false;
		}
		private bool CanStraightMove() {
			Square at = new Square(fm.from);
			do {
				at = new Square(at.x + fm.SignX, at.y + fm.SignY);
				if (at == fm.to)
					return true;
			} while (at.OnBoard()&&board.GetFigureAt(at)==Figure.none);
			return false;
		}
		private bool CanKingMove() {
			if (fm.AbsDeltaX <= 1 && fm.AbsDeltaY <= 1) return true;
			if (this.CanKingCastling()) return true;
			return false;
		}
		private bool CanKnightMove()
		{
			if (fm.AbsDeltaX == 1 && fm.AbsDeltaY == 2) return true;
			if (fm.AbsDeltaX == 2 && fm.AbsDeltaY == 1) return true;
			return false;
		}
		private bool CanKingCastling()
		{
			string[] parts = board.fen.Split();
			int side = fm.DeltaX>0  ? 0 : 1;
			int color = fm.figure.GetColor() == Color.white ? 0 : 1;
			Square bittenSquare = new Square(fm.from.x + fm.SignX, fm.from.y);
			if (parts[2][color * 2 + side] != '-')
				if (this.fm.AbsDeltaX==2&&this.fm.DeltaY==0)
				//if (fm.figure.GetColor() == Color.white)
					if (!board.IsCheck())
						if (board.GetFigureAt(bittenSquare) == Figure.none && board.GetFigureAt(fm.to) == Figure.none)
							if (!board.IsCheckAfterMove(fm) &&
									!board.IsCheckAfterMove(new FigureMoving(new FigureOnSquare(fm.figure, fm.from), bittenSquare)))
							{
								return true;
							}
			return false;
		}

	}

}