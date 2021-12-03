using System;
using System.Collections.Generic;
using System.Text;

namespace ChessRules
{
    class FigureMoving
    {
        public string Castling { get; set; }
        public string EnPassant { get; set; }
        public Figure figure { get; private set; }
        public Square from { get; private set; }
        public Square to { get; private set; }
        public Figure promotion { get; private set; }
        public FigureMoving(FigureOnSquare fs, Square to, Figure promotion = Figure.none)
        {
            this.figure = fs.figure;
            this.from = fs.square;
            this.to = to;
            this.promotion = promotion;
            this.EnPassant = "-";
            this.Castling = "----";
        }
        public FigureMoving(string move)
        {
            this.figure = (Figure)move[0];
            this.from = new Square(move.Substring(1, 2));
            this.to = new Square(move.Substring(3, 2));
            this.promotion = (move.Length == 6) ? (Figure)move[5] : Figure.none;
            this.EnPassant = "-";
            this.Castling = "----";
        }
        ~FigureMoving() {


        }
        public Square GetEnPassantSquare(Board board) {
            int stepY = this.figure.GetColor() == Color.white ? 1 : -1;
            string[] parts = board.fen.Split();
            if (parts[3] != "-")
                if (new Square(parts[3]) == this.to)
                    if (board.GetFigureAt(this.to) == Figure.none)
                        if (this.AbsDeltaX == 1 && this.DeltaY == stepY)
                        {                          
                            return new Square(this.to.x, this.to.y - stepY);
                        }
            this.EnPassant = "-";
            return new Square(-1,-1);

        }
        public void FixEnPassant(Board board)
        {
            int stepY = this.figure.GetColor() == Color.white ? 1 : -1;
            if (this.from.y == 1 || this.from.y == 6)
                if (board.GetFigureAt(this.to) == Figure.none)
                    if (this.DeltaX == 0 && this.DeltaY == 2 * stepY)
                        if (board.GetFigureAt(new Square(this.from.x, this.from.y + stepY)) == Figure.none)
                        {
                            this.EnPassant = (new Square(this.from.x, this.from.y + stepY)).Name;
                            return;
                        }
            this.EnPassant = "-";
            return;
        }
        public void FixCastling(Board board)
        {            
            string[] parts = board.fen.Split();
            this.Castling = parts[2];
            char[] newCastling=parts[2].ToCharArray();
            if (this.figure == Figure.whiteKing)
            {
                newCastling[0] = '-';
                newCastling[1] = '-';
            }
            if (this.figure == Figure.blackKing)
            {
                newCastling[2] = '-';
                newCastling[3] = '-';
            }
            if (this.figure == Figure.whiteRook) {
                if (this.from.y == 0) { 
                    newCastling[1] = '-';
                }
                if (this.from.y == 7)
                {
                    newCastling[0] = '-';
                }
            }
            if (this.figure == Figure.blackRook)
            {

                if (this.from.y == 0) { 
                newCastling[3] = '-';
                }
                if (this.from.y == 7)
                {
                newCastling[2] = '-';

                }
            }
            if (this.to == new Square(0,0)) { newCastling[1] = '-'; }
            if (this.to == new Square(0, 7)) { newCastling[0] = '-'; }
            if (this.to == new Square(7, 0)) { newCastling[3] = '-'; }
            if (this.to == new Square(7, 7)) { newCastling[2] = '-'; }
            this.Castling = new String(newCastling);
        }




        public Square GetCastlingSquare(Board board) {
            if  ((this.figure==Figure.blackKing|| this.figure == Figure.whiteKing)&&this.AbsDeltaX == 2 && this.DeltaY == 0)
                    return new Square(this.from.x + this.SignX, this.from.y);
            return Square.none;
        }
        public Square GetRookSquare(Board board)
        {
            int rookSide = this.SignX > 0 ? 7 : 0;
                return new Square(rookSide, this.from.y);
            
        }

        public int DeltaX { get { return to.x - from.x; } }
        public int DeltaY { get { return to.y - from.y; } }
        public int AbsDeltaX { get { return Math.Abs(DeltaX); } }
        public int AbsDeltaY { get { return Math.Abs(DeltaY); } }
        public int SignX { get { return Math.Sign(DeltaX); } }
        public int SignY { get { return Math.Sign(DeltaY); } }
        
        override public string ToString() {
            return (char)figure + from.Name + to.Name + (promotion == Figure.none ? ' ' : (char)promotion);
            //here may be errors
        }
    }
}