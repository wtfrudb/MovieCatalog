using Microsoft.EntityFrameworkCore;
using MovieCatalog.Models;

namespace MovieCatalog.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<RentalOrder> RentalOrders { get; set; }
        public DbSet<RentalItem> RentalItems { get; set; }
        public DbSet<Movie> Movies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User - RentalOrder (один ко многим)
            modelBuilder.Entity<RentalOrder>()
                .HasOne(ro => ro.User)
                .WithMany() // или WithMany(u => u.RentalOrders), если добавишь навигацию в User
                .HasForeignKey(ro => ro.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // RentalOrder - RentalItems (один ко многим)
            modelBuilder.Entity<RentalItem>()
                .HasOne(ri => ri.RentalOrder)
                .WithMany(ro => ro.Items)
                .HasForeignKey(ri => ri.RentalOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            // RentalItem - Movie (один к одному/многим)
            modelBuilder.Entity<RentalItem>()
                .HasOne(ri => ri.Movie)
                .WithMany() // или WithMany(m => m.RentalItems), если добавишь в Movie
                .HasForeignKey(ri => ri.MovieId)
                .OnDelete(DeleteBehavior.Restrict); // или .Cascade если хочешь удалять
        }
    }
}