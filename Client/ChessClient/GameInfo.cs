using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
namespace ChessClient
{

    public struct GameInfo
    {
        public int GameID;
        public string FEN;
        public string Status;
        public string White;
        public string Black;
        public string LastMove;
        public string YourColor;
        public bool IsYourMove;
        public string OfferDraw;
        public string Winner;
        public static bool BoolParse(string str) {
            if (str == "true") {
                return true;
            }
            return false;
        }
        public GameInfo(NameValueCollection list) {
            GameID = int.Parse(list["GameID"]);
            FEN = list["FEN"];
            Status = list["Status"];
            White = list["White"];
            Black = list["Black"];
            LastMove = list["LastMove"];
            YourColor = list["YourColor"];
            IsYourMove = BoolParse(list["IsYourMove"]);
            OfferDraw = list["OfferDraw"];
            Winner = list["Winner"];

        }
        override
        public string ToString() =>
            "GameID = " + GameID +
            "\nFEN = " + FEN +
            "\nStatus = " + Status +
            "\nWhite = " + White +
            "\nBlack = " + Black +
            "\nLastMove = " + LastMove +
            "\nYourColor = " + YourColor +
            "\nIsYourMove = " + IsYourMove +
            "\nOfferDraw = " + OfferDraw +
            "\nWinner = " + Winner;
    } 


}
