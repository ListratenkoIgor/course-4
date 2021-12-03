using System;
using System.Collections.Generic;
using System.Text;

namespace ChessRules
{
    class Board
    {
		public Chess chess { get; private set; }
        public string fen { get; private set; }
        public Color moveColor { get; private set; }
        public int moveNumber { get; private set; }
        Figure[,] figures;

		public string LastMove { get; private set; }
		public bool IsCheckMate { get; private set; }
		public bool IsStalemate { get; private set; }

		public Board(string fen) {
			this.chess = null;
            this.fen = fen;
			this.LastMove = "-";
			this.IsCheckMate = false;
			this.IsStalemate = false;
            figures = new Figure[8, 8];
            Init();
			/////Console.WriteLine("After INIT()\nMemory used before collection:       {0:N0}", GC.GetTotalMemory(false));
			GC.Collect(2, GCCollectionMode.Forced, false);
			//Console.WriteLine("Memory used after full collection:   {0:N0}", GC.GetTotalMemory(true));
		}
		public Board(Chess chess,string fen)
		{
			this.chess = chess;
			this.fen = fen;
			this.LastMove = "-";
			this.IsCheckMate = false;
			this.IsStalemate = false;
			figures = new Figure[8, 8];
			Init();
			//Console.WriteLine("After INIT()\nMemory used before collection:       {0:N0}", GC.GetTotalMemory(false));
			GC.Collect(2, GCCollectionMode.Forced, false);
			//Console.WriteLine("Memory used after full collection:   {0:N0}", GC.GetTotalMemory(true));
		}
		~Board(){
			figures = null;
			//Console.WriteLine("Board destruction\nMemory used before collection:       {0:N0}", GC.GetTotalMemory(false));
			GC.Collect(2, GCCollectionMode.Forced, false);
			//Console.WriteLine("Memory used after full collection:   {0:N0}", GC.GetTotalMemory(true));
		}
        void Init() {
			string[] parts = fen.Split();
			if(parts.Length !=6)return;
			InitFigures(parts[0]);
			//InitColor(parts[1]);
			moveColor = (parts[1]=="b")?Color.black:Color.white;
			moveNumber = int.Parse(parts[5]);

		}
		void InitFigures(string data){
			string newData = "";
			//Console.WriteLine("Start initfigures\nMemory used before collection:       {0:N0}", GC.GetTotalMemory(false));
			GC.Collect(2, GCCollectionMode.Forced, false);
			//Console.WriteLine("Memory used after full collection:   {0:N0}", GC.GetTotalMemory(true));
			foreach (char symbol in data) {
				if (symbol <='8'||symbol>='1') {
					switch ((int)(symbol - '0')) { 
						case 1:
							newData = newData + ".";
							continue;
						case 2:
							newData = newData + "..";
							continue;
						case 3:
							newData = newData + "...";
							continue;
						case 4:
							newData = newData + "....";
							continue;
						case 5:
							newData = newData + ".....";
							continue;
						case 6:
							newData = newData + "......";
							continue;
						case 7:
							newData = newData + ".......";
							continue;
						case 8:
							newData = newData + "........";
							continue;
						default:
							newData = newData + symbol;
							continue;
					}
				}
			}
			/*for (int j = 8;j>=2;j--){
				data = data.Replace(j.ToString(),(j-1).ToString()+"1");
			}	
			data = data.Replace("1",".");*/
			//Console.WriteLine("Memory used before collection:       {0:N0}",GC.GetTotalMemory(false));
			//GC.Collect(2, GCCollectionMode.Forced, false);
			//Console.WriteLine("Memory used after full collection:   {0:N0}", GC.GetTotalMemory(true));
			string[] lines = newData.Split('/');
			for (int y = 7;y>=0;y--){
				for (int x = 0;x<8;x++){
					figures[x,y]=lines[7-y][x]=='.'?Figure.none:(Figure)lines[7-y][x];				
				}
			}
			//Console.WriteLine("after initfigures\nMemory used before collection:       {0:N0}", GC.GetTotalMemory(false));
			GC.Collect(2, GCCollectionMode.Forced, false);
			//Console.WriteLine("Memory used after full collection:   {0:N0}", GC.GetTotalMemory(true));

		}
		public IEnumerable<FigureOnSquare> YieldFigures() {
			foreach (Square square in Square.YieldSquares()) {
				if (GetFigureAt(square).GetColor() == moveColor) {
					yield return new FigureOnSquare(GetFigureAt(square), square);
				} 
			}
		}


		void GenerateFEN(FigureMoving fm)
		{
			this.fen = FenFigures() + " " + (moveColor == Color.white ? "w" : "b") + " " + fm.Castling + " "+ fm.EnPassant + " "+"0"+" " + moveNumber.ToString();
		}
		string FenFigures(){
			StringBuilder sb = new StringBuilder();
			for (int y = 7;y>=0;y--){
				for (int x = 0;x<8;x++){
					sb.Append(figures[x,y]==Figure.none?'1':(char)figures[x,y]);		
				}
				if (y>0)
					sb.Append('/');
			}
			string eight = "11111111";
			for (int j = 8;j>=2;j--){
				sb.Replace(eight.Substring(0,j),j.ToString());
			}
			//Console.WriteLine("After generate fen\nMemory used before collection:       {0:N0}", GC.GetTotalMemory(false));
			GC.Collect(2, GCCollectionMode.Forced, true);
			//Console.WriteLine("Memory used after full collection:   {0:N0}", GC.GetTotalMemory(true));

			return sb.ToString();
		}
        public Figure GetFigureAt(Square square) {
            if (square.OnBoard()) return figures[square.x, square.y];
            return Figure.none;
        }
        void SetFigureAt(Square square,Figure figure) {
            if (square.OnBoard()) figures[square.x, square.y] = figure;
        
        }
		public bool FixCheckMate()
		{
			return IsSelfCheck();
		}

		public bool FixStaleMate()
		{	if (chess == null) return false;
				if (chess.GetAllMoves().Count == 0) 
				{
					return true;
			
				}
			return false;
		}
		public bool FixCheckMate2()
		{
			return IsSelfCheck();
		}

		public bool FixStaleMate2()
		{
			Moves moves = new Moves(this);
			List<FigureMoving> allMoves = new List<FigureMoving>();
			foreach (FigureOnSquare fs in this.YieldFigures())
			{
				foreach (Square to in Square.YieldSquares())
				{
					FigureMoving fm = new FigureMoving(fs, to);
					if (moves.CanMove(fm))
					{
						if (!this.IsCheckAfterMove(fm))
							return false;
					}
				}
			}

			return true;
		}
		public Board Move(FigureMoving fm)
        {
            Board next = new Board(fen);
			fm.FixCastling(this);
			Square squareCastling = fm.GetCastlingSquare(this);
			if (squareCastling != Square.none) {
				Square squareRook = fm.GetRookSquare(this);
				next.SetFigureAt(squareRook, Figure.none);
				if (moveColor == Color.white) 
					next.SetFigureAt(squareCastling, Figure.whiteRook);
				else
					next.SetFigureAt(squareCastling, Figure.blackRook);
			}

			fm.FixEnPassant(this);
			Square squareEnPassant = fm.GetEnPassantSquare(this);
			if (squareEnPassant != Square.none) { 
				next.SetFigureAt(squareEnPassant, Figure.none); 
			}
	
            next.SetFigureAt(fm.from, Figure.none);
            next.SetFigureAt(fm.to, fm.promotion == Figure.none ? fm.figure : fm.promotion);
            if (moveColor == Color.black) 
				next.moveNumber++;
			next.moveColor = moveColor.FlipColor();
			LastMove = fm.ToString();
			next.GenerateFEN(fm);
			/*
			if (next.FixStaleMate()) {
				IsStalemate = true;
				if (next.FixCheckMate()) {
					IsStalemate = false;
					IsCheckMate = true;
				}
			}*/
			//Console.WriteLine("After move\nMemory used before collection:       {0:N0}", GC.GetTotalMemory(false));
			GC.Collect(2, GCCollectionMode.Forced, false);
			//Console.WriteLine("Memory used after full collection:   {0:N0}", GC.GetTotalMemory(true));

			//Console.WriteLine(next.fen);
			return next;
            //this.delete
        }
		private Square FindEnemyKing() {
			Figure enemyKing = moveColor == Color.black ? Figure.whiteKing : Figure.blackKing;
			foreach (Square square in Square.YieldSquares()) {
				if (GetFigureAt(square)==enemyKing) { return square; };
			}
			return Square.none;
		}
		bool CanEatKing() {
			Square enemyKing = FindEnemyKing();
			Moves moves = new Moves(this);
			foreach (FigureOnSquare fs in YieldFigures()) {
				FigureMoving fm = new FigureMoving(fs, enemyKing);
				if (moves.CanMove(fm)) 
					return true;
			}
			return false;
		}
		public bool IsCheck() {
			Board after = new Board(fen);
			after.moveColor = moveColor.FlipColor();
			return after.CanEatKing();
		}
		public bool IsSelfCheck() {
			Board after = new Board(fen);
			after.moveColor = moveColor.FlipColor();
			return after.CanEatKing();

		}
		public bool IsCheckAfterMove(FigureMoving fm)
		{
			Board after = new Board(fen);
			after = after.Move(fm);
			return after.CanEatKing();
		}
	}
}
