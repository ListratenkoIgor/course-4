using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChessRules;
using ChessClient;
namespace WindowsChess
{
    public partial class WindowsChess : Form
    {
        public WindowsChess()
        {
            InitializeComponent();
            client = new Client(HOST, USER);
            InitPanels();
            RefreshPosition();
        }

        #region Client Api
        Client client;
        public string HOST = "https://localhost:44378/api/Games";
        public string USER = "White";


        #endregion
        #region boardDraw

        int size = 50;
        Panel[,] map;
        Chess chess;
        bool IsSellected = false;
        int xFrom, yFrom;
        List<string> moves;
        void RefreshPosition() {
            chess = new Chess(client.GetCurrentGame().FEN);
            moves = chess.GetAllMoves();
            ShowPosition();
        }
        void InitPanels() {
            map = new Panel[8, 8];
            for (int x = 0; x < 8; x++) {
                for (int y = 0; y < 8; y++) {
                    map[x, y] = AddPanel(x, y);
                }
            }
        }
        void ShowPosition() {

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    ShowFigure(x, y,chess.GetFigureAt(x,y));
                }
            }
            //MarkSquared();
        }
        void ShowFigure(int x, int y ,char figure) {
            map[x, y].BackgroundImage = GetFigureImage(figure);
        
        }
        void MarkSquared() {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    map[x, y].BackColor = GetColor(x, y);
                }
            }
            if (IsSellected)
            {
                MarkSquaresTo();    
            }
            else {
                MarkSquaresFrom();
            }
        }
        Image GetFigureImage(char figure) {
            switch (figure)
            {
                case 'p':
                    return Properties.Resources.BlackPawn;
                case 'r':
                    return Properties.Resources.BlackRook;
                case 'n':
                    return Properties.Resources.BlackKnight;
                case 'b':
                    return Properties.Resources.BlackBishop;
                case 'q':
                    return Properties.Resources.BlackQueen;
                case 'k':
                    return Properties.Resources.BlackKing;
                case 'P':
                    return Properties.Resources.WhitePawn;
                case 'R':
                    return Properties.Resources.WhiteRook;
                case 'N':
                    return Properties.Resources.WhiteKnight;
                case 'B':
                    return Properties.Resources.WhiteBishop;
                case 'Q':
                    return Properties.Resources.WhiteQueen;
                case 'K':
                    return Properties.Resources.WhiteKing;
                default:
                    return null;

            }

        }
        Color GetColor(int x, int y) {
            return (x + y) % 2 == 0 ?Color.DarkGray:Color.White;
        }
        Color GetMarkedColor(int x, int y)
        {
            return (x + y) % 2 == 0 ? Color.Green : Color.LightGreen;
        }
        Point GetLocation(int x, int y)
        {
            return new Point(size/2+x*size,size/2+(7-y)*size);
        }
        Panel AddPanel(int x , int y) {
            Panel panel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            panel.BackColor = GetColor(x, y);
            panel.Location = GetLocation(x, y);
            panel.Name = "p" + x+y;
            panel.Size = new System.Drawing.Size(size, size);
            panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;

            panel.MouseClick += new System.Windows.Forms.MouseEventHandler(panel_MouseClick);
            // 
            pnlBoard.Controls.Add(panel);
            return panel;
        }
        void MarkSquaresFrom() {
            foreach (string move in moves) {
                int x = move[1] - 'a';
                int y = move[2] - '1';
                map[x, y].BackColor = GetMarkedColor(x,y);
            }
        }
        string ToCoord(int x, int y) {
            return ((char)('a' + x)).ToString() + ((char)('1' + y)).ToString();
        }
        void MarkSquaresTo() {
            string prefix = chess.GetFigureAt(xFrom, yFrom)+ToCoord(xFrom, yFrom);
            foreach (string move in moves)
            {
                if (move.StartsWith(prefix)) {
                    int x = move[3] - 'a';
                    int y = move[4] - '1';
                    map[x, y].BackColor = GetMarkedColor(x, y);
                }
            }

        }
        private void panel_MouseClick(object sender, MouseEventArgs e)
        {
            string xy = ((Panel)sender).Name.ToString().Substring(1);
            int x = xy[0] - '0';
            int y = xy[1] - '0';
            if (!IsSellected)
            {
                IsSellected = true;
                xFrom = x;
                yFrom = y;
                map[x, y].BackColor = GetMarkedColor(x, y);
            }
            else {
                map[xFrom, yFrom].BackColor = GetColor(xFrom, yFrom);
                IsSellected = false;
                string move = chess.GetFigureAt(xFrom, yFrom) + ToCoord(xFrom, yFrom) + ToCoord(x, y);
                chess = chess.Move(client.SendMove(move).FEN);
                
            }
            RefreshPosition();       
        }

        #endregion
        private void Form1_Load(object sender, EventArgs e)
        {

        }


    }
}
