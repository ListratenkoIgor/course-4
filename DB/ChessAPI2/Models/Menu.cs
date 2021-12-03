using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ChessRules;

namespace ChessAPI2.Models
{

    public class Menu
    {

        private ModelChessDB db;
        public Menu()
        {  
            db = new ModelChessDB();
        }
    }
}