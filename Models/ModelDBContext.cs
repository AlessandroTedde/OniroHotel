using System.Data.Entity;

namespace OniroHotel.Models
{
    public partial class ModelDBContext : DbContext
    {
        public ModelDBContext()
            : base("name=ModelDBContext")
        {
        }

        public virtual DbSet<Applications> Applications { get; set; }
        public virtual DbSet<Extras> Extras { get; set; }
        public virtual DbSet<ReservationDetails> ReservationDetails { get; set; }
        public virtual DbSet<Reservations> Reservations { get; set; }
        public virtual DbSet<Rooms> Rooms { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservations>()
                .Property(e => e.Treatment)
                .IsFixedLength();

            modelBuilder.Entity<Reservations>()
                .HasMany(e => e.ReservationDetails)
                .WithRequired(e => e.Reservations)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Rooms>()
                .HasMany(e => e.ReservationDetails)
                .WithRequired(e => e.Rooms)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Users>()
                .Property(e => e.UserRole)
                .IsFixedLength();

            modelBuilder.Entity<Users>()
                .HasMany(e => e.Reservations)
                .WithRequired(e => e.Users)
                .WillCascadeOnDelete(false);
        }
    }
}
