namespace ChessAPI2.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Game
    {
        public int GameID { get; set; }

        [StringLength(255)]
        public string FEN { get; set; }

        [Required]
        [StringLength(4)]
        public string Status { get; set; }

        [StringLength(255)]
        public string White { get; set; }

        [StringLength(255)]
        public string Black { get; set; }

        [StringLength(10)]
        public string LastMove { get; set; }

        [StringLength(5)]
        public string YourColor { get; set; }

        [StringLength(5)]
        public string OfferDraw { get; set; }

        [StringLength(255)]
        public string Winner { get; set; }
    }
}
