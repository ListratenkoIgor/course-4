namespace ChessAPI2.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ModelChessDB : DbContext
    {
        public ModelChessDB()
            : base("name=ModelChessDB")
        {
        }

        public virtual DbSet<Game> Games { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .Property(e => e.FEN)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.Status)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.White)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.Black)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.LastMove)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.YourColor)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.OfferDraw)
                .IsUnicode(false);

            modelBuilder.Entity<Game>()
                .Property(e => e.Winner)
                .IsUnicode(false);
        }
    }
}
