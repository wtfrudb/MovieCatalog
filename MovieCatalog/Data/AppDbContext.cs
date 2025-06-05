using Microsoft.EntityFrameworkCore;
using MovieCatalog.Models;

namespace MovieCatalog.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Таблицы (таблицы будут созданы на основе этих DbSet)
        public DbSet<Movie> Movies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Rental> Rentals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Связь: Rental -> Movie (один фильм может быть в нескольких арендах)
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.Movie)
                .WithMany() // можно также WithMany(m => m.Rentals), если хочешь добавить навигацию обратно
                .HasForeignKey(r => r.MovieId)
                .OnDelete(DeleteBehavior.Restrict); // предотвратить удаление фильма, если он выдан

            // Связь: Rental -> User (один пользователь может взять много фильмов)
            modelBuilder.Entity<Rental>()
                .HasOne(r => r.User)
                .WithMany(u => u.Rentals) // в User.cs: public ICollection<Rental> Rentals { get; set; }
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade); // при удалении пользователя удалить и его аренды

            // Установка даты аренды по умолчанию (если не указана)
            modelBuilder.Entity<Rental>()
                .Property(r => r.RentalDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
